using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class APParent : MonoBehaviour
{
    public const float RealGravitationalConstant = 6.6743e-11f; 
    float GravitationalConstantUnity = 0;
    public float DistanceScaleM2U = 0; // input distance in m equal to one Unity Unit
    public float TimeScaleS2U = 0; // input number of seconds to pass in 1 second Real time
    public float MassScaleKg2U = 0; // input number of kgs equal to one Unity Mass
    public float DistanceScaleU2M;
    public float TimeScaleU2S; // 
    public float MassScaleU2Kg; //
    public List<AstroPhysics> APObjs;
    // Start is called before the first frame update
    void Start()
    {
        if ((DistanceScaleM2U != 0) && (TimeScaleS2U != 0) && (MassScaleKg2U != 0))
        {
            DistanceScaleU2M = 1 / DistanceScaleM2U;
            TimeScaleU2S = 1 / TimeScaleS2U;
            MassScaleU2Kg = 1 / MassScaleKg2U;
            GravitationalConstantUnity = RealGravitationalConstant * (float)(Mathd.Pow(DistanceScaleU2M, 3) / ((double)MassScaleU2Kg * Mathd.Pow(TimeScaleU2S, 2)));
            Debug.Log(GravitationalConstantUnity);
            
            //GravitationalConstantUnity = RealGravitationalConstant 
        } 
        else
        {
            // Default value for 10 Unity = 1AU (1.496e+10), 1sec = 1 MegaSec (1e+6), 1 Kg = 1 Solar Mass (1.989e+30)
            DistanceScaleM2U = 1.496e+10f;
            MassScaleKg2U = 1.989e+30f;
            TimeScaleS2U = 1e+6f;
            DistanceScaleU2M = 1 / DistanceScaleM2U;
            TimeScaleU2S = 1 / TimeScaleS2U;
            MassScaleU2Kg = 1 / MassScaleKg2U;
            GravitationalConstantUnity = RealGravitationalConstant * (float)(Mathd.Pow(DistanceScaleU2M, 3) / ((double)MassScaleU2Kg * Mathd.Pow(TimeScaleU2S, 2)));
             
        }
    }

    // Update is called once per frame
    void Update()
    {
        APObjs = ((AstroPhysics[])FindObjectsOfType<AstroPhysics>()).ToList();
          
    }
    public float GetGUnity()
    {
        return GravitationalConstantUnity;
    }
   
}
