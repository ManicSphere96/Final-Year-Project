using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AstroPhysics : MonoBehaviour
{
    [SerializeField] public float ThisRealMass;
    [SerializeField] Vector3 thisVelUnity;
    [SerializeField] public bool Active = true;

    public float OrbitalInclination;

    public float GravConstUnity = 40.48f;

    public float ThisSolarMass;

    float UnityScale = 1;

    float SizeBalloning;

    public float RealDiameter;

    public float UnityDiameter;
    
    float AccelerationUnity;

    float solarMassSize;
    
    Vector3 NormDirection;
    Vector3 AccelerationInDirection;

    //bool Binarystarsystem = false;

    //Measurements in masses in solar units, distances in parsecs, velocities in km/s
    // 1 parsec = 206,000 au = 3.086e16 m
    // 1 solar unit 1.98847e30 kg
    // 1 km/s = 1000 m/s
    //converting unity distance to a real life distance  10 units = 1 au = 1.4987e11

    void Start()
    {
        
        ThisSolarMass = RealMassToUnity(ThisRealMass);
        /*float Unity = diameter * ScaleConst;
        this.transform.localScale = new Vector3(size, size, size);*/
    }

    // Update is called once per frame
    void Update()
    {
        List<AstroPhysics> PhysObjs = this.gameObject.GetComponentInParent<APParent>().APObjs;
        //print(this.gameObject.name);
        if ((Active)&&(PhysObjs != null))
        {
            AccelerationInDirection = new Vector3(0, 0, 0);
            
            for (int i = 0; i < PhysObjs.Count; i++)
            {
                if (!PhysObjs[i].Active)
                {
                    PhysObjs.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < PhysObjs.Count; i++)
            {
                if (this != PhysObjs[i])
                {
                    /*if ((PhysObjs[i].gameObject.GetComponent<Planet>()!=null)&& (this.gameObject.GetComponent<Moon>() != null))
                    {
                        Debug.Log("planet effect on moon = " + Acceleration);
                    }
                    if ((PhysObjs[i].ThisSolarMass > 0.1f) && (this.gameObject.GetComponent<Planet>() != null))
                    {
                        Debug.Log("sun on planet" + Acceleration);
                    }
                    if ((PhysObjs[i].ThisSolarMass > 0.1f) && (this.gameObject.GetComponent<Moon>() != null))
                    {
                        Debug.Log("sun on Moon" + Acceleration);
                    }*/

                    /*
                    Debug.Log(ThisRealMass + "  " + PhysObjs[i].ThisRealMass);
                    Debug.Log(ThisSolarMass + "  " + PhysObjs[i].ThisSolarMass);*/
                    float DistanceU = Vector3.Distance(PhysObjs[i].GetComponent<Transform>().position, this.transform.position);
                    //force acting on current Gravitational object from given gravitational object
                    AccelerationUnity = AccelerationFromGravity(PhysObjs[i], DistanceU);
                    //print(AffectedByGravity(PhysObjs[i].gameObject));
                    Vector3 Direction = this.transform.position - PhysObjs[i].GetComponent<Transform>().position;
                    NormDirection = Vector3.Normalize(Direction);
                    AccelerationInDirection += -AccelerationUnity * NormDirection;
                    //Debug.Log(Acceleration);
                }
            }
            //current velocity is equal to the previous velocity 
            thisVelUnity += (AccelerationInDirection * Time.deltaTime);
            //                  f=ma f/m = a                a=km/s/s a*t = v = km/s 
            // the new position = the old position + the change due to the current velocity
            this.transform.position += (thisVelUnity * Time.deltaTime);
        }
        

    }

    float AccelerationFromGravity(AstroPhysics other, float Distance)
    {
       



        return ((GravConstUnity * other.ThisSolarMass) /
                        (Distance * Distance));
        /* 
         * find the force acted apon it by the other object using f =G * M * m / r * r
         * to return acceleration we can cancel 
         */
    }
    public float GetDistance(AstroPhysics other)
    {
        return Vector3.Distance(this.transform.position, other.transform.position);
    }
    public void AddVelocity(Vector3 vel)
    {
        thisVelUnity = thisVelUnity + vel;
    }
    public Vector3 GetVelocity()
    {
        return thisVelUnity;
    }
    public void SetDistance (float dist)
    {
        this.transform.position.Set(0,0,dist);
    }

    public float RealMassToUnity(double RealMass)
    {
        return (float)(RealMass / 1.989e+30f);
    }
    public double UnityMassToReal (float SolarMass)
    {
        return (double)SolarMass * 1.989e+30;
    }
    public float RealDistToUnity(double Dist)
    {
        return (float)(Dist / 1.4987e10f);
    }
    public double UnityDistToReal(float Dist)
    {
        return (double)Dist * 1.4987e10;
    }
    public Vector3 RealVelocityToUnity(Vector3 Vel)
    {
       
        Vel = new Vector3 (RealDistToUnity(Vel.x), RealDistToUnity(Vel.y), RealDistToUnity(Vel.z));
        Vel = Vel * 1e6f;
        return Vel;
    }

    public Vector3 UnityVelocityToReal(Vector3 Vel)
    {
        Vel = new Vector3((float)UnityDistToReal(Vel.x), (float)UnityDistToReal(Vel.y), (float)UnityDistToReal(Vel.z));
        Vel = Vel / 1e6f;
        return Vel;
    }



    
       
}