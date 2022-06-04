using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slaveable : MonoBehaviour
{
    bool IsSlaved;
    public RawImage UILight4;
    Vector3 LastPosition;
    // Start is called before the first frame update
    void Start()
    {
        UILight4 = GameObject.Find("Light4").GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null)
        {
            
            this.GetComponent<AstroPhysics>().SetVelocity((this.transform.position - LastPosition) / Time.smoothDeltaTime);
            LastPosition = this.transform.position;
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
            UILight4.color = new Color(1, 0, 1, 1);
            this.transform.parent = FindObjectOfType<Player>().transform;
            this.GetComponent<AstroPhysics>().Active = false;
        }
        if (!slave)
        {
            UILight4.color = new Color(1, 1, 1, 1);
            this.transform.parent = null;
            this.GetComponent<AstroPhysics>().Active = true;
        }
    }
    public bool GetIsSlaved()
    {
        return IsSlaved;
    }
}
