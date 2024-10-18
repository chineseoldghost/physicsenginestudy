using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarShip : MonoBehaviour
{
    public RaptorEngine[] mEngine_0;  
    public RaptorEngine[] mEngine_1;
    public RaptorEngine[] mEngine_2; 

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
            mEngine_0[i].AttackToRocket(this.gameObject, mEngine_0[i].transform.position);
        for (int i = 0; i < 10; i++)
            mEngine_1[i].AttackToRocket(this.gameObject, mEngine_1[i].transform.position);
        for (int i = 0; i < 20; i++)
            mEngine_2[i].AttackToRocket(this.gameObject, mEngine_2[i].transform.position);
    }

    // Update is called once per frame
    void Update()
    {

    }
 
    void SimulateEngineBroken(int iEngineLevel,int levelID)
    {
        switch (iEngineLevel)
        {
            case 0:
                if(levelID >=0 && levelID <3)
                {
                    mEngine_0[levelID].SetWorking(false);
                }
                break;
            case 1:
                if (levelID >= 0 && levelID < 10)
                {
                    mEngine_1[levelID].SetWorking(false);
                }
                break;
            case 2:
                if (levelID >= 0 && levelID < 20)
                {
                    mEngine_2[levelID].SetWorking(false);
                }
                break;
            default:
                break;
        }
        
    }

    public void SimulateEngine12Broken()
    {
        SimulateEngineBroken(1, 2);
    }
}
