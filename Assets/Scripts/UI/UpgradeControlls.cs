using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeControlls : MonoBehaviour
{
    [SerializeField] private Upgrade _upgrade;
    [SerializeField] private SegmentedBar _bar;
    [SerializeField] private Button _button;

    private EventTrigger _eventTrigger;
    private TMP_Text _upgradeInfoText;
    private PropertyInfo _prop;
    private int _nextLevel = 0;

    private void Awake()
    {

        Type type = _upgrade.component.GetType();
        _prop = type.GetProperty(_upgrade.propertyName, BindingFlags.Public | BindingFlags.Instance);

        if (_prop == null)
        {
            Debug.LogWarning($"Property {_upgrade.propertyName} doesn't exists in {_upgrade.component.GetType()}");
            this.enabled = false;
        }

        _upgradeInfoText = Links.upgradeInfoText;
        _eventTrigger = GetComponent<EventTrigger>();
        _bar.segmentsCount = _upgrade.LevelValues.Length;
    }

    private void OnValidate()
    {
        if (_upgrade.LevelValues != null && _upgrade.LevelCosts != null)
        {
            if (_upgrade.LevelValues.Length != _upgrade.LevelCosts.Length)
            {
                Array.Resize(ref _upgrade.LevelCosts, _upgrade.LevelValues.Length);
            }
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Wallet.AddCoins(1);
        }
    }
#endif

    public void ApplyNextUpgrade()
    {
        if (Wallet.BringCoins(_upgrade.LevelCosts[_nextLevel]))
        {
            Type propType = _prop.GetValue(_upgrade.component).GetType();
            if (propType == typeof(float))
            {
                _prop.SetValue(_upgrade.component, _upgrade.LevelValues[_nextLevel]);
            }
            else if (propType == typeof(int))
            {
                _prop.SetValue(_upgrade.component, (int)_upgrade.LevelValues[_nextLevel]);
            }


            Debug.Log(_prop.GetValue(_upgrade.component));
            _nextLevel++;
            _bar.EnableNewSegments(1);

            if (_nextLevel >= _upgrade.LevelValues.Length)
            {
                _button.interactable = false;
                this.enabled = false;
                _eventTrigger.enabled = false;
                OnMouse_Exit();
            }
        }
    }

    public void OnMouse_Enter()
    {
        if (this.enabled)
        {
            _upgradeInfoText.gameObject.SetActive(true);
            _upgradeInfoText.text = $"[New: {_upgrade.LevelValues[_nextLevel]} | Old: {_prop.GetValue(_upgrade.component)}] - {_upgrade.LevelCosts[_nextLevel]}";
        }
    }

    public void OnMouse_Exit()
    {
        _upgradeInfoText.gameObject.SetActive(false);
        _upgradeInfoText.text = "";
    }
}

[Serializable]
public struct Upgrade
{
    public Component component;
    public string propertyName;
    public float[] LevelValues;
    public int[] LevelCosts;
}