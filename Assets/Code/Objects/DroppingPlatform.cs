using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppingPlatform : MonoBehaviour
{
    [SerializeField] private float drop_delay = 1f;
    public float contactThreshold = 30f;
    protected Animator m_animator;

    protected virtual void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController playerController = collision.collider.GetComponent<PlayerController>();
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (Vector3.Angle(contact.normal, Vector3.down) <= contactThreshold)
            {
                if (!collision.collider.tag.Equals("Player"))
                {
                    return;
                }
                if (collision.collider.tag.Equals("Player"))
                {
                    StartCoroutine(Drop());
                }
            }
        }
    }

    IEnumerator Drop()
    {
        yield return new WaitForSeconds(drop_delay);
        m_animator.SetTrigger("drop");
        GetComponent<PolygonCollider2D>().enabled = false;
    }
}
