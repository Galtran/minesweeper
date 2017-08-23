using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace MineSweeperCore
{
    public class GameFieldSerializer
    {
        public GameFieldSerializer() { }

        public void Serialize(GameField gf)
        {
            FileStream fstream;
#if UNITY_ANDROID && !UNITY_EDITOR
            fstream = File.Open(Path.Combine(Application.persistentDataPath, "save.msw"), FileMode.Create);
#else
            fstream = File.Open(Path.Combine(Application.dataPath, "save.msw"), FileMode.Create);
#endif
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fstream, gf);
            fstream.Close();
        }

        public GameField DeSerialize()
        {
            FileStream fstream;
#if UNITY_ANDROID && !UNITY_EDITOR
            fstream = File.Open(Path.Combine(Application.persistentDataPath, "save.msw"), FileMode.Open);
#else
            fstream = File.Open(Path.Combine(Application.dataPath, "save.msw"), FileMode.Open);
#endif
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            GameField gf = (GameField)binaryFormatter.Deserialize(fstream);
            
            fstream.Close();

            gf.InitCellsNeighbors();
            return gf;
        }

        public GameField DeserializeByByteArray(byte[] file)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            memStream.Write(file, 0, file.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            GameField gf = (GameField)binaryFormatter.Deserialize(memStream);

            gf.InitCellsNeighbors();
            return gf;
        }
    }
}
