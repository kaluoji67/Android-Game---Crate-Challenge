using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class envSpawner : MonoBehaviour
{
    public List<GameObject> sections;
    public float zpos;
    GameObject sectionToSpawn;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnEnv()
    {
        /*
        sectionToSpawn = sections[0].transform;
        sections.RemoveAt(0);

        sectionToSpawn.position = new Vector3(sectionToSpawn.position.x, sectionToSpawn.position.y, sectionToSpawn.position.z + zpos);
        sections.Add(sectionToSpawn.gameObject);
        */
     
        sectionToSpawn =Instantiate(sections[0], new Vector3(sections[0].transform.position.x, sections[0].transform.position.y, sections[0].transform.position.z + zpos), Quaternion.identity);
        Destroy(sections[0]);
        sections.RemoveAt(0);
        sections.Add(sectionToSpawn);
    }
}
