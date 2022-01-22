using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crateDestroyerScript : MonoBehaviour
{
    public Transform character;
    float offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = character.position.z - transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, character.position.z - offset);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "crateTag")
        {
            Destroy(other.gameObject);
        }
    }
}
