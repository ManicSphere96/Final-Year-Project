using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{
    public Transform PlayerTransform;
    public float LookSpeed;
    public float velocity;
    float PlayerAngleX;
    float PlayerAngleY;
    public float FastScalarVelForward;
    public float FastScalarVelRight;
    public float SlowScalarVelForward;
    public float SlowScalarVelRight;

    
    // Update is called once per frame
    void Update()
    {
        Rotate();
        Move();
        List<AstroPhysics> PhysObjs = this.gameObject.GetComponentInParent<APParent>().APObjs;

        for (int i = 0; i < PhysObjs.Count; i++)
        {
            if ((this.gameObject.GetComponent<AstroPhysics>() != PhysObjs[i])&& (PhysObjs[i].gameObject.GetComponent<Planet>() != null))
            {
                float distance = PhysObjs[i].GetDistanceUnity(this.GetComponent<AstroPhysics>());
                
                if (distance < 2)
                {
                    PhysObjs[i].GetComponent<Planet>().Collecting = true;
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
                /*if(MyColour.a !=1)
                {
                    MyColour.a= SmoothScale(PhysObjs[i].GetDistanceUnity(this.GetComponent<AstroPhysics>()), OuterMR.gameObject.transform.localScale.x / 2, (OuterMR.gameObject.transform.localScale.x / 2) + 10.0f);
                    OuterMR.material.SetColor("_Color", MyColour);
                }*/
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
            this.GetComponent<AstroPhysics>().AddVelocity(-this.GetComponent<AstroPhysics>().GetVelocity());
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
