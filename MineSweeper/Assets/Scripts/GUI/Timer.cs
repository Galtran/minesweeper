using MineSweeperCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    private GameField.StatusGame currStatus = GameField.StatusGame.sgPAUSE;

    private float time = 0.0f;
    private bool needResetTimer = false;

    Text textTimer;

    void Start () {
        EventController.OnChangeGameStatus += OnChangeGameStatus;

        textTimer = GetComponent<Text>();
    }
	
	void Update () {
        if (currStatus == GameField.StatusGame.sgGAME)
            time += Time.deltaTime;

        textTimer.text = time.ToString("N2");
    }

    private void OnChangeGameStatus(GameField.StatusGame st)
    {
        currStatus = st;
        if (st == GameField.StatusGame.sgWIN || st == GameField.StatusGame.sgLOOSE)
            needResetTimer = true;

        if(st == GameField.StatusGame.sgPAUSE && needResetTimer)
        {
            needResetTimer = false;
            time = 0;
        }
    }
}
