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
    public GameObject AsteroidPrefab;
    
   
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
            DestroyPlanet();
        }
    }
    public Vector3 StableOrbitVector(AstroPhysics PlayerAP, float DistUnity)
    {
        Vector3 A = PlayerAP.GetVelocityUnity();
        Vector3 n = - (PlayerAP.transform.position -this.gameObject.transform.position).normalized;

        PlayerAP.StableVelocity(this.GetComponent<AstroPhysics>().ThisSolarMass, DistUnity);

        float Magnitude = PlayerAP.StableVelocity(this.GetComponent<AstroPhysics>().ThisSolarMass, DistUnity); //  ((PlayerAP.transform.position.magnitude) / transform.position.magnitude));
        Vector3 TargetVel = (GetComponent<AstroPhysics>().GetVelocityUnity() * ((PlayerAP.transform.position.magnitude) / transform.position.magnitude))  + (A - (Vector3.Dot(A, n) * n)).normalized * Magnitude ;
        return (TargetVel);

    }
    public void DestroyPlanet()
    {
        AstroPhysics PlanetAP = GetComponent<AstroPhysics>();
        int NumberOfAsteroids = 3;//Random.Range(1, 4);
        for (int i = 0; i < NumberOfAsteroids; i++)
        {
            GameObject CurrentAsteroid = Instantiate(AsteroidPrefab, this.transform.position +new Vector3(0,0,i*0.0001f), Quaternion.identity);
            AstroPhysics CurrentAsteroidAP = CurrentAsteroid.GetComponent<AstroPhysics>();
            
            Vector3 A = this.GetComponent<AstroPhysics>().GetVelocityUnity();
            Vector3 n = new Vector3(Random.Range(0.0f, 1.0f),
                                    Random.Range(0.0f, 1.0f),
                                    Random.Range(0.0f, 1.0f));
            Vector3 AdditionalMovement = (A - (Vector3.Dot(A, n) * n)).normalized;
            
            
            CurrentAsteroidAP.SetVelocity(this.GetComponent<AstroPhysics>().GetVelocityUnity() + AdditionalMovement/10);
            //CurrentAsteroidAP.ThisRealMass = PlanetAP.ThisRealMass / NumberOfAsteroids;
            //CurrentAsteroidAP.ThisSolarMass = PlanetAP.ThisSolarMass / NumberOfAsteroids;
            CurrentAsteroidAP.ID = PlanetAP.ID * 10 + i;
            CurrentAsteroidAP.UnityDiameter = PlanetAP.UnityDiameter / NumberOfAsteroids;
            CurrentAsteroidAP.RealDiameter = PlanetAP.RealDiameter / NumberOfAsteroids;
            CurrentAsteroid.transform.localScale = new Vector3(CurrentAsteroidAP.UnityDiameter/4,
                                                               CurrentAsteroidAP.UnityDiameter/4,
                                                               CurrentAsteroidAP.UnityDiameter/4);


        }
        Player PlayerObj = FindObjectOfType<Player>();
        PlayerObj.LookAtPlanetLookAt = false;
        PlayerObj.LookAtPlanet = false;
        PlayerObj.ParticleSystemActive = 0;
        PlayerObj.AttemptingToCollect = false;
        PlanetAP.ThisRealMass = 0;
        PlanetAP.ThisSolarMass = 0;
        StartCoroutine(ShrinkPlanet());       
        
    }
    IEnumerator ShrinkPlanet()
    {
        Transform PlanetTransform = this.transform;
        while(PlanetTransform.localScale.x > 0.001)
        {
            PlanetTransform.localScale -= new Vector3(0.001f, 0.001f, 0.001f);
            yield return null;
        }
        Destroy(gameObject);

    }

        
}
    

