using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escape : MonoBehaviour
{
    public GameObject PanelToActivate;

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
        if (Input.GetKeyDown(KeyCode.B) == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape) == true)
            { }
        }
    }
}
