using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject GameTitle;
    [SerializeField] private GameObject Buttons;
    [SerializeField] private GameObject Prompt;
    // 0 = on boot up, 1 = main menu, 2 = lobby, 3 = credits, 4 = start game
    [SerializeField] private int MenuState = 0;

    void Update()
    {
        MenuStateManager();
    }

    public void MenuStateManager()
    {
        switch (MenuState)
        {
        case 2:
            GameTitle.GetComponent<Animator>().SetTrigger("title_menu");
            Prompt.SetActive(false);
            break;
        case 1:
            GameTitle.GetComponent<Animator>().SetTrigger("title_menu");
            Prompt.SetActive(true);
            Prompt.transform.localPosition = new Vector3(0, -32, 0);
            break;
        case 0:
            GameTitle.GetComponent<Animator>().SetTrigger("title_start");
            Prompt.transform.localPosition = new Vector3(0, -52, 0);
            break;
        default:
            break;
        }
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
