using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    private static readonly string UnlockLevelKey = "SaveData_UnlockLevel";

    public static void SaveLevel(int level)
    {
        PlayerPrefs.SetInt(UnlockLevelKey, level);
        
        PlayerPrefs.Save();
    }

    public static int GetUnlockLevel()
    {
        return PlayerPrefs.GetInt(UnlockLevelKey, 0);
    }
}
