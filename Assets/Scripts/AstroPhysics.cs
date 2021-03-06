using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AstroPhysics : MonoBehaviour
{
    
    [SerializeField] public double ThisRealMass;
    [SerializeField] Vector3 thisVelUnity;
    [SerializeField] public bool Active = true;
    public APParent APParentObj;
    bool CollisionOnlyOnce = false;
    public int ID;
    public float OrbitalInclination, ThisSolarMass, DistanceFromPlanet, UnityDiameter;
    public double RealDiameter;
    Vector3 AccelerationInDirection;
    Vector3d AccelerationInDirectionDouble;
    double AccelerationUnityDouble;
   

    //Measurements in masses in solar units, distances in parsecs, velocities in km/s
    // 1 parsec = 206,000 au = 3.086e16 m
    // 1 solar unit 1.98847e30 kg
    // 1 km/s = 1000 m/s
    //converting unity distance to a real life distance  10 units = 1 au = 1.4987e11

    void Start()
    {
        APParentObj = FindObjectOfType<APParent>();
        ThisSolarMass = RealMassToUnity(ThisRealMass);
    }

    
    void Update()
    {
        
        List<AstroPhysics> PhysObjs = APParentObj.APObjs;
        if ((Active) && (PhysObjs != null))
        {
            AccelerationInDirectionDouble = new Vector3d(0, 0, 0);
            for (int i = 0; i < PhysObjs.Count; i++)// Order n ^2 loop
            {
                AstroPhysics other = PhysObjs[i];
                if (this != other)
                {
                    double DistanceSQUDouble = (new Vector3d(other.GetComponent<Transform>().position) - new Vector3d(this.transform.position)).sqrMagnitude;
                    //force acting on current Gravitational object from given gravitational object
                    AccelerationUnityDouble = AccelerationFromGravity(other, DistanceSQUDouble);
                    AccelerationInDirectionDouble += -AccelerationUnityDouble * (new Vector3d(this.transform.position) - new Vector3d(other.GetComponent<Transform>().position)).normalized;
                }
            }
            AccelerationInDirection = new Vector3((float)AccelerationInDirectionDouble.x, (float)AccelerationInDirectionDouble.y, (float)AccelerationInDirectionDouble.z);
            thisVelUnity += (AccelerationInDirection * Time.smoothDeltaTime);
            this.transform.position = this.transform.position + (thisVelUnity * Time.smoothDeltaTime);
        }      
    }

    void OnCollisionEnter(Collision collision)
    {
        if (this.ID == 0)  
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.GetComponent<AstroPhysics>().ID == 0)
        {
            Destroy(gameObject);
        }
        if ((!CollisionOnlyOnce)&&(!collision.gameObject.GetComponent<AstroPhysics>().CollisionOnlyOnce))
        {
            GameObject other = collision.gameObject;
            float m1, m2, x1, x2;
            Vector3 v1, v2, v1x, v2x, v1y, v2y, x = (this.transform.position - other.transform.position).normalized;

            v1 = thisVelUnity;
            x1 = Vector3.Dot(x, v1);
            v1x = x * x1;
            v1y = v1 - v1x;
            m1 = ThisSolarMass;

            x = x * -1;
            v2 = other.GetComponent<AstroPhysics>().thisVelUnity;
            x2 = Vector3.Dot(x, v2);
            v2x = x * x2;
            v2y = v2 - v2x;
            m2 = other.GetComponent<AstroPhysics>().ThisSolarMass;

            this.SetVelocity(((v1x * ((m1 - m2) / (m1 + m2))) + (v2x * ((2 * m2) / (m1 + m2))) + v1y));
            other.GetComponent<AstroPhysics>().SetVelocity((v1x * ((2 * m1) / (m1 + m2))) + (v2x * ((m2 - m1) / (m1 + m2))) + v2y);
            CollisionOnlyOnce = true;
            other.GetComponent<AstroPhysics>().CollisionOnlyOnce = true;
        }
        else
        {
            CollisionOnlyOnce = false;
            collision.gameObject.GetComponent<AstroPhysics>().CollisionOnlyOnce = false;
        }
    }

    double AccelerationFromGravity(AstroPhysics other, double DistanceSQ)
    {

        return (((double)APParentObj.GetGUnity() * (double)other.ThisSolarMass) /
                        (DistanceSQ));
        /* 
         * find the force acted apon it by the other object using f =G * M * m / r * r
         * to return acceleration we can cancel m out so we only require the other objects mass.
         */
    }
    public float StableOrbitVelocity(float CenterMass, float Distance)
    {
        // this gives avelocity to obtain a circular orbit for the object in question. 
        return Mathf.Sqrt((FindObjectOfType<APParent>().GetGUnity() * CenterMass) / Distance);
    }
    public float GetDistanceUnity(Vector3 other)
    {
        return Vector3.Distance(this.transform.position, other);
    }
    public void AddVelocity(Vector3 vel)
    {
        thisVelUnity = thisVelUnity + vel;
    }
    public void SetVelocity(Vector3 vel)
    {
        thisVelUnity = vel;
    }
    public Vector3 GetVelocityUnity()
    {
        return thisVelUnity;
    }

    public float RealMassToUnity(double RealMass)
    {
        return (float)(RealMass * FindObjectOfType<APParent>().MassScaleU2Kg);
    }
    public double UnityMassToReal (float SolarMass)
    {
        return (double)SolarMass * FindObjectOfType<APParent>().MassScaleKg2U;
    }
    public float RealDistToUnity(double Dist)
    {
        return (float)(Dist * FindObjectOfType<APParent>().DistanceScaleU2M);
    }
    public double UnityDistToReal(float Dist)
    {
        return (double)Dist * FindObjectOfType<APParent>().DistanceScaleM2U;
    }
    public Vector3 RealVelocityToUnity(Vector3 Vel)
    {
       
        Vel = new Vector3 (RealDistToUnity(Vel.x), RealDistToUnity(Vel.y), RealDistToUnity(Vel.z));
        Vel = Vel * APParentObj.TimeScaleU2S;
        return Vel;
    }

    public Vector3 UnityVelocityToReal(Vector3 Vel)
    {
        Vel = new Vector3((float)UnityDistToReal(Vel.x), (float)UnityDistToReal(Vel.y), (float)UnityDistToReal(Vel.z));
        Vel = Vel *APParentObj.TimeScaleS2U;
        return Vel;
    }



    
       
}