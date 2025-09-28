using System;
using UnityEngine;
using UnityEngine.Events;

public class ShortcutsManager : MonoBehaviour
{
    [SerializeField] private ShortcutEvent[] Shortcuts;

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            foreach (var shortcut in Shortcuts)
            {
                if (Input.GetKeyDown(shortcut.key))
                {
                    shortcut.keyEvent?.Invoke();
                    //break;
                }
            }
        }
    }
}

[Serializable]
public struct ShortcutEvent
{
    public KeyCode key { get => _shortKey; }
    public UnityEvent keyEvent { get => _keyEvent; }

    [SerializeField] private KeyCode _shortKey;
    [SerializeField] private UnityEvent _keyEvent;
}