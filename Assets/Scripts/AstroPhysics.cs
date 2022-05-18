using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AstroPhysics : MonoBehaviour
{
    [SerializeField] public float ThisRealMass;
    [SerializeField] Vector3 thisVelUnity;
    [SerializeField] public bool Active = true;
    public int ID;
    public float OrbitalInclination;

    //public float GravConstUnityA = 00040.48f;
    float GravConstUnityA = 0.004048f;

    public float ThisSolarMass;
    public float DistanceFromPlanet;

    float UnityScale = 1;

    float SizeBalloning;

    public float RealDiameter;

    public float UnityDiameter;
    
    float AccelerationUnity;

    float solarMassSize;
    
    Vector3 NormDirection;
    Vector3 AccelerationInDirection;
    Vector3d thisVelUnityDouble;
    Vector3d NormDirectionDouble;
    Vector3d AccelerationInDirectionDouble;
    double AccelerationUnityDouble;


    //Measurements in masses in solar units, distances in parsecs, velocities in km/s
    // 1 parsec = 206,000 au = 3.086e16 m
    // 1 solar unit 1.98847e30 kg
    // 1 km/s = 1000 m/s
    //converting unity distance to a real life distance  10 units = 1 au = 1.4987e11

    void Start()
    {
        ThisSolarMass = RealMassToUnity(ThisRealMass);
        thisVelUnityDouble = new Vector3d(thisVelUnity);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        
        List<AstroPhysics> PhysObjs = this.gameObject.GetComponentInParent<APParent>().APObjs;
        if ((Active) && (PhysObjs != null))
        {
            AccelerationInDirection = new Vector3(0, 0, 0);
            AccelerationInDirectionDouble = new Vector3d(0, 0, 0);

            for (int i = 0; i < PhysObjs.Count; i++)
            {
                if (this != PhysObjs[i])
                {
#if flase
                    AstroPhysics other = PhysObjs[i];
                    double DistanceU = Vector3d.Distance(new Vector3d(other.GetComponent<Transform>().position), new Vector3d(this.transform.position));
                    //force acting on current Gravitational object from given gravitational object
                    double Neumarator = (double)GravConstUnityA * (double)other.ThisSolarMass;
                    double Denominator = DistanceU * DistanceU;
                    AccelerationUnityDouble = Neumarator / Denominator;
                    //print(AffectedByGravity(PhysObjs[i].gameObject));
                    Vector3d Direction = new Vector3d(this.transform.position) - new Vector3d(PhysObjs[i].GetComponent<Transform>().position);
                    NormDirectionDouble = Vector3d.Normalize(Direction);
                    AccelerationInDirectionDouble += -AccelerationUnityDouble * NormDirectionDouble;
#else

                    AstroPhysics other = PhysObjs[i];
                    double DistanceU = (new Vector3d(other.GetComponent<Transform>().position)- new Vector3d(this.transform.position)).sqrMagnitude;
                    //force acting on current Gravitational object from given gravitational object
                    double Neumarator = (double)GravConstUnityA * (double)other.ThisSolarMass;
                    double Denominator = DistanceU;
                    AccelerationUnityDouble = Mathd.Abs(Neumarator / Denominator);
                    //print(AffectedByGravity(PhysObjs[i].gameObject));
                    Vector3d diffPositionDouble = new Vector3d(this.transform.position) - new Vector3d(PhysObjs[i].GetComponent<Transform>().position);
                    AccelerationInDirectionDouble += -AccelerationUnityDouble * diffPositionDouble.normalized;
                    //Debug.Log("ObjectID = ," + this.ID + ",  other ID = ," + other.ID + ", Acceleration = ," + AccelerationUnityDouble);
                    float DistanceSQU = (PhysObjs[i].GetComponent<Transform>().position-this.transform.position).sqrMagnitude;
                    //force acting on current Gravitational object from given gravitational object
                    AccelerationUnity = AccelerationFromGravity(PhysObjs[i], DistanceSQU);
                    //print(AffectedByGravity(PhysObjs[i].gameObject));
                    Vector3 diffPosition = this.transform.position - PhysObjs[i].GetComponent<Transform>().position;
                    AccelerationInDirection += -AccelerationUnity * diffPosition.normalized;
#endif
                }
            }
            //current velocity is equal to the previous velocity 
            thisVelUnity += (AccelerationInDirection * Time.smoothDeltaTime);
            //                  f=ma f/m = a                a=km/s/s a*t = v = km/s 
            // the new position = the old position + the change due to the current velocity    
            Vector3 NewPosition = this.transform.position + (thisVelUnity * Time.smoothDeltaTime);


            thisVelUnityDouble += (AccelerationInDirectionDouble * (double)Time.smoothDeltaTime);
            Vector3d NewPositionDouble = new Vector3d(this.transform.position) + (thisVelUnityDouble * (double) Time.smoothDeltaTime); ;
            this.transform.position = new Vector3((float)NewPositionDouble.x, (float)NewPositionDouble.y, (float)NewPositionDouble.z);
            //this.transform.position = new Vector3((float)NewPosition.x, (float)NewPosition.y, (float)NewPosition.z);
        }
    }
    

    float AccelerationFromGravity(AstroPhysics other, float DistanceSQ)
    {
       



        return ((GravConstUnityA * other.ThisSolarMass) /
                        (DistanceSQ));
        /* 
         * find the force acted apon it by the other object using f =G * M * m / r * r
         * to return acceleration we can cancel m out so we only require the other objects mass.
         */
    }
    public float GetDistanceUnity(AstroPhysics other)
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