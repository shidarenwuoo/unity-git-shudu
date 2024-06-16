using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp : MonoBehaviour
{
    private void Start()
    {
        // 创建必要的Manager
        UIManager.Create();
        
        // 打开主界面
        UIManager.Instance.OpenUI(UINames.UIStartUp);
    }
}
