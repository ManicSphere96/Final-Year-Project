using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSystem : MonoBehaviour
{
    public GameObject SunPrefab;
    public GameObject RockyPlanetPrefab;
    public GameObject GasPlanetPrefab;
    public GameObject MoonPrefab;
    public GameObject Parent;

    float SunScaleConstant = 100;
    float PlanetScaleConstant = 2;
    float MoonScaleConstant = 10;
    float GasOuterScaleConstant= 300;
    float RockyOuterScaleConstant = 450;
    double GravityRatioConstant = 5;
    void Start()
    {

    }

    void Update()
    {
        
    }

    double normsinv(double p)
    {
        // Coefficients in rational approximations
        double a1 = -3.969683028665376e+01;
        double a2 = 2.209460984245205e+02;
        double a3 = -2.759285104469687e+02;
        double a4 = 1.383577518672690e+02;
        double a5 = -3.066479806614716e+01;
        double a6 = 2.506628277459239e+00;

        double b1 = -5.447609879822406e+01;
        double b2 = 1.615858368580409e+02;
        double b3 = -1.556989798598866e+02;
        double b4 = 6.680131188771972e+01;
        double b5 = -1.328068155288572e+01;

        double c1 = -7.784894002430293e-03;
        double c2 = -3.223964580411365e-01;
        double c3 = -2.400758277161838e+00;
        double c4 = -2.549732539343734e+00;
        double c5 = 4.374664141464968e+00;
        double c6 = 2.938163982698783e+00;

        double d1 = 7.784695709041462e-03;
        double d2 = 3.224671290700398e-01;
        double d3 = 2.445134137142996e+00;
        double d4 = 3.754408661907416e+00;
        // Define break-points
        double p_low = 0.02425;
        double p_high = 1 - p_low;
        // Declare output value
        double x = 0.0;

        
        // Rational approximation for lower region.
        if (0.0 < p && p < p_low)
        {
            double q = Mathf.Sqrt(-2 * Mathf.Log((float)p));
            x = (((((c1 * q + c2) * q + c3) * q + c4) * q + c5) * q + c6) / ((((d1 * q + d2) * q + d3) * q + d4) * q + 1);
        }
        // Rational approximation for central region.
        else if (p_low <= p && p <= p_high)
        {
            double q = p - 0.5;
            double r = q * q;
            x = (((((a1 * r + a2) * r + a3) * r + a4) * r + a5) * r + a6) * q / (((((b1 * r + b2) * r + b3) * r + b4) * r + b5) * r + 1);
        }
        // Rational approximation for upper region.
        else if (p_high < p && p < 1.0)
        {
            double q = Mathf.Sqrt(-2 * Mathf.Log(1f - (float)p));
            x = -(((((c1 * q + c2) * q + c3) * q + c4) * q + c5) * q + c6) / ((((d1 * q + d2) * q + d3) * q + d4) * q + 1);
        }

        return x;
    }

    double norminv( double p,  double mu,  double sigma)
    {
    // Take the mean and sigma (standard deviation) into account
    return sigma* normsinv(p) + mu;
    }


    public void CreateRandomSystem()
    {

        Random.InitState(1);
        float Solarmass = Random.Range(0.57f, 1.64f);
        double RealMass = FindObjectOfType<AstroPhysics>().UnityMassToReal(Solarmass);
        double mu = 89.18;
        double sigma = 0.89377;
        GameObject TheSun = Instantiate(SunPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        AstroPhysics SunPrefabPhys = TheSun.GetComponent<AstroPhysics>();
        SunPrefabPhys.ThisRealMass = RealMass;
        SunPrefabPhys.ThisSolarMass = Solarmass;
        SunPrefabPhys.RealDiameter = (4e-22f * RealMass) + 5e+08f;
        SunPrefabPhys.UnityDiameter = SunPrefabPhys.RealDistToUnity(SunPrefabPhys.RealDiameter);
        SunPrefabPhys.ID = 0;
        TheSun.transform.localScale = new Vector3(1, 1, 1) * (SunPrefabPhys.UnityDiameter * SunScaleConstant);

        int NumberOfPlanets = Random.Range(3, 9);
        int NumOfRockPlanets;
        int NumOfGasPlanets;
        if (NumberOfPlanets % 2 != 0)
        {
            NumberOfPlanets--;
            NumOfRockPlanets = NumberOfPlanets / 2;
            NumOfGasPlanets = NumOfRockPlanets;
            NumOfRockPlanets++;
            NumberOfPlanets++;
        }
        else
        {
            NumOfRockPlanets = NumberOfPlanets / 2;
            NumOfGasPlanets = NumOfRockPlanets;
        }

        GameObject[] Planets = new GameObject[NumberOfPlanets];
        float[] Distances = new float[NumberOfPlanets];
        float RandomDistanceStartPoint = Random.Range(0.05f, 0.15f);
        for (int i = 0; i < Planets.Length; i++)
        {

            Distances[i] = (SunPrefabPhys.UnityDiameter * SunScaleConstant)/1.75f + RandomDistanceStartPoint * (Mathf.Exp(0.6f * (i + 1.0f)))*10;
        }
        for (int i = 0; i < Planets.Length; i++)
        {
            Quaternion PlanetStartRotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up);
            AstroPhysics PlanetAP;
            if (i < NumOfRockPlanets)
            {
                Planets[i] = Instantiate(RockyPlanetPrefab, new Vector3(0, 0, Distances[i]), Quaternion.identity);
                PlanetAP = Planets[i].GetComponent<AstroPhysics>();
                PlanetAP.ThisRealMass = Random.Range(0.03e+24f, 15e+24f);
                PlanetAP.ThisSolarMass = PlanetAP.RealMassToUnity(PlanetAP.ThisRealMass);
                PlanetAP.RealDiameter = 0.0009f * Mathf.Pow((float)PlanetAP.ThisRealMass, 0.4122f);
                PlanetAP.UnityDiameter = PlanetAP.RealDistToUnity(PlanetAP.RealDiameter);
                Planets[i].transform.localScale = new Vector3(1, 1, 1) * (PlanetAP.UnityDiameter * PlanetScaleConstant);
                
                if ((Random.Range(0.0f, 1.0f) > 0.5f)&&(i!=0))
                {
                    Planets[i].GetComponent<Planet>().NumOfMoons = Random.Range(1, 3);
                }
                else
                {
                    Planets[i].GetComponent<Planet>().NumOfMoons = 0;
                }
            }
            else
            {
                Planets[i] = Instantiate(GasPlanetPrefab, new Vector3(0, 0, Distances[i]), Quaternion.identity);
                PlanetAP = Planets[i].GetComponent<AstroPhysics>();
                PlanetAP.ThisRealMass = Random.Range(15e+24f, 2467e+24f);
                PlanetAP.ThisSolarMass = (float)PlanetAP.ThisRealMass / 1.989e+30f;
                PlanetAP.RealDiameter = 0.0009f * Mathf.Pow((float)PlanetAP.ThisRealMass, 0.4122f);
                PlanetAP.UnityDiameter = PlanetAP.RealDistToUnity(PlanetAP.RealDiameter);
                Planets[i].transform.localScale = new Vector3(1, 1, 1) * (PlanetAP.UnityDiameter * PlanetScaleConstant);
                Planets[i].GetComponent<Planet>().NumOfMoons = Random.Range(3, 120);
            }
            Planets[i].GetComponent<Planet>().PlanetNum = i;
            PlanetAP.ID = (i + 1) * 100;
            if   (Random.Range(0,10) !=1)
            {
                float Randnumb = (float)Random.Range(0.0f, 1.0f);
                //Debug.Log(Randnumb);
                PlanetAP.OrbitalInclination = (float)norminv(Randnumb, mu, sigma);
            }
            else
            {
                PlanetAP.OrbitalInclination =  (float)Random.Range(0, 180);
            }
            float initVelocity = PlanetAP.StableOrbitVelocity(TheSun.GetComponent<AstroPhysics>().ThisSolarMass, PlanetAP.GetDistanceUnity(TheSun.transform.position));
            Quaternion Planetangleaxis = Quaternion.AngleAxis(PlanetAP.OrbitalInclination,  Vector3.forward);
            PlanetAP.AddVelocity(Planetangleaxis * new Vector3(0, initVelocity, 0));
            float RandomMoonDistanceStartPoint = Random.Range((float)PlanetAP.RealDiameter, (float)PlanetAP.RealDiameter*3.0f);

            
           



            GameObject[] Moons = new GameObject[Planets[i].GetComponent<Planet>().NumOfMoons];
            double MaxMoonDist = Mathd.Sqrt(Mathd.Pow((double)PlanetAP.UnityDistToReal(Distances[i]), 2) * (double)PlanetAP.ThisRealMass) / Mathd.Sqrt(GravityRatioConstant * (double)SunPrefabPhys.ThisRealMass );



            if (i < NumOfRockPlanets)
            {
                Planets[i].gameObject.transform.Find("Outer").transform.localScale = new Vector3(1, 1, 1) * RockyOuterScaleConstant;
                if ((i != 0)&& (RockyOuterScaleConstant * PlanetAP.UnityDiameter > Distances[i] - Distances[i - 1]))
                {    
                    Planets[i].gameObject.transform.Find("Outer").transform.localScale = new Vector3(1, 1, 1) * (0.75f * (Distances[i] - Distances[i - 1]) / PlanetAP.UnityDiameter);
                }
                else if ((RockyOuterScaleConstant * PlanetAP.UnityDiameter > Distances[i]) || (RockyOuterScaleConstant * PlanetAP.UnityDiameter > Distances[i + 1] - Distances[i]))
                {
                        Planets[i].gameObject.transform.Find("Outer").transform.localScale = new Vector3(1, 1, 1) * (0.5f * (Distances[i]) / PlanetAP.UnityDiameter);
                }
            }
            else
            {

                Planets[i].gameObject.transform.Find("Outer").transform.localScale = new Vector3(1, 1, 1) * GasOuterScaleConstant;
                if (RockyOuterScaleConstant * PlanetAP.UnityDiameter > Distances[i] - Distances[i - 1])
                {
                    Planets[i].gameObject.transform.Find("Outer").transform.localScale = new Vector3(1, 1, 1) * (0.75f * (Distances[i] - Distances[i - 1]) / PlanetAP.UnityDiameter);
                }
                
                

            }















            for (int j = 0; j <Moons.Length; j++)
            {
                
                float Dist;
                
               
                Dist =PlanetAP.RealDistToUnity(RandomMoonDistanceStartPoint);
                RandomMoonDistanceStartPoint = RandomMoonDistanceStartPoint * 2;
                if(RandomMoonDistanceStartPoint> MaxMoonDist)
                {
                    break;
                }

                Moons[j] = Instantiate(MoonPrefab, new Vector3(0, 0, (Planets[i].transform.position.z + Dist)), Quaternion.identity);
                AstroPhysics MoonAP = Moons[j].GetComponent<AstroPhysics>();
                

                if (i < NumOfRockPlanets)
                {
                    MoonAP.ThisRealMass = Random.Range((float)PlanetAP.ThisRealMass / 10000.0f, (float)PlanetAP.ThisRealMass / 1000.0f);
                    
                }
                else
                {
                    //MoonAP.ThisRealMass = Random.Range(7.3e10f, 1.48e25f);
                    MoonAP.ThisRealMass = Random.Range((float)PlanetAP.ThisRealMass / 100000.0f, (float)PlanetAP.ThisRealMass / 10000.0f);
                }

                MoonAP.ThisSolarMass = (float)MoonAP.ThisRealMass / 1.989e+30f;
                if (Random.Range(0, 10) >5)
                {
                    MoonAP.OrbitalInclination = (float)norminv(Random.Range(0.0f, 1.0f), mu, sigma);
                }
                else
                {
                    MoonAP.OrbitalInclination = (float)Random.Range(0, 180);
                }
                float InitMoonVelocity = PlanetAP.StableOrbitVelocity(PlanetAP.ThisSolarMass, Dist);
                Quaternion moonangleaxis = Quaternion.AngleAxis(MoonAP.OrbitalInclination, new Vector3(0, 0, 1));
                // Parent Planet velocity + the angular velocity of the moon at its position
                Vector3 MoonVelocity = PlanetAP.GetVelocityUnity() * ((Planets[i].transform.position.z + Dist) / Planets[i].transform.position.z);
                MoonAP.AddVelocity((moonangleaxis * new Vector3(0, InitMoonVelocity, 0)) + MoonVelocity);

                MoonAP.ID = PlanetAP.ID + j + 1;
                MoonAP.RealDiameter = 0.1666f * Mathf.Pow((float)MoonAP.ThisRealMass, 0.3237f);
                MoonAP.UnityDiameter = MoonAP.RealDistToUnity(MoonAP.RealDiameter);
                Moons[j].transform.localScale = new Vector3(MoonAP.UnityDiameter * MoonScaleConstant, MoonAP.UnityDiameter * MoonScaleConstant, MoonAP.UnityDiameter * MoonScaleConstant);
                Moons[j].transform.position = PlanetStartRotation * Moons[j].transform.position;
                MoonAP.SetVelocity(PlanetStartRotation * MoonAP.GetVelocityUnity());



            }
            Planets[i].transform.position = PlanetStartRotation * Planets[i].transform.position;
            PlanetAP.SetVelocity(PlanetStartRotation * PlanetAP.GetVelocityUnity());

        }
        

    }
}