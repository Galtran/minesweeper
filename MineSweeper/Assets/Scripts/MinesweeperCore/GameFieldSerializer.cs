using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MineSweeperCore
{
    public class GameFieldSerializer
    {
        public GameFieldSerializer() { }

        public void Serialize(GameField gf)
        {
            FileStream fstream = File.Open(@"save.msw", FileMode.Create);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fstream, gf);
            fstream.Close();
        }

        public GameField DeSerialize()
        {
            FileStream fstream = File.Open(@"save.msw", FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            GameField gf = (GameField)binaryFormatter.Deserialize(fstream);
            fstream.Close();

            gf.InitCellsNeighbors();
            return gf;
        }
    }
}
