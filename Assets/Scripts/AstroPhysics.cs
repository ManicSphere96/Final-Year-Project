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

    public float GravConstUnity = 0.004048f;

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
    bool Double = false;
    [SerializeField] Vector3d thisVelUnityDouble;
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
    void Update()
    {
        if (Double == false)
        {
            List<AstroPhysics> PhysObjs = this.gameObject.GetComponentInParent<APParent>().APObjs;
            if ((Active) && (PhysObjs != null))
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

                        if ((ID == 401) && (PhysObjs[i].ID == 4000))
                        {
                            Debug.Log("");
                            Debug.Log("This Position " + this.transform.position.ToString("E8"));
                            Debug.Log("Other Position " + PhysObjs[i].GetComponent<Transform>().position.ToString("E8"));

                            AstroPhysics other = PhysObjs[i];
                            float DistanceU = Vector3.Distance(other.GetComponent<Transform>().position, this.transform.position);
                            //force acting on current Gravitational object from given gravitational object
                            Debug.Log("DistanceU = " + DistanceU);

                            Debug.Log("Grav Const = " + GravConstUnity);
                            Debug.Log("Other Mass in solar masses = " + other.ThisSolarMass);

                            float Neumarator = GravConstUnity * other.ThisSolarMass;
                            float Denominator = DistanceU * DistanceU;
                            Debug.Log("Numerator = " + Neumarator);
                            Debug.Log("Denominator = " + Denominator);
                            AccelerationUnity = Neumarator / Denominator;
                            Debug.Log("Acceleration Unity = " + AccelerationUnity);

                            //AccelerationUnity = AccelerationFromGravity(PhysObjs[i], DistanceU);

                            Vector3 Direction = this.transform.position - PhysObjs[i].GetComponent<Transform>().position;
                            Debug.Log("Direction Not normalized = " + Direction.ToString("E8"));


                            NormDirection = Vector3.Normalize(Direction);
                            Debug.Log("Normalized direction = " + NormDirection.ToString("E8"));
                            AccelerationInDirection += -AccelerationUnity * NormDirection;
                            Debug.Log("Acceleration in direction =" + AccelerationInDirection.ToString("E8"));
                            Debug.Log("**********************************************************************************");
                        }
                        else
                        {
                            float DistanceU = Vector3.Distance(PhysObjs[i].GetComponent<Transform>().position, this.transform.position);
                            //force acting on current Gravitational object from given gravitational object
                            AccelerationUnity = AccelerationFromGravity(PhysObjs[i], DistanceU);
                            //print(AffectedByGravity(PhysObjs[i].gameObject));
                            Vector3 Direction = this.transform.position - PhysObjs[i].GetComponent<Transform>().position;
                            NormDirection = Vector3.Normalize(Direction);
                            AccelerationInDirection += -AccelerationUnity * NormDirection;
                        }
                    }
                }
                //current velocity is equal to the previous velocity 
                thisVelUnity += (AccelerationInDirection * Time.smoothDeltaTime);
                //                  f=ma f/m = a                a=km/s/s a*t = v = km/s 
                // the new position = the old position + the change due to the current velocity
                this.transform.position += (thisVelUnity * Time.smoothDeltaTime);
            }

        }
        else
        {
            List<AstroPhysics> PhysObjs = this.gameObject.GetComponentInParent<APParent>().APObjs;
            if ((Active) && (PhysObjs != null))
            {
                AccelerationInDirectionDouble = new Vector3d(0.0d, 0.0d, 0.0d);

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
                        
                        if ((ID == 401) && (PhysObjs[i].ID == 4000))
                        {
                            Debug.Log("");
                            Debug.Log("This Position " + new Vector3d(this.transform.position));
                            Debug.Log(PhysObjs[i].GetComponent<Transform>().position.x);
                            Debug.Log(PhysObjs[i].GetComponent<Transform>().position.y);
                            Debug.Log("Other Position " + new Vector3d(PhysObjs[i].GetComponent<Transform>().position));

                            AstroPhysics other = PhysObjs[i];
                            double DistanceU = Vector3d.Distance(new Vector3d(other.GetComponent<Transform>().position), new Vector3d(this.transform.position));
                            //force acting on current Gravitational object from given gravitational object
                            Debug.Log("DistanceU = " + DistanceU);

                            Debug.Log("Grav Const = " + GravConstUnity);
                            Debug.Log("Other Mass in solar masses = " + other.ThisSolarMass);

                            double Neumarator = (double)GravConstUnity * (double)other.ThisSolarMass;
                            double Denominator = DistanceU * DistanceU;
                            Debug.Log("Numerator = " + Neumarator);
                            Debug.Log("Denominator = " + Denominator);
                            AccelerationUnityDouble = Neumarator / Denominator;
                            Debug.Log("Acceleration Unity = " + AccelerationUnity);

                            //AccelerationUnity = AccelerationFromGravity(PhysObjs[i], DistanceU);

                            Vector3d Direction = new Vector3d(this.transform.position) - new Vector3d(PhysObjs[i].GetComponent<Transform>().position);
                            Debug.Log("Direction Not normalized = " + Direction);


                            NormDirectionDouble = Vector3d.Normalize(Direction);
                            Debug.Log("Normalized direction = " + NormDirectionDouble);
                            AccelerationInDirectionDouble += -AccelerationUnityDouble * NormDirectionDouble;
                            Debug.Log("Acceleration in direction =" + AccelerationInDirectionDouble);
                            Debug.Log("**********************************************************************************");
                        }
                        else
                        {
                            AstroPhysics other = PhysObjs[i];
                            double DistanceU = Vector3d.Distance(new Vector3d(other.GetComponent<Transform>().position), new Vector3d(this.transform.position));
                            //force acting on current Gravitational object from given gravitational object
                            double Neumarator = (double)GravConstUnity * (double)other.ThisSolarMass;
                            double Denominator = DistanceU * DistanceU;
                            AccelerationUnityDouble = Neumarator / Denominator;
                            //print(AffectedByGravity(PhysObjs[i].gameObject));
                            Vector3d Direction = new Vector3d(this.transform.position) - new Vector3d(PhysObjs[i].GetComponent<Transform>().position);
                            NormDirectionDouble = Vector3d.Normalize(Direction);
                            AccelerationInDirectionDouble += -AccelerationUnityDouble * NormDirectionDouble;
                        }
                    }
                }
                //current velocity is equal to the previous velocity 
                thisVelUnityDouble += (AccelerationInDirectionDouble * (double)Time.smoothDeltaTime);
                //                  f=ma f/m = a                a=km/s/s a*t = v = km/s 
                // the new position = the old position + the change due to the current velocity
                Vector3d AmountMovedInDouble = thisVelUnityDouble * (double)Time.smoothDeltaTime;
                this.transform.position += new Vector3((float)AmountMovedInDouble.x, (float)AmountMovedInDouble.y, (float)AmountMovedInDouble.z);
                
            }

        }
    }
    

    float AccelerationFromGravity(AstroPhysics other, float Distance)
    {
       



        return ((GravConstUnity * other.ThisSolarMass) /
                        (Distance * Distance));
        /* 
         * find the force acted apon it by the other object using f =G * M * m / r * r
         * to return acceleration we can cancel m out so we only require the other objects mass.
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