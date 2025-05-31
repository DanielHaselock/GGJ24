using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class NumbRounds : MonoBehaviour
{
    [SerializeField] private int startNumbRounds = 3;

    private int numbRounds;

    void Start()
    {
        numbRounds = startNumbRounds;
    }

    public void increaseNumbRounds() //call from input in scene
    {
        numbRounds++;
        updateUI();
    }

    public void decreaseNumbRounds() //call from input in scene
    {
        numbRounds--;
        updateUI();
    }

    private void updateUI()
    {
        //Update textMeshPro or other UI here
    }

    public void submit()
    {
        //call a manager and save numbRounds to it here
    }

}
