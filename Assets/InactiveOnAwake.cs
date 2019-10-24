using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveOnAwake : MonoBehaviour
{
    void Start()
    {
        this.gameObject.SetActive(false);
    }

}
