using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class UIManager : SingletonMono<UIManager>
{
    private static readonly string UIPathRoot = "Prefab/UI/";

    private Transform uiRoot;

    protected override void Init()
    {
        base.Init();

        GameObject rootPrefab = Resources.Load<GameObject>("Prefab/Util/UIRoot");
        GameObject rootInstance = GameObject.Instantiate(rootPrefab);
        GameObject.DontDestroyOnLoad(rootInstance);
        uiRoot = rootInstance.transform;
    }

    public UIPanel OpenUI(string uiPath)
    {
        UIPanel uiPanel = Resources.Load<UIPanel>(UIPathRoot + uiPath);
        UIPanel instance = GameObject.Instantiate(uiPanel, uiRoot);
        instance.Open();
        
        return instance;
    }
}
