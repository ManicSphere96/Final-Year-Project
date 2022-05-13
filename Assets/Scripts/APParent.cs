using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class APParent : MonoBehaviour
{
    public List<AstroPhysics> APObjs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        APObjs = ((AstroPhysics[])FindObjectsOfType<AstroPhysics>()).ToList();
          
    }
}
