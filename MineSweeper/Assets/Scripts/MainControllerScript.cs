using MineSweeperCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainControllerScript : MonoBehaviour {

    public int sizeX = 10;
    public int sizeY = 8;
    public int countMines = 10;

    public GameObject cellTemplate;
    public GameObject basePlane;

    private GameField _gf;
    private GameField.StatusGame currStatus = GameField.StatusGame.sgGAME;


    private void Awake()
    {
        //SetUniform();

        if (!cellTemplate)
        {
            Debug.LogError("cellTemplate is empty !");
        }

        _gf = new GameField(sizeX, sizeY);
        _gf.InitializeGameField(countMines);

        //_gf.GetCell(0, 0).TypeCell = Cell.CellType.ctNONE;
        //_gf.GetCell(9, 0).TypeCell = Cell.CellType.ctNONE;
    }

    // Use this for initialization
    void Start () {
        //Создаем игровое поле
        CreateVisualField();
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void CreateVisualField()
    {
        Camera camera = Camera.main;
        float width = camera.pixelWidth;
        float height = camera.pixelHeight;

        Vector2 bottomLeft = camera.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 bottomRight = camera.ScreenToWorldPoint(new Vector2(width, 0));
        Vector2 topLeft = camera.ScreenToWorldPoint(new Vector2(0, height));
        Vector2 topRight = camera.ScreenToWorldPoint(new Vector2(width, height));

        float start_pos_x = topLeft.x;
        float start_pos_y = topLeft.y;

        for (int i = 0; i < sizeX; i++) {
            for (int k = 0; k < sizeY; k++)
            {
                GameObject new_cell = (GameObject)Instantiate(cellTemplate, new Vector3((float)(i * 1.1) + start_pos_x + 1.5f, (float)((sizeY - k) * 1.1) - start_pos_y - 0.5f, 10), Quaternion.identity);

                CellController cc = new_cell.GetComponent<CellController>();
                cc.InitController(_gf.GetCell(i, k));
                int _i = i;
                int _k = k;
                cc.onCellClick += (isLeft) => { onClickCellHandler(_i, _k, isLeft); };

            }
        }
    }

    private void onClickCellHandler(int x, int y, bool isLeft)
    {
        _gf.ClickCell(x, y, isLeft);
        currStatus = _gf.GameStatus;

        if(currStatus == GameField.StatusGame.sgLOOSE)
        {
            _gf.OpenAllMines();
        }
    }

    void OnGUI()
    {
        GUI.BeginGroup(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200));

        if (currStatus == GameField.StatusGame.sgLOOSE)
        {
            GUI.Label(new Rect(50, 10, 180, 30), "Кирдык, прориграли");
            if (GUI.Button(new Rect(10, 30, 180, 30), "Еще раз!"))
            {
                _gf.ClearGameField();
                _gf.InitializeGameField(countMines);
                currStatus = _gf.GameStatus;
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


    private void SetUniform()
    {
        Camera camera = Camera.main;
        float orthographicSize = camera.pixelHeight / 2;
        if (orthographicSize != camera.orthographicSize)
            camera.orthographicSize = orthographicSize;
    }
}
