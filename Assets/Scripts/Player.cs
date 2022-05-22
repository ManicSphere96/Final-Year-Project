using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{
    public Transform PlayerTransform;
    public AstroPhysics PlayerAP;
    Camera PlayerCam;
    bool LookAtPlanet = false;
    public float LookSpeed;
    public float velocity;
    float PlayerAngleX;
    float PlayerAngleY;
    public float FastScalarVelForward;
    public float FastScalarVelRight;
    public float SlowScalarVelForward;
    public float SlowScalarVelRight;

    void Start()
    {
        PlayerAP = GetComponent<AstroPhysics>();
    }
    // Update is called once per frame
    void Update()
    {
        Rotate();
        Move();
        List<AstroPhysics> PhysObjs = FindObjectOfType<APParent>().APObjs;
        if (Input.GetKeyDown(KeyCode.E))
        {
            LookAtPlanet = !LookAtPlanet;
        }
        for (int i = 0; i < PhysObjs.Count; i++)
        {
            if ((PlayerAP != PhysObjs[i])&& (PhysObjs[i].gameObject.GetComponent<Planet>() != null))
            {
                float distance = PhysObjs[i].GetDistanceUnity(PlayerAP);
                
                if (distance < 2)
                {
                    PhysObjs[i].GetComponent<Planet>().Collecting = true;
                    
                    
                    if (LookAtPlanet)
                    {
                        this.transform.LookAt(PhysObjs[i].gameObject.transform);
                    }
                    
                    if (Input.GetKeyDown(KeyCode.LeftControl))
                    {
                        Vector3 NewVel = PhysObjs[i].GetComponent<Planet>().StableOrbitVector(PlayerAP, distance);
                        PlayerAP.SetVelocity(NewVel*Time.deltaTime);
                    }
                }
                else
                {
                    PhysObjs[i].GetComponent<Planet>().Collecting = false;
                }
                
            }
            
            if (PhysObjs[i].gameObject.transform.Find("Outer") != null)
            {
                MeshRenderer OuterMR = PhysObjs[i].gameObject.transform.Find("Outer").GetComponent<MeshRenderer>();
                Color MyColour = OuterMR.material.color;
                MyColour.a = SmoothScale(PhysObjs[i].GetDistanceUnity(this.GetComponent<AstroPhysics>()), (PhysObjs[i].UnityDiameter *OuterMR.gameObject.transform.localScale.x)/2, ((PhysObjs[i].UnityDiameter * OuterMR.gameObject.transform.localScale.x )/ 2 )+10.0f);
                OuterMR.material.SetColor("_Color", MyColour);
                
            }
        }
    }

    
    void Rotate()
    {
        transform.Rotate( new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * LookSpeed);
        transform.rotation = Quaternion.Euler(PlayerTransform.rotation.eulerAngles.x, PlayerTransform.rotation.eulerAngles.y, 0);
    }
    void Move()
    {
        if (Input.GetKey(KeyCode.W))
        { 
            this.GetComponent<AstroPhysics>().AddVelocity(SlowScalarVelForward * PlayerTransform.forward);
        }
        //Moves Player Back
        if (Input.GetKey(KeyCode.S))
        {
            this.GetComponent<AstroPhysics>().AddVelocity(-SlowScalarVelForward * PlayerTransform.forward);
        }
            
        //Moves Player strafe Right
        if (Input.GetKey(KeyCode.A))
        {
            this.GetComponent<AstroPhysics>().AddVelocity(-SlowScalarVelRight * PlayerTransform.right);
        }
            
        //Moves Player strafe Left
        if (Input.GetKey(KeyCode.D))
        {
            this.GetComponent<AstroPhysics>().AddVelocity(SlowScalarVelRight * PlayerTransform.right);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            this.GetComponent<AstroPhysics>().AddVelocity(-this.GetComponent<AstroPhysics>().GetVelocityUnity());
        }


        if ((Input.GetKey(KeyCode.W)) && (Input.GetKey(KeyCode.LeftShift)))
        {

            this.GetComponent<AstroPhysics>().AddVelocity(FastScalarVelForward * PlayerTransform.forward);
        }
        //Moves Player Back
        if ((Input.GetKey(KeyCode.S)) && (Input.GetKey(KeyCode.LeftShift)))
        {
            this.GetComponent<AstroPhysics>().AddVelocity(-FastScalarVelForward * PlayerTransform.forward);
        }

        //Moves Player strafe Right
        if ((Input.GetKey(KeyCode.A)) && (Input.GetKey(KeyCode.LeftShift)))
        {
            this.GetComponent<AstroPhysics>().AddVelocity(-FastScalarVelRight * PlayerTransform.right);
        }

        //Moves Player strafe Left
        if ((Input.GetKey(KeyCode.D)) && (Input.GetKey(KeyCode.LeftShift)))
        {
            this.GetComponent<AstroPhysics>().AddVelocity(FastScalarVelRight * PlayerTransform.right);
        }
        

    }
    float SmoothScale(float Dist, float MinDist, float MaxDist)
    {
        
        float x = Mathf.Clamp(Dist, MinDist, MaxDist) - (MinDist);
        x = x / (MaxDist-MinDist);
        x = x * x * (3 - 2 * x);
        return x;
    }

}
