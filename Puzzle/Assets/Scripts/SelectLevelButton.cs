using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectLevelButton : MonoBehaviour
{
    public Button selectLevelButton;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Controller.Instance.CurrentLevel = System.Convert.ToInt32(GetComponentInChildren<Text>().text);
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

}
