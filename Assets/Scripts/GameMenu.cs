using UnityEngine;
using UnityEngine.Networking;

public class GameMenu : MonoBehaviour
{
    [SerializeField]
    private CharacterMenu _characterMenu;

    public void ShowHideMenu()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }

    public void ChangeCharacterButtonClicked()
    {
        _characterMenu.ShowMenu();
        gameObject.SetActive(false);
    }

    public void QuitGameButtonClicked()
    {
        //NetworkManager.singleton.StopClient();
        NetworkManager.singleton.StopHost();
        //NetworkManager.singleton.StopServer();
    }
}
