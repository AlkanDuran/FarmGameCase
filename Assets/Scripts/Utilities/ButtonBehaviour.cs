using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class ButtonBehaviour : MonoBehaviour,IPointerUpHandler,IPointerDownHandler{

    public bool hideUnlessLaunch;
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
    }

    public void HandleOnButtonClicked()
    {
        if(!_button.interactable) return;
       
        transform.DOKill();
        transform.DOScale(Vector3.one * .95f, .07f);
    }
    
    
    public void HandleOnButtonDeselect()
    {
        if(!_button.interactable) return;
        
        transform.DOKill();
        transform.DOScale(Vector3.one, .07f);
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        HandleOnButtonClicked();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        HandleOnButtonDeselect();
    }
}
