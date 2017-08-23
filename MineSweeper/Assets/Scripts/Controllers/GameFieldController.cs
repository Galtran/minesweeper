﻿using MineSweeperCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFieldController : MonoBehaviour {

    public int sizeX = 10;
    public int sizeY = 8;
    public int countMines = 1;

    public GameObject cellTemplate;


    private GameField _gf;
    private GameField.StatusGame currStatus = GameField.StatusGame.sgPAUSE;
    private List<GameObject> visualCells = new List<GameObject>();

    #region Base Unity methods
    private void Awake()
    {
        EventController.OnPrepareStartNewGame += PrepareGame;
        EventController.OnRestartGame += RecreateGameField;

        EventController.OnSaveGame += SaveGame;
        EventController.onLoadGame += LoadGame;
    }
    #endregion


    private void PrepareGame()
    {
        _gf = new GameField(sizeX, sizeY);
        _gf.InitializeGameField(countMines);

        CreateVisualField();
    }

    private void RecreateGameField()
    {
        _gf.ClearGameField();
        _gf.InitializeGameField(countMines);
        currStatus = GameField.StatusGame.sgPAUSE;

        EventController.Instance.ChangeGameStatus(currStatus);
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

        foreach(GameObject vc in visualCells)
        {
            Destroy(vc);
        }

        for (int i = 0; i < _gf.M; i++)
        {
            for (int k = 0; k < _gf.N; k++)
            {
                GameObject new_cell = (GameObject)Instantiate(cellTemplate, new Vector3((float)(i * 1.1) + start_pos_x + 1.5f, (float)((sizeY - k) * 1.1) - start_pos_y - 0.5f, 10), Quaternion.identity);
                visualCells.Add(new_cell);

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
        bool hasChanged = _gf.ClickCell(x, y, isLeft);
        GameField.StatusGame newStatus = _gf.GameStatus;
        if (newStatus != currStatus)
            EventController.Instance.ChangeGameStatus(newStatus);

        currStatus = newStatus;

        if (currStatus == GameField.StatusGame.sgLOOSE)
        {
            _gf.OpenAllMines();
        }
    }

    private void SaveGame()
    {
        GameFieldSerializer gfs = new GameFieldSerializer();
        gfs.Serialize(_gf);
    }

    private void LoadGame(string level)
    {
        GameFieldSerializer gfs = new GameFieldSerializer();

        if(level == "last")
            _gf = gfs.DeSerialize();
        else _gf = gfs.DeserializeByByteArray(LevelsManager.GetBegLevel());

        CreateVisualField();
    }
}
