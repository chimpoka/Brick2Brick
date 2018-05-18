using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelTurns
{
    public int OneStar;
    public int TwoStars;
    public int ThreeStars;
}

[System.Serializable]
public class LevelParameters
{
    [SerializeField]
    private int m_fieldSize;

    [SerializeField]
    private int m_freeSpace;

    [SerializeField]
    private int m_tokenTypes;

    [SerializeField]
    private int m_turns;

    





    public int FieldSize { get { return m_fieldSize; } set { m_fieldSize = value; } }

    public int FreeSpace { get { return m_freeSpace; } }

    public int TokenTypes { get { return m_tokenTypes; } set { m_tokenTypes = value; } }

    public int Turns
    {
        get
        {
            return m_turns;
        }

        set
        {
            m_turns = value;
            if (HUD.Instance != null)
            {
                HUD.Instance.UpdateTurnsValue(m_turns);
                HUD.Instance.SetStar(Controller.Instance.CurrentLevel, Controller.Instance.Difficulty);
                HUD.Instance.UpdateStarIndicator();
            }
        }
    }

    public int MaxTurns;


    


    private int GetTokenTypesCount(int level, int difficulty)
    {
        int[,] matrix = Controller.Instance.Matrixes[difficulty][level];
        float size = Mathf.Sqrt(matrix.Length);
        int count = 0;

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (matrix[i,j] > count)
                    count = matrix[i,j];
            }
        }
        return count;
    }

    public void SetTokenTypesCount(int level, int difficulty)
    {
        level--;
        TokenTypes = GetTokenTypesCount(level, difficulty);
    }

    public void SetTurns(int level, int difficulty)
    {
        level--;
        Turns = Controller.Instance.LevelTurnsList[difficulty][level].OneStar;
        MaxTurns = Turns;
        HUD.Instance.UpdateStarIndicator();
    }

    public LevelParameters(int currentLevel)
    {
        if (Controller.Instance.Difficulty == Constants.EASY)
        {
            int[] fieldsize =       { 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 5, 5};
            int[] freeSpace =       { 2, 2, 1, 1, 1, 1, 4, 4, 4, 4, 3, 3, 2, 5, 4};
            int[] tokenTypes =      { 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4};
            int[] turns =           { 7, 7, 10, 9, 14, 14, 18, 16, 20, 18, 28, 24, 38, 30, 40};
            int[] levelScoreBonus = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15};

            m_fieldSize = fieldsize[currentLevel - 1];
            m_freeSpace = freeSpace[currentLevel - 1];
            m_tokenTypes = tokenTypes[currentLevel - 1];
            Turns = turns[currentLevel - 1];
            MaxTurns = Turns;
            Controller.Instance.Score.LevelScoreBonus = levelScoreBonus[currentLevel - 1];
        }

        if (Controller.Instance.Difficulty == Constants.NORMAL)
        {
            int[] fieldsize =       { 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5/*, 6, 6, 6, 6, 6, 6, 6*/};
            int[] freeSpace =       { 4, 4, 3, 3, 2, 2, 5, 5, 4, 4, 5, 5, 4, 3, 3/*, 6, 5, 5, 4, 3, 4, 3*/};
            int[] tokenTypes =      { 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 5, 5, 5, 4, 5/*, 4, 4, 5, 5, 5, 6, 6*/};
            int[] turns =           { 18, 18, 22, 22, 28, 28, 32, 32, 46, 46, 46, 46, 70, 60, 60/*, 75, 80, 85, 150, 150, 150, 150*/ };
            int[] levelScoreBonus = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15/*, 16, 17, 18, 19, 20*/ };

            m_fieldSize = fieldsize[currentLevel - 1];
            m_freeSpace = freeSpace[currentLevel - 1];
            m_tokenTypes = tokenTypes[currentLevel - 1];
            Turns = turns[currentLevel - 1];
            MaxTurns = Turns;
            Controller.Instance.Score.LevelScoreBonus = levelScoreBonus[currentLevel - 1];
        }

        if (Controller.Instance.Difficulty == Constants.HARD)
        {
            int[] fieldsize =       { 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6, 6/*, 6, 6, 6, 6, 6*/ };
            int[] freeSpace =       { 3, 2, 3, 2, 1, 1, 3, 3, 2, 2, 5, 5, 4, 4, 3/*, 3, 2, 2, 2, 2 */};
            int[] tokenTypes =      { 4, 4, 5, 5, 4, 5, 4, 5, 4, 5, 5, 6, 5, 6, 5/*, 6, 5, 5, 6, 6 */};
            int[] turns =           { 20, 18, 20, 36, 42, 50, 50, 60, 60, 60, 85, 85, 100, 100, 110/*, 200, 200, 200, 200, 200*/};
            int[] levelScoreBonus = { 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30/*, 160, 170, 180, 190, 200*/};

            m_fieldSize = fieldsize[currentLevel - 1];
            m_freeSpace = freeSpace[currentLevel - 1];
            m_tokenTypes = tokenTypes[currentLevel - 1];
            Turns = turns[currentLevel - 1];
            MaxTurns = Turns;
            Controller.Instance.Score.LevelScoreBonus = levelScoreBonus[currentLevel - 1];
        }

        /*// Увеличивается на 1 каждые 4 уровня
        int fieldIncreaseStep = currentLevel / 4;

        // Увеличивается от 0 до 1 в течение 4-х уровней, пока размер поля не изменится
        float subSetup = (currentLevel / 4f) - fieldIncreaseStep;

        // Начальный размер поля 3х3
        // Размер увеличивается на 1 каждые 4 уровня
        m_fieldSize = 3 + fieldIncreaseStep;

        // Рассчитываем свободное пространство в зависимости от уровня сложности
        m_freeSpace = (int)(m_fieldSize * (1f - subSetup));
        if (m_freeSpace < 1)
        {
            // Минимальное число пустых клеток
            m_freeSpace = 1;
        }

        // Начальное число цветов 2
        // Увеличивается на 1 каждые 2 уровня, увеличение начинается с 4-го уровня
        m_tokenTypes = 2 + (currentLevel / 3);

        if (m_tokenTypes > 10)
        {
            // Максимальное число цветов
            m_tokenTypes = 10;
        }

        // Количество ходов, за которые надо успеть закончить уровень, чтобы получить бонус, зависит от остальных параметров

        //Turns = ((m_fieldSize * m_fieldSize * m_fieldSize / 6) - (m_freeSpace * m_freeSpace) * m_tokenTypes) + m_fieldSize;
        if (m_fieldSize == 3)
        {
            Turns = ((m_fieldSize - m_freeSpace) * m_tokenTypes) + m_fieldSize * 2;
        }
        else
        {
            Turns = (((m_fieldSize * m_fieldSize * m_fieldSize / 12) + (m_fieldSize - m_freeSpace) * 3) * m_tokenTypes) + m_fieldSize;
        }
        */
    }


}
