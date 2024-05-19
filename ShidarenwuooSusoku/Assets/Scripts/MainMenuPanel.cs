using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    public Button btnPlay; // 开始按钮
    public Slider sldDifficulty; // 难度滑动条

    private void Awake()
    {
        // 监听滑动条滑动事件
        sldDifficulty.onValueChanged.AddListener(OnSliderChange);
        
        // 开始按钮点击事件
        btnPlay.onClick.AddListener(OnPlayGame);
    }
    
    
    void Start()
    {
        // 启动的时候保存难度值
        SudokuGameManager.Instance.difficulty = (int)sldDifficulty.value;
    }

    public void OnSliderChange(float value)
    {
        // 滑动条变化的时候保存难度值
        SudokuGameManager.Instance.difficulty = (int)sldDifficulty.value;
    }
    
    public void OnPlayGame()
    {
        SudokuGameManager.Instance.OnPlayNewGame();
    }
}