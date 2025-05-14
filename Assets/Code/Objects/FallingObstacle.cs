using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObstacle : GenericObstacle
{
    bool m_hasFallen;

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Solid"))
            m_hasFallen = true;

        if (!m_hasFallen)
            base.OnCollisionEnter2D(collision);

        m_animator.SetTrigger("Impact");
    }
}
