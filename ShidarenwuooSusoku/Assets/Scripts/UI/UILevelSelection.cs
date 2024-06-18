using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelSelection : UIPanel
{
    [SerializeField] private List<Button> levels;

    [SerializeField] private Button returnButton;
    
    private void Awake()
    {
        for (int i = 0; i < levels.Count; ++i)
        {
            int levelIndex = i;
            levels[i].onClick.AddListener(()=> SelectLevel(levelIndex));
        }
        
        returnButton.onClick.AddListener(OnReturn);
    }
    
    public override void Open()
    {
        int unlockLevel = GameData.GetUnlockLevel();
        for (int i = 0; i < levels.Count; ++i)
        {
            levels[i].gameObject.SetActive(i <= unlockLevel);
        }
    }

    private void SelectLevel(int level)
    {
        // 进入游玩界面
        var playPanel = UIManager.Instance.OpenUI(UINames.UISudokuPanel) as UISudokuPanel;
        playPanel.CreateSudoku(level);

        Close();
    }

    private void OnReturn()
    {
        Close();
        UIManager.Instance.OpenUI(UINames.UIStartUp);
    }
}
