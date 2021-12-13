using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    Idle,
    Playing,
    LevelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; // ������� ������-��������

    [Header("Set in Inspector")]
    public Text TextLevel; // ������ �� ������ UITextLevel
    public Text TextShots; // ������ �� ������ UITextShots
    public Text TextButton; // ������ �� �������� ������ Text � UIButtonView
    public Vector3 CastlePos; // �������������� �����
    public GameObject[] Castles; // ������ ������

    [Header("Set Dynamically")]
    public GameObject Castle; // ������� �����
    public GameMode Mode = GameMode.Idle;
    public int Level; // ������� �������
    public int LevelMax; // ���������� �������
    public int ShotsTaken;
    public string Showing = "Show Slingshot"; // ����� FollowCam 

    // ����������� �����, ����������� �� ������ ���� ��������� ShotsTaken
    public static void ShotFired()
    {
        S.ShotsTaken++;
    }

    public void SwitchView(string eView)
    {
        if (eView == "")
        {
            eView = TextButton.text;
        }
        Showing = eView;
        switch (Showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                TextButton.text = "Show Castle";
                break;

            case "Show Castle":
                FollowCam.POI = S.Castle;
                TextButton.text = "Show Both";
                break;

            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                TextButton.text = "Show Slingshot";
                break;
        }
    }

    private void Start()
    {
        S = this; // ���������� ������-��������

        Level = 0;
        LevelMax = Castles.Length;
        StartLevel();
    }

    private void Update()
    {
        UpdateGUI();

        // ��������� ���������� ������
        if ((Mode == GameMode.Playing) && Goal.GoalMet)
        {
            // �������� �����, ����� ���������� �������� ���������� ������
            Mode = GameMode.LevelEnd;
            // ��������� �������
            SwitchView("Show Both");
            // ������ ����� ������� ����� 2 �������
            Invoke("NextLevel", 2f);
        }
    }

    private void StartLevel()
    {
        // ���������� ������� �����, ���� �� ����������
        if (Castle != null)
        {
            Destroy(Castle);
        }

        // ���������� ������� �������, ���� ��� ����������
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        // ������� ����� �����
        Castle = Instantiate<GameObject>(Castles[Level]);
        Castle.transform.position = CastlePos;
        ShotsTaken = 0;

        // �������������� ������ � ��������� �������
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        // �������� ����
        Goal.GoalMet = false;

        UpdateGUI();

        Mode = GameMode.Playing;
    }

    private void NextLevel()
    {
        Level++;
        if (Level == LevelMax)
        {
            Level = 0;
        }
        StartLevel();
    }

    private void UpdateGUI()
    {
        // �������� ������ � ��������� �� (����������������� ����������)
        TextLevel.text = "Level: " + (Level + 1) + " of " + LevelMax;
        TextShots.text = "Shots Taken: " + ShotsTaken;
    }
}