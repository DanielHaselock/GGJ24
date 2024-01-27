using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : GenericObstacle
{
    BoxCollider2D m_collider;
    SpriteRenderer m_spriteRenderer;

    void Start()
    {
        StartCoroutine(ActiveInactiveLoop());
        m_collider = GetComponent<BoxCollider2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private IEnumerator ActiveInactiveLoop() { 
        while (true)
        {
            yield return new WaitForSeconds(1.875f);
            m_collider.enabled = false;
            m_spriteRenderer.enabled = false;

            yield return new WaitForSeconds(1.875f);
            m_collider.enabled = true;
            m_spriteRenderer.enabled = true;
        }
    }
}
