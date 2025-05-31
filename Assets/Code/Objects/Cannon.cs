using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private float SpawnRateUpper = 2f;
    [SerializeField] private float SpawnRateLower = 5f;
    // initial timer value to set up a delay before cannons start shooting when the level starts
    private float Timer = 2f;
    [SerializeField] private Animator m_animator;
    [SerializeField] private Transform SpawnPoint;

    private float SpawnRate;
    [SerializeField] public float forceMagnitude = 1000f;

    [SerializeField] public GameObject SpawnedObject;
    [SerializeField] private GameObject CannonPuff;

    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        StartCoroutine(ShootObject());
    }

   IEnumerator ShootObject()
   {
        Timer -= Time.deltaTime;
        while (Timer <= 0)
        {
            SpawnRate = Random.Range(SpawnRateLower, SpawnRateUpper);
            Timer = SpawnRate;
            // spawn object
            GameObject instance = Instantiate(SpawnedObject, SpawnPoint.transform.position, Quaternion.identity);
            // get the rigidbody of the object and apply a force
            Rigidbody2D rigidbody = instance.GetComponent<Rigidbody2D>();
            if (rigidbody != null)
            {
                rigidbody.AddForce(SpawnPoint.transform.up * forceMagnitude, ForceMode2D.Impulse);
            }
            // play shooting animations
            m_animator.SetTrigger("shoot");
            CannonPuff.GetComponent<Animator>().Play("cannon_puff");
        }
        yield return new WaitForSeconds(Timer);
   }
}
