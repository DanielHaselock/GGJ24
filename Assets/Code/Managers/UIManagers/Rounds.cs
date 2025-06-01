using TMPro;
using UnityEngine;

public class Rounds : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numbText;
    public void updateUI(int numb)
    {
        if (numbText)
        {
            numbText.text = numb.ToString();
        }
    }
}
