using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericObstacle : MonoBehaviour
{

    [SerializeField] private float m_BounceForce;
    [SerializeField] private int m_xBounceBias;
    [SerializeField] private int m_yBounceBias;
    [SerializeField] private bool can_stun = false;
    [SerializeField] private bool can_kill = false;
    protected Animator m_animator;
    private float m_DefaultBounceForce;

    protected virtual void Start()
    {
        m_animator = GetComponent<Animator>();
        m_DefaultBounceForce = m_BounceForce;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController playerController = collision.collider.GetComponent<PlayerController>();

        if (!collision.collider.tag.Equals("Player"))
        {
            m_BounceForce = m_DefaultBounceForce;
            return;
        }
        // Higher bounce force when colliding with player
        if (collision.collider.tag.Equals("Player") && can_stun)
        {
            m_BounceForce = m_DefaultBounceForce * 1.75f;
        }
        else
        {
            m_BounceForce = m_DefaultBounceForce;
        }
        // Compute bounce direction
        float newX = m_BounceForce * (collision.collider.transform.position.x - transform.position.x) + m_xBounceBias;
        float newY = m_BounceForce * (collision.collider.transform.position.y - transform.position.y) + m_yBounceBias;
        // Bounce
        Rigidbody2D playerRb = collision.collider.GetComponent<Rigidbody2D>();
        playerRb.linearVelocity = new Vector2(newX, newY);
        if (can_kill == false && playerController.m_isGrounded == false) {
            playerController.GetComponent<Animator>().SetTrigger("Jump");
        }
        // Kill player
        if (can_kill == true) {
            playerController.Hurt(); 
        }
    }
}
