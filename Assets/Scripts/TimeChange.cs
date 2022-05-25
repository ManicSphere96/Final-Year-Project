using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeChange : MonoBehaviour
{
    public float RateOfTime;
    // Update is called once per frame
    private void Awake()
    {
        Application.targetFrameRate = 60;

    }
    void Update()
    {
        Time.timeScale = RateOfTime;
                
    }
    
    
}
