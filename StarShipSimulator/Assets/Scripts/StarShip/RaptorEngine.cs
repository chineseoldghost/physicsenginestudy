using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaptorEngine : MonoBehaviour
{
    GameObject mRocketGO;
    public GameObject mEffectGO;
    Rigidbody mBodyRocket;
    Vector3 mAttachPos;
    float mPower = 230* 9806.65f;//1 tf = 9806.65 N
    bool mbWorking = true;
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
        if (mbWorking)
        {
            if (mBodyRocket != null)
                mBodyRocket.AddForceAtPosition(mPower * Time.fixedDeltaTime * mRocketGO.transform.up, mAttachPos, ForceMode.Impulse);
        }
    }

    public void SetWorking(bool bWorking)
    {
        mbWorking = bWorking;
        mEffectGO.SetActive(bWorking);
    }
}
