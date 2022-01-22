using System.Collections;
using UnityEngine;

public class IkFootSolver : MonoBehaviour
{
    [SerializeField] LayerMask terrainLayer = default;
    [SerializeField] Transform body = default;
    [SerializeField] IkFootSolver otherFoot = default;
    [SerializeField] float speed = 1;
    [SerializeField] float stepDistance;
    [SerializeField] float stepLength ;
    [SerializeField] float stepHeight;
    [SerializeField] Vector3 footOffset = default;
    [SerializeField] Transform rayPointer;

    //send this to balancer
    public int crateIndex;

    

    Vector3 oldPosition, currentPosition, newPosition;
    Vector3 oldNormal, currentNormal, newNormal;
    public float lerp;
    MovementControl control;

    int hook;

    string footName;

    private void Start()
    {
        footName = gameObject.name;
        control = body.GetComponent<MovementControl>();
        currentPosition = newPosition = oldPosition = transform.position;
        currentNormal = newNormal = oldNormal = transform.up;
        lerp = 1;
    }

    // Update is called once per frame

    void Update()
    {

        transform.position = currentPosition;
        transform.up = currentNormal;

        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value))
        {           
                newNormal = info.normal;
        }

        // control sets first left move as true
        if (!IsMoving() && control.firstMove && control.globalCounter>2)
        {
            if (control.legToMove.name == footName)
            {
                currentPosition = control.nextCrate.GetChild(hook).transform.position + footOffset;
            }
            else
                currentPosition = control.currentCrate.GetChild(hook).transform.position + footOffset;

        }

    }


    public IEnumerator firstSingleMove()
    {
        control.leftMove = false;
        currentPosition = transform.position;
        newPosition = transform.position + new Vector3(0, 1, 1) + footOffset;
        while (lerp < 0.5f)
        {
            Vector3 tempPosition = Vector3.Lerp(currentPosition, newPosition  , lerp/0.5f);
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * 0.5f;

            transform.position =tempPosition;

            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime ;
            yield return null;
        }

        transform.position = newPosition;
        currentPosition = transform.position;
        
        control.firstMove = true;
        lerp = 1;

        //sent to balancer
        crateIndex = control.globalCounter - 1;
    }

    public IEnumerator legStepUp()
    {
        float stepH = stepHeight;
        if (control.stepMidH)
            stepH += 0.3f;

        currentPosition = transform.position;

        // hook to correct side
        hook = (control.leftMove) ? 1 : 2;
        

        newPosition = control.nextCrate.GetChild(hook).transform.position + footOffset;
        while (lerp < 1)
        {
            Vector3 tempPosition = Vector3.Lerp(currentPosition, newPosition, lerp);
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepH;

            transform.position = tempPosition;

            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime;
            yield return null;
        }
        //activate rigid bodies




        transform.position = newPosition;
        currentPosition = transform.position;
        control.leftMove = !control.leftMove;
        control.globalCounter++;

        lerp = 1;

        //sent to balancer
        crateIndex = control.globalCounter - 1;
        yield return null;
    }

    public IEnumerator legStepMid()
    {
        currentPosition = transform.position;
        int i = (control.leftMove) ? 1 : 2;

        newPosition = control.nextCrate.GetChild(i).transform.position + footOffset;
        while (lerp < 1)
        {
            Vector3 tempPosition = Vector3.Lerp(currentPosition, newPosition, lerp);
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            transform.position = tempPosition;

            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime;
            yield return null;
        }

        transform.position = newPosition;
        currentPosition = transform.position;
        control.leftMove = !control.leftMove;
        control.globalCounter++;

        yield return null;
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.5f);
    }



    public bool IsMoving()
    {
        return lerp < 1;
    }



}
