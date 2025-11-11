using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private bool deathOn = true;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!deathOn)
        {
            Debug.Log("Death Zone is turned off");
            return;  
        } 
        if (collision.gameObject.tag != "Player")
        {
            Destroy(collision.gameObject);
        }
        else
        {
            collision.gameObject.GetComponent<PlayerRespawn>().onDeath();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!deathOn)
        {
            Debug.Log("Death Zone is turned off");
            return;  
        } 
        if (collision.gameObject.tag != "Player")
        {
            Destroy(collision.gameObject);
        }
        else
        {
            collision.gameObject.GetComponent<PlayerRespawn>().onDeath();
        }
    }

    public void ToggleDeathZone(bool toggle)
    {
        deathOn = toggle;
        Debug.Log("DeathZone Toggled to " + deathOn);
    }
}
