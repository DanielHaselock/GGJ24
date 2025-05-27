using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericObstacle : MonoBehaviour
{

    [SerializeField] private int m_BounceForce = 25;
    [SerializeField] private int m_xBounceBias;
    [SerializeField] private int m_yBounceBias;
    [SerializeField] private bool can_kill = false;
    protected Animator m_animator;

    protected virtual void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController playerController = collision.collider.GetComponent<PlayerController>();

        if (!collision.collider.tag.Equals("Player"))
            return;
        
        // Compute bounce direction
        float newX = m_BounceForce * (collision.collider.transform.position.x - transform.position.x) + m_xBounceBias;
        float newY = m_BounceForce * (collision.collider.transform.position.y - transform.position.y) + m_yBounceBias;
        // Bounce
        Rigidbody2D playerRb = collision.collider.GetComponent<Rigidbody2D>();
        playerRb.linearVelocity = new Vector2(newX, newY);
        if (can_kill == false) {
            playerController.GetComponent<Animator>().SetBool("jumping", true);
        }
        // Kill player
        if (can_kill == true) {
            playerController.Hurt(); 
        }
    }
}
