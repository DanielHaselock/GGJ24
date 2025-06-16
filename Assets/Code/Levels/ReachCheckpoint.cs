using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ReachCheckpoint : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private int priority = 0;

    [SerializeField] private bool isFinal = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("PlayerFinished");
            if (isFinal)
            {
                FindFirstObjectByType<RaceLevelManager>().OnPlayerFinished(collision.gameObject);
                collision.gameObject.GetComponent<PlayerController>().cheer();
            }
               
            else
                collision.gameObject.GetComponent<PlayerRespawn>().setNewCheckpoint(this);
        }
    }

    public int getPriority()
    {
        return priority;
    }
}
