using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    private InputField _networkAddressField;

    [SerializeField]
    private CustomNetworkManager _customNetworkManager;

    public void HostGameButtonClicked()
    {
        _customNetworkManager.HostGame();
    }

    public void JoinGameButtonClicked()
    {
        _customNetworkManager.JoinGame(_networkAddressField.text);
    }
}
