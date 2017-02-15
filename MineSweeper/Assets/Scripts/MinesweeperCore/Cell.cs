using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MineSweeperCore
{
    /**
     * класс для работы с одной клеткой на игровом поле
     */
    [Serializable]
    public class Cell : ISerializable
    {
        //Тип клетки
        public enum CellType
        {
            ctNONE,     //неактивная клетка (для непрямоугольных полей)
            ctMINE,     //мина
            ctEMPTY,    //Пустая
            ctNUMBER,   //число мин вокруг
        }

        //Данная клетка имеет такой тип
        private CellType typeCell;
        public CellType TypeCell
        {
            get { return typeCell; }
            set { typeCell = value; }
        }

        //Для удобства вычислений каждая клетка хранит ссылки на своих соседей
        private List<Cell> neighbors;
        public List<Cell> Neighbors
        {
            get { return neighbors; }
        }

        //Количство заминированных соседей
        private int neighborsMine;
        public int NeighborsMine
        {
            get 
            {
                if (neighborsMine == -1)
                    CalcNeighborsMine();

                return neighborsMine; 
            }
        }

        //Количство соседей, отмеченных флагом
        public int NeighborsMarked
        {
            get
            {
                int neighborsMark = 0;
                foreach (Cell c in neighbors)
                {
                    if (c.IsMark)
                        neighborsMark++;
                }
                return neighborsMark;
            }
        }

        //Открыта ли сейчас клетка
        private bool isOpen;
        public bool IsOpen
        {
            get { return isOpen; }
            set { isOpen = value; }
        }

        //Отмечена ли клетка флажком
        private bool isMark;
        public bool IsMark
        {
            get { return isMark; }
            set { isMark = value; }
        }

        //клетка-не клетка
        public bool IsNoneType
        {
            get { return typeCell == CellType.ctNONE; }
        }

        //данный конструктор необходим для сериализации.
        public Cell(SerializationInfo sInfo, StreamingContext contextArg)
        {
            this.isOpen = (bool)sInfo.GetValue("is_open", typeof(bool));
            this.isMark = (bool)sInfo.GetValue("is_mark", typeof(bool));
            this.typeCell = (CellType)sInfo.GetValue("cell_type", typeof(CellType));

            InitCell();
        }

        public void GetObjectData(SerializationInfo s_info, StreamingContext contextArg)
        {
            s_info.AddValue("is_open", this.isOpen);
            s_info.AddValue("is_mark", this.isMark);
            s_info.AddValue("cell_type", this.typeCell);
        }

        public Cell()
        {
            InitCell();
            TypeCell = CellType.ctEMPTY;
        }

        public Cell(CellType ct)
        {
            InitCell();
            TypeCell = ct;
        }

        //Добавить клетку в список соседей
        public void AddNeighbor(Cell cell)
        {
            if( cell != null )
                neighbors.Add(cell);
        }

        //метод для инициализации внутренних полей
        public void InitCell()
        {
            neighbors = new List<Cell>();
            neighborsMine = -1;
        }

        //метод для подсчета количества заминированных соседей
        private void CalcNeighborsMine()
        {
            neighborsMine = 0;
            foreach (Cell c in neighbors)
            {
                if (c.TypeCell == CellType.ctMINE)
                    neighborsMine++;
            }

            if (this.TypeCell != CellType.ctMINE && this.NeighborsMine > 0)
                this.TypeCell = CellType.ctNUMBER;
        }
    }
}
