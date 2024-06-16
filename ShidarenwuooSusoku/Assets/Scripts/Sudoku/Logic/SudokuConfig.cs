using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuConfig : ScriptableObject
{
    private static SudokuConfig s_instance;

    public static SudokuConfig Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = Resources.Load<SudokuConfig>("Config/SudokuConfig");
            }

            return s_instance;
        }
    }
    
    
    [SerializeField]
    public List<SudokuLevelConfig> sudokuLevels = new List<SudokuLevelConfig>();

    public int LevelCount => sudokuLevels.Count;

    public SudokuLevelConfig GetLevelPuzzle(int level)
    {
        level = Mathf.Clamp(level, 0, LevelCount - 1);

        return sudokuLevels[level];
    }
}
