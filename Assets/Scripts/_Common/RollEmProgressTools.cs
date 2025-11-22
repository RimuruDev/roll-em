using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class RollEmProgressTools
{
#if UNITY_EDITOR
    [MenuItem("Tools/Roll'em/Clear Progress")]
#endif
    public static void ClearProgress()
    {
        var saveDirectory = Application.persistentDataPath + "/saves";
        var saveFile = saveDirectory + "/save.json";

        if (File.Exists(saveFile))
            File.Delete(saveFile);

        if (Directory.Exists(saveDirectory) && Directory.GetFiles(saveDirectory).Length == 0)
            Directory.Delete(saveDirectory, true);

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        AppLogWrapper.Send("<color=yellow>Roll'em progress cleared!</color>");
    }
}
