using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public int currentWindow;

    // Use this for initialization
    void Start () {
        currentWindow = 1;
    }

    void OnGUI () {
        GUI.BeginGroup(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200));

        GUI.Label(new Rect(50, 10, 180, 30), "Главное меню");
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
        }

        GUI.EndGroup();
    }
}
