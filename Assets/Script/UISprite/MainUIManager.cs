using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUIManager : MonoBehaviour
{
    [SerializeField]
    Button backButton, chapter1Button, chapter2Button, chapter3Button, chapterStartButton;

    int selectChapterIndex = 1;
    string selectChapter;

    void Start()
    {
        SetChapterButton(selectChapterIndex);

        // AddListener
        backButton.onClick.AddListener(OnClick_BackButton);
        chapter1Button.onClick.AddListener(OnClick_Chapter1Button);
        chapter2Button.onClick.AddListener(OnClick_Chapter2Button);
        chapter3Button.onClick.AddListener(OnClick_Chapter3Button);
        chapterStartButton.onClick.AddListener(Onclick_ChapterStartButton);
    }

    void Update()
    {

    }

    void SetChapterButton(int pChapterIndex)
    {
        chapter1Button.interactable = pChapterIndex == 1 ? false : true;
        chapter2Button.interactable = pChapterIndex == 2 ? false : true;
        chapter3Button.interactable = pChapterIndex == 3 ? false : true;

        selectChapter = string.Format("Chapter{0}", selectChapterIndex);
    }

    void OnClick_BackButton()
    {
        SceneManager.LoadScene("Title");
    }

    void OnClick_Chapter1Button()
    {
        selectChapterIndex = 1;

        SetChapterButton(selectChapterIndex);
    }

    void OnClick_Chapter2Button()
    {
        selectChapterIndex = 2;

        SetChapterButton(selectChapterIndex);
    }

    void OnClick_Chapter3Button()
    {
        selectChapterIndex = 3;

        SetChapterButton(selectChapterIndex);
    }

    void Onclick_ChapterStartButton()
    {
        SceneManager.LoadScene(selectChapter);
    }
}
