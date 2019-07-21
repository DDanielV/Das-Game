using UnityEngine;

public class Gunner : PlayerCharacter
{
    private void LateUpdate()
    {
        RenderSettings.fog = _characterCamera.transform.position.y <= 0.1;
    }
}
