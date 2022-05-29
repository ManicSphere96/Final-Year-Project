using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Planet : MonoBehaviour
{
    public int NumOfMoons = 0;
    public int PlanetNum;
    public int BufferNumberInit = 500;
    public int BufferNumber ;
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
            BufferNumber = BufferNumberInit;
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

        PlayerAP.StableOrbitVelocity(this.GetComponent<AstroPhysics>().ThisSolarMass, DistUnity);

        float Magnitude = PlayerAP.StableOrbitVelocity(this.GetComponent<AstroPhysics>().ThisSolarMass, DistUnity); //  ((PlayerAP.transform.position.magnitude) / transform.position.magnitude));
        Vector3 TargetVel = (GetComponent<AstroPhysics>().GetVelocityUnity() * ((PlayerAP.transform.position.magnitude) / transform.position.magnitude))  + (A - (Vector3.Dot(A, n) * n)).normalized * Magnitude ;
        return (TargetVel);

    }
    public void DestroyPlanet()
    {
        AstroPhysics PlanetAP = GetComponent<AstroPhysics>();
        MeshRenderer PlanetMR = this.GetComponent<MeshRenderer>();
        int NumberOfAsteroids = 4;//Random.Range(1, 4);
        GameObject CurrentAsteroid = Instantiate(AsteroidPrefab, this.transform.position, Quaternion.identity);
        foreach (Transform child in CurrentAsteroid.transform)
        {
            
            AstroPhysics ChildAsteroidAP = child.GetComponent<AstroPhysics>();

            Vector3 AdditionalMovement =-(CurrentAsteroid.transform.position - ChildAsteroidAP.GetComponent<Transform>().position).normalized;

            ChildAsteroidAP.SetVelocity(this.GetComponent<AstroPhysics>().GetVelocityUnity() + AdditionalMovement/10);
            //ChildAsteroidAP.ThisRealMass = PlanetAP.ThisRealMass / 4;
            //ChildAsteroidAP.ThisSolarMass = PlanetAP.ThisSolarMass / 4;
            ChildAsteroidAP.ID = PlanetAP.ID * 10 + NumberOfAsteroids;
            ChildAsteroidAP.UnityDiameter = PlanetAP.UnityDiameter ;
            ChildAsteroidAP.RealDiameter = PlanetAP.RealDiameter ;
            CurrentAsteroid.transform.localScale = new Vector3(ChildAsteroidAP.UnityDiameter/2,
                                                               ChildAsteroidAP.UnityDiameter/2,
                                                               ChildAsteroidAP.UnityDiameter/2);
            NumberOfAsteroids--;
            child.GetComponent<MeshRenderer>().material = PlanetMR.material;
        }
        Player PlayerObj = FindObjectOfType<Player>();
        PlayerObj.LookAtPlanetLookAt = false;
        PlayerObj.LookAtPlanet = false;
        PlayerObj.ParticleSystemActive = 0;
        PlayerObj.AttemptingToCollect = false;
        PlanetAP.ThisRealMass = 0;
        PlanetAP.ThisSolarMass = 0;
        Destroy(gameObject);
        //StartCoroutine(ShrinkPlanet());       

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
    

