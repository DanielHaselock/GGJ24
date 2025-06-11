using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] private float movedDistance = 0;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maximumMovingDistance;
    [SerializeField] private Vector3 direction; // 1 as positive direction

    void Update()
    {
        movedDistance += Time.deltaTime * moveSpeed;
        Vector3 temp = new Vector3(transform.position.x + Time.deltaTime * moveSpeed * direction.x, transform.position.y + Time.deltaTime * moveSpeed * direction.y, 0);
        if(movedDistance > maximumMovingDistance)
        {
            direction = -direction;
            movedDistance = 0;
        }
        transform.position = temp;
    }
}
