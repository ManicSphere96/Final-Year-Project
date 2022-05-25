using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slaveable : MonoBehaviour
{
    bool IsSlaved;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null)
        {
            this.GetComponent<AstroPhysics>().SetVelocity(transform.parent.gameObject.GetComponent<AstroPhysics>().GetVelocityUnity());
        }
    }
    public void SlaveObject()
    {
        SlaveObject(!IsSlaved);
    }
    public void SlaveObject(bool slave)
    {
        IsSlaved = slave;
        if (slave)
        {
            this.transform.parent = FindObjectOfType<Player>().transform;
            this.GetComponent<AstroPhysics>().Active = false;
        }
        if (!slave)
        {
            this.transform.parent = null;
            this.GetComponent<AstroPhysics>().Active = false;
        }
    }
    public bool GetIsSlaved()
    {
        return IsSlaved;
    }
}
