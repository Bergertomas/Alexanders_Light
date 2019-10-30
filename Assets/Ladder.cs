using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    [SerializeField]
    bool canClimb = false;
    float speed = 20.0f;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canClimb = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canClimb = false;
        }
    }
    void Update()
    {
        if (canClimb)
        {
            var moveY = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            var moveClimb = new Vector2(0, moveY);
            player.transform.Translate(moveClimb * Time.deltaTime * speed);
        }
    }

}
