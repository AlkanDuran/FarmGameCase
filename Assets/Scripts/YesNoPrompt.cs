using UnityEngine;
using TMPro;
using System;

public class YesNoPrompt : LocalSingleton<YesNoPrompt>
{
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private GameObject _inner;
    
    private Action OnYesSelected = null;
    public bool IsYesNoPromptOpen { get; set; } = false;
    
    public void CreatePrompt(string message, Action onYesSelected)
    {
        this.OnYesSelected = onYesSelected;
        promptText.text = message;
    }

    public void Answer(bool yes)
    {
        if(yes && OnYesSelected !=null)
        {
            OnYesSelected();
            TutorialManager.Instance.IsPlayerSlept = true;
        }
        else
        {
            IsYesNoPromptOpen = false;
        }
        
        OnYesSelected = null;
        GameManager.Instance.CheckCursorLockState();
        ShowPrompt(false);
    }

    public void ShowPrompt(bool value)
    {
        IsYesNoPromptOpen = value;
        _inner.SetActive(value);
    }
}
