using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Destroy(collision.gameObject);
        }
        else
        {
            collision.gameObject.GetComponent<PlayerRespawn>().onDeath();
        }
    }
}
