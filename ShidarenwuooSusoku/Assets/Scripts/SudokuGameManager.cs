using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// 数独游戏的管理器
/// </summary>
public class SudokuGameManager : MonoBehaviour
{
    public static int girdLength = 9;// 网格的宽高
    public static int subGirdLength = 3;// 子网格的宽高
    // 子网格中 Cell的宽高
    public static int cellLength = SudokuGameManager.girdLength / SudokuGameManager.subGirdLength;
    
    public SudokuCell SudokuCell_Prefab; // 单元格的预制体
    
    // get; private set; 的写法是一种语法糖，表示这个值别的类可以读取和使用，但只有自己才能设置它
    public static SudokuGameManager Instance { get; private set; }
    
    public MainMenuPanel mainMenu;// 主界面相关逻辑
    public SudokuPlayPanel sudokuPlay; // 数独的游戏界面
    
    // 按照9X9的格子来算，一共是81个有效数字，如果已知的数字越多，那么未知的数字就越好推导
    // 所以难度的表达方式就是，设定一个数字，在数独创建完成之后，隐藏掉这些数字来增加难度
    public int difficulty = 20; // 默认的难度值
    
    public Image ipButtonsPanel; // 游戏输入按钮

    public List<Button>btnNums = new List<Button>(); // 数字输入

    // 记录上一次点击的格子 以便数字进行输入
    SudokuCell lastCell;
    
    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < btnNums.Count; i++)
        {
            // 经典的闭包错误
            // btnNums[i].onClick.AddListener(delegate(){ OnNumBtnClicked(i);});
            
            // 需要将i的地址传递出来 
            int index = i;
            btnNums[i].onClick.AddListener(delegate(){ OnNumBtnClicked(index);});
        }
    }

    private void Start()
    {
        // 程序启动的时候，默认显示开始界面
        OnBackToMenu();
    }
    
    // 返回到开始菜单
    public void OnBackToMenu()
    {
        // 程序启动的时候，默认显示开始界面
        mainMenu.gameObject.SetActive(true);
        sudokuPlay.gameObject.SetActive(false);
        
        // 执行游戏清理
        sudokuPlay.Clear();
    }
    
    // 开始游戏
    public void OnPlayNewGame()
    {
        // 隐藏开始界面 显示游戏界面
        mainMenu.gameObject.SetActive(false);
        sudokuPlay.gameObject.SetActive(true);
        
        // 隐藏数字输入面板
        ipButtonsPanel.gameObject.SetActive(false);
        
        // 执行游戏初始化
        sudokuPlay.Init();
    }
    
    /// <summary>
    /// 将当前的数字输入和指定的Cell进行绑定
    /// </summary>
    /// <param name="cell"></param>
    public void ActivateInputButton(SudokuCell cell)
    {
        ipButtonsPanel.gameObject.SetActive(true);
        lastCell = cell;
    }
    
    /// <summary>
    /// 点击了某个数字按钮
    /// </summary>
    /// <param name="num"></param>
    public void OnNumBtnClicked(int num)
    {
        lastCell.UpdateValue(num);
        sudokuPlay.board.UpdatePuzzle(lastCell.coordinate.x, lastCell.coordinate.y, num);
        ipButtonsPanel.gameObject.SetActive(false);
    }
    
}
