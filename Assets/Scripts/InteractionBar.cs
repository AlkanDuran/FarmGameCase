using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InteractionBar : MonoBehaviour
{
    [SerializeField] private GameObject _interactionBar;
    [SerializeField] private Image _fillImage;
    private float _fillAmount = 1f;
    private Coroutine _fillCor;

    private void Start()
    {
        
        _fillImage.fillAmount = 0f;
        _interactionBar.SetActive(false);
    }

    public void StartFilling(float duration)
    {
        if (_fillCor != null) StopCoroutine(_fillCor);
        _fillCor = StartCoroutine(FillProcess(duration));
    }

    public void StopFilling()
    {
        if (_fillCor != null) StopCoroutine(_fillCor);
        _fillImage.fillAmount = 0f;
        _interactionBar.SetActive(false);
    }

    private IEnumerator FillProcess(float duration)
    {
        var elapsed = 0f;
        _interactionBar.SetActive(true);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _fillImage.fillAmount = elapsed / duration;
            yield return null;
        }

        _fillImage.fillAmount = 1f; 
        PlayerInteract.Instance.CompleteInteraction();
    }
}