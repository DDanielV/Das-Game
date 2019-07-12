using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    private void Start()
    {
        // If we dont start from the StartMenu we host a new game.
        // This enables starting the game from a different scene than the StartMenu for testing.
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            HostGame();
        }
    }

    public void HostGame()
    {
        StartHost();
    }

    public void JoinGame(string networkAdress)
    {
        this.networkAddress = networkAdress;
        StartClient();
    }
}
