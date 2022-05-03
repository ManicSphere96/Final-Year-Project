using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public GameObject PanelToActivate;
    public GameObject PanelToDeactivate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ButtonPush()
    {
        if (PanelToActivate != null)
        {
            if (PanelToActivate.activeInHierarchy)
            {
                PanelToActivate.SetActive(false);
            }
            else
            {
                PanelToActivate.SetActive(true);
            }
        }

        if (PanelToDeactivate != null)
        {

            if (PanelToDeactivate.activeInHierarchy)
            {
                PanelToDeactivate.SetActive(false);
            }
            else
            {
                PanelToDeactivate.SetActive(true);
            }
        }
    }
}
