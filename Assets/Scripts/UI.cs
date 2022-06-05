using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameObject TextPanel;
    public Text Name;
    public Text Designation;
    public Text Mass;
    public Text Diameter;
    public Text NumOfMoons;
    public Text Inhabited;
    public Text ResourcesAvailable;
    public Text PercentCollected;
    public GameObject EndPanel;
    public Text ResourcesCollected, PercentageRemaining, NumberPlanetsDestroyed, TimeTaken;
    public Text TimerText;
    public Text EndPanelText;
    public int PlanetsDestroyed = 0, TotalResources = 0;
    public float Seconds, Minutes, LevelStartTime, CountdownFromSecs;
    public bool CountingDown;

    // Start is called before the first frame update
    void Start()
    {
        LevelStartTime = Time.realtimeSinceStartup;
         
    }

    // Update is called once per frame
    void Update()
    {
        if (!CountingDown)
        {
            Minutes = (int)((Time.realtimeSinceStartup - LevelStartTime) / 60);
            Seconds = (int)((Time.realtimeSinceStartup - LevelStartTime) % 60);
            TimerText.text = Minutes.ToString("00") + ":" + Seconds.ToString("00");
        }
        else
        {
            Minutes = (int)((CountdownFromSecs - (Time.realtimeSinceStartup - LevelStartTime)) / 60);
            Seconds = (int)((CountdownFromSecs -(Time.realtimeSinceStartup - LevelStartTime)) % 60);
            TimerText.text = Minutes.ToString("00") + ":" + Seconds.ToString("00");
            if ((int)(Time.realtimeSinceStartup - LevelStartTime) == CountdownFromSecs)
            {
                EndUIText();
                EndPanel.SetActive(true);
                EndPanelText.text = "You FAILED the species called for aid and their army arrived";
            }
        }
    }
    public void UpdateUIText(Planet ThePlanet, AstroPhysics PlanetAP)
    {
        
        Name.text ="Unknown";
        Designation.text = PlanetAP.ID.ToString();
        Mass.text =PlanetAP.ThisRealMass.ToString();
        Diameter.text = PlanetAP.RealDiameter.ToString();
        NumOfMoons.text = ThePlanet.NumOfMoons.ToString();
        Inhabited.text = ThePlanet.IsPlanetInhabited.ToString();
        ResourcesAvailable.text = ThePlanet.InitNumberOfResources.ToString();
        PercentCollected.text = ((((float)ThePlanet.InitNumberOfResources-((float)ThePlanet.ResourceANumber + (float)ThePlanet.ResourceBNumber + (float)ThePlanet.ResourceCNumber + (float)ThePlanet.ResourceDNumber))/(float)ThePlanet.InitNumberOfResources)*100).ToString("00"); 

    }
    public void EndUIText()
    {
        Planet[] Planets = FindObjectsOfType<Planet>();
        int Remain = 0;
        for (int i = 0; i < Planets.Length; i++)
        {
            Remain += Planets[i].RemainingResources;
        }
        ResourcesCollected.text = (TotalResources - Remain).ToString();
        PercentageRemaining.text = (((float)Remain / (float)TotalResources) * 100).ToString("00") + "%";
        NumberPlanetsDestroyed.text = PlanetsDestroyed.ToString();
        if (CountingDown)
        {
            Minutes = (int)((Time.realtimeSinceStartup - LevelStartTime) / 60);
            Seconds = (int)((Time.realtimeSinceStartup - LevelStartTime) % 60);
            TimeTaken.text = TimerText.text = Minutes.ToString("00") + ":" + Seconds.ToString("00");
        }
        else
        {
            TimeTaken.text = TimerText.text;
        } 
    }
}

