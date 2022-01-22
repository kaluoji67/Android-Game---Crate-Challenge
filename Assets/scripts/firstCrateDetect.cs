using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstCrateDetect : MonoBehaviour
{
    public gameControllerScript gameController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "crateTag")
        {
            if (!gameController.gameOverBool)
            {
                gameController.gameOverBool = true;
                gameController.gameOver();
            }
            
        }
    }
}
