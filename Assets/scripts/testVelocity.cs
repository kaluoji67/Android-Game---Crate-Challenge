using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testVelocity : MonoBehaviour
{
    bool moving;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        moving = true;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (true)
            rb.velocity = Vector3.forward * 3;

        if (Input.GetMouseButtonDown(0))
        {
            moving = false;
            rb.velocity = Vector3.zero;
        }
    }
}
