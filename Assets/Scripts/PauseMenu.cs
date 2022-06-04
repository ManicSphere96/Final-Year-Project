using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject PanelToActivate;
    public GameObject CameraToActivate;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            PanelToActivate.SetActive(!PanelToActivate.activeInHierarchy);           
        }
        if (PanelToActivate.activeInHierarchy)
        {
            FindObjectOfType<TimeChange>().RateOfTime = 0;
        }
    }
}
