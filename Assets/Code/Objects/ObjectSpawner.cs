using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float SpawnRateUpper = 2f;
    [SerializeField] private float SpawnRateLower = 5f;
    private float Timer = 0f;

    private float SpawnRate;

    [SerializeField] private GameObject Spawned;
    void Start()
    {
        SpawnRate = Random.Range(SpawnRateLower, SpawnRateUpper);
        Timer = SpawnRate;
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if(Timer <= 0)
        {
            Instantiate(Spawned, gameObject.transform.position, Quaternion.identity);
            SpawnRate = Random.Range(SpawnRateLower, SpawnRateUpper);
            Timer = SpawnRate;
        }

    }
}
