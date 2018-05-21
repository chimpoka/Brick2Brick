using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectLevelScene : MonoBehaviour
{
    [SerializeField]
    private GameObject m_ButtonsContainer;

    private void Start()
    {
        UpdateContainerLength();
        CreateButtons();
        Controller.Instance.MaxLevel = Controller.Instance.LevelTurnsList[Controller.Instance.Difficulty].Count;
    }

    private void CreateButtons()
    {
        int difficulty = Controller.Instance.Difficulty;       

        for (int level = 0; level < Controller.Instance.LevelTurnsList[difficulty].Count; level++)
        {
            GameObject levelSelectButton = Instantiate(Resources.Load("Prefabs/SelectLevelButton")) as GameObject;
            levelSelectButton.GetComponentInChildren<Text>().text = (level + 1).ToString();
            levelSelectButton.GetComponent<RectTransform>().localScale = new Vector3(Screen.width / 654.5f, Screen.width / 654.5f, Screen.width / 720f);
            ShowStars(level, difficulty, levelSelectButton);
        
            levelSelectButton.transform.SetParent(m_ButtonsContainer.transform);
        }
    }

    private void UpdateContainerLength()
    {
        GridLayoutGroup GLG = m_ButtonsContainer.GetComponent<GridLayoutGroup>();
       // GLG.padding = new RectOffset(Screen.width / 120, 0, Screen.height / 256, Screen.height / 256);
       // GLG.cellSize = new Vector2 (Screen.width / 5.07f, Screen.height / 8.89f);
       // GLG.spacing = new Vector2(0, Screen.height / 25.6f);
        

        RectTransform RT = m_ButtonsContainer.GetComponent<RectTransform>();
        int difficulty = Controller.Instance.Difficulty;

        float containerLength = Mathf.Ceil((float)Controller.Instance.LevelTurnsList[difficulty].Count / 5f);
        containerLength = GLG.padding.top + GLG.padding.bottom + (GLG.cellSize.y + GLG.spacing.y) * containerLength;

        RT.sizeDelta = new Vector2(RT.sizeDelta.x, containerLength);
    }

    public void BackButton()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    private void ShowStars(int level, int difficulty, GameObject levelSelectButton)
    {
        Image[] objects = levelSelectButton.GetComponentsInChildren<Image>(true);
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
        Color disabled = new Color(1, 1, 1, 0/*40f/255*/);

        if (Controller.Instance.LevelStars[Controller.Instance.Difficulty][level] == 3)
        {
            foreach (var star in stars)
                star.color = enabled;
        }
        if (Controller.Instance.LevelStars[Controller.Instance.Difficulty][level] == 2)
        {
            stars[0].color = enabled;
            stars[1].color = enabled;
            stars[2].color = disabled;
        }
        if (Controller.Instance.LevelStars[Controller.Instance.Difficulty][level] == 1)
        {
            stars[0].color = enabled;
            stars[1].color = disabled;
            stars[2].color = disabled;
        }
        if (Controller.Instance.LevelStars[Controller.Instance.Difficulty][level] == 0)
        {
            stars[0].color = disabled;
            stars[1].color = disabled;
            stars[2].color = disabled;
        }
    }
}
