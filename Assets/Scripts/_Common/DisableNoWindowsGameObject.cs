#if (UNITY_STANDALONE_WIN && !UNITY_EDITOR)
#define RIM_DEF_WIN_API
#endif

using UnityEngine;
using UnityEngine.Scripting;

namespace _Common
{
    [Preserve]
    [DisallowMultipleComponent]
    public sealed class DisableNoWindowsGameObject : MonoBehaviour
    {
        private void Awake()
        {
#if !RIM_DEF_WIN_API
            gameObject.SetActive(false);
#endif
        }
    }
}