using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;
using System.Text.RegularExpressions;

public class XMLManager : MonoBehaviour
{
    public static XMLManager Instance;

    public LevelInfo info;

    //private List<List<int[,]>> m_matrixes;

    //public List<List<int>> TurnsForStars;

    private List<string> m_lines;

    //public MatrixesDatabase Matrixes;

    //public List<List<List<int>>> Matrixes;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
                Destroy(gameObject);
        }  
    }

    private void Start()
    {
        //GetDataFromTxt(out Controller.Instance.Level.Matrixes, out Controller.Instance.Level.LevelTurnsList);
    }

    //public void func( List<int> x,  List<int> y)
    //{
    //    x = new List<int>();
    //    y = new List<int>();
    //    x.Add(5);
    //    y.Add(7);
    //}

    //private void CreateTxt()
    //{
    //    StreamWriter writer = new StreamWriter(Application.dataPath + "/StreamingAssets/Matrixes.txt", true);
    //}

    private void ReadFromScript()
    {
        string[] lines = Matrixes.matrixes.Split('\n');
        m_lines = new List<string>();

        foreach (String line in lines)
            m_lines.Add(line);
    }

    private void ReadTxt()
    {
        StreamReader stream = new StreamReader(Application.dataPath + "/StreamingAssets/Matrixes.txt");
        m_lines = new List<string>();
        string line;

        while ((line = stream.ReadLine()) != null)
        {
            Debug.Log(line);
            m_lines.Add(line);
        }
    }

    public void GetDataFromTxt(List<List<int[,]>> matrixes, List<List<LevelTurns>> levelTurnsList)
    {
        //ReadTxt();
        ReadFromScript();
        
        int startCount = 0, levelsCount = 0;
        int difficulty = Constants.EASY - 1;
       
        for (int i = 0; i < m_lines.Count; i++)
        {
            if (m_lines[i] == "")
            {
                int size = i - startCount;
                // Пустая строка после названия уровня сложности
                if (size <= 1)
                {
                    startCount = i + 1;
                    // Создаем список матриц данного уровня сложности
                    matrixes.Add(new List<int[,]>());
                    // Создаем список допустимых ходов для данного уровня сложности
                    levelTurnsList.Add(new List<LevelTurns>());
                    levelsCount = 0;
                    difficulty++;
                }
                else
                {
                    LevelTurns levelTurns = new LevelTurns();
                    levelTurns.ThreeStars = System.Convert.ToInt32(m_lines[i - 1]);
                    levelTurns.TwoStars = System.Convert.ToInt32(m_lines[i - 2]);
                    levelTurns.OneStar = System.Convert.ToInt32(m_lines[i - 3]);
                    levelTurnsList[difficulty].Add(levelTurns);

                    size -= Controller.Instance.MaxStars;

                    matrixes[difficulty].Add(new int[size, size]);
                  

                    for (int y = 0; y < size; y++)
                    {
                        string[] row = m_lines[startCount + y].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        for (int x = 0; x < size; x++)
                        {
                            int elem;
                            Int32.TryParse(row[x], out elem);
                            matrixes[difficulty][levelsCount][y, x] = elem;
                        }
                    }
                    levelsCount++;
                    startCount = i + 1;
                }
            }
        }
    }



    //private List<List<int[,]>> InitMatrixes()
    //{

    //    Matrixes = new List<List<int[,]>>();
    //    for (int i = 0; i < 3; i++)
    //    {
    //        Matrixes.Add(new List<int[,]>());
    //    }

    //    int[,] Matrix0 =
    //    {
    //        {2, 0, 1 },
    //        {1, 2, 1 },
    //        {2, 2, 1 }
    //    };

    //    int[,] Matrix1 =
    //    {
    //        {2, 1, 1 },
    //        {1, 2, 1 },
    //        {2, 2, 0 }
    //    };

    //    Matrixes[0].Add(Matrix0);
    //    Matrixes[0].Add(Matrix1);

    //    return Matrixes;
    //}

    //private void CreateXML()
    //{
    //    //List<List<int[,]>> Matrixes = InitMatrixes();

    //    //for (int i = 0; i < 3; i++)
    //    //{
    //    //    Matrixes.database.Add(new List<int[,]>());
    //    //}


    //    int[,] Matrix0 =
    //    {
    //                {2, 0, 1 },
    //                {1, 2, 1 },
    //                {2, 2, 1 }
    //            };

    //    int[,] Matrix1 =
    //    {
    //                {2, 1, 1 },
    //                {1, 2, 1 },
    //                {2, 2, 0 }
    //            };

    //    Matrixes = new List>();
    //    //Matrixes.database[0].Add(Matrix0);
    //    //Matrixes.database[0].Add(Matrix1);
    //    Matrixes.Add(Matrix0);
    //    Matrixes.Add(Matrix1);

    //    XmlSerializer serializer = new XmlSerializer(typeof(List<int[,]>));
    //    FileStream stream = new FileStream(Application.dataPath + "/StreamingAssets/Data.xml", FileMode.Create);
    //    serializer.Serialize(stream, Matrixes);
    //    stream.Close();
    //}

    public void SaveData(string difficulty, int level, int turns, int maxTurns, bool addPlus = false)
    {
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/PuzzleInfo.txt", true);

        if (addPlus == true)
            writer.WriteLine(difficulty + " " + level + " " + turns + "+/" + maxTurns);
        else
            writer.WriteLine(difficulty + " " + level + " " + turns + "/" + maxTurns);

        writer.Close();
    }

    public void SaveData2(int fieldSize, int freeSpace, int tokenTypes, int turns, int maxTurns, bool addPlus = false)
    {
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/PuzzleInfo.txt", true);

        if (addPlus == true)
            writer.WriteLine(fieldSize + " " + freeSpace + " " + tokenTypes + " " + turns + "+/" + maxTurns);
        else
            writer.WriteLine(fieldSize + " " + freeSpace + " " + tokenTypes + " " + turns + "/" + maxTurns);

        writer.Close();
    }

    public void SaveLevelInfo()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(LevelInfo));
        FileStream stream = new FileStream(Application.dataPath + "/StreamingAssets/Data.xml", FileMode.Append);
        serializer.Serialize(stream, info);
        stream.Close();
    }

}

[System.Serializable]
public class LevelInfo
{
    public string difficulty;
    public int level;
    public int turns;
}

[System.Serializable]
public class LevelDatabase
{
    public List<LevelInfo> database = new List<LevelInfo>();
}

[System.Serializable]
public class MatrixesDatabase
{
    public List<List<int[,]>> database = new List<List<int[,]>>();
}

