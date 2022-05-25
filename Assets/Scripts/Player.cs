using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{
    public Transform PlayerTransform;
    public AstroPhysics PlayerAP;
    public ParticleSystem PlayerPS;
    public bool LookAtPlanet = false;
    public bool AttemptingToCollect = false;
    public int ParticleSystemActive = 0;
    public float LookSpeed;
    public float velocity;
    public float FastScalarVelForward;
    public float FastScalarVelRight;
    public float SlowScalarVelForward;
    public float SlowScalarVelRight;
    public bool LookAtPlanetLookAt = false;
    public bool OnlyOnce = true;

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
            if (!OnlyOnce)
            { 
                OnlyOnce = true; 
            }
            if (LookAtPlanetLookAt)
            {
                LookAtPlanetLookAt = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            AttemptingToCollect = !AttemptingToCollect;
            
        }
        if (ParticleSystemActive == 0)
        {
            PlayerPS.gameObject.SetActive(false);
        }
        
        for (int i = 0; i < PhysObjs.Count; i++)
        {
            if ((PlayerAP != PhysObjs[i])&& (PhysObjs[i].gameObject.GetComponent<Planet>() != null))
            {
                if (PhysObjs[i].gameObject.transform.Find("Outer") != null)
                {
                    MeshRenderer OuterMR = PhysObjs[i].gameObject.transform.Find("Outer").GetComponent<MeshRenderer>();
                    Color MyColour = OuterMR.material.color;
                    MyColour.a = SmoothScale(PhysObjs[i].GetDistanceUnity(this.GetComponent<AstroPhysics>()), 
                                            (PhysObjs[i].UnityDiameter * OuterMR.gameObject.transform.localScale.x) / 2, 
                                            ((PhysObjs[i].UnityDiameter * OuterMR.gameObject.transform.localScale.x) / 2) + 10.0f);
                    OuterMR.material.SetColor("_Color", MyColour);
                     float TimeChangeFloat = SmoothScale(PhysObjs[i].GetDistanceUnity(this.GetComponent<AstroPhysics>()), 
                                                         PhysObjs[i].UnityDiameter * OuterMR.gameObject.transform.localScale.x / 4, 
                                                         PhysObjs[i].UnityDiameter * OuterMR.gameObject.transform.localScale.x / 2);
                    if ((TimeChangeFloat != 1.0f)&& (TimeChangeFloat<0.9f))
                    {
                        FindObjectOfType<TimeChange>().RateOfTime = TimeChangeFloat + 0.1f;
                    }
                
                }
                
                
                float distance = PhysObjs[i].GetDistanceUnity(PlayerAP);
                
                if (distance < 2)
                {
                    if (AttemptingToCollect)
                    {
                        if (PhysObjs[i].GetComponent<Slaveable>() != null)
                        {
                            if(!PhysObjs[i].GetComponent<Slaveable>().GetIsSlaved())
                            {
                                PhysObjs[i].GetComponent<Planet>().Collecting = true;
                                ParticleSystem.MainModule newMain = PlayerPS.main;
                                newMain.startColor = new Color(1, 0, 0, 1);
                                PlayerPS.gameObject.SetActive(true);
                                ParticleSystemActive = PhysObjs[i].ID;
                            }
                        }
                        else
                        {
                            PhysObjs[i].GetComponent<Planet>().Collecting = true;
                            ParticleSystem.MainModule newMain = PlayerPS.main;
                            newMain.startColor = new Color(1, 0, 0, 1);
                            ParticleSystem.EmissionModule newemission = PlayerPS.emission;
                            newemission.rateOverTime =  5  /FindObjectOfType<TimeChange>().RateOfTime;
                            PlayerPS.gameObject.SetActive(true);
                            ParticleSystemActive = PhysObjs[i].ID;
                        }
                    }
                    else
                    {
                        PhysObjs[i].GetComponent<Planet>().Collecting = false;
                        if ((ParticleSystemActive == PhysObjs[i].ID))
                        {
                            PlayerPS.gameObject.SetActive(false);
                            ParticleSystemActive = 0;
                        }
                    }
                    
                    if (LookAtPlanet)
                    {
                        if (LookAtPlanetLookAt)
                        {
                            this.transform.LookAt(PhysObjs[i].transform);
                        }
                        else
                        {
                            Quaternion LookRotation = Quaternion.LookRotation(PhysObjs[i].transform.position - this.transform.position);
                            transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, Time.deltaTime * 50.0f);
                            StartCoroutine(WaitForLookAt());
                        }
                    }
                    
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        Vector3 NewVel = PhysObjs[i].GetComponent<Planet>().StableOrbitVector(PlayerAP, distance);
                        PlayerAP.SetVelocity(NewVel);
                    }
                }
                else
                {
                    PhysObjs[i].GetComponent<Planet>().Collecting = false;
                    if ((ParticleSystemActive == PhysObjs[i].ID)&&(distance>2))
                    {
                        PlayerPS.gameObject.SetActive(false);
                        ParticleSystemActive = 0 ;
                    }
                }
                
            }
            
            if (PhysObjs[i].gameObject.name == "Asteroid(Clone)")
            {
                float TimeChangeFloat = SmoothScale(PhysObjs[i].GetDistanceUnity(this.GetComponent<AstroPhysics>()),1,2);
                if ((TimeChangeFloat != 1.0f) && (TimeChangeFloat < 0.9f))
                {
                    FindObjectOfType<TimeChange>().RateOfTime = TimeChangeFloat + 0.1f;
                }
            }
        }
    }
    
    IEnumerator  WaitForLookAt()
    {
        if (OnlyOnce)
        {
            OnlyOnce = false;
            yield return new WaitForSeconds(1);
            LookAtPlanetLookAt = true;
            
        }
        yield return null;
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
