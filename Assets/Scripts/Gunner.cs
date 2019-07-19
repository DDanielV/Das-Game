using UnityEngine;

public class Gunner : PlayerCharacter
{
    private void LateUpdate()
    {
        if (_characterCamera.transform.position.y <= 0.1) { RenderSettings.fog = true; } // (_camera.transform.position.y <= 0) werkt niet goed, kans op een _camera half onderwater maar geen fog
        else { RenderSettings.fog = false; }
    }
}
