using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MineSweeperCore
{
    /**
     * для хранения данных о игровом поле
     */
    [Serializable]
    public class GameField : ISerializable
    {
        //ВОзможные состояния игры
        public enum StatusGame
        {
            sgPAUSE,    //Игра еще не началась
            sgGAME,     //Игра идет
            sgWIN,      //мы выйграли
            sgLOOSE,    //мы проиграли
        }

        private Cell[,] gameField;

        private int m;
        public int M
        {
            get { return m; }
        }

        private int n;
        public int N
        {
            get { return n; }
        }

        private StatusGame gameStatus;
        public StatusGame GameStatus
        {
            get { gameStatus = CalcGameStatus(); return gameStatus; }
        }

        //данный конструктор необходим для сериализации.
        public GameField(SerializationInfo sInfo, StreamingContext contextArg)
        {
            this.m = (int)sInfo.GetValue("M", typeof(int));
            this.n = (int)sInfo.GetValue("N", typeof(int));
            this.gameStatus = (StatusGame)sInfo.GetValue("game_status", typeof(StatusGame));
            this.gameField = (Cell[,])sInfo.GetValue("game_field", typeof(Cell[,]));
       }

       public void GetObjectData(SerializationInfo s_info, StreamingContext contextArg)
       {
           s_info.AddValue("M", this.M);
           s_info.AddValue("N", this.N);
           s_info.AddValue("game_status", this.GameStatus);
           s_info.AddValue("game_field", this.gameField);
       }

        //m - высота игрового поля(количество строк), n - ширина игрового поля(количество столбцов).
        public GameField(int _m, int _n)
        {
            m = _m;
            n = _n;

            gameField = new Cell[m, n];

            for( int i = 0; i < m; i++ )
                for (int k = 0; k < n; k++)
                    gameField[i, k] = new Cell();

            gameStatus = StatusGame.sgPAUSE;
        }

        public bool InitializeGameField(int count_mines)
        {
            gameStatus = StatusGame.sgGAME;

            if (m * n <= count_mines)
                return false;

            GenerateMines(count_mines);
            InitCellsNeighbors();

            return true;
        }

        public void ClearGameField()
        {
            for (int i = 0; i < m; i++)
                for (int k = 0; k < n; k++)
                {
                    gameField[i, k].IsMark = false;
                    gameField[i, k].IsOpen = false;
                    gameField[i, k].TypeCell = Cell.CellType.ctEMPTY;
                    gameField[i, k].InitCell();
                }
        }


        //Метод обрабатывае клик мышкой по клетке с координатами (х; у)
        //параметр left == true, если кликнули левой кнопкой, иначе == false
        //Возвращает true, если требуется перерисовка игрового поля.
        public bool ClickCell(int x, int y, bool left)
        {
            //Если мы уже проиграли - больше ничего открывать нельзя.
            if (GameStatus == StatusGame.sgLOOSE)
                return false;

            Cell cell = GetCell(x, y);
            //Клетки с такими координатами не существует.
            if (cell == null)
                return false;

            //Если кликнули правой кнопкой
            if (!left)
            {
                //по закрытой клетке - поставим или снимем флажок.
                if (!cell.IsOpen)
                {
                    cell.IsMark = !cell.IsMark;
                    return true;
                }
                else
                    return false;   //Иначе ничего делать не надо.
            }
            else
            {
                //Если кликнули левой кнопкой по помеченной клетке - нчиего не делаем
                if (cell.IsMark)
                    return false;

                //Если клетка открыта и вокруг стоит нужное число флагов - открываем все остальное
                if (cell.IsOpen && cell.TypeCell == Cell.CellType.ctNUMBER && cell.NeighborsMine == cell.NeighborsMarked)
                {
                    OpenNotMarkedNeighbors(cell);
                    gameStatus = CalcGameStatus();
                    return true;
                }

                //Клетка закрыта и не помечена флагом. Если кликнули по бомбе - конец игры(
                if (cell.TypeCell == Cell.CellType.ctMINE)
                {
                    gameStatus = StatusGame.sgLOOSE;
                    cell.IsOpen = true;
                    return true;
                }

                //Если в клетке цифра - просто откроем ее.
                if (cell.TypeCell == Cell.CellType.ctNUMBER)
                {
                    cell.IsOpen = true;
                    return true;
                }

                //Если в клетке ничего нет - надо открыть большую область
                OpenNeighbors(cell);

            }
            return true;
        }

        //Метод для открытия всех мин на поле
        public void OpenAllMines()
        {
            for (int i = 0; i < m; i++)
                for (int k = 0; k < n; k++)
                    if(gameField[i, k].TypeCell == Cell.CellType.ctMINE)
                        gameField[i, k].IsOpen = true;

        }

        public int CountMines()
        {
            int res = 0;
            for (int i = 0; i < m; i++)
                for (int k = 0; k < n; k++)
                    if (gameField[i, k].TypeCell == Cell.CellType.ctMINE)
                        res++;

            return res;
        }

        //возвращает количество клеток, которые еще нужно отметить флажком (колВоБомб - колВоОтмеченных)
        public int NeedMarkCell()
        {
            int res = 0;
            for (int i = 0; i < m; i++)
                for (int k = 0; k < n; k++)
                {
                    Cell cell = gameField[i, k];
                    if (cell.TypeCell == Cell.CellType.ctMINE)
                        res++;
                    if (cell.IsMark)
                        res--;
                }
            return res;
        }

        //Метод для открытия неотмеченных соседей клетки cell
        private void OpenNotMarkedNeighbors(Cell cell)
        {
            foreach (Cell tmp_cell in cell.Neighbors)
            {
                if (tmp_cell.IsMark || tmp_cell.IsOpen)
                    continue;
                OpenNeighbors(tmp_cell);
            }
        }

        //Метод для открытия пустой области вокруг клетки cell
        private void OpenNeighbors(Cell cell)
        {
            List<Cell> cells = new List<Cell>();
            cells.Add(cell);

            while (cells.Count > 0)
            {
                Cell tmp_cell = cells.ElementAt<Cell>(0);
                cells.RemoveAt(0);

                if (tmp_cell.IsOpen || tmp_cell.IsMark)
                    continue;
                if (tmp_cell.TypeCell == Cell.CellType.ctNUMBER || tmp_cell.TypeCell == Cell.CellType.ctMINE)
                    tmp_cell.IsOpen = true;
                if (tmp_cell.TypeCell == Cell.CellType.ctEMPTY)
                {
                    tmp_cell.IsOpen = true;
                    cells.AddRange(tmp_cell.Neighbors);
                }
            }
        }

        //получить клетку с координатами (x; y). х - номер строки, y - номер столбца
        //Если клетки по данным координатам не существует - получим null
        public Cell GetCell(int x, int y)
        {
            if (x < 0 || x >= m || y < 0 || y >= n)
                return null;

            return gameField[x, y];
        }

        public void PrintField()
        {
            for (int i = 0; i < m; i++)
            {
                for (int k = 0; k < n; k++)
                {
                    Cell cell = gameField[i, k];
                    switch (cell.TypeCell)
                    {
                        case Cell.CellType.ctMINE: Console.Write("* "); break;
                        case Cell.CellType.ctEMPTY: Console.Write("  "); break;
                        case Cell.CellType.ctNUMBER: Console.Write(cell.NeighborsMine.ToString() + " "); break;
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            for (int i = 0; i < m; i++)
            {
                for (int k = 0; k < n; k++)
                {
                    Cell cell = gameField[i, k];
                    if (!cell.IsOpen)
                    {
                        Console.Write("# ");
                        continue;
                    }
                    switch (cell.TypeCell)
                    {
                        case Cell.CellType.ctMINE: Console.Write("* "); break;
                        case Cell.CellType.ctEMPTY: Console.Write("  "); break;
                        case Cell.CellType.ctNUMBER: Console.Write(cell.NeighborsMine.ToString() + " "); break;
                    }
                }
                Console.WriteLine();
            }
        }

        //Анализ игровой ситауции. Возвращает текущее состояние
        private StatusGame CalcGameStatus()
        {
            StatusGame sg = StatusGame.sgWIN;
            for( int i = 0; i < m; i++ )
                for (int k = 0; k < n; k++)
                {
                    Cell cell = gameField[i, k];
                    //Если наткнулись на открытую клетку с миной - эт проигрыш без вопросов.
                    if (cell.TypeCell == Cell.CellType.ctMINE && cell.IsOpen)
                        return StatusGame.sgLOOSE;

                    //если нашли путсую клетку или с цифрой, но она не открыта - эт точно не победа.
                    if ((cell.TypeCell == Cell.CellType.ctEMPTY || cell.TypeCell == Cell.CellType.ctNUMBER) &&
                        !cell.IsOpen)
                        sg = StatusGame.sgGAME;
                }

            return sg;
        }

        //проинициализировать у каждой клетки игрового поля список соседей
        public void InitCellsNeighbors()
        {
            for( int i = 0; i < m; i++ )
                for (int k = 0; k < n; k++)
                {
                    Cell cell = gameField[i, k];

                    //добавляем соседей.
                    cell.AddNeighbor(GetCell(i - 1, k));    //верхний
                    cell.AddNeighbor(GetCell(i - 1, k + 1));    //верхний правый
                    cell.AddNeighbor(GetCell(i, k + 1));    //правый
                    cell.AddNeighbor(GetCell(i + 1, k + 1));    //нижний правый
                    cell.AddNeighbor(GetCell(i + 1, k));    //нижний
                    cell.AddNeighbor(GetCell(i + 1, k - 1));    //нижний левый
                    cell.AddNeighbor(GetCell(i, k - 1));    //левый
                    cell.AddNeighbor(GetCell(i - 1, k - 1));    //верхний левый

                    int mines = cell.NeighborsMine;
                }
        }

        //Метод для генерации на поле нужного количества мин
        private void GenerateMines(int count_mines)
        {
            Random rnd = new Random();
            for (int i = 0; i < count_mines; )
            {
                //попробуем поставить мину в клетку (x; y)
                int x = rnd.Next(m);
                int y = rnd.Next(n);

                //Нельзя, там уже и так стоит мина
                if (gameField[x, y].TypeCell == Cell.CellType.ctMINE)
                    continue;

                gameField[x, y].TypeCell = Cell.CellType.ctMINE;
                i++;
            }
        }
    }
}
