using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class assignGravity : MonoBehaviour
{
    public gameControllerScript gameController;
    Rigidbody rb;
    float y;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        y = rb.velocity.y;
    }

    // Update is called once per frame
    void Update()
    {
        //rb.velocity = new Vector3(rb.velocity.x, -10f, rb.velocity.z);
    }
}
