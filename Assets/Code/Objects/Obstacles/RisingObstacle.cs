using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingObstacle : GenericObstacle
{
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.collider.tag.Equals("Player"))
        {
            Destroy(gameObject); // pop balloon
        }
    }
}
