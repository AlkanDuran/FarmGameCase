using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyPanel : MonoBehaviour
{
    [SerializeField] private SmoothNumberText _currencyText;
    [SerializeField] private Image _currencyIconImage;
    [SerializeField] private float _punchIconRate = 0.05f;
    [SerializeField] private float _punchTextRate = 0.1f;
        
    private RectTransform _rectTransform;
    private void Start()
    {
        _rectTransform = (RectTransform)transform;
    }

    public void SetCurrencyAmount(float amount, bool instantly = false)
    {
        if(instantly)
        {
            _currencyText.SetPointsInstantly(amount);
        }
        else
        {
            _currencyText.SetPoints(amount);
            PunchIcon();
            PunchText();
        }
    }

    void PunchIcon()
    {
        _currencyIconImage.rectTransform.DOKill(true);
        _currencyIconImage.rectTransform.DOPunchScale(Vector3.one * _punchIconRate, 0.3f, 6);
    }

    void PunchText()
    {
        _currencyText.transform.DOKill(true);
        _currencyText.transform.DOPunchScale(Vector3.one * _punchTextRate, 0.6f, 6);
    }
    
    
}