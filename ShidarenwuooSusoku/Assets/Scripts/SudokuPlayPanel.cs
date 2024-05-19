using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SudokuPlayPanel : MonoBehaviour
{
    public Button btnBackToMenu;// 返回主界面按钮
    public Button btnBackToMenuInWinPanel;// 获胜面板上的返回主界面按钮
    public Button btnNewLevel;// 新建一个关卡
    public Button btnReplay; // 重新开始本次关卡
    public Button btnComplete; // 完成关卡
    public TextMeshProUGUI txtWrongTips; // 结果错误提示
    public Image imgWinPanel; // 获胜界面
    public TextMeshProUGUI txtTimer;// 计时器
    float levelStartTime = 0f; // 关卡开始的时间，用于计时器的计算

    public SudokuBoard board;// 游戏核心逻辑
    
    private void Awake()
    {
        // 返回主菜单
        btnBackToMenu.onClick.AddListener(OnBtnBackToMenuClicked);
        btnBackToMenuInWinPanel.onClick.AddListener(OnBtnBackToMenuClicked);
        
        // 新建关卡
        btnNewLevel.onClick.AddListener(OnBtnNewClicked);
        
        // 重新开始本关卡
        btnReplay.onClick.AddListener(OnbtnReplayClicked);
        
        // 完成关卡
        btnComplete.onClick.AddListener(OnbtnCompleteClicked);
    }
    
    private void Update()
    {
        // 计时器
        CountTimer();
    }
    
    /// <summary>
    /// 初始化游戏
    /// </summary>
    public void Init()
    {
        // 隐藏错误提示
        txtWrongTips.gameObject.SetActive(false);
        
        // 隐藏获胜面板
        imgWinPanel.gameObject.SetActive(false);
        
        // 记录当前的时间戳
        levelStartTime = Time.realtimeSinceStartup;
        
        // 核心逻辑初始化
        board.Init();
    }


    public void Clear()
    {
        levelStartTime = 0;// 清除计时器开始时间
        
        // 核心逻辑清理
        board.Clear();
    }
    
    /// <summary>
    /// 计时器逻辑
    /// </summary>
    void CountTimer()
    {
        float t = Time.realtimeSinceStartup - levelStartTime;
        int seconds = (int)(t % 60);
        t /= 60;
        int minutes = (int)(t % 60);

        txtTimer.text = string.Format("{0}:{1}", minutes.ToString("00"), seconds.ToString("00"));
    }
    
    /// <summary>
    /// 返回主菜单
    /// </summary>
    public void OnBtnBackToMenuClicked()
    {
        // 清理游戏
        Clear();
        
        // 返回主菜单
        SudokuGameManager.Instance.OnBackToMenu();
    }
    
    /// <summary>
    /// 开始新关卡
    /// </summary>
    public void OnBtnNewClicked()
    {
        // 先清理再初始化
        Clear();
        Init();
    }
    
    /// <summary>
    /// 重玩本关卡
    /// </summary>
    public void OnbtnReplayClicked()
    {
        // 调用核心逻辑的重玩本局
        board.RestartGame();
    }
    
    /// <summary>
    /// 完成游戏
    /// </summary>
    public void OnbtnCompleteClicked()
    {
        // 检查是否完成游戏
        if (board.CheckComplete())
        {
            imgWinPanel.gameObject.SetActive(true);
        }
        else
        {
            txtWrongTips.gameObject.SetActive(true);
            // 3秒后 错误提示消失
            StartCoroutine(HideWrongText());
        }
    }

    IEnumerator HideWrongText()
    {
        yield return new WaitForSeconds(3.0f);
        txtWrongTips.gameObject.SetActive(false);
    }
    
}