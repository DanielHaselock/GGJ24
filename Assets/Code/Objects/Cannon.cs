using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private float SpawnRateUpper;
    [SerializeField] private float SpawnRateLower;
    // initial timer value to set up a delay before cannons start shooting when the level starts
    private float Timer = 0.5f;
    private Vector3 initial_position;
    private bool start_shooting = false;
    [SerializeField] private Animator m_animator;
    [SerializeField] private Transform SpawnPoint;

    private float SpawnRate;
    [SerializeField] public float forceMagnitudeUpper;
    [SerializeField] public float forceMagnitudeLower;

    [SerializeField] public GameObject SpawnedObject;
    [SerializeField] private GameObject CannonPuff;

    void Start()
    {
        initial_position = transform.position;
        m_animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        StartCoroutine(ShootObject());
        // change speed of animations depending on if moving
        if (initial_position != transform.position || m_animator.GetCurrentAnimatorStateInfo(0).IsName("cannon_shoot"))
        {
            m_animator.speed = 1f;
        }
        else if (initial_position == transform.position)
        {
            m_animator.speed = 0.66f;
        }
    }

   IEnumerator ShootObject()
   {
        Timer -= Time.deltaTime;
        while (Timer <= 0)
        {
            SpawnRate = Random.Range(SpawnRateLower, SpawnRateUpper);
            Timer = SpawnRate;
            if (start_shooting == true)
            {
                // spawn object
                GameObject instance = Instantiate(SpawnedObject, SpawnPoint.transform.position, Quaternion.identity);
                // get the rigidbody of the object and apply a force
                Rigidbody2D rigidbody = instance.GetComponent<Rigidbody2D>();
                if (rigidbody != null)
                {
                    rigidbody.AddForce(SpawnPoint.transform.up * Random.Range(forceMagnitudeLower, forceMagnitudeUpper), ForceMode2D.Impulse);
                }
                // play shooting animations
                m_animator.SetTrigger("shoot");
                CannonPuff.GetComponent<Animator>().Play("cannon_puff");
            }
            start_shooting = true;
        }
        yield return new WaitForSeconds(Timer);
   }
}
