using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class balancer : MonoBehaviour
{
    [Header("From other Scripts")]
    public MovementControl control;
    public balancer otherBalancer;
    public spawnCrates spawner;
    public IkFootSolver leg;
    public gameControllerScript gameController;
    Transform[] crateArray;
    

    //needed for progress
    public float randomness;
    float forceIntensity;
    float forceFrequency;

    [Header("others")]

    public Image footUI;
    public float footToBalScale;
    public float balToFootScale;
    public float speed;
    public float balanceSpeed;

    private string footUIName;
    public bool clicked;
    public bool dragged;
    bool sameTouch;

    bool canControlUI;

    private Vector3 pointA;
    private Vector3 pointB;

    private Vector2 start;

    public float velocityThreshold;

    private Vector2 previousVelocity;

    public float crateDamp;


    Rigidbody2D rb;
    Rigidbody balancerRB;

    Vector3 oldUIPos;
    Vector3 newUIPos;
    float lerp;
    Vector2 dir;
    float height;

    float oldCrate;
    public float newCrate;

    bool setInitial;

    public Joystick joystick;
    bool swiping;

    float x;
    float y;

    int half;

    [Header("CONTROLS")]
    
    public float displaceRotatation;

    public float returnRotatation;

    int xRotateDirection;
    float zRotateDirection;

    // Start is called before the first frame update
    void Start()
    {
        //balancerRB = GetComponent<Rigidbody>();
        //rb = footUI.GetComponent<Rigidbody2D>();

        canControlUI = false;
        footUIName = footUI.name;

        oldUIPos = new Vector3(footUI.GetComponent<RectTransform>().anchoredPosition.x, 0, footUI.GetComponent<RectTransform>().anchoredPosition.y);

        oldCrate = leg.crateIndex;
    }

    // Update is called once per frame
    void Update()
    {

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            swiping = true;
        }
        else
            swiping = false;


        #region oldcode

        //////////////////////////////////////////////////   stop runners
        newCrate = leg.crateIndex;
        if (newCrate != oldCrate)
        {
            /////////////////////////////////////////////////////////////////////////IMPORTANT REDUCE DIPLACE ROTATION WITH HEIGHT
            ///
            //displaceRotatation += -0.014f;
            //returnRotatation += -0.016f;

            crateArray = spawner.crates[leg.crateIndex];

            displaceRotatation = Mathf.Rad2Deg * Mathf.Atan(0.18f / crateArray.Length);
            returnRotatation = Mathf.Rad2Deg * Mathf.Atan(0.15f / crateArray.Length);
            //instead of lerping control to crate, move crate to control
            
            //canControlUI = true;

            Vector2 uiStepAway = footUI.transform.localPosition;
            

            
            float rotateZ = Mathf.Rad2Deg * Mathf.Atan((footUI.transform.localPosition.x/47f) / crateArray.Length);
            //float rotateX = Mathf.Rad2Deg * Mathf.Atan((footUI.transform.localPosition.z / 80f) / crateArray.Length);

            crateArray[0].Rotate(0,0, -rotateZ, Space.World);

            newUIPos = crateArray[crateArray.Length - 1].transform.position;
            newUIPos.z = newUIPos.z - leg.crateIndex;
            newUIPos.y = 0;
            newUIPos = newUIPos * balToFootScale;
            if (oldUIPos != newUIPos)
            {

                lerp = 0;
                if (lerp < 1f)
                {

                    footUI.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(new Vector3(oldUIPos.x, oldUIPos.z, 0), new Vector3(newUIPos.x, newUIPos.z, 0), lerp / 1f);
                    lerp += Time.deltaTime;
                }

            }
            oldUIPos =  footUI.GetComponent<RectTransform>().anchoredPosition = new Vector3(newUIPos.x, newUIPos.z, 0);




            //IMPORTANT
            //canControlUI = false;

            /*
            for (int i = crateArray.Length/2; i < crateArray.Length; i++)
            {
                crateArray[i].GetComponent<Rigidbody>().AddForce(new Vector3(uiStepAway.x, 0, uiStepAway.y) * 25f * (i + 1) / (crateArray.Length), ForceMode.Impulse);


            }

            lerp = 0;
            if (lerp < 1f && crateArray[crateArray.Length-1].GetComponent<Rigidbody>().velocity.magnitude<1f)
            {

                footUI.transform.localPosition = Vector2.Lerp(new Vector2(oldUIPos.x, oldUIPos.z), new Vector2(newUIPos.x, newUIPos.z), lerp / 1f);
                lerp += Time.deltaTime;
            }
            
            
            if (lerp > 1)
                canControlUI = false;
            */


            
        }
        oldCrate = newCrate;


        crateArray = spawner.crates[leg.crateIndex];
        height = crateArray.Length;


        ////////////////////////////////////////////////////////////// SWIPING BEGINS
        if (swiping && control.firstMove)
        {
            crateArray = spawner.crates[leg.crateIndex];
            crateArray[0].Rotate(0, 0, returnRotatation * -joystick.Horizontal, Space.World);
            
            /*
            if (joystick.Vertical > 0)
            {
                int i = 1;

                while (i < 3)
                {
                    spawner.crates[leg.crateIndex + i][0].Rotate(returnRotatation * joystick.Vertical / (i + 1), 0, 0 , Space.World);
                    i++;
                }

            }
            if (joystick.Vertical < 0)
            {
                int i = 2;
                while (i > 0)
                {
                    if (leg.crateIndex - i >= 0)
                    {
                        spawner.crates[leg.crateIndex - i][0].Rotate(returnRotatation * joystick.Vertical / (i + 1), 0, 0, Space.World);
                        i--;
                    }
                    
                    
                }

            }
            */


            /*
            for (int i = crateArray.Length / 2; i < height; i++)
            {
               //crateArray[i].GetComponent<Rigidbody>().AddForce(new Vector3(joystick.Horizontal, 0, joystick.Vertical) * (i + 1) / height * crateDamp);


            }
            */
            // make UI follow 
            newUIPos = crateArray[crateArray.Length - 1].transform.position;
            newUIPos.z = newUIPos.z - leg.crateIndex;
            newUIPos.y = 0;
            newUIPos = newUIPos * balToFootScale;
            if (oldUIPos != newUIPos)
            {

                lerp = 0;
                if (lerp < 0.1f)
                {

                    footUI.transform.localPosition = Vector2.Lerp(new Vector2(oldUIPos.x, oldUIPos.z), new Vector2(newUIPos.x, newUIPos.z), lerp / 0.1f);
                    lerp += Time.deltaTime;
                }

            }
            footUI.transform.localPosition = newUIPos;
            oldUIPos = newUIPos;



        }

       
        // for balancer to move foot GUI
        if ((!swiping) && newCrate >= 2 && !leg.IsMoving() && (oldCrate ==newCrate))
        {
            if (!canControlUI)
            {
                forceFrequency = leg.crateIndex;
                StartCoroutine(randomForce());
            }
            
            Vector3 temp2 = new Vector3(transform.localPosition.x, 0,transform.localPosition.z);


            if (control.firstMove)
            {
                // get height to add intensity


                //Vector2 temp = new Vector2(transform.localPosition.x, transform.localPosition.z) * balToFootScale;
                //oldUIPos = footUI.transform.localPosition;

                newUIPos = crateArray[crateArray.Length - 1].transform.position;
                newUIPos.z = newUIPos.z - leg.crateIndex;
                newUIPos.y = 0;
                newUIPos = newUIPos * balToFootScale;
                if (oldUIPos != newUIPos)
                {

                    lerp = 0;
                    if (lerp < 1f)
                    {

                        footUI.transform.localPosition = Vector2.Lerp(new Vector2(oldUIPos.x, oldUIPos.z), new Vector2(newUIPos.x, newUIPos.z), lerp / 1f);
                        lerp += Time.deltaTime;
                    }

                }
                footUI.transform.localPosition = newUIPos;
                oldUIPos = newUIPos;

            }


        }

        
        

        if (gameController.gameOverBool) {
        
            crateArray = spawner.crates[leg.crateIndex];

        }

        #endregion
    }

    public IEnumerator randomForce()
    {
        canControlUI = true;
               
        while (!swiping )
        {
            xRotateDirection = Random.Range(0, 2) == 0 ? -1 : 1;

            zRotateDirection = 0f;
            crateArray[0].Rotate(displaceRotatation * zRotateDirection, 0, displaceRotatation * xRotateDirection, Space.World);

            #region
            //rotate boxes before and after

            //No need to cycle through
            /*
            if (zRotateDirection>0)
            {
                int i = 1;
                while (i<3)
                {
                    spawner.crates[leg.crateIndex + i][0].Rotate(displaceRotatation * zRotateDirection / (i + 1), 0, 0, Space.World);
                    i++;
                }
                
            }
            if (zRotateDirection < 0)
            {
                int i = 2;
                while (i > 0)
                {
                    if (leg.crateIndex - i>=0)
                    {
                        spawner.crates[leg.crateIndex - i][0].Rotate(displaceRotatation * zRotateDirection / (i + 1), 0, 0, Space.World);
                        i--;
                    }
                    
                }

            }

            x = Random.Range(-1.0f, 1.0f);
            y = Random.Range(-1.0f, 1.0f);
            for (int i = 0; i < crateArray.Length; i++)
            {

                Vector3 v = new Vector3(x, 0, y) ;
                v = v.normalized;

                //crateArray[i].Translate(v * Time.deltaTime * (i + 1) * displaceDistance);
                
                

                //used this
                //crateArray[i].GetComponent<Rigidbody>().AddForce(v * (i + 1) * height * speed * Time.deltaTime, ForceMode.Impulse);
            }
            */

            //balancerRB.AddForce(new Vector3(x, 0, y) * balanceSpeed * forceFrequency * height);

            #endregion
            forceFrequency = 1.5f - (leg.crateIndex * 0.015f);
            
            if (forceFrequency <= 0.5f)
                forceFrequency = 0.5f;
           
            yield return new WaitForSeconds(forceFrequency);
        }

        canControlUI = false;
        yield return null;
    }

    public void randomnessCalc()
    {
        Vector3 foot1 = footUI.transform.localPosition;
        Vector3 foot2 = otherBalancer.footUI.transform.localPosition;

        float r = (foot1 - foot2).magnitude;
        r = Mathf.Abs(r);

        randomness = r;
        

    }

    /*
    private void OnDestroy()
    {
        crateArray = spawner.crates[leg.crateIndex];
        for (int i = 0; i < crateArray.Length; i++)
        {

            crateArray[i].GetComponent<Rigidbody>().velocity = new Vector3(0,-10f, 0) * 5f;
        }
    }
    */
    
    public void clickEvent()
    {
        clicked = true;
    }
    public void dragEvent()
    {
        dragged = true;
    }

    public void dragStop()
    {
        dragged = clicked = false;
        
    }

}
