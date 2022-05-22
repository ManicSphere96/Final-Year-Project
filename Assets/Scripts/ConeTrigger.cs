using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerStay(Collider other)
    {

        if (Input.GetKey(KeyCode.Q))
        {
            if (other.GetComponent<Slaveable>() != null)
            {
                other.GetComponent<Slaveable>().SlaveObject();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Slaveable>() != null)
        {
            other.GetComponent<Slaveable>().SlaveObject(false);
        }
    }
}
