using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyOfficial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        mRigidBody = GetComponent<Rigidbody>();

        ApplyLinearImpluse(Vector3.right , 5);
        ApplyAngularImpluse(Vector3.right, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int m_FixedUpateCount = 0;
    private void FixedUpdate()
    {
        m_FixedUpateCount++;
        if (m_LinearImpluseFrames>0)
        {
            mRigidBody.AddForceAtPosition(m_LinearImpluse, transform.position, ForceMode.Impulse);
            m_LinearImpluseFrames--;
             
        }

        if (m_AngularImpluseFrames > 0)
        {
            mRigidBody.AddTorque(Vector3.Cross(transform.rotation * m_AngularImpluse, transform.rotation * m_AngularApplyPosLS), ForceMode.Impulse);
            m_AngularImpluseFrames--;

        }

        if (m_FixedUpateCount == 50)
        {
            ApplyLinearImpluse(-Vector3.right , 5);
            ApplyAngularImpluse(-Vector3.right, 2);
        }
    }

    int m_LinearImpluseFrames = 0;
    Vector3 m_LinearImpluse;
    int m_AngularImpluseFrames = 0;
    Vector3 m_AngularImpluse;
    Vector3 m_AngularApplyPosLS = new Vector3(0,0,0.5f);
    Rigidbody mRigidBody;
    
    void ApplyLinearImpluse(Vector3 impluse,int continueFrames)
    {
        m_LinearImpluseFrames = continueFrames;
        m_LinearImpluse = impluse;
    }

    void ApplyAngularImpluse(Vector3 impluse, int continueFrames)
    {
        m_AngularImpluseFrames = continueFrames;
        m_AngularImpluse = impluse;
    }
}
