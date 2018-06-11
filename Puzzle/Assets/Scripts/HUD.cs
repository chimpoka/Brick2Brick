using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class HUD : MonoBehaviour
{
    private static HUD m_instance;

    [SerializeField]
    private Text[] m_scoreValue;

    [SerializeField]
    private Text m_turnsValue;

    [SerializeField]
    private Text[] m_levelValues;

    [SerializeField]
    private Text m_difficultyValue;

    [SerializeField]
    private Text m_ScoreDecreaseValue;

    [SerializeField]
    private GameObject m_levelCompletedWindow;

    [SerializeField]
    private GameObject m_endGameWindow;

    [SerializeField]
    private GameObject m_winGameWindow;

    [SerializeField]
    private GameObject m_winGameLastDifficultyWindow;

    [SerializeField]
    private GameObject m_newHighScoreWindow;

    [SerializeField]
    private GameObject m_optionsWindow;

    [SerializeField]
    private GameObject m_menuWindow;

    [SerializeField]
    private GameObject soundButton;

    [SerializeField]
    private GameObject musicButton;

    [SerializeField]
    private Text[] m_levelBonusValue;

    [SerializeField]
    private GameObject m_firstPlayHelpWindow;

    [SerializeField]
    private GameObject m_starIndicator;

    private int m_levelBonus;

    private int m_turnsBonus;

    private IEnumerator coroutine;

    [SerializeField]
    private Button m_undoButton;

    [SerializeField]
    private GameObject m_levelCompletedNormalModeWindow;

    [SerializeField]
    private GameObject m_levelLostNormalModeWindow;

    [SerializeField]
    private GameObject m_openNormalDifficultyWindow;

    [SerializeField]
    private GameObject m_openHardDifficultyWindow;



    public static HUD Instance { get { return m_instance; } }



    private void Awake()
    {
        m_instance = this;
    }

    private void Start()
    {
        DataStore.LoadGame();
        //Controller.Instance.CurrentLevel = 1;
        //Controller.Instance.Difficulty = DifficultyLevel.Normal;
        if (Controller.Instance.Mode == GameMode.Normal)
        {
            Controller.Instance.InitializeLevelNormalMode();
        }

        m_optionsWindow.SetActive(true);
        m_menuWindow.SetActive(true);

        if (Controller.Instance.Audio.SfxVolume == 0)
        {
            soundButton.GetComponent<Image>().sprite = GetSprite("SoundOff");
        }
        if (Controller.Instance.Audio.MusicVolume == 0)
        {
            musicButton.GetComponent<Image>().sprite = GetSprite("MusicOff");
        }

        if (Controller.Instance.FirstPlay == 1)
        {
            m_firstPlayHelpWindow.SetActive(true);
            Controller.Instance.FirstPlay = 0;
        }
        else
        {
            m_firstPlayHelpWindow.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.position.y < Screen.height * 0.7)
            {
                HideOptionsWindow();
                HideMenuWindow();
            }
        }
    }




    public void UpdateLevelBonusValue(int value)
    {
        for (int i = 0; i < m_scoreValue.Length; i++)
        {
            m_levelBonusValue[i].text = value.ToString();
        }
    }

    public void UpdateTurnsValue(int value)
    {
        m_turnsValue.text = value.ToString();
        //PlayAnimationChangeTurnsValue();
    }

    public void UpdateScoreDecreaseValue(int value)
    {
        m_ScoreDecreaseValue.text = "-" + value.ToString();
        m_ScoreDecreaseValue.gameObject.GetComponent<Animator>().SetTrigger("ValueChanged");
    }

    public void PlayAnimationChangeTurnsValue()
    {
        m_turnsValue.gameObject.GetComponent<Animator>().SetTrigger("ValueChanged");
    }

    public void UpdateLevelValue(int value)
    {      

        m_levelValues[0].text = value.ToString() + "/" + Controller.Instance.LevelTurnsList[Controller.Instance.Difficulty].Count.ToString();
        m_levelValues[1].text = "Level " + value.ToString();
        m_levelValues[2].text = "Level " + value.ToString();
    }

    public void UpdateDifficultyValue(int value)
    {
        if (value == Constants.EASY)
            m_difficultyValue.text = "Easy";
        if (value == Constants.NORMAL)
            m_difficultyValue.text = "Normal";
        if (value == Constants.HARD)
            m_difficultyValue.text = "Hard";
    }

    public void UpdateScoreValue(int value)
    {
        for (int i = 0; i < m_scoreValue.Length; i++)
        {
            m_scoreValue[i].text = value.ToString();
        }
    }

    public void ShowWindow(GameObject window)
    {
        window.GetComponent<Animator>().SetBool("Open", true);
    }

    public void HideWindow(GameObject window)
    {
        window.GetComponent<Animator>().SetBool("Open", false);
    }

    public void ShowOpenNormalDifficultyWindow()
    {
        ShowWindow(m_openNormalDifficultyWindow);
    }

    public void ShowOpenHardDifficultyWindow()
    {
        ShowWindow(m_openHardDifficultyWindow);
    }

    public void onOpenNormalDifficultyWindowClick()
    {
        HideWindow(m_openNormalDifficultyWindow);
        ShowLevelCompletedNormalModeWindow(Controller.Instance.LevelStars[Constants.EASY][Controller.Instance.CurrentLevel - 1]);
    }

    public void onOpenHardDifficultyWindowClick()
    {
        HideWindow(m_openHardDifficultyWindow);
        ShowLevelCompletedNormalModeWindow(Controller.Instance.LevelStars[Constants.NORMAL][Controller.Instance.CurrentLevel - 1]);
    }

    //public void ShowEndGameWindow()
    //{
    //    ShowWindow(m_endGameWindow);
    //}

    //public void ShowLevelCompletedWindow()
    //{
    //    ShowWindow(m_levelCompletedWindow);
    //}

    //public void ShowWinGameWindow()
    //{
    //    ShowWindow(m_winGameWindow);
    //}

    //public void ShowWinGameLastDifficultyWindow()
    //{
    //    ShowWindow(m_winGameLastDifficultyWindow);
    //}

    //public void ShowNewHighScoreWindow()
    //{
    //    ShowWindow(m_newHighScoreWindow);
    //}

    //public void NewGame()
    //{
    //    if (Controller.Instance.IsNewHighScore(Controller.Instance.Score.CurrentScore, Controller.Instance.Difficulty))
    //    {
    //        ShowNewHighScoreWindow();
    //    }
    //    else
    //    {
    //        HideWindow(m_endGameWindow);
    //        Controller.Instance.NewGame();
    //        DataStore.SaveGame();
    //        UpdateCurrentLevels();
    //    }
    //}

    //public void RestartLevel()
    //{
    //    Controller.Instance.RestartLevel();
    //}

    public void RestartLevelNew()
    {
        while (Controller.Instance.TokenGameObjects.Count > 0)
            UndoTurn();
    }

    public void UndoTurn()
    {
        Controller.Instance.UndoTurn();

        //if (Input.GetButton("Fire1"))
        //{
        //    Debug.Log("mouse");
        //    UndoTurn();
        //}
    }

    public void Quit()
    {
        Application.Quit();
    }

    //public void Next()
    //{
    //    StopCoroutine(coroutine);
    //    CountScoreIfBreak();
    //    HideWindow(m_levelCompletedWindow);
    //    UpdateCurrentLevels();
    //    DataStore.SaveGame();
    //    StartCoroutine(WaitBeforeInitializeLevel(0.35f));
    //    //Controller.Instance.InitializeLevel();
    //}
    
    //private IEnumerator WaitBeforeInitializeLevel(float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    Controller.Instance.InitializeLevel();
    //}



    /*
    private IEnumerator Count (int to, float delayMax, float delayMin)
    {
        m_turnsBonus = Controller.Instance.Level.Turns;
        m_levelBonus = Controller.Instance.Score.LevelScoreBonus;
        float deltaDelay;
        int count = 10;
        deltaDelay = (delayMax - delayMin) / count;
        float delatMaxTemp = delayMax;
        
        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < to; i++)
        {          
            yield return new WaitForSeconds(delayMax);
            if (delayMax > delayMin)
            {
                delayMax -= deltaDelay;
            }

            Controller.Instance.Score.AddBonus();
            if (m_levelBonus > 0)
            {
                UpdateLevelBonusValue(--m_levelBonus);
                if (m_levelBonus == 0)
                {
                    yield return new WaitForSeconds(1.2f);
                    delayMax = delatMaxTemp;
                }
            }
            else
            {
                UpdateTurnsValue(--m_turnsBonus);
            }
        }
    }

    public void CountScore(int to)
    {
        //ShowWindow(m_levelCompletedWindow);
        coroutine = Count(to, 0.12f, 0.03f);
        StartCoroutine(coroutine);
    }

    private void CountScoreIfBreak()
    {
        Controller.Instance.Score.CurrentScore += m_turnsBonus + m_levelBonus;
        m_turnsBonus = 0;
        m_turnsBonus = 0;
    }
    */
   



    public void OptionsButton()
    {
        if (m_optionsWindow.GetComponent<Animator>().GetBool("Open") == false)
            ShowWindow(m_optionsWindow);
        else
            HideWindow(m_optionsWindow);
    }

    public void OpenMenuButton()
    {
        if (m_menuWindow.GetComponent<Animator>().GetBool("Open") == false)
            ShowWindow(m_menuWindow);
        else
            HideWindow(m_menuWindow);
    }

    public void MainMenuButton()
    {
        //HideWindow(m_winGameWindow);
        //HideWindow(m_winGameLastDifficultyWindow);

        //if (Controller.Instance.CurrentLevel == Controller.Instance.MaxLevel)
        //{
        //    Controller.Instance.CurrentLevel = 1;
        //    UpdateCurrentLevels();
        //}

        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    /*
    public void MainMenuAfterWin()
    {
        HideWindow(m_winGameWindow);
        HideWindow(m_winGameLastDifficultyWindow);

        ShowNewHighScoreWindow();
        
        //Controller.Instance.CurrentLevel = 1;
        //UpdateCurrentLevels();
        
    }
    */

    public void HideOptionsWindow()
    {
        HideWindow(m_optionsWindow);
    }

    public void HideMenuWindow()
    {
        HideWindow(m_menuWindow);
    }

    public void SoundButton()
    {
        float volume = Controller.Instance.Audio.SfxVolume;

        if (volume != 0)
        {
            Controller.Instance.Audio.SfxVolume = 0;
            soundButton.GetComponent<Image>().sprite = GetSprite("SoundOff");
        }
        else
        {
            Controller.Instance.Audio.SfxVolume = Controller.Instance.SoundVolume;
            soundButton.GetComponent<Image>().sprite = GetSprite("SoundOn");
        }
        DataStore.SaveOptions();
    }

    public void MusicButton()
    {
        float volume = Controller.Instance.Audio.MusicVolume;

        if (volume != 0)
        {
            Controller.Instance.Audio.MusicVolume = 0;
            musicButton.GetComponent<Image>().sprite = GetSprite("MusicOff");
        }
        else
        {
            Controller.Instance.Audio.MusicVolume = Controller.Instance.MusicVolume;
            musicButton.GetComponent<Image>().sprite = GetSprite("MusicOn");
        }
        DataStore.SaveOptions();
    }

    private Sprite GetSprite(string spriteName)
    {
        Sprite[] sprites = Controller.Instance.ButtonSprites;
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].name == spriteName)
            {
                return sprites[i];
            }
        }
        Debug.LogError("Can not find sprite " + spriteName);
        return null;
    }


    //public void OnEndEditNewHighScore()
    //{
    //    HideWindow(m_newHighScoreWindow);
    //    string name = m_newHighScoreWindow.transform.Find("InputField").Find("Text").GetComponent<Text>().text;

    //    Controller.Instance.AddHighScoreValue(Controller.Instance.Score.CurrentScore, name, Controller.Instance.Difficulty);
    //    Controller.Instance.NewGame();
    //    DataStore.SaveGame();
    //}

    //private void UpdateCurrentLevels()
    //{
    //    //int difficulty = Controller.Instance.Difficulty;

    //    //if (difficulty == Constants.EASY)
    //    //    Controller.Instance.CurrentLevelEasy = Controller.Instance.CurrentLevel;
    //    //if (difficulty == Constants.NORMAL)
    //    //    Controller.Instance.CurrentLevelNormal = Controller.Instance.CurrentLevel;
    //    //if (difficulty == Constants.HARD)
    //    //    Controller.Instance.CurrentLevelHard = Controller.Instance.CurrentLevel;
    //}

    public void TapToClose()
    {
        m_firstPlayHelpWindow.SetActive(false);
    }



    public void CreateStarIndicator(int level, int difficulty)
    {
        m_starIndicator.SetActive(true);
        level--;

        Image[] objects = m_starIndicator.GetComponentsInChildren<Image>();

        foreach (Image obj in objects)
        {
            if (obj.name == "CoinHUD" || obj.name == "CoinHUD(Clone)")
                Destroy(obj.gameObject);
        }

        CreateStars(m_starIndicator, level, difficulty);
    }

    private void CreateStars(GameObject starIndicator, int level, int difficulty)
    {
        RectTransform[] points = starIndicator.GetComponentsInChildren<RectTransform>();
        float startPoint = 0;
        float endPoint = 0;
        foreach (RectTransform point in points)
        {
            if (point.name == "StartPoint")
                startPoint = point.position.x;
            if (point.name == "EndPoint")
                endPoint = point.position.x;
        }

        float starIndicatorLength = Controller.Instance.LevelTurnsList[difficulty][level].OneStar;

        float[] starPositions = new float[3];
        starPositions[0] = Controller.Instance.LevelTurnsList[difficulty][level].OneStar;
        starPositions[1] = Controller.Instance.LevelTurnsList[difficulty][level].TwoStars;
        starPositions[2] = Controller.Instance.LevelTurnsList[difficulty][level].ThreeStars;
        
        for (int i = 0; i < 3; i++)
        {
            starPositions[i] = starIndicatorLength - starPositions[i];
            //starPositions[i] += 0.5f;
            starPositions[i] = Mathf.InverseLerp(0, starIndicatorLength, starPositions[i]);
            starPositions[i] = Mathf.Lerp(startPoint, endPoint, starPositions[i]);

            Vector3 starPosition = starIndicator.transform.position;
            starPosition.x = starPositions[i];
            starPosition.y += 0.02f;

            GameObject star = Instantiate(Resources.Load("Prefabs/CoinHUD") as GameObject, starPosition, Quaternion.identity) as GameObject;
            //star.GetComponent<Animator>().SetBool("Enabled", true);
            star.transform.SetParent(starIndicator.transform);

            star.transform.localScale = Vector3.one;
            RectTransform StarRectTransform = star.GetComponent<RectTransform>();
            StarRectTransform.offsetMin = new Vector2(StarRectTransform.offsetMin.x - 100, StarRectTransform.offsetMin.y);
            StarRectTransform.offsetMax = new Vector2(StarRectTransform.offsetMax.x + 100, StarRectTransform.offsetMax.y);
        }
    }

    public void SetStar(int level, int difficulty, bool undo = false)
    {
        level--;
        int turns = Controller.Instance.Level.Turns;
        int starIndicatorLength = Controller.Instance.LevelTurnsList[difficulty][level].OneStar;
        Animator[] starAnimators = m_starIndicator.GetComponentsInChildren<Animator>();

        if (undo)
        {
            if (turns == starIndicatorLength - Controller.Instance.LevelTurnsList[difficulty][level].ThreeStars)
                starAnimators[2].SetBool("Disabled", false);
            else if (turns == starIndicatorLength - Controller.Instance.LevelTurnsList[difficulty][level].TwoStars)
                starAnimators[1].SetBool("Disabled", false);
            else if (turns == starIndicatorLength - Controller.Instance.LevelTurnsList[difficulty][level].OneStar)
                starAnimators[0].SetBool("Disabled", false);
        }
        else
        {
            if (turns == starIndicatorLength - Controller.Instance.LevelTurnsList[difficulty][level].ThreeStars - 1)
                starAnimators[2].SetBool("Disabled", true);
            else if (turns == starIndicatorLength - Controller.Instance.LevelTurnsList[difficulty][level].TwoStars - 1)
                starAnimators[1].SetBool("Disabled", true);
            //else if (turns == starIndicatorLength - Controller.Instance.LevelTurnsList[difficulty][level].OneStar - 1)
            //    starAnimators[0].SetBool("Disabled", true);
            if (turns == 0 && Controller.Instance.IsAllTokensConnected() == false)
                starAnimators[0].SetBool("Disabled", true);
            

        }
    }

    public void ShowLevelCompletedNormalModeWindow(int stars)
    {
        ShowWindow(m_levelCompletedNormalModeWindow);
        ShowStars(stars, m_levelCompletedNormalModeWindow);
    }

    public void ShowLevelLostNormalModeWindow()
    {
        ShowWindow(m_levelLostNormalModeWindow);
    }

    public void Replay()
    {
        Controller.Instance.InitializeLevelNormalMode();
        HideWindow(m_levelCompletedNormalModeWindow);
        HideWindow(m_levelLostNormalModeWindow);
    }

    public void NextLevel()
    {
        if (Controller.Instance.CurrentLevel == Controller.Instance.MaxLevel)
        {
            Controller.Instance.Difficulty++;
            Controller.Instance.CurrentLevel = 1;
        }
        else
        {
            Controller.Instance.CurrentLevel++;
        }
        HideWindow(m_levelCompletedNormalModeWindow);
        Controller.Instance.InitializeLevelNormalMode();
    }

    public void UpdateStarIndicator()
    {
        m_starIndicator.GetComponent<Image>().fillAmount = Mathf.InverseLerp(0, Controller.Instance.Level.MaxTurns, Controller.Instance.Level.Turns);
    }

    public void ShowStars(int starsCount, GameObject window)
    {
        
        Image[] objects = window.GetComponentsInChildren<Image>(true);
        Image[] stars = new Image[3];
        
        foreach (var obj in objects)
        {
            if (obj.name == "Star1")
                stars[0] = obj;
            if (obj.name == "Star2")
                stars[1] = obj;
            if (obj.name == "Star3")
                stars[2] = obj;
        }

        Color enabled = new Color(1, 1, 1, 1);
        Color disabled = new Color(1, 1, 1, 40f / 255);

        if (starsCount == 3)
        {
            foreach (var star in stars)
                star.color = enabled;
        }
        if (starsCount == 2)
        {
            stars[0].color = enabled;
            stars[1].color = enabled;
            stars[2].color = disabled;
        }
        if (starsCount == 1 || starsCount == 0)
        {
            stars[0].color = enabled;
            stars[1].color = disabled;
            stars[2].color = disabled;
        }
        //if (starsCount == 0)
        //{
        //    foreach (var star in stars)
        //        star.gameObject.SetActive(true);
        //}

    }

}
