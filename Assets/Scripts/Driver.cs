﻿using UnityEngine;
using UnityEngine.UI;

// The Driver class is a PlayerCharacter that contains the code for controlling the _submarine.
public class Driver : PlayerCharacter
{
    protected Rigidbody _rigidbody;
    private float steering, throttle, maxSteering, inputStearing, inputThrotle, inputDiving, speed, depthThrottle, maxSpeed;
    private float[] steeringArray = new float[10], throttleArray = new float[10];
    private int steeringArrayIndex, touchId;
    public float addforce = 200000;
    public float addtorque = 800000;
    private Vector2 touchStartPos;

    //_canvas
    private Slider throttleSlider, depthSlider;
    private Image steeringwheelLeftSprite, steeringwheelRightSprite, steeringwheelPointerSprite;
    private Text speedText, depthText, directionText;

    private void Start()
    {
        // Get a reference to the rigidbody of the submarine so we can control it.
        _rigidbody = Submarine.GetComponent<Rigidbody>();

        GetSliders();
        GetImages();
        GetTexts();

        maxSpeed = (addforce / _rigidbody.drag - 0.01f * addforce) / _rigidbody.mass;

        // Set the center of mass and intertiaTensor to the center of the object.
        _rigidbody.centerOfMass = Vector3.zero;
        _rigidbody.inertiaTensorRotation = Quaternion.identity;
    }

    private void GetSliders()
    {
        Slider[] sliderArray = _characterCanvas.GetComponentsInChildren<Slider>();
        foreach (Slider slider in sliderArray)
        {
            switch (slider.gameObject.name)
            {
                case "ThrottleSlider":
                    throttleSlider = slider;
                    break;

                case "DepthSlider":
                    depthSlider = slider;
                    break;

                default:
                    Debug.Log("Unkown slider component: " + slider.gameObject.name);
                    break;
            }
        }
    }

    private void GetImages()
    {
        Image[] imageArray = _characterCanvas.GetComponentsInChildren<Image>();
        foreach (Image image in imageArray)
        {
            switch (image.gameObject.name)
            {
                case "SteeringwheelLeft":
                    steeringwheelLeftSprite = image;
                    break;

                case "SteeringwheelRight":
                    steeringwheelRightSprite = image;
                    break;

                case "SteeringwheelPointer":
                    steeringwheelPointerSprite = image;
                    break;

                case "Handle":
                    //do nothing, is an image of the slider
                    break;

                case "Background":
                    //do nothing, is an image of the slider
                    break;

                default:
                    Debug.Log("Unkown image component: " + image.gameObject.name);
                    break;
            }
        }
    }

    private void GetTexts()
    {
        Text[] textArray = _characterCanvas.GetComponentsInChildren<Text>();
        foreach (Text text in textArray)
        {
            switch (text.gameObject.name)
            {
                case "SpeedText":
                    speedText = text;
                    break;

                case "DepthText":
                    depthText = text;
                    break;

                case "DirectionText":
                    directionText = text;
                    break;

                default:
                    Debug.Log("Unkown text component: " + text.gameObject.name);
                    break;
            }
        }
    }

    // Update is called once per frame.
    // Get the player input for controlling the _submarine
    private void Update()
    {
        GetInput();
        UpdateCanvas();
    }

    // FixedUpdate is called every fixed timestep.
    private void FixedUpdate()
    {
        _rigidbody.AddTorque(_rigidbody.transform.up * steering * speed * addtorque);
        _rigidbody.velocity = _rigidbody.transform.forward * speed;
        _rigidbody.AddForce(_rigidbody.transform.forward * addforce * throttle);


        if (_rigidbody.transform.position.y + depthThrottle * Time.deltaTime > 0)
        {
            _rigidbody.MovePosition(_rigidbody.transform.position + new Vector3(0, -_rigidbody.transform.position.y, 0));
        }
        else
        {
            _rigidbody.MovePosition(_rigidbody.transform.position + _rigidbody.transform.up * depthThrottle * Time.deltaTime);
        }
    }

    private void GetInput()
    {
        //**Getting the stearing and throttle input values**//
        //takes the average of 10 inputs of 
        steeringArrayIndex++;
        if (steeringArrayIndex >= 10) { steeringArrayIndex = 0; }
        steeringArray[steeringArrayIndex] = Input.acceleration.x;
        throttleArray[steeringArrayIndex] = Input.acceleration.y;
        float totalsteering = 0, totalthrotle = 0;
        for (int i = 0; i < 10; i++)
        {
            totalsteering += steeringArray[i];
            totalthrotle += throttleArray[i];

        }
        inputStearing = totalsteering / 10;
        inputThrotle = totalthrotle / 10;

        //**getting the diving input value**//
        if(Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                touchId = touch.fingerId;
                inputDiving = 0;
            }
            else
            {
                if (touchId == touch.fingerId)
                {
                    float dY = touch.position.y - touchStartPos.y;
                    inputDiving = Mathf.Clamp(2*(dY / Screen.height),-1f,1f);
                }
            }
        }
        else
        {
            inputDiving = 0;
        }

        //**converting the input values to the actual values**//
        throttle = (Mathf.Clamp(inputThrotle, -1f, -0.4f) + 0.7f) / 0.3f;
        maxSteering = 1f - _rigidbody.velocity.magnitude / maxSpeed * 0.7f; //0.7f betekent dat bij max speed nog (1-0.7)30% stuurkracht over hebt
        steering = inputStearing * 2 + Input.GetAxis("Horizontal");
        steering = Mathf.Clamp(steering, -maxSteering, maxSteering);
        speed = _rigidbody.velocity.magnitude * Vector3.Dot(_rigidbody.transform.forward, Vector3.Normalize(_rigidbody.velocity));
        depthThrottle = inputDiving * 10;
    }

    private void UpdateCanvas()
    {
        steeringwheelLeftSprite.fillAmount = maxSteering;
        steeringwheelRightSprite.fillAmount = maxSteering;

        Quaternion steeringwheel_rotation = Quaternion.Euler(0f, 0f, -90 * steering);
        steeringwheelPointerSprite.transform.SetPositionAndRotation(new Vector3(Screen.width / 2,Screen.height/10 ,0) + steeringwheel_rotation * new Vector3(0, Screen.height / 2, 0), steeringwheel_rotation);

        directionText.text = Quaternion.LookRotation(transform.forward).eulerAngles.y.ToString("F0") + "°";
        speedText.text = "Speed: " + speed.ToString("F1");
        depthText.text = "Depth: " + transform.position.y.ToString("F1");

        throttleSlider.value = throttle;
        depthSlider.value = inputDiving;
    }

    private void LateUpdate()
    {
        if (_characterCamera.transform.position.y <= 0.1f) { RenderSettings.fog = true; } // (_camera.transform.position.y <= 0) werkt niet goed, kans op een _camera half onderwater maar geen fog
        else { RenderSettings.fog = false; }
    }
}