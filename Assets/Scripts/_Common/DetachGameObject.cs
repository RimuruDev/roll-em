using UnityEngine;

namespace _Common
{
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-9)]
    public sealed class DetachGameObject : MonoBehaviour
    {
        private void Awake() => 
            transform.SetParent(null);
    }
}