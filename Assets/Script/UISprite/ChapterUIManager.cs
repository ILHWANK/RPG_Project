using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChapterUIManager : MonoBehaviour
{
    [SerializeField]
    Button backButton, skiil1Button, skill2Button, skill3Button, victoryExitButton, defeatRetryButton, defeatExitButton, PauseRetryButton, PauseExitButton, PauseReturnButton;

    [SerializeField]
    GameObject skill1Object, skill2Object, skill3Object, victoryObject, defeatObject, pauseObject, allySkillInfoObject, enemySkillInfoObject;

    [SerializeField]
    Text timeText, allySkillCharacterName, allySkillInfo, enemySkillCharacterName, enemySkillInfo, skill1NameText, skill1CoolText, skill2NameText, skill2CoolText, skill3NameText, skill3CoolText;

    [SerializeField]
    Slider allyHpSlider, enemyHpSlider;

    GameManager gameManager;

    bool isPause = false;

    private void Awake()
    {
        
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        // AddListener
        backButton.onClick.AddListener(OnClick_BackButton);

        skiil1Button.onClick.AddListener(OnClick_Skill1Button);
        skill2Button.onClick.AddListener(OnClick_Skill2Button);
        skill3Button.onClick.AddListener(OnClick_Skill3Button);

        victoryExitButton.onClick.AddListener(OnClick_VictoryExitButton);

        defeatRetryButton.onClick.AddListener(OnClick_DefeatRetryButton);
        defeatExitButton.onClick.AddListener(OnClick_DefeatExitButton);

        PauseRetryButton.onClick.AddListener(OnClick_PauseRetryButton);
        PauseExitButton.onClick.AddListener(OnClick_PauseExitButton);
        PauseReturnButton.onClick.AddListener(OnClick_PauseReturnButton);
    }

    void Update()
    {
        gameManager = FindObjectOfType<GameManager>();

        for (int i = 0; i < 3; ++i)
        {
            bool isSkillObject = false;
            bool isSkillActive = true;

            string skillName = "";
            string skillCool = "";

            if (i < gameManager.GetAllyAll().Length)
            {
                isSkillObject = gameManager.GetAllyAll()[i].isAlive;
                isSkillActive = gameManager.GetAllyAll()[i].SkillActive();

                skillName = gameManager.GetAllyAll()[i].skillName;
                skillCool = string.Format("{0}", Mathf.FloorToInt(gameManager.GetAllyAll()[i].skillCoolTimeCur) + 1);
            }

            if (1 == i + 1)
            {
                skiil1Button.gameObject.SetActive(isSkillObject);
                skiil1Button.interactable = isSkillActive;

                skill1NameText.text = skillName;
                skill1CoolText.text = skillCool;
            }
            else if (2 == i + 1)
            {
                skill2Button.gameObject.SetActive(isSkillObject);
                skill2Button.interactable = isSkillActive;

                skill2NameText.text = skillName;
                skill2CoolText.text = skillCool;
            }
            else if (3 == i + 1)
            {
                skill3Button.gameObject.SetActive(isSkillObject);
                skill3Button.interactable = isSkillActive;

                skill3NameText.text = skillName;
                skill3CoolText.text = skillCool;
            }

        }

        if (gameManager.isVictory)
        {
            EnableVictoryPanel(true);
            gameManager.Pause();
        }
        else if (gameManager.isDefeat)
        {
            EnableDefeatPanel(true);
            gameManager.Pause();
        }
        else if (isPause)
        {
            gameManager.Pause();
        }
        else
        {
            Time.timeScale = gameManager.gameSpeed;
        }

        // Temp UI Update
        timeText.text = string.Format("{0:D2}", Mathf.FloorToInt((gameManager.chapterMaxTime - gameManager.chapterCurTime) % 60));

        allyHpSlider.value  = gameManager.allyCurHp / gameManager.allyTotalHp;
        enemyHpSlider.value = gameManager.enemyCurHp / gameManager.enemyTotalHp;
    }

    void TempExit()
    {
        SceneManager.LoadScene("Main");
    }

    void TempReturn()
    {
        isPause = false;
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

    void EnablePausePanel(bool isEnable)
    {
        pauseObject.SetActive(isEnable);
    }

    void EnableAllySkillInfoPanel(bool isEnable)
    {
        allySkillInfoObject.SetActive(isEnable);
    }

    void EnableEnemySkillInfoPanel(bool isEnable)
    {
        enemySkillInfoObject.SetActive(isEnable);
    }

    public void OpenSkillAllyInfo(int pIndex)
    {
        Ally allyData = gameManager.GetAlly(pIndex);

        allySkillCharacterName.text = allyData.skillName;
        allySkillInfo.text = allyData.skillInfo;

        allyData.SkillAction();

        EnableAllySkillInfoPanel(true);
    }

    public void CloseSkillAllyInfo()
    {
        EnableAllySkillInfoPanel(false);
    }

    // OnClick
    void OnClick_BackButton()
    {
        EnablePausePanel(true);
        isPause = true;
    }

    void OnClick_Skill1Button()
    {
        OpenSkillAllyInfo(0);
    }

    void OnClick_Skill2Button()
    {
        OpenSkillAllyInfo(1);
    }

    void OnClick_Skill3Button()
    {
        OpenSkillAllyInfo(2);
    }

    void OnClick_VictoryExitButton()
    {
        TempExit();
    }

    void OnClick_DefeatRetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnClick_DefeatExitButton()
    {
        TempExit();
    }

    void OnClick_PauseRetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnClick_PauseExitButton()
    {
        TempExit();
    }

    void OnClick_PauseReturnButton()
    {
        EnablePausePanel(false);
        TempReturn();
    }
}
