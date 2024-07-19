using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanel : LocalSingleton<SettingsPanel>
{
    [SerializeField] GameObject _inputPanel;
    [SerializeField] GameObject _soundPanel;
    [SerializeField] GameObject _exitPanel;
    
    [SerializeField] GameObject _inner;

    [SerializeField] GameObject _gamePanels;

    public bool isSettingsOpen;

    public void ActivateSelectedPanel(string selectedPanel)
    {
        
       _inputPanel.SetActive(false);
       _soundPanel.SetActive(false);
       _exitPanel.SetActive(false);

       if(selectedPanel=="inputPanel") _inputPanel.SetActive(true);
       else if(selectedPanel=="soundPanel") _soundPanel.SetActive(true);
       else _exitPanel.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyUp(GameManager.Instance.GetPcKeyBindData().GetSettingsPanelKey()) && !InventoryManager.Instance.IsInventoryOpen)
        {
            OpenSettings();
        }
    }



    public void OpenSettings()
    {
      
        
        if (!isSettingsOpen)
        {
            isSettingsOpen=true;
           _gamePanels.SetActive(false);
            _inner.SetActive(true);
            UIManager.Instance.HideCrosshair();
            UIManager.Instance.HideInteractionText();
        }
        else
        {
            isSettingsOpen=false;
             _gamePanels.SetActive(true);
            _inner.SetActive(false);
            UIManager.Instance.ShowCrosshair();
        }
        GameManager.Instance.CheckCursorLockState();
    }

    public void NoButton_OnClick()
    {
        _inner.SetActive(false);
        isSettingsOpen = false;
        _gamePanels.SetActive(true);
    }

}
