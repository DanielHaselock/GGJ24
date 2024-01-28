using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BouncingObstacle : GenericObstacle
{
    private Rigidbody2D m_rb;
    [SerializeField] private Vector2 m_startingVelocity;

    [SerializeField] private float m_bouncingforce = 0.5f;

    protected override void Start()
    {
        base.Start();
        m_rb = GetComponent<Rigidbody2D>();
        m_rb.velocity = m_startingVelocity;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        AudioManager.Instance.Boing();

        base.OnCollisionEnter2D(collision);

        Vector2 relativevVelocity = collision.relativeVelocity;

        if (Mathf.Abs(Mathf.Acos(Vector2.Dot(relativevVelocity, Vector2.down))) < Mathf.PI / 4)
            m_animator.SetTrigger("BounceFloor");

        else
            m_animator.SetTrigger("BounceWall");

        if (collision.collider.tag.Equals("Player"))
            return;

        // Get average surface normal
        Vector2 averageNormal = Vector2.zero;
        foreach (var item in collision.contacts)
        {
            averageNormal += item.normal;
        }
        averageNormal /= collision.contacts.Length;

        // Bouncing direction
        m_rb.velocity = (-relativevVelocity + 2 * Vector2.Dot(relativevVelocity, averageNormal) * averageNormal) * m_bouncingforce;
    }
}
