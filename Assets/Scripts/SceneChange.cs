using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // Start is called before the first frame update

    public string SceneToLoad;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ButtonPush()
    {
        SceneManager.LoadScene(SceneToLoad);
    }
    public void ExitGame ()
    {
        Application.Quit();
    }
}
