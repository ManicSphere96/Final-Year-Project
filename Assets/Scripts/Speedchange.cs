using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedchange : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int buttonpressnum = 0;

    // Update is called once per frame
    void Update ()
    {
        if (buttonpressnum > 3)
        {
            buttonpressnum = 0;
        }
        switch (buttonpressnum)
        {
            case 0:
                Time.timeScale = 1f;
                GameObject.Find("Game speed").GetComponentInChildren<Text>().text = "1x";
                break;
            case 1:
                Time.timeScale = 10f;
                GameObject.Find("Game speed").GetComponentInChildren<Text>().text = "10x";
                break;
            case 2:
                Time.timeScale = 50f;
                GameObject.Find("Game speed").GetComponentInChildren<Text>().text = "50x";
                break;
            case 3:
                Time.timeScale = 0.5f;
                GameObject.Find("Game speed").GetComponentInChildren<Text>().text = "0.5x";
                break;
        }
    }
    
    public void BUttonpressed ()
    {
        buttonpressnum++;
        
       
    }
}
