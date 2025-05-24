using UnityEngine;
/// <summary>
///  MonoBehaviour script that manages scene loading.
/// - Ensures the right scene is loading using strings.
/// - Handles scene transitions.
/// </summary>
public class Loader : MonoBehaviour
{
    [SerializeField] private GameObject LeftCurtain;
    [SerializeField] private GameObject RightCurtain;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        LeftCurtain.GetComponent<Animator>().SetTrigger("open");
        RightCurtain.GetComponent<Animator>().SetTrigger("open");
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
