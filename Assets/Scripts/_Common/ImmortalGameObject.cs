using System;
using UnityEngine;

namespace _Common
{
    [DisallowMultipleComponent]
    public sealed class ImmortalGameObject : MonoBehaviour
    {
        private void Awake() =>
            DontDestroyOnLoad(gameObject);
    }
}