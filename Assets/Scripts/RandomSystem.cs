using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSystem : MonoBehaviour
{
    public GameObject SunPrefab;
    public GameObject RockyPlanetPrefab;
    public GameObject GasPlanetPrefab;
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
        print(Solarmass);
        float RealMass = Solarmass * 1.989e30f;
        Instantiate(SunPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        AstroPhysics SunPrefabPhys = SunPrefab.GetComponent<AstroPhysics>();
        SunPrefabPhys.thisMass = RealMass;
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
        bool FarEnough = false;
        for (int i =0; i < NumofRockPlan; i++)
        {
            DistFromSun[i] = Random.Range(3.0f, 30.0f);
        }
        while (!FarEnough)
        {
            for (int i = 0; i < DistFromSun.Length; i++)
            {
                for(int j = 0; j <DistFromSun.Length; j++)
                {
                    if ()
                }
            }
        }


        Instantiate(RockyPlanetPrefab, , Quaternion.identity);



    }
}
