using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Physics : MonoBehaviour
{
    [SerializeField] float thisMass;
    [SerializeField] Vector3 thisVelUnity;
    [SerializeField] float thisDist;
    [SerializeField] bool Active;


    float GravConstUnity;

    float ThisSolarMass;

    float Acceleration;

    float solarMassSize;

    Vector3 NormDirection;
    Vector3 AccelerationInDirection;

    bool Binarystarsystem = false;

    //Measurements in masses in solar units, distances in parsecs, velocities in km/s
    // 1 parsec = 206,000 au = 3.086e16 m
    // 1 solar unit 1.98847e30 kg
    // 1 km/s = 1000 m/s
    //converting unity distance to a real life distance  10 units = 1 au = 149.87e11

    void Start()
    {
        solarMassSize = 1.98847e30f;
        GravConstUnity = 40.48f;
        ThisSolarMass = (thisMass / solarMassSize);
    }

    // Update is called once per frame
    void Update()
    {
        //print(this.gameObject.name);
        if (Active)
        {
            AccelerationInDirection = new Vector3(0, 0, 0);
            Physics[] PhysicsObjects = (Physics[])GameObject.FindObjectsOfType(typeof(Physics));
            List<Physics> PhysObjs = PhysicsObjects.ToList();
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
                thisDist = Vector3.Distance(PhysObjs[i].GetComponent<Transform>().position, this.transform.position);
                if (this != PhysObjs[i])
                {
                    //force acting on current Gravitational object from given gravitational object
                    Acceleration = AccelerationFromGravity(PhysObjs[i].gameObject);
                    //print(AffectedByGravity(PhysObjs[i].gameObject));
                    Vector3 Direction = this.transform.position - PhysObjs[i].GetComponent<Transform>().position;
                    NormDirection = Vector3.Normalize(Direction);
                    AccelerationInDirection = -Acceleration * NormDirection;
                }
            }
            //current velocity is equal to the previous velocity 
            thisVelUnity = thisVelUnity + (AccelerationInDirection * Time.deltaTime);
            //                  f=ma f/m = a                a=km/s/s a*t = v = km/s 
            // the new position = the old position + the change due to the current velocity
            this.transform.position = this.transform.position + (thisVelUnity * Time.deltaTime);
        }
    }

    float AccelerationFromGravity(GameObject other)
    {
        return ((GravConstUnity * other.GetComponent<Physics>().ThisSolarMass) /
                        (thisDist * thisDist));
        /* 
         * find the force acted apon it by the other object using f =G * M * m / r * r
         */
    }
}