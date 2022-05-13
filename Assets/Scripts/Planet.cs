using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
            Destroy(this.gameObject);
        }
    }
    
}
