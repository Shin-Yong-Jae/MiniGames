using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game1TopPanel : MonoBehaviour
{
    #region Variables

    public int score { get; private set; } = 0;
    [SerializeField] private TMP_Text textScore;
    #endregion

    #region Unity Methods

    private void Start()
    {
        SetScore(0);
    }

    #endregion
    
    #region Main Methods
    /// <summary> 점수 증가 UI </summary>
    public void SetScore(int value)
    {
        score += value;
        textScore.text = $"Score : {score.ToString()}";
    }

    public void GameOver()
    {
        textScore.text = $"Game Over";
    }
    #endregion
}
