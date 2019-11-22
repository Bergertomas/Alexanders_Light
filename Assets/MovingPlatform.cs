
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private const float SMALL_DISTANCE = 0.05f;
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private Transform[] positions;
    private int currentDestination = 0;

    private void Update()
    {
        transform.position = Vector3.MoveTowards
            (transform.position, positions[currentDestination].position, Time.deltaTime * speed);
        if (Vector3.Distance(transform.position, positions[currentDestination].position) < SMALL_DISTANCE)
        {
            //change destination
            currentDestination++;
            if (currentDestination >= positions.Length)
            {
                currentDestination = 0;
            }
        }
    }
}