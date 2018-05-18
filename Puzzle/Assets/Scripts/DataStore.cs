using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataStore
{
    private const string
        MUSIC_VOLUME_KEY = "MusicVolume",
        SOUND_VOLUME_KEY = "SoundVolume",
        //SAVED_LEVEL_EASY_KEY = "LevelEasy",
        SAVED_SCORE_EASY_KEY = "ScoreEasy",
        //SAVED_LEVEL_NORMAL_KEY = "LevelNormal",
        SAVED_SCORE_NORMAL_KEY = "ScoreNormal",
        //SAVED_LEVEL_HARD_KEY = "LevelHard",
        SAVED_SCORE_HARD_KEY = "ScoreHard",
        //SAVED_ENABLED_NORMAL_KEY = "EnabledNormal",
        //SAVED_ENABLED_HARD_KEY = "EnabledHard",
        SAVED_FIRST_PLAY_KEY = "FirstPlay";

    static public void SaveOptions()
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, Controller.Instance.Audio.MusicVolume);
        PlayerPrefs.SetFloat(SOUND_VOLUME_KEY, Controller.Instance.Audio.SfxVolume);
        PlayerPrefs.Save();
    }

    static public void LoadOptions()
    {
        Controller.Instance.Audio.MusicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.2f);
        Controller.Instance.Audio.SfxVolume = PlayerPrefs.GetFloat(SOUND_VOLUME_KEY, 1f);
    }

    static public void SaveGame()
    {
        PlayerPrefs.SetInt(SAVED_FIRST_PLAY_KEY, Controller.Instance.FirstPlay);

        List<List<int>> levelStars = Controller.Instance.LevelStars;
        for (int i = 0; i < levelStars[Constants.EASY].Count; i++)
            PlayerPrefs.SetInt("LevelStarsEasy" + i, levelStars[Constants.EASY][i]);
        for (int i = 0; i < levelStars[Constants.NORMAL].Count; i++)
            PlayerPrefs.SetInt("LevelStarsNormal" + i, levelStars[Constants.NORMAL][i]);
        for (int i = 0; i < levelStars[Constants.HARD].Count; i++)
            PlayerPrefs.SetInt("LevelStarsHard" + i, levelStars[Constants.HARD][i]);



        // PlayerPrefs.SetInt(SAVED_ENABLED_NORMAL_KEY, Controller.Instance.NormalButtonEnabled);
        // PlayerPrefs.SetInt(SAVED_ENABLED_HARD_KEY, Controller.Instance.HardButtonEnabled);

        //int[] completedLevels = Controller.Instance.CompletedLevels;
        //for (int i = 0; i < completedLevels.Length; i++)
        //{
        //    PlayerPrefs.SetInt("CompletedLevels" + i, completedLevels[i]);
        //}

        //PlayerPrefs.SetInt(SAVED_LEVEL_EASY_KEY, Controller.Instance.CurrentLevelEasy);
        //PlayerPrefs.SetInt(SAVED_LEVEL_NORMAL_KEY, Controller.Instance.CurrentLevelNormal);
        //PlayerPrefs.SetInt(SAVED_LEVEL_HARD_KEY, Controller.Instance.CurrentLevelHard);

        //int difficulty = Controller.Instance.Difficulty;

        //switch(difficulty)
        //{
        //    case Constants.EASY:
        //        PlayerPrefs.SetInt(SAVED_SCORE_EASY_KEY, Controller.Instance.Score.CurrentScore);
        //        break;
        //    case Constants.NORMAL:
        //        PlayerPrefs.SetInt(SAVED_SCORE_NORMAL_KEY, Controller.Instance.Score.CurrentScore);
        //        break;
        //    case Constants.HARD:
        //        PlayerPrefs.SetInt(SAVED_SCORE_HARD_KEY, Controller.Instance.Score.CurrentScore);
        //        break;
        //}

        PlayerPrefs.Save();
    }

    static public void LoadGame()
    {

        //int[] completedLevels = Controller.Instance.CompletedLevels;
        //for (int i = 0; i < completedLevels.Length; i++)
        //{
        //    completedLevels[i] = PlayerPrefs.GetInt("CompletedLevels" + i, 0);
        //}

        //Controller.Instance.FirstPlay = PlayerPrefs.GetInt(SAVED_FIRST_PLAY_KEY, 1);

        //Controller.Instance.NormalButtonEnabled = PlayerPrefs.GetInt(SAVED_ENABLED_NORMAL_KEY, 0);
        //Controller.Instance.HardButtonEnabled = PlayerPrefs.GetInt(SAVED_ENABLED_HARD_KEY, 0);

        //Controller.Instance.CurrentLevelEasy = PlayerPrefs.GetInt(SAVED_LEVEL_EASY_KEY, 1);
        //Controller.Instance.CurrentLevelNormal = PlayerPrefs.GetInt(SAVED_LEVEL_NORMAL_KEY, 1);
        //Controller.Instance.CurrentLevelHard = PlayerPrefs.GetInt(SAVED_LEVEL_HARD_KEY, 1);

        //int difficulty = Controller.Instance.Difficulty;

        //switch (difficulty)
        //{
        //    case Constants.EASY:
        //        Controller.Instance.Score.CurrentScore = PlayerPrefs.GetInt(SAVED_SCORE_EASY_KEY, 0);
        //        break;
        //    case Constants.NORMAL:
        //        Controller.Instance.Score.CurrentScore = PlayerPrefs.GetInt(SAVED_SCORE_NORMAL_KEY, 0); ;
        //        break;
        //    case Constants.HARD:
        //        Controller.Instance.Score.CurrentScore = PlayerPrefs.GetInt(SAVED_SCORE_HARD_KEY, 0);
        //        break;
        //}

        List<List<int>> levelStars = Controller.Instance.LevelStars;
        for (int i = 0; i < levelStars[Constants.EASY].Count; i++)
            levelStars[Constants.EASY][i] = PlayerPrefs.GetInt("LevelStarsEasy" + i, 0);
        for (int i = 0; i < levelStars[Constants.NORMAL].Count; i++)
            levelStars[Constants.NORMAL][i] = PlayerPrefs.GetInt("LevelStarsNormal" + i, 0);
        for (int i = 0; i < levelStars[Constants.HARD].Count; i++)
            levelStars[Constants.HARD][i] = PlayerPrefs.GetInt("LevelStarsHard" + i, 0);

    }

    static public void SaveHighScore()
    {
        List<List<HighScore>> highScoreValues = Controller.Instance.HighScoreValues;
        for (int i = 0; i < highScoreValues.Count; i++)
        {
            for (int j = 0; j < highScoreValues[i].Count; j++)
            {
                PlayerPrefs.SetInt("HighScoreValues" + (highScoreValues.Count * i + j), highScoreValues[i][j].ScoreValue);
                PlayerPrefs.SetString("HighScoreNames" + (highScoreValues.Count * i + j), highScoreValues[i][j].Name);
            }
        }

        PlayerPrefs.Save();
    }
    
    static public void LoadHighScore()
    {
        List<List<HighScore>> highScoreValues = Controller.Instance.HighScoreValues;
        for (int i = 0; i < highScoreValues.Count; i++)
        {
            for (int j = 0; j < highScoreValues[i].Count; j++)
            {
                
                //  PlayerPrefs.SetInt("HighScoreValues" + (highScoreValues.Count * i + j), 0);
                //  PlayerPrefs.SetString("HighScoreNames" + (highScoreValues.Count * i + j), "Default");
                highScoreValues[i][j].ScoreValue = PlayerPrefs.GetInt("HighScoreValues" + (highScoreValues.Count * i + j), 0);
                highScoreValues[i][j].Name = PlayerPrefs.GetString("HighScoreNames" + (highScoreValues.Count * i + j), "Default");
            }
        }
    }

}
