using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private float xSpeed=5;
    [SerializeField]
    private float ySpeed=5;
    [SerializeField]
    private float yOffset = 1;
    //[SerializeField]
    // private float xOffsetWhenRunning = 5;
    [SerializeField]
    private PlayerController player;

    void FixedUpdate()
    {
        //float playerXOffset = player.currentHorizontalSpeed * xOffsetWhenRunning;
        float newX = this.transform.position.x+
            (((player.transform.position.x+ player.CameraXOffset) - this.transform.position.x)
              *xSpeed*Time.deltaTime);
        float newY = this.transform.position.y +
           (((player.transform.position.y+yOffset) - this.transform.position.y)
              * ySpeed * Time.deltaTime);

        transform.position = new Vector3(newX, newY, transform.position.z);
    }
}
