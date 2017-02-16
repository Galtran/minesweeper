using MineSweeperCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    private GameField.StatusGame currStatus = GameField.StatusGame.sgPAUSE;

    private float time = 0;


    void Start () {
        EventController.OnChangeGameStatus += OnChangeGameStatus;
    }
	
	void Update () {
        if (currStatus == GameField.StatusGame.sgGAME)
            time += Time.deltaTime;
    }

    void OnGUI()
    {
        GUI.BeginGroup(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 200, 200, 200));

        GUI.Label(new Rect(50, 10, 180, 30), "Время игры: " + time.ToString());

        GUI.EndGroup();
    }

    private void OnChangeGameStatus(GameField.StatusGame st)
    {
        currStatus = st;
    }
}
