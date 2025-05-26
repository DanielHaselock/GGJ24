using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject GameTitle;
    [SerializeField] private GameObject Buttons;
    [SerializeField] private GameObject Prompt;
    // 0 = on boot up, 1 = main menu, 2 = lobby, 3 = credits, 4 = start game
    [SerializeField] private MenuStates currentMenuState = MenuStates.Title;

    enum MenuStates
    {
        Title,
        MainMenu,
        Lobby,
        Credits,
        StartGame
    }

    void Update()
    {
        MenuStateManager();
    }

    public void MenuStateManager()
    {
        switch (currentMenuState)
        {
        case MenuStates.Lobby:
            GameTitle.GetComponent<Animator>().SetTrigger("title_menu");
            Prompt.SetActive(false);
            break;
        case MenuStates.MainMenu:
            GameTitle.GetComponent<Animator>().SetTrigger("title_menu");
            Prompt.SetActive(true);
            Prompt.transform.localPosition = new Vector3(0, -32, 0);
            break;
        case MenuStates.Title:
            GameTitle.GetComponent<Animator>().SetTrigger("title_start");
            Prompt.transform.localPosition = new Vector3(0, -52, 0);
            break;
        default:
            break;
        }
    }
}
