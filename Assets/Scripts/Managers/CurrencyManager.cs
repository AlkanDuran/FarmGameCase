using System;
using UnityEngine;

public class CurrencyManager : LocalSingleton<CurrencyManager>
{
    [SerializeField] private CurrencyPanel _currencyPanel;
    [SerializeField] private string _currencyPrefName = "CURRENCY";
    [SerializeField] private bool _resetMoney = false;

    private float _currency = 0;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            DealCurrency(999);
        }
    }

    public void Init()
    {
        Load();
    }

    public float GetCurrency()
    {
        return _currency;
    }

    public void DealCurrency(float value)
    {
        _currency += value;
        UpdateCurrencyPanel();
        Save();
    }
    
    private void UpdateCurrencyPanel()
    {
        _currencyPanel.SetCurrencyAmount(_currency, false);
    }
       

    private void Load()
    {
        _currency = _resetMoney ? 0 : PlayerPrefs.GetFloat(_currencyPrefName);
        UpdateCurrencyPanel();
    }

    private void Save()
    {
        PlayerPrefs.SetFloat(_currencyPrefName, _currency);
    }

    public CurrencyPanel GetCurrencyPanel() => _currencyPanel;
}