using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Score
{
    [SerializeField]
    private int m_currentScore;

    public int LevelScoreBonus;

    public string Name;

    public int CurrentScore
    {
        get
        {
            return m_currentScore;
        }
        set
        {
            m_currentScore = value;
            if (HUD.Instance != null)
                HUD.Instance.UpdateScoreValue(m_currentScore);
        }
    }


    public void AddBonus()
    {
        CurrentScore += 1;
    }
}
