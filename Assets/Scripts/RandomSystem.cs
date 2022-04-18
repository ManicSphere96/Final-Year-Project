using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSystem : MonoBehaviour
{
    public GameObject SunPrefab;
    public GameObject RockyPlanetPrefab;
    public GameObject GasPlanetPrefab;
    public float GravConstUnity = 40.48f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateRandomSystem()
    {

        float Solarmass;
        Solarmass = Random.Range(0.57f, 1.64f);
        float RealMass = Solarmass * 1.989e30f;
        GameObject TheSun = Instantiate(SunPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        AstroPhysics SunPrefabPhys = SunPrefab.GetComponent<AstroPhysics>();
        SunPrefabPhys.ThisRealMass = RealMass;
        SunPrefabPhys.ThisSolarMass = Solarmass;

        int Numofplanets = Random.Range(3, 9);
        int NumofRockPlan;
        int NumofGasPlan;
        if (Numofplanets % 2 != 0)
        {
            Numofplanets--;
            NumofRockPlan = Numofplanets / 2;
            NumofGasPlan = NumofRockPlan;
            NumofRockPlan++;

        }
        else
        {
            NumofRockPlan = Numofplanets / 2;
            NumofGasPlan = NumofRockPlan;
        }

        //distance from sun 
        float[] DistFromSun = new float[NumofRockPlan];
        //bool FarEnough = false;
        for (int i = 0; i < NumofRockPlan; i++)
        {
            DistFromSun[i] = Random.Range(3.0f, 30.0f);
        }

        for (int i = 0; i < DistFromSun.Length; i++)
        {
            for (int j = 0; j < DistFromSun.Length; j++)
            {
                while ((DistFromSun[j] - DistFromSun[i] < 3) && (DistFromSun[j] - DistFromSun[i] > -3) && (DistFromSun[j] - DistFromSun[i] != 0))
                {
                    DistFromSun[j] = Random.Range(3.0f, 30.0f);

                }
            }
        }
        float[] MassOfRockyPlanet = new float[NumofRockPlan];
        for (int i = 0; i < NumofRockPlan; i++)
        {
            MassOfRockyPlanet[i] = Random.Range(1.659e-7f, 7.541e-6f);
        }
        GameObject[] RockyPlanets = new GameObject[NumofRockPlan];
        for (int i = 0; i < NumofRockPlan; i++)
        {
            RockyPlanets[i] = Instantiate(RockyPlanetPrefab, new Vector3(0, 0, DistFromSun[i]), Quaternion.identity);

        }

        for (int i = 0; i < NumofRockPlan; i++)
        {
            AstroPhysics RPAP = RockyPlanets[i].GetComponent<AstroPhysics>();
            AstroPhysics TSAP = TheSun.GetComponent<AstroPhysics>();
            RPAP.ThisRealMass = MassOfRockyPlanet[i]* 1.989e30f;
            float InitVel = Mathf.Sqrt((GravConstUnity * TSAP.ThisSolarMass) / RPAP.GetDistance(TSAP));
            Vector3 InitVelVec = new Vector3(InitVel, 0, 0);
            RPAP.AddVelocity(InitVelVec);
        }

        //distance from sun 
        float[] DistFromSunGas = new float[NumofGasPlan];
        //bool FarEnough = false;
        for (int i = 0; i < NumofGasPlan; i++)
        {
            DistFromSunGas[i] = Random.Range(30.0f, 50.0f);
        }
        for (int i = 0; i < DistFromSunGas.Length; i++)
        {
            for (int j = 0; j < DistFromSunGas.Length; j++)
            {
                while ((DistFromSunGas[j] - DistFromSunGas[i] < 3) && (DistFromSunGas[j] - DistFromSunGas[i] > -3) && (DistFromSunGas[j] - DistFromSunGas[i] != 0))
                {
                    DistFromSunGas[j] = Random.Range(30.0f, 50.0f);

                }
            }
        }

        float[] MassOfGasPlanet = new float[NumofGasPlan];
        for (int i = 0; i < NumofGasPlan; i++)
        {
            MassOfGasPlanet[i] = Random.Range(7.541e-6f, 1.240e-3f);
        }
        GameObject[] GasPlanets = new GameObject[NumofGasPlan];
        for (int i = 0; i < NumofGasPlan; i++)
        {
            GasPlanets[i] = Instantiate(GasPlanetPrefab, new Vector3(0, 0, DistFromSunGas[i]), Quaternion.identity);

        }
        for (int i = 0; i < NumofGasPlan; i++)
        {
            AstroPhysics GPAP = GasPlanets[i].GetComponent<AstroPhysics>();
            AstroPhysics TSAP = TheSun.GetComponent<AstroPhysics>();
            GPAP.ThisRealMass = MassOfGasPlanet[i]*1.989e30f;
            float InitVel = Mathf.Sqrt((GravConstUnity * TSAP.ThisSolarMass) / GPAP.GetDistance(TSAP));
            Vector3 InitVelVec = new Vector3(InitVel, 0, 0);
            GPAP.AddVelocity(InitVelVec);
        }
    }
}
