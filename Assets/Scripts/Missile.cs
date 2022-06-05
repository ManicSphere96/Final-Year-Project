using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject Target;
    float speed = 0.01f;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAFterTime());
    }

    // Update is called once per frame
    void Update()
    {
        List<AstroPhysics> PhysObjs = FindObjectOfType<APParent>().APObjs;
        this.transform.LookAt(Target.transform);
        this.transform.position += speed * (Target.transform.position - transform.position).normalized;
        if (Target.GetComponent<AstroPhysics>().GetDistanceUnity(this.transform.position) < 0.001)
        {
            FindObjectOfType<Player>().PushPlayer((Target.transform.position - transform.position).normalized);
            Destroy(this.gameObject);
        }

        for (int i =0; i<PhysObjs.Count; i++)
        {
            if (PhysObjs[i].GetDistanceUnity(this.transform.position)< 0.01)
            {
                Destroy(this.gameObject);
            }
        }

    }
    IEnumerator DestroyAFterTime()
    {
        yield return new WaitForSecondsRealtime(30);
        Destroy(this.gameObject);
    }
}
