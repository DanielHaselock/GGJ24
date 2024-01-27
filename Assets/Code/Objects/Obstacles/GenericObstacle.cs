using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericObstacle : MonoBehaviour
{

    [SerializeField] private int m_xBounceForce = 25, m_xBounceBias = -10, m_yBounceForce = 15, m_yBounceBias = -5;

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Player"))
        {
            // Compute bounce direction
            float newX = m_xBounceForce * (collision.collider.transform.position.x - transform.position.x) + m_xBounceBias;
            float newY = m_yBounceForce * (collision.transform.position.y - transform.position.y) + m_yBounceBias;

            // Bounce
            Rigidbody2D playerRb = collision.collider.GetComponent<Rigidbody2D>();
            playerRb.velocity = new Vector2(newX, newY);
        }
    }
}
