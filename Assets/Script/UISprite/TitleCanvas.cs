using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleCanvas : MonoBehaviour
{
    [SerializeField]
    Button gameStartButton;
 
    void Start()
    {
        // AddListener
        gameStartButton.onClick.AddListener(OnClick_GameStartButton);
    }

    void OnClick_GameStartButton()
    {
        SceneManager.LoadScene("Main");
    }
}
