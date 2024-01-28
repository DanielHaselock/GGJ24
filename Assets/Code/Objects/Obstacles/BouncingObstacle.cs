using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingObstacle : GenericObstacle
{
    private Rigidbody2D m_rb;
    [SerializeField] private Vector2 m_startingVelocity;

    [SerializeField] private float m_bouncingforce = 0.5f;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_rb.velocity = m_startingVelocity;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.collider.tag.Equals("Player"))
            return;

        Vector2 velocity = collision.relativeVelocity;

        // Get average surface normal
        Vector2 averageNormal = Vector2.zero;
        foreach (var item in collision.contacts)
        {
            averageNormal += item.normal;
        }
        averageNormal /= collision.contacts.Length;

        // Bouncing direction
        m_rb.velocity = (-velocity + 2 * Vector2.Dot(velocity, averageNormal) * averageNormal) * m_bouncingforce;
    }
}
