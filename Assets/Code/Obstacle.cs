using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Player"))
        {
            Rigidbody2D playerRb = collision.collider.GetComponent<Rigidbody2D>();
            float newX = 25 * (collision.collider.transform.position.x - transform.position.x);
            float newY = 15 * (collision.transform.position.y - transform.position.y);
            playerRb.velocity = new Vector2(newX, newY);
        }
    }
}
