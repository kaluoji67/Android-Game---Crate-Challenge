using System.Collections.Generic;
using UnityEngine;

public class spawnCrates : MonoBehaviour
{
    public int levelSize;

    public int crateSize;
    private int startingHeight;
    private int ArraySet;
    private int previousLevelSize;

    public Transform crate;
    public List<Transform[]> crates;

    public IkFootSolver LeftLeg;
    
    bool spawnNew;
    
    // Start is called before the first frame update
    void Start()
    {
        crates = new List<Transform[]>();
        ArraySet = 1;
        previousLevelSize = 0;
        startingHeight = 1;

        spawnSomeCrates(startingHeight,levelSize);
        spawnSomeCrates(startingHeight, levelSize);
        
    }


// Update is called once per frame
    void Update()
    {
        if (crates.Count - LeftLeg.crateIndex < 10)
        {
            spawnSomeCrates(startingHeight, levelSize);
        }
    }

    void spawnSomeCrates(int startHeight, int levelSiz)
    {
        int count = 0;
        while (count < levelSiz)
        {

            Transform[] crateArr = new Transform[startHeight];
            for (int y = 0; y < startHeight; y++)
            {
                float extra;
                extra = Random.Range(-0.07f,0.07f);
                crateArr[y] = Instantiate(crate, new Vector3(0 + extra, (y + 0.5f) * crateSize, count + (previousLevelSize)), Quaternion.identity).transform;
                crateArr[y].GetComponent<Rigidbody>().isKinematic=true;

                //set Parent
                if (y != 0)
                    crateArr[y].SetParent(crateArr[0]);
            }
            crates.Add(crateArr);

            if (count < levelSiz / 2)
            {
                startHeight++;
            }
            else
            {
                startHeight--;
            }

            count++;
        }

        previousLevelSize += levelSize;
        startingHeight++;
        ArraySet++;
        levelSize = levelSize + 2;

    }



}
