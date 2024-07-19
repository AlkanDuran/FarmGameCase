using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialManager : LocalSingleton<TutorialManager>
{
    [SerializeField] private GameObject _bed;
    [SerializeField] private GameObject _house;
    public bool IsPlayerSlept { get; set; } = false;
    public bool IsTutorialActive { get; set; } = false;


    public void StartTutorial()
    {
        if (PlayerPrefs.GetInt("IS_TUTORIAL_PLAYED", 0) == 0)
        {
            StartCoroutine(StartTutorialProcess());
        }
    }
    
    private IEnumerator StartTutorialProcess()
    {
        IsTutorialActive = true;
        
        var tutorialHandUI = TutorialHandUI.Instance;
        var handSprite = UIManager.Instance.GetTutorialHandSpriteTransform();
        
        var inventoryImageTransform = UIManager.Instance.GetInventoryImageTransform().GetComponent<RectTransform>();
        tutorialHandUI.ShowClick(inventoryImageTransform.position, false, 1f);
        CharacterControllerScript.Instance.SetCanMove(false);
        
        // inventory açılana kadar bekle
        yield return new WaitUntil(() => InventoryManager.Instance.IsInventoryOpen);
        tutorialHandUI.StopTutorial();
        
        tutorialHandUI.ShowClick(UIManager.Instance.GetInventorySlots()[2].GetComponent<RectTransform>().position, false, 1f);
        
        // hoe ele alınana kadar bekle
        
        yield return new WaitUntil(() =>
        {
            var equippedItem = InventoryManager.Instance.GetEquippedSlotItem() as EquipmentData;
            return equippedItem != null && equippedItem.toolType is EquipmentData.ToolType.Hoe;
        });
        tutorialHandUI.StopTutorial();
        
        tutorialHandUI.ShowClick(inventoryImageTransform.position, false, 1f);
        // inventory kapanana kadar bekle
        yield return new WaitUntil(() => !InventoryManager.Instance.IsInventoryOpen);
        tutorialHandUI.StopTutorial();
        CharacterControllerScript.Instance.SetCanMove(true);

        var firstLand = LandManager.Instance.landPlots[0];
        handSprite.gameObject.SetActive(true);
        handSprite.position = new Vector3(firstLand.transform.position.x, 1.5f, firstLand.transform.position.z);
        var handSpriteTween = UIManager.Instance.GetTutorialHandSpriteTransform().DOMoveY(2f, 1f).SetSpeedBased().SetLoops(-1,LoopType.Yoyo);
        // tarla sürülene kadar bekle
        yield return new WaitUntil(() => firstLand.GetLandStatus() is Land.LandStatus.Plowed);
        tutorialHandUI.StopTutorial();
        handSpriteTween.Kill(true);
        handSprite.gameObject.SetActive(false);
        
        CharacterControllerScript.Instance.SetCanMove(true);
        tutorialHandUI.ShowClick(inventoryImageTransform.position, false, 1f);
        // inventory açılana kadar bekle
        yield return new WaitUntil(() => InventoryManager.Instance.IsInventoryOpen);
        tutorialHandUI.StopTutorial();
        tutorialHandUI.ShowClick(UIManager.Instance.GetInventorySlots()[1].GetComponent<RectTransform>().position, false, 1f);
        
        yield return new WaitUntil(() =>
        {
            var equippedItem = InventoryManager.Instance.GetEquippedSlotItem();
            return equippedItem != null && equippedItem.itemType == ItemType.Sellable;
        });
        tutorialHandUI.StopTutorial();
        tutorialHandUI.ShowClick(inventoryImageTransform.position, false, 1f);
        // inventory kapanana kadar bekle
        yield return new WaitUntil(() => !InventoryManager.Instance.IsInventoryOpen);
        tutorialHandUI.StopTutorial();
        
        handSprite.gameObject.SetActive(true);
        handSprite.position = new Vector3(firstLand.transform.position.x, 1.5f, firstLand.transform.position.z);
        handSpriteTween = UIManager.Instance.GetTutorialHandSpriteTransform().DOMoveY(2f, 1f).SetSpeedBased().SetLoops(-1,LoopType.Yoyo);
        
        // seed ekilene kadar bekle
        yield return new WaitUntil(() => firstLand.GetComponentInChildren<CropBehaviour>());
        handSpriteTween.Kill(true);
        handSprite.gameObject.SetActive(false);
        
        tutorialHandUI.ShowClick(inventoryImageTransform.position, false, 1f);
        // inventory açılana kadar bekle
        yield return new WaitUntil(() => InventoryManager.Instance.IsInventoryOpen);
        tutorialHandUI.StopTutorial();
        tutorialHandUI.ShowClick(UIManager.Instance.GetInventorySlots()[3].GetComponent<RectTransform>().position, false, 1f);
        
        // watering can ele alınana kadar bekle
        yield return new WaitUntil(() =>
        {
            var equippedItem = InventoryManager.Instance.GetEquippedSlotItem() as EquipmentData;
            return equippedItem != null && equippedItem.toolType is EquipmentData.ToolType.WateringCan;
        });
        tutorialHandUI.StopTutorial();
        
        tutorialHandUI.ShowClick(inventoryImageTransform.position, false, 1f);
        // inventory kapanana kadar bekle
        yield return new WaitUntil(() => !InventoryManager.Instance.IsInventoryOpen);
        tutorialHandUI.StopTutorial();
        
        handSprite.gameObject.SetActive(true);
        handSprite.position = new Vector3(firstLand.transform.position.x, 1.5f, firstLand.transform.position.z);
        handSpriteTween = UIManager.Instance.GetTutorialHandSpriteTransform().DOMoveY(2f, 1f).SetSpeedBased().SetLoops(-1,LoopType.Yoyo);
        // tarla sulanana kadar bekle
        yield return new WaitUntil(() => firstLand.GetLandStatus() is Land.LandStatus.Wet);
        tutorialHandUI.StopTutorial();
        handSpriteTween.Kill(true);
        handSprite.gameObject.SetActive(false);
        
        handSprite.gameObject.SetActive(true);
        var handSpriteForHouse = Instantiate(handSprite);
        handSpriteForHouse.AddComponent<Billboard>();
        handSpriteForHouse.position = new Vector3(4.5f, 1.75f, 17.5f);
        handSprite.position = _bed.transform.position + Vector3.up * 2f;
        var handSpriteForHouseTween = handSpriteForHouse.DOMoveY(2.5f, 1f).SetSpeedBased().SetLoops(-1,LoopType.Yoyo);
        handSpriteTween = UIManager.Instance.GetTutorialHandSpriteTransform().DOMoveY(2.5f, 1f).SetSpeedBased().SetLoops(-1,LoopType.Yoyo);
        // karakter uyuyana kadar bekle
        yield return new WaitUntil(() => IsPlayerSlept);
        tutorialHandUI.StopTutorial();
        handSpriteTween.Kill(true);
        handSpriteForHouseTween.Kill(true);
        handSpriteForHouse.gameObject.SetActive(false);
        handSprite.gameObject.SetActive(false);
        CharacterControllerScript.Instance.SetCanMove(true);

        IsTutorialActive = false;
        PlayerPrefs.SetInt("IS_TUTORIAL_PLAYED", 1);

    }
}
