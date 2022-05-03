using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSystem : MonoBehaviour
{
    public GameObject SunPrefab;
    public GameObject RockyPlanetPrefab;
    public GameObject GasPlanetPrefab;
    public float GravConstUnity = 40.48f;
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

    float StableVelocity(float SunMass, float Distance)
    {
        // this gives avelocity to obtain a circular orbit for the object in question. 
        return Mathf.Sqrt((40.48f * SunMass) / Distance);
    }
    public void CreateRandomSystem()
    {
        float Solarmass;
        Solarmass = Random.Range(0.57f, 1.64f);
        float RealMass = Solarmass * 1.989e30f;
        double mu = 89.18;
        double sigma = 0.89377;
        GameObject TheSun = Instantiate(SunPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        AstroPhysics SunPrefabPhys = TheSun.GetComponent<AstroPhysics>();
        SunPrefabPhys.ThisRealMass = RealMass;
        SunPrefabPhys.ThisSolarMass = Solarmass;

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
        float RandomDistanceStartPoint = Random.Range(0.08f, 0.15f);
        for (int i = 0; i < Planets.Length; i++)
        {
            Distances[i] = RandomDistanceStartPoint * (Mathf.Exp(0.7f * (i + 1.0f)))*10;
        }
        for (int i = 0; i < Planets.Length; i++)
        {
            
            if (i < NumOfRockPlanets)
            {
                Planets[i] = Instantiate(RockyPlanetPrefab, new Vector3(0, 0, Distances[i]), Quaternion.identity);
                AstroPhysics PlanetAP = Planets[i].GetComponent<AstroPhysics>();
                PlanetAP.ThisRealMass  = Random.Range(3.3e23f, 15e24f);
                PlanetAP.ThisSolarMass = PlanetAP.ThisRealMass / 1.989e+30f;
            }
            else
            {
                Planets[i] = Instantiate(GasPlanetPrefab, new Vector3(0, 0, Distances[i]), Quaternion.identity);
                AstroPhysics PlanetAP = Planets[i].GetComponent<AstroPhysics>();
                PlanetAP.ThisRealMass = Random.Range(15e24f , 2467e24f);
                PlanetAP.ThisSolarMass = PlanetAP.ThisRealMass / 1.989e+30f;
            }

            if   (Random.Range(0,10) !=1)
            {
                float Randnumb = (float)Random.Range(0.0f, 1.0f);
                //Debug.Log(Randnumb);
               Planets[i].GetComponent<AstroPhysics>().OrbitalInclination = (float)norminv(Randnumb, mu, sigma);
            }
            else
            {
                Planets[i].GetComponent<AstroPhysics>().OrbitalInclination = (float)Random.Range(0, 180);
            }
            float initVelocity = StableVelocity(TheSun.GetComponent<AstroPhysics>().ThisSolarMass, Planets[i].GetComponent<AstroPhysics>().GetDistance(TheSun.GetComponent<AstroPhysics>()));
            Planets[i].GetComponent<AstroPhysics>().AddVelocity(Quaternion.AngleAxis(Planets[i].GetComponent<AstroPhysics>().OrbitalInclination, new Vector3(0, 0, 1))* new Vector3(0, initVelocity, 0));
        }
    }
}