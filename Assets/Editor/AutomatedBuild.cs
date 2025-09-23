using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class AutomatedBuild
{
    [MenuItem("Build/Build Player to New Folder %#&b")] // Ctrl+Shift+Alt+B
    public static void BuildPlayerToNewFolder()
    {
        // Получаем платформу сборки
        BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;

        // Определяем имя папки
        string buildFolderName = $"v {Application.version} - " + DateTime.Now.ToString("yyyy.MM.dd_HH-mm-ss");
        string buildPath = Path.Combine("_builds", buildFolderName); // Папка "Builds" в корне проекта

        // Создаем папку, если ее нет
        if (!Directory.Exists("_builds"))
        {
            Directory.CreateDirectory("_builds");
        }

        Directory.CreateDirectory(buildPath);

        // Получаем список сцен для сборки
        string[] scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();

        // Настраиваем параметры сборки
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;

        // Определяем путь к исполняемому файлу (зависит от платформы)
        string executableName = PlayerSettings.productName;
        if (buildTarget == BuildTarget.StandaloneWindows64 || buildTarget == BuildTarget.StandaloneWindows)
        {
            executableName += ".exe";
        }
        else if (buildTarget == BuildTarget.StandaloneOSX)
        {
            executableName += ".app";
        }
        // Добавьте другие платформы по необходимости

        buildPlayerOptions.locationPathName = Path.Combine(buildPath, executableName);
        buildPlayerOptions.target = buildTarget;
        buildPlayerOptions.options = BuildOptions.None; // Например, BuildOptions.Development для отладочной сборки

        // Запускаем сборку
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            EditorUtility.RevealInFinder(buildPath); // Открываем папку с билдом
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed.");
        }
    }
}
