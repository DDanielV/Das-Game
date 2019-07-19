using UnityEngine;

// The CharacterMenu class alows the player to choose a PlayerClass
public class CharacterMenu : MonoBehaviour
{
    private enum PlayerCharacters
    {
        Driver = 0,
        Gunner = 1,
        Commander = 2
    }

    private PlayerCharacter _character = null;

    // For now this is fine, we might want to change this in the future when there are multiple submarines.
    [SerializeField] private GameObject _submarine;

    public void Start()
    {
        //bug in unity, als fog niet aan staat bij createn van de APK wordt de shader hiervoor ook niet toegevoegd aan de apk, daarom wordt hier de fog uigezet aan het begin van het spel
        RenderSettings.fog = false;
    }

    public void ShowMenu()
    {
        gameObject.SetActive(true);
    }

    // Finds the character in the submarine and activates it.
    // This method is called by the buttons on the CharacterMenu panel.
    // Using an int since button can't use enums as parameter.
    public void SetPlayerCharacter(int character)
    {
        // We deactivate the playercharacter gameobject if we already have one.
        if (_character != null)
        {
            _character.Deactivate();
        }

        switch (character)
        {
            case (int)PlayerCharacters.Driver:
                _character = _submarine.GetComponentInChildren<Driver>(true);
                break;
            case (int)PlayerCharacters.Gunner:
                _character = _submarine.GetComponentInChildren<Gunner>(true);
                break;
            case (int)PlayerCharacters.Commander:
                _character = _submarine.GetComponentInChildren<Commander>(true);
                break;
        }

        // We activate the chosen character and hide the character menu.
        if (_character != null)
        {
            _character.Submarine = _submarine;
            _character.Activate();
        }

        gameObject.SetActive(false);
    }
}
