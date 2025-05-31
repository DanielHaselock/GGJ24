using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
    }
}
