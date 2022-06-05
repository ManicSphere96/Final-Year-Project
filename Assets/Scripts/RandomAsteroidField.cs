using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAsteroidField : MonoBehaviour
{
    public GameObject Asteroid1, Asteroid2, Asteroid3, Asteroid4, Asteroid5;
    // Start is called before the first frame update
    void Start()
    {
        RandomAsteroidFieldGen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void RandomAsteroidFieldGen()
    {
        int AsteroidNumber;
        AsteroidNumber = Random.Range(5, 50);
        for (int i = 0; i < AsteroidNumber; i++)
        {
            GameObject ThisAsteroid;
            int Rand = Random.Range(1, 5);
            if (Rand == 1)
            {
                ThisAsteroid =  Instantiate(Asteroid1, new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), Random.Range(0f, 10f)), Quaternion.identity);
            }
            else if(Rand == 2)
            {
                ThisAsteroid = Instantiate(Asteroid2, new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), Random.Range(0f, 10f)), Quaternion.identity);

            }
            else if(Rand == 3)
            {
                ThisAsteroid = Instantiate(Asteroid3, new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), Random.Range(0f, 10f)), Quaternion.identity);

            }
            else if (Rand == 4)
            {
                ThisAsteroid = Instantiate(Asteroid4, new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), Random.Range(0f, 10f)), Quaternion.identity);

            }
            else
            {
                ThisAsteroid = Instantiate(Asteroid5, new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), Random.Range(0f, 10f)), Quaternion.identity);

            }
            AstroPhysics ThisAAP =ThisAsteroid.GetComponent<AstroPhysics>();
            ThisAAP.ThisRealMass = Random.Range(1e10f, 5e10f);
            ThisAAP.ID = i;
        }
    }
}
