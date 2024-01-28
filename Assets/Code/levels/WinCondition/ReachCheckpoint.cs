using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ReachCheckpoint : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject LevelEditor;
    void Start()
    {
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
            LevelEditor.GetComponent<CurrentLevelManager>().SetWinAndFinish();
    }
}
