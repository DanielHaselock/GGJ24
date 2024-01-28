using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingObstacle : GenericObstacle
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.collider.tag.Equals("Player"))
        {
            m_animator.SetTrigger("Pop");
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
