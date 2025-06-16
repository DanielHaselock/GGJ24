using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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
        float horizontalInput = Input.GetAxis("Horizontal");

        if (numbText)
        {
            numbText.text = numb.ToString("00");
        }

        if (horizontalInput > 0)
        {
            arrows.GetComponent<Animator>().SetTrigger("more");
        }
        else if (horizontalInput < 0)
        {
            arrows.GetComponent<Animator>().SetTrigger("less");
        }
    }
}
