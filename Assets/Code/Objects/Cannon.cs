using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float SpawnRateUpper = 2f;
    [SerializeField] private float SpawnRateLower = 5f;
    private float Timer = 0f;
    [SerializeField] private Animator m_animator;
    [SerializeField] private Transform SpawnPoint;

    private float SpawnRate;
    [SerializeField] private float SpawnedObjectSpeed = 10f;

    [SerializeField] private GameObject SpawnedObject;

    void Start()
    {
        m_animator = GetComponent<Animator>();

        SpawnRate = Random.Range(SpawnRateLower, SpawnRateUpper);
        Timer = SpawnRate;
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if(Timer <= 0)
        {
            Instantiate(SpawnedObject, SpawnPoint.transform.position, Quaternion.identity);

            Rigidbody2D SpawnedObjectRb = SpawnedObject.GetComponent<Rigidbody2D>();
            SpawnedObjectRb.linearVelocity = SpawnPoint.forward * SpawnedObjectSpeed;

            SpawnRate = Random.Range(SpawnRateLower, SpawnRateUpper);
            Timer = SpawnRate;
            m_animator.SetTrigger("shoot");
        }

    }
}
