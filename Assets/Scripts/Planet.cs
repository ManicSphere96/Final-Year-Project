using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Planet : MonoBehaviour
{
    public int NumOfMoons = 0;
    public int PlanetNum;
    public int BufferNumber = 500;
    public int ResourceANumber;
    public int ResourceBNumber;
    public int ResourceCNumber;
    public int ResourceDNumber;
    public bool Collecting;
    
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Collecting == true)
        {
            RemoveResources();
        }
        else
        {
            BufferNumber = 500;
        }
        
        
    }
    void RemoveResources()
    { 
        if (BufferNumber> 0)
        {
            BufferNumber--;
        }
        else if (ResourceANumber>0)
        {
            ResourceANumber--;
        }
        else if (ResourceBNumber > 0)
        {
            ResourceBNumber--;
        }
        else if (ResourceCNumber > 0)
        {
            ResourceCNumber--;
        }
        else if (ResourceDNumber > 0)
        {
            ResourceDNumber--;
        }
        if (BufferNumber + ResourceANumber +ResourceBNumber+ResourceCNumber+ResourceDNumber==0)
        {
            Destroy(this.gameObject);
        }
    }
    public Vector3 StableOrbitVector(AstroPhysics PlayerAP, float DistUnity)
    {
        Vector3 A = PlayerAP.GetVelocityUnity();
        Vector3 n = - (PlayerAP.transform.position -this.gameObject.transform.position).normalized;

        PlayerAP.StableVelocity(this.GetComponent<AstroPhysics>().ThisSolarMass, DistUnity);

        float Magnitude = PlayerAP.StableVelocity(this.GetComponent<AstroPhysics>().ThisSolarMass, DistUnity); //  ((PlayerAP.transform.position.magnitude) / transform.position.magnitude));
        Vector3 TargetVel = (GetComponent<AstroPhysics>().GetVelocityUnity() * ((PlayerAP.transform.position.magnitude) / transform.position.magnitude))  + (A - (Vector3.Dot(A, n) * n)).normalized * Magnitude ;
        Vector3 DiffInVel = TargetVel - PlayerAP.GetVelocityUnity();
        return (TargetVel);

    }
}
