using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericObstacle : MonoBehaviour
{

    [SerializeField] private int m_xBounceForce = 25, m_xBounceBias = -10, m_yBounceForce = 15, m_yBounceBias = -5;
    protected Animator m_animator;

    protected virtual void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.tag.Equals("Player"))
            return;
        
        // Compute bounce direction
        float newX = m_xBounceForce * (collision.collider.transform.position.x - transform.position.x) + m_xBounceBias;
        float newY = m_yBounceForce * (collision.transform.position.y - transform.position.y) + m_yBounceBias;

        // Bounce
        Rigidbody2D playerRb = collision.collider.GetComponent<Rigidbody2D>();
        playerRb.linearVelocity = new Vector2(newX, newY);

        PlayerController playerController = collision.collider.GetComponent<PlayerController>();
        playerController.Hurt();

        AudioManager.Instance.PlayLaughTrackOrGasp();
    }
}
