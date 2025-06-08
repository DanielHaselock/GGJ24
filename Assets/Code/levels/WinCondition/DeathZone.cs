using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("COLLISION ENTERING");
        if (collision.gameObject.tag != "Player")
        {
            Destroy(collision.gameObject);
        }
        else
        {
            Debug.Log("COLLISION HITTING PLAYER");
            collision.gameObject.GetComponent<PlayerRespawn>().onDeath();
        }
    }
}
