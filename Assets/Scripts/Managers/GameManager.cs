using System;
using UnityEngine;

public class GameManager : LocalSingleton<GameManager>
{
    [SerializeField] private KeybindData _pcKeybindData;
    

    private void Start()
    {
        InventoryManager.Instance.Init();
        UIManager.Instance.Init();
        CurrencyManager.Instance.Init();
        ShopManager.Instance.Init();
        UpgradeManager.Instance.Init();
        TutorialManager.Instance.StartTutorial();
    }

    public KeybindData GetPcKeyBindData() => _pcKeybindData;

    public void CheckCursorLockState()
    {
        var characterController = CharacterControllerScript.Instance;
        if (ShopManager.Instance.IsShopOpen || InventoryManager.Instance.IsInventoryOpen || YesNoPrompt.Instance.IsYesNoPromptOpen || SettingsPanel.Instance.isSettingsOpen)
        {
            characterController.SetCanMove(false);
        }
        else
        {
            characterController.SetCanMove(true);
        }
        Cursor.lockState = characterController.GetCanMove() ? CursorLockMode.Locked : CursorLockMode.None;
    }

    public void Sleep()
    {
        GameTimestamp timeStampOfNextDay = TimeManager.Instance.GetGameTimestamp();
        timeStampOfNextDay.day +=1;
        timeStampOfNextDay.hour = 6;
        timeStampOfNextDay.minute = 0;

        TimeManager.Instance.SkipTime(timeStampOfNextDay);
    }
}
