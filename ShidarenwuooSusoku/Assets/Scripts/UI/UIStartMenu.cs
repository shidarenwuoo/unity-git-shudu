using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStartMenu : UIPanel
{
    [SerializeField]
    private Button startButton;

    [SerializeField]
    private Button existButton;

    private void Awake()
    {
        startButton.onClick.AddListener(OnStartGame);
        existButton.onClick.AddListener(OnExistGame);
    }


    private void OnStartGame()
    {
        // 进入选关界面
        UIManager.Instance.OpenUI(UINames.UILevelSelection);

        Close();
    }

    private void OnExistGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public override void Open()
    {
        
    }

}
