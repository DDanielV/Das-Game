using UnityEngine.SceneManagement;
using UnityEngine;

public class Submarine : ShipBase
{
    // If we die we want to reload the current scene.
    protected override void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
