using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum DifficultyLevel { Easy, Normal, Hard }

public enum GameMode { Normal, Endless}

//[System.Serializable]
//public class HighScore
//{
//    public string Name;
//    public int ScoreValue;
//    public int Difficulty;
//}

public class Controller : MonoBehaviour 
{
	#region Variables

	static private Controller m_instance;

    public int CurrentLevel;

    //public int CurrentLevelEasy;

    //public int CurrentLevelNormal;

    //public int CurrentLevelHard;

    //public int[] CompletedLevels;

    public LevelParameters Level;

    public int MaxLevel;

    public Score Score;

    public Audio Audio = new Audio();

    public float SoundVolume;

    public float MusicVolume;

    public Sprite[] ButtonSprites;

    public Material[] TokenMaterials;

    public int Difficulty;

    public GameMode Mode;

    //public List<List<HighScore>> HighScoreValues;

    //public int HighScoreDifficultiesNumber = 3;

    //public int HighScoreValuesNumber = 3;

   // public int NormalButtonEnabled;

   // public int HardButtonEnabled;

    public int FirstPlay = 1;

    public int MaxStars = 3;

    //public Constants Constants;

    public List<List<int[,]>> Matrixes;

    public List<List<LevelTurns>> LevelTurnsList;

    public List<List<int>> LevelStars;

   // private int m_turns;

    private int m_tokensCount;

    #endregion



    #region Encapsulators

    public static Controller Instance 
	{
		get 
		{
			if (m_instance == null)
			{
				GameObject controller = Instantiate (Resources.Load ("Prefabs/Controller")) as GameObject;
				m_instance = controller.GetComponent<Controller> ();
			}

			return m_instance;
		}
	}

    public List<List<Token>> TokensByTypes { get; set; }

    public Field Field { get; set; }

    public List<GameObject> TokenGameObjects { get; set; }

    public List<Vector3> TokenPositions { get; set; }
   
    public int TokensCount
    {
        get
        {
            return m_tokensCount;
        }

        set
        {
            m_tokensCount = value;
            if (m_tokensCount >= (Level.FieldSize * Level.FieldSize - Level.FreeSpace))
            {               
                if (IsAllTokensConnected() || (IsTokensLoadedCorrectly(m_tokensCount, Level.TokenTypes, Level.FieldSize) == false))
                {
                    //Destroy(Field.gameObject);
                    //InitializeLevel();
                    //Level.Turns = m_turns;                 
                }
                m_tokensCount = 0;
            }
        }
    }

    //public List<int> x;
    //public List<int> y;

    #endregion



    #region MonoBehaviour_Methods

    private void Awake()
	{
		if (m_instance == null) 
		{
			m_instance = this;
			DontDestroyOnLoad (gameObject);
		} 
		else 
		{
			if (m_instance != this) 
			{
				Destroy (gameObject);
			}
		}

		TokensByTypes = new List<List<Token>> ();
		for (int i = 0; i < Level.TokenTypes; i++) 
		{
			TokensByTypes.Add (new List<Token> ());
		}

        //HighScoreValues = new List<List<HighScore>>();
        //for (int i = 0; i < HighScoreDifficultiesNumber; i++)
        //{
        //    HighScoreValues.Add(new List<HighScore>());
        //    for (int j = 0; j < HighScoreValuesNumber; j++)
        //    {
        //        HighScoreValues[i].Add(new HighScore());
        //    }
        //}

        Audio.SourceMusic = gameObject.AddComponent<AudioSource>();
        Audio.SourceRandomPitchSFX = gameObject.AddComponent<AudioSource>();
        Audio.SourceSFX = gameObject.AddComponent<AudioSource>();

        DataStore.LoadOptions();
	}

	private void Start()
	{
        MusicVolume = 0.2f;
        SoundVolume = 1f;
        Audio.PlayMusic(true);

        Matrixes = new List<List<int[,]>>();
        LevelTurnsList = new List<List<LevelTurns>>();
        XMLManager.Instance.GetDataFromTxt(Matrixes, LevelTurnsList);

        LevelStars = new List<List<int>>();
        for (int i = 0; i < LevelTurnsList.Count; i++)
        {
            LevelStars.Add(new List<int>());
            for (int j = 0; j < LevelTurnsList[i].Count; j++)
                LevelStars[i].Add(0);
        }

        GameObject.FindGameObjectWithTag("MainMenu").GetComponent<MainMenu>().LoadMainMenu();

        //CompletedLevels = new int[3];
    }

    #endregion





    #region User_Methods

    public bool IsTokensLoadedCorrectly(int tokensCount, int tokenTypesCount, int fieldSize)
    {
        for (int i = 0; i < TokensByTypes.Count; i++)
        {
            if (fieldSize < 5)
            {
                if (TokensByTypes[i].Count < (tokensCount / tokenTypesCount))
                {
                    return false;
                }
            }
            if ((TokensByTypes[i].Count < (tokensCount/tokenTypesCount - 1))/* || (TokensByTypes[i].Count < (fieldSize - 1))*/)
            {
                return false;
            }
        }
        return true;
    }

    public bool IsAllTokensConnected()
	{
        
		for (int i = 0; i < TokensByTypes.Count; i++) 
		{
            if (IsTokensConnected (TokensByTypes [i]) == false) 
			{
				return false;
			}
		}
		return true;
	}

	private bool IsTokensConnected(List<Token> tokens)
	{
		if (tokens.Count == 0 || tokens.Count == 1)
		{
			return true;
		}

		List<Token> connectedTokens = new List<Token>();
		connectedTokens.Add(tokens[0]);
		bool moved = true;

		while(moved)
		{
			moved = false;
			for (int i = 0; i < connectedTokens.Count; i++)
			{
				for (int j = 0; j < tokens.Count; j++)
				{
					if (IsTokensNear(tokens[j], connectedTokens[i]))
					{
						if (connectedTokens.Contains(tokens[j]) == false)
						{
							connectedTokens.Add(tokens[j]);
							moved = true;
						}
					}
				}
			}
		}

		if (tokens.Count == connectedTokens.Count)
		{
			return true;
		}
		return false;
	}

	private bool IsTokensNear(Token first, Token second)
	{
		if (((int)first.transform.position.x == (int)second.transform.position.x + 1) ||
		    ((int)first.transform.position.x == (int)second.transform.position.x - 1)) 
		{
			if ((int)first.transform.position.y == (int)second.transform.position.y) 
			{
				return true;
			}
		}

		if (((int)first.transform.position.y == (int)second.transform.position.y + 1) ||
		    ((int)first.transform.position.y == (int)second.transform.position.y - 1)) 
		{
			if ((int)first.transform.position.x == (int)second.transform.position.x) 
			{
				return true;
			}
		}

		return false;
	}

    public void TurnDone()
    {
        Audio.PlaySound("Click");

        if (Level.Turns > 0)
        {
            Level.Turns--;
            HUD.Instance.PlayAnimationChangeTurnsValue();
        }

        if (Mode == GameMode.Normal)
        {
            if (IsAllTokensConnected())
            {
                Destroy(Field.gameObject);
                int stars = CountStars(CurrentLevel, Difficulty);

                int[] completedLevels = GetCompletedLevels();
                if (Difficulty == Constants.EASY && completedLevels[Constants.EASY] == 15 && completedLevels[Constants.NORMAL] == 0)
                    HUD.Instance.ShowOpenNormalDifficultyWindow();
                else if (Difficulty == Constants.NORMAL && completedLevels[Constants.NORMAL] == 15 && completedLevels[Constants.HARD] == 0)
                    HUD.Instance.ShowOpenHardDifficultyWindow();
                else
                    HUD.Instance.ShowLevelCompletedNormalModeWindow(stars);

                //CompletedLevels[Difficulty]++;  
            }
            else
            {
                if (Level.Turns == 0)
                {
                    Destroy(Field.gameObject);
                    HUD.Instance.ShowLevelLostNormalModeWindow();
                }
            }
            DataStore.SaveGame();
        }
        //else if (Mode == GameMode.Endless)
        //{
        //    if (IsAllTokensConnected())
        //    {
        //        //Audio.PlaySound("Victory");

        //        //if (Level.Turns > 0)
        //        //{
        //        //    Level.Turns--;
        //        //    HUD.Instance.PlayAnimationChangeTurnsValue();
        //        //}

        //        // XMLManager.Instance.SaveData(Difficulty.ToString(), CurrentLevel, Level.MaxTurns - Level.Turns, Level.MaxTurns);
        //        // XMLManager.Instance.SaveData2(Level.FieldSize, Level.FreeSpace, Level.TokenTypes, Level.MaxTurns - Level.Turns, Level.MaxTurns);

        //        HUD.Instance.UpdateLevelBonusValue(Score.LevelScoreBonus);
        //        HUD.Instance.CountScore(Level.Turns + Score.LevelScoreBonus);

        //        if (CurrentLevel == MaxLevel)
        //        {
        //            // Open new difficulty
        //            if (Difficulty == Constants.EASY)
        //            {
        //                //CurrentLevelEasy = 1;
        //                //NormalButtonEnabled = 1;
        //            }
        //            else if (Difficulty == Constants.NORMAL)
        //            {
        //                //CurrentLevelNormal = 1;
        //                //HardButtonEnabled = 1;
        //            }
        //            else if (Difficulty == Constants.HARD)
        //            {
        //                //CurrentLevelHard = 1;
        //            }

        //            // Show windows
        //            /*if (IsNewHighScore(Score.CurrentScore, Difficulty))
        //                HUD.Instance.ShowNewHighScoreWindow();
        //            else*/
        //            if (Difficulty == Constants.EASY)
        //                HUD.Instance.ShowWinGameWindow();
        //            else if (Difficulty == Constants.NORMAL)
        //                HUD.Instance.ShowWinGameWindow();
        //            else if (Difficulty != Constants.HARD)
        //                HUD.Instance.ShowWinGameLastDifficultyWindow();

        //            EndGame();
        //        }
        //        else
        //        {
        //            HUD.Instance.ShowLevelCompletedWindow();
        //            CurrentLevel++;
        //            DataStore.SaveGame();
        //            Destroy(Field.gameObject);
        //        }
        //    }
        //    else
        //    {
        //        //if (Level.Turns > 0)
        //        //{
        //        //    Level.Turns--;
        //        //    HUD.Instance.PlayAnimationChangeTurnsValue();
        //        //}

        //        if (Level.Turns <= 0)
        //        {
        //            if (IsNewHighScore(Score.CurrentScore, Difficulty))
        //                HUD.Instance.ShowNewHighScoreWindow();
        //            else
        //                HUD.Instance.ShowEndGameWindow();

        //            EndGame();
        //        }
        //    }
        //}
    }

 //   public void InitializeLevel()
	//{
 //       TokenGameObjects = new List<GameObject>();
 //       TokenPositions = new List<Vector3>();

 //       Level = new LevelParameters(CurrentLevel);
 //       HUD.Instance.UpdateDifficultyValue(Difficulty);
 //       HUD.Instance.UpdateLevelValue(CurrentLevel);

 //       if (CurrentLevel == 1)
 //       {
 //           Score.CurrentScore = 0;
 //       }

 //       m_turns = Level.Turns;

	//	TokensByTypes = new List<List<Token>> ();
	//	for (int i = 0; i < Level.TokenTypes; i++) 
	//	{
	//		TokensByTypes.Add (new List<Token> ());
	//	}

	//	Field = Field.Create (Level.FieldSize, Level.FreeSpace);
 //   }

    public void InitializeLevelNormalMode()
    {
        TokenGameObjects = new List<GameObject>();
        TokenPositions = new List<Vector3>();

        //Level = new LevelParameters(CurrentLevel);
        HUD.Instance.UpdateDifficultyValue(Difficulty);
        HUD.Instance.UpdateLevelValue(CurrentLevel);

        //if (CurrentLevel == 1)
        //{
        //    Score.CurrentScore = 0;
        //}

        Level.SetTokenTypesCount(CurrentLevel, Difficulty);
        TokensByTypes = new List<List<Token>>();
        for (int i = 0; i < Level.TokenTypes; i++)
        {
            TokensByTypes.Add(new List<Token>());
        }

        Field = Field.CreateNormalMode(CurrentLevel, Difficulty);
        HUD.Instance.CreateStarIndicator(CurrentLevel, Difficulty);
        Level.SetTurns(CurrentLevel, Difficulty);
    }

    //public void NewGame()
    //{
    //    //if (Difficulty == Constants.EASY)
    //    //    CurrentLevelEasy = 1;
    //    //else if (Difficulty == Constants.NORMAL)
    //    //    CurrentLevelNormal = 1;
    //    //else if (Difficulty == Constants.HARD)
    //    //    CurrentLevelHard = 1;

    //    CurrentLevel = 1;
    //    Score.CurrentScore = 0;
    //    DataStore.SaveGame();

    //    if (Field != null)
    //    {
    //        Destroy(Field.gameObject);
    //    }

    //    InitializeLevel();
    //    HUD.Instance.PlayAnimationChangeTurnsValue();
    //}

    //public void RestartLevel()
    //{
    //    int scoreDecrease = Level.FieldSize - 1;
    //    if ((Score.CurrentScore - scoreDecrease) > 0)
    //    {
    //        Destroy(Field.gameObject);
    //        InitializeLevel();
    //        Score.CurrentScore -= scoreDecrease;
    //        HUD.Instance.UpdateScoreDecreaseValue(scoreDecrease);
    //        DataStore.SaveGame();
    //        //HUD.Instance.PlayAnimationChangeTurnsValue();
    //    }
    //}

    public void UndoTurn()
    {
        int lastElement = TokenGameObjects.Count - 1;
        if (lastElement >= 0)
        {
            TokenGameObjects[lastElement].transform.position = TokenPositions[lastElement];
            TokenGameObjects.RemoveAt(lastElement);
            TokenPositions.RemoveAt(lastElement);
            Level.Turns++;
            HUD.Instance.PlayAnimationChangeTurnsValue();

            if (Mode == GameMode.Normal)
            {
                HUD.Instance.SetStar(CurrentLevel, Difficulty, true);
            }
        }
    }

    //public void AddHighScoreValue(int score, string name, int difficulty)
    //{
    //    for (int i = 0; i < HighScoreValues[difficulty].Count; i++)
    //    {
    //        if (HighScoreValues[difficulty][i].ScoreValue < score)
    //        {
    //            Swap(ref HighScoreValues[difficulty][i].ScoreValue, ref score);
    //            Swap(ref HighScoreValues[difficulty][i].Name, ref name);
    //        }
    //    }
    //    DataStore.SaveHighScore();


    //    //switch (difficulty)
    //    //{
    //    //    case DifficultyLevel.Easy:
    //    //        for (int i = 0; i < HighScoreValues[0].Count; i++)
    //    //        {
    //    //            if (HighScoreValues[0][i].ScoreValue < score)
    //    //            {
    //    //                Swap(ref HighScoreValues[0][i].ScoreValue, ref score);
    //    //                Swap(ref HighScoreValues[0][i].Name, ref name);
    //    //            }
    //    //        }

    //    //        break;
    //    //    case DifficultyLevel.Normal:
    //    //        for (int i = 0; i < HighScoreValues[1].Count; i++)
    //    //        {
    //    //            Debug.Log(HighScoreValues[1][i].ScoreValue);
    //    //            if (HighScoreValues[1][i].ScoreValue < score)
    //    //            {
    //    //                Swap(ref HighScoreValues[1][i].ScoreValue, ref score);
    //    //                Swap(ref HighScoreValues[1][i].Name, ref name);
    //    //            }
    //    //        }
    //    //        break;
    //    //    case DifficultyLevel.Hard:
    //    //        for (int i = 0; i < HighScoreValues[2].Count; i++)
    //    //        {
    //    //            if (HighScoreValues[2][i].ScoreValue < score)
    //    //            {
    //    //                Swap(ref HighScoreValues[2][i].ScoreValue, ref score);
    //    //                Swap(ref HighScoreValues[2][i].Name, ref name);
    //    //            }
    //    //        }
    //    //        break;
    //    //}
    //    //DataStore.SaveHighScore();
    //}

    //private void Swap<T>(ref T lhs, ref T rhs)
    //{
    //    T temp;
    //    temp = lhs;
    //    lhs = rhs;
    //    rhs = temp;
    //}

    //public bool IsNewHighScore(int score, int difficulty)
    //{
    //    for (int i = 0; i < HighScoreValues[difficulty].Count; i++)
    //    {
    //        if (HighScoreValues[difficulty][i].ScoreValue < score)
    //            return true;
    //    }

    //    return false;

    //    //switch (difficulty)
    //    //{
    //    //    case DifficultyLevel.Easy:
    //    //        for (int i = 0; i < HighScoreValues[0].Count; i++)
    //    //        {
    //    //            if (HighScoreValues[0][i].ScoreValue < score)
    //    //                return true;
    //    //        }
    //    //        break;
    //    //    case DifficultyLevel.Normal:
    //    //        for (int i = 0; i < HighScoreValues[1].Count; i++)
    //    //        {
    //    //            if (HighScoreValues[1][i].ScoreValue < score)
    //    //                return true;
    //    //        }
    //    //        break;
    //    //    case DifficultyLevel.Hard:
    //    //        for (int i = 0; i < HighScoreValues[2].Count; i++)
    //    //        {
    //    //            if (HighScoreValues[2][i].ScoreValue < score)
    //    //                return true;
    //    //        }
    //    //        break;
    //    //    default:
    //    //        return false;
    //    //}
    //    //return false;  // mindless

    //}

    //private void EndGame()
    //{
    //   // XMLManager.Instance.SaveData(Difficulty.ToString(), CurrentLevel, Level.MaxTurns - Level.Turns, Level.MaxTurns, true);
    //   // XMLManager.Instance.SaveData2(Level.FieldSize, Level.FreeSpace, Level.TokenTypes, Level.MaxTurns - Level.Turns, Level.MaxTurns, true);
    
    //    //AddHighScoreValue(Score.CurrentScore, Score.Name, Difficulty);
    //    CurrentLevel = 1;
    //    DataStore.SaveGame();
    //    Destroy(Field.gameObject);
    //}

    private int CountStars(int level, int difficulty)
    {
        level--;
        int stars = 0;

        if (Level.Turns >= (Level.MaxTurns - LevelTurnsList[difficulty][level].ThreeStars))
            stars = 3;
        if (Level.Turns < (Level.MaxTurns - LevelTurnsList[difficulty][level].ThreeStars))
            stars = 2;
        if (Level.Turns < (Level.MaxTurns - LevelTurnsList[difficulty][level].TwoStars))
            stars = 1;
        //if (Level.Turns < (Level.MaxTurns - LevelTurnsList[difficulty][level].OneStar))
        //    stars = 0;

        if (LevelStars[difficulty][level] < stars)
        {
            LevelStars[difficulty][level] = stars;
        }

        return stars;
    }

    public int[] GetCompletedLevels()
    {
        int[] completedLevels = new int[LevelStars.Count];
        for (int i = 0; i < LevelStars.Count; i++)
        {
            for (int j = 0; j < LevelStars[i].Count; j++)
            {
                if (LevelStars[i][j] != 0)
                    completedLevels[i]++;
            }
        }

        return completedLevels;
    }

    private void OnApplicationQuit()
    {
        DataStore.SaveGame();
        //DataStore.SaveNewGame();
    }

    #endregion
}
