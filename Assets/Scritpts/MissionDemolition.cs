using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}
public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; //private single-obj

    [Header("Set in Inspector")]
    public Text uitLevel; //url to UIText_Level
    public Text uitShots; //url to UIText_Shots
    public Text uitButton; // url to UIButton_View's child Text
    public Vector3 castlePos; //Pos of castle
    public GameObject[] castles; //Massive of castles

    [Header("Set Dynamically")]
    public int level; // curent lvl
    public int levelMax; //amount of lvls
    public int shotsTaken;
    public GameObject castle; // vurrent castle
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";

    void Start()
    {
        S = this;
        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel()
    {
        //destroy last castle if it was
        if (castle != null)
        {
            Destroy(castle);
        }
        //destroy previous projectiles
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        //create new castle
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        //reposition camera in starting pos
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        //breake goal
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }

    void UpdateGUI()
    {
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    void Update()
    {
        UpdateGUI();

        //Chek the ending of lvl
        if ( (mode == GameMode.playing) && Goal.goalMet)
        {
            mode = GameMode.levelEnd;
            SwitchView("Show Both");
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel()
    {
        level++;
        if(level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }

    public void SwitchView(string eView = "")
    {
        if (eView == "")
        {
            eView = uitButton.text;
        }
        showing = eView;
        switch (showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;

            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;

            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }
    }

    public static void ShotFired()
    {
        S.shotsTaken++;
    }
}
