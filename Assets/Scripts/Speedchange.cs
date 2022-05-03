using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedchange : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int buttonpressnum = 0;
    public GameObject PauseMenu;

    // Update is called once per frame
    void Update()
    {
        if (buttonpressnum > 3)
        {
            buttonpressnum = 0;
        }
        if (PauseMenu.activeInHierarchy)
        {
            Time.timeScale = 0f;
        }
        else
        {
            switch (buttonpressnum)
            {
                case 0:
                    Time.timeScale = 0.25f;
                    GameObject.Find("Game speed").GetComponentInChildren<Text>().text = "0.25x";
                    break;
                case 1:
                    Time.timeScale = 1f;
                    GameObject.Find("Game speed").GetComponentInChildren<Text>().text = "1x";
                    break;
                case 2:
                    Time.timeScale = 10f;
                    GameObject.Find("Game speed").GetComponentInChildren<Text>().text = "10x";
                    break;
                case 3:
                    Time.timeScale = 50f;
                    GameObject.Find("Game speed").GetComponentInChildren<Text>().text = "50x";
                    break;

            }
        }
    }
    
    public void BUttonpressed ()
    {
        buttonpressnum++;
        
       
    }
}
