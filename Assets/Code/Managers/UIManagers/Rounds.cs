using TMPro;
using UnityEngine;

public class Rounds : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numbText;
    [SerializeField] private GameObject arrows;

    public void Start()
    {
        numbText.text = GameManager.Instance.maxRounds.ToString("00");
    }

    public void updateUI(int numb)
    {
        if (numbText)
        {
            numbText.text = numb.ToString("00");

            // if (???)
            // arrows.GetComponent<Animator>().SetTrigger("more");

            // else if (???)
            // arrows.GetComponent<Animator>().SetTrigger("less");
        }
    }
}
