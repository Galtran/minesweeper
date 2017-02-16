using MineSweeperCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainControllerScript : MonoBehaviour {

    private GameField.StatusGame currStatus = GameField.StatusGame.sgPAUSE;

    private void Awake()
    {

    }

    // Use this for initialization
    void Start () {
        EventController.OnChangeGameStatus += OnChangeGameStatus;

        EventController.Instance.PrepareStartNewGame();
    }

    // Update is called once per frame
    void Update () {
		
	}



    void OnGUI()
    {
        GUI.BeginGroup(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200));

        if (currStatus == GameField.StatusGame.sgLOOSE)
        {
            GUI.Label(new Rect(50, 10, 180, 30), "Кирдык, прориграли");
            if (GUI.Button(new Rect(10, 30, 180, 30), "Еще раз!"))
            {
                EventController.Instance.RestartGame();
            }
        }

        if (currStatus == GameField.StatusGame.sgWIN)
        {
            GUI.Label(new Rect(50, 10, 180, 30), "Успех!!");
            if (GUI.Button(new Rect(10, 30, 180, 30), "Еще раз!"))
            {
                EventController.Instance.RestartGame();
            }
        }

        /*GUI.Label(new Rect(50, 10, 180, 30), "Главное меню");
        if (GUI.Button(new Rect(10, 30, 180, 30), "Играть"))
        {
            currentWindow = 2;
        }
        if (GUI.Button(new Rect(10, 70, 180, 30), "Настройки"))
        {
            currentWindow = 3;
        }
        if (GUI.Button(new Rect(10, 110, 180, 30), "Об Игре"))
        {
            currentWindow = 4;
        }
        if (GUI.Button(new Rect(10, 150, 180, 30), "Выход"))
        {
            currentWindow = 5;
        }*/

        GUI.EndGroup();
    }

    private void OnChangeGameStatus(GameField.StatusGame st)
    {
        currStatus = st;
    }
}
