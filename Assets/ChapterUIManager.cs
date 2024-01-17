using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChapterUIManager : MonoBehaviour
{
    [SerializeField]
    Button backButton;

    void Start()
    {
        // AddListener
        backButton.onClick.AddListener(OnClick_BackButton);
    }

    void OnClick_BackButton()
    {
        Debug.Log("확인용");
        SceneManager.LoadScene("Main");
    }
}
