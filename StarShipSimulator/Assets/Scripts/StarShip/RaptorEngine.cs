using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaptorEngine : MonoBehaviour
{
    GameObject mRocketGO;
    Rigidbody mBodyRocket;
    Vector3 mAttachPos;
    float mPower = 230* 9806.65f;//1 tf = 9806.65 N
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttackToRocket(GameObject rocketGO,Vector3 attackPos)
    {
        mRocketGO = rocketGO;
        mBodyRocket = mRocketGO.GetComponent<Rigidbody>();
        mAttachPos = attackPos;
    }

    public void FixedUpdate()
    {
      
        if (mBodyRocket != null)
            mBodyRocket.AddForceAtPosition(mPower*Time.fixedDeltaTime*mRocketGO.transform.up,mAttachPos,ForceMode.Impulse);
    }
}
