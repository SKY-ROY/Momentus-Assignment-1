using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text healthCountText;
    [SerializeField] TMP_Text scoreCountText;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Button restartButton;

    public static UIManager Instance;

    #region Lifecycle Methods
    void Awake()
    {
        restartButton.onClick.AddListener(delegate
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }

    void OnEnable()
    {
        PlayerController.OnHealthUpdate += OnHealthUpdateHandler;
        PlayerController.OnScoreUpdate += OnScoreUpdateHandler;
        PlayerController.OnGameOver += OnGameOverHandler;
    }

    void OnDisable()
    {
        PlayerController.OnHealthUpdate -= OnHealthUpdateHandler;
        PlayerController.OnScoreUpdate -= OnScoreUpdateHandler;
        PlayerController.OnGameOver -= OnGameOverHandler;
    }
    #endregion

    #region Event Handler Methods
    void OnHealthUpdateHandler(int hp)
    {
        healthCountText.text = hp.ToString();
    }

    void OnScoreUpdateHandler(int score)
    {
        scoreCountText.text = score.ToString();
    }

    void OnGameOverHandler()
    {
        gameOverPanel.SetActive(true);
    }
    #endregion
}
