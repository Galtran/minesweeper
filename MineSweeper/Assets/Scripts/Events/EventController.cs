using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MineSweeperCore;

public class EventController
{

    #region Singleton implementation
    private static volatile EventController _instance;
    private static readonly object SyncRoot = new System.Object();

    private EventController()
    {

    }

    public static EventController Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new EventController();
                }
            }

            return _instance;
        }
    }
    #endregion

    #region События с игровым полем

    //Событие для инициализации и отрисовки игрового поля
    public delegate void _PrepareStartNewGame();
    public static event _PrepareStartNewGame OnPrepareStartNewGame;
    public void PrepareStartNewGame()
    {
        OnPrepareStartNewGame();
    }

    //Открыли первую клетку - началась игра
    public delegate void StartNewGame();
    public static event StartNewGame OnStartNewGame;

    //Изменилось состояние
    public delegate void _ChangeGameStatus(GameField.StatusGame status);
    public static event _ChangeGameStatus OnChangeGameStatus;
    public void ChangeGameStatus(GameField.StatusGame status)
    {
        OnChangeGameStatus(status);
    }

    //Перезапустить игру
    public delegate void _RestartGame();
    public static event _RestartGame OnRestartGame;
    public void RestartGame()
    {
        OnRestartGame();
    }
    #endregion


    #region События с клетками поля
    //Открыли клетку
    public delegate void OpenCell(Cell cell);
    public static event OpenCell OnOpenCell;

    //Поставили/сняли флаг
    public delegate void MarkCell(Cell cell);
    public static event MarkCell OnMarkCell;
    #endregion

    
}
