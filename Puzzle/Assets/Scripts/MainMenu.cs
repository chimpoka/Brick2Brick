using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ListGameObject
{
    public List<GameObject> listGameObject;
}

public class MainMenu : MonoBehaviour
{
    [SerializeField]    
    private GameObject m_MainMenuWindow;

    //[SerializeField]
    //private GameObject m_HighScoreWindow;

    [SerializeField]
    private GameObject m_OptionsWindow;

    //[SerializeField]
    //private GameObject m_CreateLevelWindow;

    //[SerializeField]
    //private GameObject[] m_highScoreObjectsEasy;

    //[SerializeField]
    //private GameObject[] m_highScoreObjectsNormal;

    //[SerializeField]
    //private GameObject[] m_highScoreObjectsHard;

    //[SerializeField]
    //private Text m_difficultyValue;

    //[SerializeField]
    //private Text m_levelValue;

    //[SerializeField]
    //private Button m_normalButton;

    //[SerializeField]
    //private Button m_hardButton;

    //private int m_difficulty = Constants.EASY;

    //private int m_level = 1;

    [SerializeField]
    private GameObject[] m_soundButtons;

    [SerializeField]
    private GameObject[] m_difficultyButtons;




    private void Awake()
    {
        m_MainMenuWindow.GetComponent<Animator>().SetBool("Opened", true);
    }

    private void Start()
    {
        //DataStore.LoadHighScore();
        DataStore.LoadGame();
        //UpdateHighScore();
        UpdateCurrentLevels();
        //UpdateDifficultyAndLevelValues(Constants.EASY, 1);

        //if (Controller.Instance.NormalButtonEnabled == 0)
        //    m_normalButton.interactable = false;
        //else
        //    m_normalButton.interactable = true;

        //if (Controller.Instance.HardButtonEnabled == 0)
        //    m_hardButton.interactable = false;
        //else
        //    m_hardButton.interactable = true;


        if (Controller.Instance.Audio.SfxVolume == 0)
        {
            GetButton("SoundOnButton").SetActive(false);
            GetButton("SoundOffButton").SetActive(true);
        }
        else
        {
            GetButton("SoundOnButton").SetActive(true);
            GetButton("SoundOffButton").SetActive(false);
        }
        if (Controller.Instance.Audio.MusicVolume == 0)
        {
            GetButton("MusicOnButton").SetActive(false);
            GetButton("MusicOffButton").SetActive(true);
        }
        else
        {
            GetButton("MusicOnButton").SetActive(true);
            GetButton("MusicOffButton").SetActive(false);
        }

        //m_highScoreObjectsEasy = new GameObject[Controller.Instance.HighScoreValuesNumber];
        //m_highScoreObjectsNormal = new GameObject[Controller.Instance.HighScoreValuesNumber];
        //m_highScoreObjectsHard = new GameObject[Controller.Instance.HighScoreValuesNumber];
    }



    /*
    private void UpdateHighScore()
    {
        List<List<HighScore>> highScoreValues = Controller.Instance.HighScoreValues;

        for (int i = 0; i < m_highScoreObjectsEasy.Length; i++)
        {
            if (highScoreValues[0][i].ScoreValue == 0)
            {
                m_highScoreObjectsEasy[i].transform.Find("Score").GetComponent<Text>().text = "Empty";
                m_highScoreObjectsEasy[i].transform.Find("Name").GetComponent<Text>().text = "Empty";
            }
            else
            {
                m_highScoreObjectsEasy[i].transform.Find("Score").GetComponent<Text>().text = highScoreValues[0][i].ScoreValue.ToString();
                m_highScoreObjectsEasy[i].transform.Find("Name").GetComponent<Text>().text = highScoreValues[0][i].Name.ToString();
            }
        }

        for (int i = 0; i < m_highScoreObjectsNormal.Length; i++)
        {
            if (highScoreValues[1][i].ScoreValue == 0)
            {
                m_highScoreObjectsNormal[i].transform.Find("Score").GetComponent<Text>().text = "Empty";
                m_highScoreObjectsNormal[i].transform.Find("Name").GetComponent<Text>().text = "Empty";
            }
            else
            {
                m_highScoreObjectsNormal[i].transform.Find("Score").GetComponent<Text>().text = highScoreValues[1][i].ScoreValue.ToString();
                m_highScoreObjectsNormal[i].transform.Find("Name").GetComponent<Text>().text = highScoreValues[1][i].Name.ToString();
            }
        }

        for (int i = 0; i < m_highScoreObjectsHard.Length; i++)
        {
            if (highScoreValues[2][i].ScoreValue == 0)
            {
                m_highScoreObjectsHard[i].transform.Find("Score").GetComponent<Text>().text = "Empty";
                m_highScoreObjectsHard[i].transform.Find("Name").GetComponent<Text>().text = "Empty";
            }
            else
            {
                m_highScoreObjectsHard[i].transform.Find("Score").GetComponent<Text>().text = highScoreValues[2][i].ScoreValue.ToString();
                m_highScoreObjectsHard[i].transform.Find("Name").GetComponent<Text>().text = highScoreValues[2][i].Name.ToString();
            }
        }
    }
    */

    private void UpdateCurrentLevels()
    {
        int[] completedLevels = Controller.Instance.GetCompletedLevels();

        m_difficultyButtons[Constants.EASY].GetComponentInChildren<Text>().text = "Easy\n" + completedLevels[Constants.EASY].ToString()
            + "/" + Controller.Instance.LevelTurnsList[Constants.EASY].Count.ToString();
        m_difficultyButtons[Constants.NORMAL].GetComponentInChildren<Text>().text = "Normal\n" + completedLevels[Constants.NORMAL].ToString()
            + "/" + Controller.Instance.LevelTurnsList[Constants.NORMAL].Count.ToString();
        m_difficultyButtons[Constants.HARD].GetComponentInChildren<Text>().text = "Hard\n" + completedLevels[Constants.HARD].ToString()
            + "/" + Controller.Instance.LevelTurnsList[Constants.HARD].Count.ToString();

        Image[] images = m_difficultyButtons[Constants.NORMAL].GetComponentsInChildren<Image>();
        Image normalButtonDisableImage = null;
        foreach (Image image in images)
        {
            if (image.name == "DisableImage")
                normalButtonDisableImage = image;
        }
        images = m_difficultyButtons[Constants.HARD].GetComponentsInChildren<Image>();
        Image hardlButtonDisableImage = null;
        foreach (Image image in images)
        {
            if (image.name == "DisableImage")
                hardlButtonDisableImage = image;
        }

        if (completedLevels[Constants.EASY] < 15)
        {
            m_difficultyButtons[Constants.NORMAL].GetComponent<Button>().interactable = false;
            normalButtonDisableImage.enabled = true;
        }
        else
        {
            m_difficultyButtons[Constants.NORMAL].GetComponent<Button>().interactable = true;
            normalButtonDisableImage.enabled = false;
        }
        if (completedLevels[Constants.NORMAL] < 15)
        {
            m_difficultyButtons[Constants.HARD].GetComponent<Button>().interactable = false;
            hardlButtonDisableImage.enabled = true;
        }
        else
        {
            m_difficultyButtons[Constants.HARD].GetComponent<Button>().interactable = true;
            hardlButtonDisableImage.enabled = false;
        }
    }

    public void PlayButton()
    {
        m_MainMenuWindow.GetComponent<Animator>().SetBool("Enabled", false);
        //m_SelectDifficultyWindow.GetComponent<Animator>().SetBool("Enabled", true);
    }

    public void BackButton(GameObject window)
    {
        m_MainMenuWindow.GetComponent<Animator>().SetBool("Opened", true);
        window.GetComponent<Animator>().SetBool("Opened", false);
    }



    // Main Window

    public void ExitButton()
    {
        Application.Quit();
    }

    public void EasyButton()
    {
        Controller.Instance.Difficulty = Constants.EASY;
        Controller.Instance.Mode = GameMode.Normal;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void NormalButton()
    {
        Controller.Instance.Difficulty = Constants.NORMAL;
        Controller.Instance.Mode = GameMode.Normal;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void HardButton()
    {
        Controller.Instance.Difficulty = Constants.HARD;
        Controller.Instance.Mode = GameMode.Normal;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void OptionsButton()
    {
        m_MainMenuWindow.GetComponent<Animator>().SetBool("Opened", false);
        m_OptionsWindow.GetComponent<Animator>().SetBool("Opened", true);
    }

    //public void HighScoreButton()
    //{
    //    m_MainMenuWindow.GetComponent<Animator>().SetBool("Opened", false);
    //    m_HighScoreWindow.GetComponent<Animator>().SetBool("Opened", true);
    //}

    //public void CreateLevelButton()
    //{
    //    m_MainMenuWindow.GetComponent<Animator>().SetBool("Opened", false);
    //    m_CreateLevelWindow.GetComponent<Animator>().SetBool("Opened", true);
    //}



    // Options Window

    public void SoundButton()
    {
        float volume = Controller.Instance.Audio.SfxVolume;

        if (volume != 0)
        {
            Controller.Instance.Audio.SfxVolume = 0;
            GetButton("SoundOnButton").SetActive(false);
            GetButton("SoundOffButton").SetActive(true);
        }
        else
        {
            Controller.Instance.Audio.SfxVolume = Controller.Instance.SoundVolume;
            GetButton("SoundOnButton").SetActive(true);
            GetButton("SoundOffButton").SetActive(false);
        }
        DataStore.SaveOptions();
    }

    public void MusicButton()
    {
        float volume = Controller.Instance.Audio.MusicVolume;

        if (volume != 0)
        {
            Controller.Instance.Audio.MusicVolume = 0;
            GetButton("MusicOnButton").SetActive(false);
            GetButton("MusicOffButton").SetActive(true);
        }
        else
        {
            Controller.Instance.Audio.MusicVolume = Controller.Instance.MusicVolume;
            GetButton("MusicOnButton").SetActive(true);
            GetButton("MusicOffButton").SetActive(false);
        }
        DataStore.SaveOptions();
    }

    private GameObject GetButton(string buttonName)
    {
        for (int i = 0; i < m_soundButtons.Length; i++)
        {
            if (m_soundButtons[i].name == buttonName)
            {
                return m_soundButtons[i];
            }
        }
        Debug.LogError("Can not find button " + buttonName);
        return null;
    }



    // Create Level Window
    /*
    private void UpdateDifficultyAndLevelValues(int difficulty, int level)
    {
        if (difficulty == Constants.EASY)
            m_difficultyValue.text = "Easy";
        if (difficulty == Constants.NORMAL)
            m_difficultyValue.text = "Normal";
        if (difficulty == Constants.HARD)
            m_difficultyValue.text = "Hard";

        m_levelValue.text = level.ToString();
    }

    public void LevelDownButton()
    {
        m_level--;
        if (m_level < 1)
            m_level = 20;
        if (m_level > 20)
            m_level = 1;
        UpdateDifficultyAndLevelValues(m_difficulty, m_level);
    }

    public void LevelUpButton()
    {
        m_level++;
        if (m_level < 1)
            m_level = 20;
        if (m_level > 20)
            m_level = 1;
        UpdateDifficultyAndLevelValues(m_difficulty, m_level);
    }

    public void DifficultyDownButton()
    {
        m_difficulty--;
        if (m_difficulty < Constants.EASY)
            m_difficulty = Constants.HARD;
        if (m_difficulty > Constants.HARD)
            m_difficulty = Constants.EASY;
        UpdateDifficultyAndLevelValues(m_difficulty, m_level);
    }

    public void DifficultyUpButton()
    {
        m_difficulty++;
        if (m_difficulty < Constants.EASY)
            m_difficulty = Constants.HARD;
        if (m_difficulty > Constants.HARD)
            m_difficulty = Constants.EASY;
        UpdateDifficultyAndLevelValues(m_difficulty, m_level);
    }

    public void CreateLevelPlayButton()
    {
        Controller.Instance.Difficulty = m_difficulty;
        //Controller.Instance.CurrentLevel = m_level;
        Controller.Instance.Mode = GameMode.Normal;
        DataStore.SaveGame();
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    */
}
