using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;

public class RigidbodySim : MonoBehaviour
{
    public struct MassDistribution
    {
        /// <summary>   The center of mass and the orientation to principal axis space. </summary>
        public RigidTransform Transform;

        /// <summary>   Diagonalized inertia tensor for a unit mass. </summary>
        public float3 InertiaTensor;

        /// <summary>   Get the inertia as a 3x3 matrix. </summary>
        ///
        /// <value> The inertia matrix. </value>
        public float3x3 InertiaMatrix
        {
            get
            {
                var r = new float3x3(Transform.rot);
                var r2 = new float3x3(InertiaTensor.x * r.c0, InertiaTensor.y * r.c1, InertiaTensor.z * r.c2);
                return math.mul(r2, math.inverse(r));
            }
        }
    }
     

    /// <summary>   A dynamic rigid body's "hot" motion data, used during solving. </summary>
    public struct MotionVelocity
    {
        /// <summary>  Linear velocity in  World space. </summary>
        public float3 LinearVelocity;
        /// <summary>  Angular velocity in Motion space. </summary>
        public float3 AngularVelocity;
        /// <summary>   The inverse inertia. </summary>
        public float3 InverseInertia;
        /// <summary>   The inverse mass. </summary>
        public float InverseMass;
  

        /// <summary>   Gets a value indicating whether this object has infinite mass. </summary>
        ///
        /// <value> True if this object has infinite mass, false if not. </value>
        public bool HasInfiniteMass => InverseMass == 0.0f;

        /// <summary>   Gets a value indicating whether this object has infinite inertia. </summary>
        ///
        /// <value> True if this object has infinite inertia, false if not. </value>
        public bool HasInfiniteInertia => !math.any(InverseInertia);

        /// <summary>   Gets a value indicating whether this object is kinematic. </summary>
        ///
        /// <value> True if this object is kinematic, false if not. </value>
        public bool IsKinematic => HasInfiniteMass && HasInfiniteInertia;

        /// <summary>   The zero Motion Velocity. All fields are initialized to zero. </summary>
        public static readonly MotionVelocity Zero = new MotionVelocity
        {
            LinearVelocity = new float3(0),
            AngularVelocity = new float3(0),
            InverseInertia = new float3(0),
            InverseMass = 0.0f, 
        };
         
        public void ApplyLinearImpulse(float3 impulse)
        {
            LinearVelocity += impulse * InverseMass;
        }
         
        public void ApplyAngularImpulse(float3 impulse)
        {
            AngularVelocity += impulse * InverseInertia;
        } 
    }
    RigidTransform m_Pose= new RigidTransform();
    MotionVelocity m_MotionVelocity = new MotionVelocity();

    public UnityEngine.Vector3 m_Size= UnityEngine.Vector3.one;
    public float m_Mass = 0;


    public void ApplyLinearImpulse(UnityEngine.Vector3 impulse)
    {
        m_MotionVelocity.ApplyLinearImpulse(impulse);
    }

    public void ApplyAngularImpulse(UnityEngine.Vector3 impulse, UnityEngine.Vector3 applyPos)
    {
        UnityEngine.Vector3 applyImpulse = UnityEngine.Vector3.Cross(impulse, applyPos - transform.position);
        m_MotionVelocity.ApplyLinearImpulse(applyImpulse);
    }

    /// <summary>   Integrate a single transform for the provided velocity and time. </summary>
    ///
    /// <param name="transform">        [in,out] The transform. </param>
    /// <param name="motionVelocity">   The motion velocity. </param>
    /// <param name="timeStep">         The time step. </param>
    public static void Integrate(ref RigidTransform transform, in MotionVelocity motionVelocity, in float timeStep)
    {
        // center of mass
        IntegratePosition(ref transform.pos, motionVelocity.LinearVelocity, timeStep);

        // orientation
        IntegrateOrientation(ref transform.rot, motionVelocity.AngularVelocity, timeStep); 
    }
     
    internal static void IntegratePosition(ref float3 position, float3 linearVelocity, float timestep)
    {
        position += linearVelocity * timestep;
    }
     
    internal static void IntegrateOrientation(ref quaternion orientation, float3 angularVelocity, float timestep)
    {
        quaternion dq = IntegrateAngularVelocity(angularVelocity, timestep);
        quaternion r = math.mul(orientation, dq);
        orientation = math.normalize(r);
    }

    // Returns a non-normalized quaternion that approximates the change in angle angularVelocity * timestep.
    internal static quaternion IntegrateAngularVelocity(float3 angularVelocity, float timestep)
    {
        float3 halfDeltaTime = new float3(timestep * 0.5f);
        float3 halfDeltaAngle = angularVelocity * halfDeltaTime;
        return new quaternion(new float4(halfDeltaAngle, 1.0f));
    }
    // Start is called before the first frame update
    void Start()
    {
        m_Pose.rot = transform.rotation;
        m_Pose.pos = transform.position;

        MassDistribution boxMassDistribution = new MassDistribution
        {
            Transform = m_Pose,
            InertiaTensor = new float3(
            (m_Size.y * m_Size.y + m_Size.z * m_Size.z) / 12.0f,
            (m_Size.x * m_Size.x + m_Size.z * m_Size.z) / 12.0f,
            (m_Size.x * m_Size.x + m_Size.y * m_Size.y) / 12.0f)
        };

        if (m_Mass == 0)
            m_MotionVelocity.InverseMass = 0;
        else
            m_MotionVelocity.InverseMass = math.rcp(m_Mass);

        m_MotionVelocity.InverseInertia = math.rcp(boxMassDistribution.InertiaTensor * m_Mass);

        ApplyLinearImpluse(UnityEngine.Vector3.right, 5);
        ApplyAngularImpluse(UnityEngine.Vector3.right, 2);
    }

 
    int m_FixedUpateCount = 0;
    private void FixedUpdate()
    {
        m_FixedUpateCount++;
        if (m_LinearImpluseFrames > 0)
        {
            m_MotionVelocity.ApplyLinearImpulse(m_LinearImpluse);
            m_LinearImpluseFrames--;

        }

        if (m_AngularImpluseFrames > 0)
        {
            m_MotionVelocity.ApplyAngularImpulse(UnityEngine.Vector3.Cross(transform.rotation * m_AngularImpluse, transform.rotation * m_AngularApplyPosLS));
            m_AngularImpluseFrames--;

        }

        if (m_FixedUpateCount == 50)
        {
            ApplyLinearImpluse(-UnityEngine.Vector3.right, 5);
            ApplyAngularImpluse(-UnityEngine.Vector3.right, 2);
        }
        Integrate(ref m_Pose, m_MotionVelocity, Time.fixedDeltaTime);
        transform.SetPositionAndRotation(m_Pose.pos, m_Pose.rot);
    }

    int m_LinearImpluseFrames = 0;
    UnityEngine.Vector3 m_LinearImpluse;
    int m_AngularImpluseFrames = 0;
    UnityEngine.Vector3 m_AngularImpluse;
    UnityEngine.Vector3 m_AngularApplyPosLS = new UnityEngine.Vector3(0, 0, 0.5f);
 

    void ApplyLinearImpluse(UnityEngine.Vector3 impluse, int continueFrames)
    {
        m_LinearImpluseFrames = continueFrames;
        m_LinearImpluse = impluse;
    }

    void ApplyAngularImpluse(UnityEngine.Vector3 impluse, int continueFrames)
    {
        m_AngularImpluseFrames = continueFrames;
        m_AngularImpluse = impluse;
    }

}
