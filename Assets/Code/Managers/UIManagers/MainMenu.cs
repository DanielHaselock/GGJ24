using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject GameTitle;
    [SerializeField] private GameObject Buttons;

    private Button singlePlayerButton;
    private Button multiplayerButton;
    private Button creditsButton;
    private Button exitButton;
    [SerializeField] private GameObject prompt;

    private Animator promptAnimator;
    [SerializeField] private MenuStates currentMenuState = MenuStates.SinglePlayer;

    private MenuStates previousMenuState = MenuStates.None;


    enum MenuStates
    {
        SinglePlayer,
        Multiplayer,
        Credits,
        Exit,
        None
    }

    private void Start()
    {
        singlePlayerButton = Buttons.transform.Find("SinglePlayer").GetComponent<Button>();
        multiplayerButton = Buttons.transform.Find("Multiplayer").GetComponent<Button>();
        creditsButton = Buttons.transform.Find("Credits").GetComponent<Button>();
        exitButton = transform.Find("Quit").GetComponent<Button>();

        singlePlayerButton.onClick.AddListener(() => { GameManager.Instance.StartSingleplayerGame(); });
        multiplayerButton.onClick.AddListener(() => { GameManager.Instance.GoToLobby(); });
        exitButton.onClick.AddListener(() => { GameManager.Instance.QuitGame(); });

        promptAnimator = prompt.GetComponent<Animator>();
    }

    void Update()
    {
        UpdateHighlightedButton();
        MenuStateManager(); // If you want to do logic based on MenuStates
    }

    private void UpdateHighlightedButton()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected == singlePlayerButton.gameObject)
            currentMenuState = MenuStates.SinglePlayer;
        else if (selected == multiplayerButton.gameObject)
            currentMenuState = MenuStates.Multiplayer;
        else if (selected == creditsButton.gameObject)
            currentMenuState = MenuStates.Credits;
        else if (selected == exitButton.gameObject)
            currentMenuState = MenuStates.Exit;
        else
            currentMenuState = MenuStates.None;

        Debug.Log(selected);
    }

    private void MenuStateManager()
    {
        if (currentMenuState == previousMenuState) return;
        switch (currentMenuState)
        {
            case MenuStates.SinglePlayer:
                promptAnimator.SetTrigger("singleplayer");
                break;
            case MenuStates.Multiplayer:
                promptAnimator.SetTrigger("multiplayer");
                break;
            case MenuStates.Credits:
                promptAnimator.SetTrigger("credits");
                break;
            case MenuStates.Exit:
                promptAnimator.SetTrigger("exit");
                break;
            case MenuStates.None:
                break;
        }
        previousMenuState = currentMenuState;
    }

}
