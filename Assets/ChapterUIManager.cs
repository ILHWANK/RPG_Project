using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChapterUIManager : MonoBehaviour
{
    [SerializeField]
    Button backButton, skiil1Button, skill2Button, skill3Button, victoryExitButton, defeatRetryButton, defeatExitButton;

    [SerializeField]
    GameObject skill1Object, skill2Object, skill3Object, victoryObject, defeatObject;

    void Start()
    {
        // AddListener
        backButton.onClick.AddListener(OnClick_BackButton);
        skiil1Button.onClick.AddListener(OnClick_Skill1Button);
        victoryExitButton.onClick.AddListener(OnClick_VictoryExitButton);
        defeatRetryButton.onClick.AddListener(OnClick_DefeatRetryButton);
        defeatExitButton.onClick.AddListener(OnClick_DefeatExitButton);

    }

    void TempExit()
    {
        SceneManager.LoadScene("Main");
    }

    // Pnael
    void EnableVictoryPanel(bool isEnable)
    {
        victoryObject.SetActive(isEnable);
    }

    void EnableDefeatPanel(bool isEnable)
    {
        defeatObject.SetActive(isEnable);
    }

    // OnClick
    void OnClick_BackButton()
    {
        EnableDefeatPanel(true);
    }

    void OnClick_Skill1Button()
    {
        EnableVictoryPanel(true);
    }

    void OnClick_Skill2Button()
    {

    }

    void OnClick_Skill3Button()
    {

    }

    void OnClick_VictoryExitButton()
    {
        TempExit();
    }

    void OnClick_DefeatRetryButton()
    {
        SceneManager.LoadScene("Chapter1");
    }

    void OnClick_DefeatExitButton()
    {
        TempExit();
    }
}
