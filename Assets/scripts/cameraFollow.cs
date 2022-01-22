using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public Transform character;

    Vector3 oldPosition;
    Vector3 newPosition;
    Vector3 offset;
    float lerp;
    bool stopFollow;

    // Start is called before the first frame update
    void Start()
    {
        
        oldPosition = transform.position;
        offset = character.position - transform.position;
        lerp = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
       
        newPosition = character.position - offset;
        if (oldPosition != newPosition )
        {
            lerp = 0;
            
        }
            

        if (lerp<1 && !stopFollow)
        {
            lerp += Time.deltaTime;
            
            transform.position = Vector3.Lerp(oldPosition,newPosition,lerp);

        }

        oldPosition = transform.position;

        if (character.position.y<0)
        {
            stopFollow = true;
        }
        
    }
}
