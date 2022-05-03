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
    public float ScalarVelForward;
    public float ScalarVelRight;
    

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Move();
        AstroPhysics[] PhysicsObjects = (AstroPhysics[])FindObjectsOfType<AstroPhysics>();
        List<AstroPhysics> PhysObjs = PhysicsObjects.ToList();
        for (int i = 0; i < PhysObjs.Count; i++)
        {
            if (!PhysObjs[i].Active)
            {
                PhysObjs.RemoveAt(i);
                i--;
            }
        }
        Debug.Log(PhysObjs.Count);
        for (int i = 0; i < PhysObjs.Count; i++)
        {
            if ((this.gameObject.GetComponent<AstroPhysics>() != PhysObjs[i])&& (PhysObjs[i].gameObject.GetComponent<Planet>() != null))
            {
                float distance = PhysObjs[i].GetDistance(this.GetComponent<AstroPhysics>());
                Debug.Log(distance);
                if (distance < 2)
                {
                    PhysObjs[i].GetComponent<Planet>().Collecting = true;
                }
                else
                {
                    PhysObjs[i].GetComponent<Planet>().Collecting = false;
                }
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
            this.GetComponent<AstroPhysics>().AddVelocity(ScalarVelForward * PlayerTransform.forward);
        }
        //Moves Player Back
        if (Input.GetKey(KeyCode.S))
        {
            this.GetComponent<AstroPhysics>().AddVelocity(-ScalarVelForward * PlayerTransform.forward);
        }
            
        //Moves Player strafe Right
        if (Input.GetKey(KeyCode.A))
        {
            this.GetComponent<AstroPhysics>().AddVelocity(-ScalarVelRight * PlayerTransform.right);
        }
            
        //Moves Player strafe Left
        if (Input.GetKey(KeyCode.D))
        {
            this.GetComponent<AstroPhysics>().AddVelocity(ScalarVelRight * PlayerTransform.right);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            this.GetComponent<AstroPhysics>().AddVelocity(-this.GetComponent<AstroPhysics>().GetVelocity());
        }
       
    }
}
