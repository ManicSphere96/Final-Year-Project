using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    bool UIOn= false;
    int UIOnForPlanet;
    public RawImage UILight1; // able to enter stable orbit
    public RawImage UILight2; // Look At
    public RawImage UILight3; // Collecting from Planet
    public GameObject ExitGameUI;
    bool HaventAsked;
    bool ContinueClicked;
    public int MaxResources;
    public int ResourcesCollected;



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
        UIOn = false;
        for (int i = 0; i < PhysObjs.Count; i++)
        {
            if ((PlayerAP != PhysObjs[i])&& (PhysObjs[i].gameObject.GetComponent<Planet>() != null))
            {
                if (PhysObjs[i].gameObject.transform.Find("Outer") != null)
                {
                    MeshRenderer OuterMR = PhysObjs[i].gameObject.transform.Find("Outer").GetComponent<MeshRenderer>();
                    Color MyColour = OuterMR.material.color;
                    MyColour.a = SmoothScale(PhysObjs[i].GetDistanceUnity(this.transform.position), 
                                            (PhysObjs[i].UnityDiameter * OuterMR.gameObject.transform.localScale.x) / 2, 
                                            ((PhysObjs[i].UnityDiameter * OuterMR.gameObject.transform.localScale.x) / 2) + 10.0f);
                    OuterMR.material.SetColor("_Color", MyColour);
                     float TimeChangeFloat = SmoothScale(PhysObjs[i].GetDistanceUnity(this.transform.position), 
                                                         PhysObjs[i].UnityDiameter * OuterMR.gameObject.transform.localScale.x / 4, 
                                                         PhysObjs[i].UnityDiameter * OuterMR.gameObject.transform.localScale.x / 2);
                    if ((TimeChangeFloat != 1.0f)&& (TimeChangeFloat<0.9f))
                    {
                        FindObjectOfType<TimeChange>().RateOfTime = TimeChangeFloat + 0.1f;
                    }
                
                }
                
                
                float distance = PhysObjs[i].GetDistanceUnity(PlayerAP.transform.position);
                
                if (distance < 2)
                {
                    UIOn = true;
                    UIOnForPlanet = i;


                    if (PhysObjs[i].GetComponent<PlanetAttack>() !=null)
                    {
                        PhysObjs[i].GetComponent<PlanetAttack>().IsAttacking = true;
                    }
                    if (AttemptingToCollect)
                    {
                        if (PhysObjs[i].GetComponent<Slaveable>() != null)
                        {
                            if(!PhysObjs[i].GetComponent<Slaveable>().GetIsSlaved())
                            {
                                PhysObjs[i].GetComponent<Planet>().Collecting = true;
                                ParticleSystem.MainModule newMain = PlayerPS.main;
                                newMain.startColor = new Color(1, 0, 0, 1);
                                ParticleSystem.EmissionModule newemission = PlayerPS.emission;
                                newemission.rateOverTime = 5 / FindObjectOfType<TimeChange>().RateOfTime;
                                PlayerPS.gameObject.SetActive(true);
                                ParticleSystemActive = PhysObjs[i].ID;
                                UILight3.color = new Color(0, 1, 0, 1);
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
                            UILight3.color = new Color(0, 1, 0, 1);
                        }
                    }
                    else
                    {
                        UILight3.color = new Color(1, 1, 1, 1);
                        PhysObjs[i].GetComponent<Planet>().Collecting = false;
                        if ((ParticleSystemActive == PhysObjs[i].ID))
                        {
                            PlayerPS.gameObject.SetActive(false);
                            ParticleSystemActive = 0;
                        }
                    }
                    
                    if (LookAtPlanet)
                    {
                        UILight2.color = new Color(0, 0, 1, 1);
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
                    else
                    {
                        UILight2.color = new Color(1, 1, 1, 1);
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
                    if (PhysObjs[i].GetComponent<PlanetAttack>() != null)
                    {
                        PhysObjs[i].GetComponent<PlanetAttack>().IsAttacking = false;
                    }
                }
                
            }
            
            if (PhysObjs[i].gameObject.name == "Asteroid(Clone)")
            {
                float TimeChangeFloat = SmoothScale(PhysObjs[i].GetDistanceUnity(this.transform.position),1,2);
                if ((TimeChangeFloat != 1.0f) && (TimeChangeFloat < 0.9f))
                {
                    FindObjectOfType<TimeChange>().RateOfTime = TimeChangeFloat + 0.1f;
                }
            }
        }
        if (UIOn)
        {

            FindObjectOfType<UI>().UpdateUIText(PhysObjs[UIOnForPlanet].GetComponent<Planet>(), PhysObjs[UIOnForPlanet]);
            FindObjectOfType<UI>().TextPanel.SetActive(true);
            UILight1.color = new Color(1, 0, 0, 1);
        }
        else
        {
            FindObjectOfType<UI>().TextPanel.SetActive(false);
            UILight1.color = new Color(1, 1, 1, 1);
        }

        if ((this.transform.position.magnitude > 200)&& (HaventAsked) && (!ContinueClicked))
        {
            ExitGameUI.SetActive(true);
            
        }
        else if ((this.transform.position.magnitude > 200) && (!HaventAsked)&& (ContinueClicked))
        {
            ExitGameUI.SetActive(false);
        }
        else if ((this.transform.position.magnitude > 200) && (ContinueClicked))
        {
            ContinueClicked = false;
        }
        else
        {
            ExitGameUI.SetActive(false);
            HaventAsked = true;
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
    public void ContinueClick()
    {
        HaventAsked = false;
        ContinueClicked = true;
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

    public void PushPlayer(Vector3 Direction)
    {
        PlayerAP.AddVelocity(Direction * 0.1f);
    }
}
