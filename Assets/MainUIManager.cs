using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUIManager : MonoBehaviour
{
    [SerializeField]
    Button backButton, chapterStartButton;

    void Start()
    {
        // AddListener
        backButton.onClick.AddListener(OnClick_BackButton);
        chapterStartButton.onClick.AddListener(Onclick_ChapterStartButton);
    }

    void OnClick_BackButton()
    {
        SceneManager.LoadScene("Title");
    }

    void Onclick_ChapterStartButton()
    {
        SceneManager.LoadScene("Chapter1");
    }
}
