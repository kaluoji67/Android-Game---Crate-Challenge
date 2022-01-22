using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementControl : MonoBehaviour
{
    [Header("Character Fields")]
    public envSpawner EnvSpawner;
    private Rigidbody rb;
    public float speed;
    public float lerp;
    public bool firstMove;

    public int globalCounter = 0;
    public spawnCrates spawner;
    public Transform nextCrate;
    public Transform currentCrate;

    public Transform baseCrate;
    public Transform previousCrate;

    public gameControllerScript gameController;

    [Header("Foot Target Fields")]

    [SerializeField] IkFootSolver LeftFootTarget = default;
    [SerializeField] IkFootSolver RightFootTarget = default;
    public Transform spineAim;



    Transform fixedLeg;
    public Transform legToMove;

    public bool leftMove;
    public bool firstLeftMove;

    //offset for body standing relative to leg
    private Vector3 bodyToLegOffset;

    private Vector3 bodyToCrateOffset;

    public bool stepUp;
    public bool stepDown;
    public bool stepMidL;
    public bool stepMidH;

    public Vector3 originalSpinePos;

    //balancer
    public balancer Leftbalancer;
    public balancer Rightbalancer;


    //crate arrays
    public Transform[] nextCrateArray;
    public Transform[] currentCrateArray;
    public Transform[] previousCrateArray;

    float failPosition;

    //Animator
    private Animator anim;
    bool tipping;
    bool playingTipping;

    bool milding;
    bool mildplaying;

    float Timer = 1f;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        int startingAnim = Random.Range(0,2);

        if (startingAnim == 0)
            anim.Play("idleDance",-1,0f);
        else
            anim.Play("idleSwing", -1, 0f);



        //offset for body standing relative to leg
        bodyToLegOffset = transform.position - ((LeftFootTarget.transform.position + RightFootTarget.transform.position)/2);

        bodyToCrateOffset = ((currentCrate.GetChild(2).transform.position + currentCrate.GetChild(1).transform.position) / 2) - transform.position ;
        originalSpinePos = spineAim.localPosition;

        firstMove = false;
        leftMove = true;
        lerp = 1;
        failPosition=0.65f;
        //rb = GetComponent<Rigidbody>();

        milding = true;
    }

    

    // Update is called once per frame
    void Update()
    {
        
        
        if (Input.GetMouseButtonDown(0) && !gameController.gameOverBool && (!LeftFootTarget.IsMoving()) && (!RightFootTarget.IsMoving()) )
        {
            
            if (IsPointerOverUIObject())
            {
                return;
            }
            
            if (!firstMove && leftMove) 
            {

                anim.Play("mildBalance", -1, 0f);
                firstLeftMove = true;
                lerp = 0;
                StartCoroutine(bodyFirstMovement());

                LeftFootTarget.GetComponent<IkFootSolver>().lerp = 0;
                StartCoroutine(LeftFootTarget.GetComponent<IkFootSolver>().firstSingleMove() );
                
                //firstMove = true;

                //left leg moved time for right move
                //leftMove = false;

                // first leg move completed
                firstLeftMove = false;
                globalCounter++;

                //damn
                legToMove = RightFootTarget.gameObject.transform;
                
                
            }

            // After firstLegMove, handle other button clicks
            else
            {
                if (firstMove && !LeftFootTarget.IsMoving() && !RightFootTarget.IsMoving()) 
                {
                    //calculate randomness
                    

                    // set the booleans

                    legToMove = (leftMove) ? LeftFootTarget.gameObject.transform : RightFootTarget.gameObject.transform;

                    ///////
                    Transform[] SnextCrateArray = spawner.crates[globalCounter + 1];
                    
                    ///////
                    nextCrateArray = spawner.crates[globalCounter];
                    nextCrate = nextCrateArray[nextCrateArray.Length - 1];
                    
                    currentCrateArray = spawner.crates[globalCounter - 1];
                    currentCrate = currentCrateArray[currentCrateArray.Length - 1];

                    
                    previousCrate = baseCrate;

                    if (globalCounter >1) 
                    {
                        previousCrateArray = spawner.crates[globalCounter - 2];
                        previousCrate = previousCrateArray[previousCrateArray.Length - 1];
                    }

                    if (nextCrate.position.y - previousCrate.position.y > 1.5f)
                    {
                        stepUp = true;
                        stepDown = stepMidL = stepMidH = false;
                    }
                    else if ((nextCrate.position.y - previousCrate.position.y < 0.1f) && (previousCrate.position.y - nextCrate.position.y < 0.1f))
                    {
                        if (currentCrate.position.y < nextCrate.position.y)
                        {
                            stepMidL = true;
                            stepMidH = false;
                        }
                        else if (currentCrate.position.y > nextCrate.position.y)
                        {
                            stepMidH = true;
                            stepMidL = false;
                        }
                            
                        
                        stepUp = stepDown = false;
                        
                    }
                    else
                    {
                        stepDown = true;
                        stepUp = stepMidL = stepMidH = false;
                    }
                    //////////////////////////////////////////////////////////////////////////////////////
                    //Reset Rtation
                    for (int i = 0; i < previousCrateArray.Length; i++)
                    {
                       //previousCrateArray[0].rotation =Quaternion.RotateTowards(previousCrateArray[0].rotation, Quaternion.Euler(0,0,0),20f ) ;
                    }



                    ////////////////////////////////////////////
                    /////wake up
                    ////
                    /*
                    for (int i = 0; i < previousCrateArray.Length; i++)
                    {

                        previousCrateArray[i].GetComponent<Rigidbody>().isKinematic = false; ;
                    }
                    for (int i = 0; i < nextCrateArray.Length; i++)
                    {

                        nextCrateArray[i].GetComponent<Rigidbody>().isKinematic = false; ;
                    }
                    for (int i = 0; i < SnextCrateArray.Length; i++)
                    {

                        SnextCrateArray[i].GetComponent<Rigidbody>().isKinematic = false; ;
                    }
                    */

                    ///////////////////////////////////////////////////////////////////////////
                    //handle the movements

                    fixedLeg = (!leftMove) ? LeftFootTarget.gameObject.transform : RightFootTarget.gameObject.transform;
                    lerp = 0;
                    legToMove.GetComponent<IkFootSolver>().lerp = 0;
                    StartCoroutine(legToMove.GetComponent<IkFootSolver>().legStepUp());

                    StartCoroutine(bodyStepUp());

                    return;
                    
                }
                
            }

           
        }
       
        /////////////////////////////////////////////////////////// CHECK GAME OVER
        if (firstMove && globalCounter > 1 )
        {
            
            if (
                 //( !(currentCrateArray[0].rotation.eulerAngles.x > 360 - Mathf.Rad2Deg * Mathf.Atan((0.8f / currentCrateArray.Length))) &&
                 //!(currentCrateArray[0].rotation.eulerAngles.x < Mathf.Rad2Deg * Mathf.Atan((0.8f / currentCrateArray.Length))) )
                //|| 
                (!(currentCrate.rotation.eulerAngles.z > 285) && !(currentCrate.rotation.eulerAngles.z < 75))

                 //|| (!(nextCrateArray[0].rotation.eulerAngles.x > 360 - Mathf.Rad2Deg * Mathf.Atan((0.8f / nextCrateArray.Length))) &&
                 //!(nextCrateArray[0].rotation.eulerAngles.x < Mathf.Rad2Deg * Mathf.Atan((0.8f / nextCrateArray.Length))) )
                || 
                (!(nextCrate.rotation.eulerAngles.z > 285) && !(nextCrate.rotation.eulerAngles.z < 75))
            )
            {

                if (!gameController.gameOverBool)
                {
                    gameController.gameOverBool = true;
                    Debug.Log("rotation");
                    gameController.gameOver();
                }


            }

            if (currentCrate.position.x > failPosition
                || currentCrate.position.x < -failPosition
                || nextCrate.position.x > failPosition
                || nextCrate.position.x < -failPosition


                //|| currentCrate.position.z > globalCounter - 2 + failPosition
                //|| currentCrate.position.z < globalCounter - 2 - failPosition
                //|| nextCrate.position.z > globalCounter - 1 + failPosition
                //|| nextCrate.position.z < globalCounter - 1 - failPosition
                )
            {
                if (!gameController.gameOverBool)
                {
                    gameController.gameOverBool = true;
                    Debug.Log("position");
                    gameController.gameOver();
                }
            }

        }

        

        if (  (
                Leftbalancer.footUI.transform.localPosition.x >  10f 
            ||  Leftbalancer.footUI.transform.localPosition.z >  10f 
            || Rightbalancer.footUI.transform.localPosition.x >  10f 
            || Rightbalancer.footUI.transform.localPosition.z >  10f
            ||  Leftbalancer.footUI.transform.localPosition.x < -10f
            ||  Leftbalancer.footUI.transform.localPosition.z < -10f
            || Rightbalancer.footUI.transform.localPosition.x < -10f
            || Rightbalancer.footUI.transform.localPosition.z < -10f
            
            ) && milding)
        {
            milding = false;
            anim.SetBool("heavyBalance", true);
            anim.SetBool("mildBalance", false);
        }
        
        else
        {
            milding = true; ;
            anim.SetBool("mildBalance", true);
            anim.SetBool("heavyBalance", false);
        }


             

    }


    IEnumerator bodyFirstMovement() 
    {
        //rb.isKinematic = true;
        Vector3 currentPos = transform.position;
        Vector3 newPos = transform.position + (Vector3.forward * speed);
        while (lerp<0.5f) 
        {
            
            transform.position = Vector3.Lerp(currentPos, newPos, lerp/0.5f );
            lerp += Time.deltaTime;

            yield return null;
        }

        //rb.isKinematic=false;
        yield return null;
    }

    IEnumerator bodyStepUp()
    {

        //rb.isKinematic = true;

        Vector3 currentPos = transform.position;
        Vector3 newPos;

        if (stepUp)
            newPos = transform.position + (new Vector3(0,2,2) * speed);
        else if (stepMidL)
            newPos = transform.position + (new Vector3(0, 0, 0) * speed);
        else if(stepMidH)
            newPos = transform.position + (new Vector3(0, 0, 2) * speed);
        else
            newPos = transform.position + (new Vector3(0, -1, 0) * speed);

        // rotation
        Vector3 newSpinePos = originalSpinePos + new Vector3(0, -0.2f, 0.1f);

        while (lerp < 0.5f)
        {
            
            Vector3 tempPosition = Vector3.Lerp(currentPos, newPos, lerp/0.5f);
            
            transform.position = tempPosition;

            //rotate spine
            if (stepUp || stepMidH)
                spineAim.localPosition = Vector3.Lerp(originalSpinePos, newSpinePos, lerp / 0.5f);


            lerp += Time.deltaTime;

            yield return null;
        }


        transform.position = newPos;

        if (stepUp || stepMidH)
            spineAim.localPosition = newSpinePos;

            lerp = 0f;

        //get the crate to stabilize on
        
        Transform crateToStandOn;
        if (stepUp || stepMidL)
            crateToStandOn = currentCrate;
        else
            crateToStandOn = nextCrate;
        
        newPos = ((crateToStandOn.GetChild(2).transform.position + crateToStandOn.GetChild(1).transform.position) / 2) - bodyToCrateOffset -new Vector3(0,0.2f,0) ;
        currentPos = transform.position;
        
        while (lerp < 0.5f)
        {

            Vector3 tempPosition = Vector3.Lerp(currentPos, newPos, lerp/0.5f);

            if (stepUp || stepMidH)
                spineAim.localPosition = Vector3.Lerp(newSpinePos, originalSpinePos,  lerp/0.5f);

                transform.position = tempPosition;
            lerp += Time.deltaTime;

            yield return null;
        }
        transform.position = newPos;
        spineAim.localPosition = originalSpinePos;
        //rb.isKinematic = false;
        yield return null;
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }


    #region
    //Animation functions
    public void animate(string animationName)
    {
        disableOtherAnims(anim, animationName);
        anim.SetBool(animationName,true);
    }
    private void disableOtherAnims( Animator A, string motion)
    {
        foreach (AnimatorControllerParameter param in A.parameters)
        {
            if (param.name != motion)
            {
                A.SetBool(param.name,false);
            }
        }
       
    }
    #endregion

    void playTippingMethod()
    {
        anim.Play("heavyBalance",-1, 0f);
    }

    
}
