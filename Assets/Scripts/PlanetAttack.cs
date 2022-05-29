using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject MisslePrefab;
    public bool IsAttacking = false;
    bool currentlyAttacking = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsAttacking)
        {
            
            StartCoroutine(Attack());
            
        }
    }
    void FireMissle()
    {
        GameObject PlayerObj = FindObjectOfType<Player>().gameObject;
        Vector3 Direction = (PlayerObj.transform.position - transform.position).normalized;
        GameObject MissileObj = Instantiate(MisslePrefab, this.transform.position + (Direction * this.transform.localScale.x*2), this.transform.rotation);
        MissileObj.GetComponent<Missile>().Target = PlayerObj;
    }

    IEnumerator Attack()
    {
        if (!currentlyAttacking)
        {
            currentlyAttacking = true;
            yield return new WaitForSecondsRealtime(2);
            FireMissle();
            currentlyAttacking = false;

        }
        
    }
    
}
