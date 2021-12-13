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
    static private MissionDemolition S; // Скрытый объект-одиночка

    [Header("Set in Inspector")]
    public Text TextLevel; // ссылка на объект UITextLevel
    public Text TextShots; // ссылка на объект UITextShots
    public Text TextButton; // ссылка на дочерний объект Text в UIButtonView
    public Vector3 CastlePos; // Местоположение замка
    public GameObject[] Castles; // Массив замков

    [Header("Set Dynamically")]
    public GameObject Castle; // Текущий замок
    public GameMode Mode = GameMode.Idle;
    public int Level; // Текущий уровень
    public int LevelMax; // Количество уровней
    public int ShotsTaken;
    public string Showing = "Show Slingshot"; // Режим FollowCam 

    // Статический метод, позволяющий из любого кода увеличить ShotsTaken
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
        S = this; // Определить объект-одиночку

        Level = 0;
        LevelMax = Castles.Length;
        StartLevel();
    }

    private void Update()
    {
        UpdateGUI();

        // Проверить завершение уровня
        if ((Mode == GameMode.Playing) && Goal.GoalMet)
        {
            // Изменить режим, чтобы прекратить проверку завершения уровня
            Mode = GameMode.LevelEnd;
            // Уменьшить масштаб
            SwitchView("Show Both");
            // Начать новый уровень через 2 секунды
            Invoke("NextLevel", 2f);
        }
    }

    private void StartLevel()
    {
        // Уничтожить прежний замок, если он существует
        if (Castle != null)
        {
            Destroy(Castle);
        }

        // Уничтожить прежние снаряды, если они существуют
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        // Создать новый замок
        Castle = Instantiate<GameObject>(Castles[Level]);
        Castle.transform.position = CastlePos;
        ShotsTaken = 0;

        // Переустановить камеру в начальную позицию
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        // Сбросить цель
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
        // Показать данные в элементах ПИ (пользовательского интерфейса)
        TextLevel.text = "Level: " + (Level + 1) + " of " + LevelMax;
        TextShots.text = "Shots Taken: " + ShotsTaken;
    }
}