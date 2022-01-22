using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ragDollController : MonoBehaviour
{
    public Component[] boneRig;
    // Start is called before the first frame update
    void Start()
    {
        boneRig = gameObject.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in boneRig)
        {
            rb.detectCollisions = false;
            rb.isKinematic = true;
            
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateRagdoll()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        foreach (Rigidbody rb in boneRig)
        {
            
            rb.detectCollisions = true;
            rb.isKinematic = false;
            
        }

        gameObject.GetComponent<Animator>().enabled = false;

        //gameObject.GetComponent<CapsuleCollider>().enabled = true;
        //gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }    

}
