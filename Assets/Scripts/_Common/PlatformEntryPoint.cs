#if (UNITY_STANDALONE_WIN && !UNITY_EDITOR)
#define RIM_DEF_WIN_API
#endif

using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.SceneManagement;

[Preserve]
public static class AppConstants
{
    public const string BootSceneName = "LoadScene";
    public const string GameplaySceneName = "GameScene";
}

[Preserve]
public static class AppLogWrapper
{
    public static bool IsEnabled;

    public static void Send(string message, Object ctx = null)
    {
        if (!IsEnabled)
            return;

        Debug.Log($"{message}", ctx);
    }
}

[Preserve]
public static class PlatformEntryPoint
{
#if !RIM_DEF_WIN_API
    private static Camera _backgroundCamera;
#endif

    [Preserve]
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnAfterSceneLoad()
    {
        AppLogWrapper.IsEnabled = true;

#if !RIM_DEF_WIN_API
        SceneManager.activeSceneChanged += OnActiveSceneChanged;

        FixCameraBackground(SceneManager.GetActiveScene());
#endif
    }

    [Preserve]
    private static void OnActiveSceneChanged(Scene previous, Scene next)
    {
#if !RIM_DEF_WIN_API
        FixCameraBackground(next);
#endif
    }

    [Preserve]
    private static void FixCameraBackground(Scene scene)
    {
#if !RIM_DEF_WIN_API
        if (!scene.name.Contains(AppConstants.GameplaySceneName))
            return;

        var mainCamera = FindMainCamera(scene);
        if (mainCamera == null)
            return;

        mainCamera.backgroundColor = Color.black;

        EnsureBackgroundCamera(mainCamera);

        AppLogWrapper.Send(
            message: $"<color=green>Fixed backgroundColor for scene '{scene.name}'.</color>",
            ctx: mainCamera
        );
#endif
    }

#if !RIM_DEF_WIN_API
    private static Camera FindMainCamera(Scene scene)
    {
        var roots = scene.GetRootGameObjects();
        for (var i = 0; i < roots.Length; i++)
        {
            var camera = roots[i].GetComponentInChildren<Camera>(true);
            if (camera != null)
                return camera;
        }

        return Camera.main;
    }

    private static void EnsureBackgroundCamera(Camera mainCamera)
    {
        // NOTE: Что бы не менять что-либо на сцене, просто сделаю все через код)
        // В целом ничего страшного если под другие платформы !windows будет спавниться доп камера для фикса фона :3
        if (_backgroundCamera != null)
            return;

        var go = new GameObject("__PlatformBackgroundCamera");
        _backgroundCamera = go.AddComponent<Camera>();
        Object.DontDestroyOnLoad(go);

        _backgroundCamera.clearFlags = CameraClearFlags.SolidColor;
        _backgroundCamera.backgroundColor = Color.black;
        _backgroundCamera.cullingMask = 0;
        _backgroundCamera.depth = mainCamera.depth - 10f;

        _backgroundCamera.orthographic = true;
        _backgroundCamera.orthographicSize = mainCamera.orthographicSize;
        _backgroundCamera.targetDisplay = mainCamera.targetDisplay;
       
        _backgroundCamera.rect = new Rect(0f, 0f, 1f, 1f);
    }
#endif
}
