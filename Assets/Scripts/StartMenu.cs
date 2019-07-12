using TMPro;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _networkAddressField;

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
