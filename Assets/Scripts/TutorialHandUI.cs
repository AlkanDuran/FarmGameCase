using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialHandUI : LocalSingleton<TutorialHandUI>
    {
        [SerializeField] private RectTransform _canvasRect;
        [SerializeField] private Image _image;

        Coroutine _handCo;
        Coroutine _paintBucketCo;
        private bool _paintBucketTutorialActive = false;

        private Vector3 _firstScale;
        private void Start()
        {
            _image.enabled = false;
            _firstScale = _image.rectTransform.localScale;
        }

        public void StopTutorial()
        {
            KillHandCoroutine();
            _paintBucketTutorialActive = false;
        }

        public bool IsPaintBucketTutorialActive()
        {
            return _paintBucketTutorialActive;
        }

        void KillHandCoroutine()
        {
            if (_handCo != null)
            {
                StopCoroutine(_handCo);
            }
            if (_paintBucketCo != null)
            {
                StopCoroutine(_paintBucketCo);
            }
            _handCo = null;
            _image.rectTransform.DOKill();
            _image.DOKill();
            _image.enabled = false;
            _image.rectTransform.localScale = _firstScale;
        }

        public void ShowSwipe(Vector2 startAnchorPos, Vector2 deltaAnchorPos, float speed = 10f)
        {
            KillHandCoroutine();
            _handCo = StartCoroutine(ShowSwipeProcess(startAnchorPos, deltaAnchorPos, speed));
        }

        IEnumerator ShowSwipeProcess(Vector2 startAnchorPos, Vector2 deltaAnchorPos, float speed)
        {
            while (true)
            {
                _image.rectTransform.anchoredPosition = startAnchorPos;
                _image.enabled = true;
                yield return new WaitForSeconds(0.2f);
                yield return _image.rectTransform.DOScale(_image.rectTransform.localScale * 0.8f, 0.3f).WaitForCompletion();
                yield return _image.rectTransform.DOAnchorPos(deltaAnchorPos, speed).SetSpeedBased().SetRelative().WaitForCompletion();
                yield return _image.rectTransform.DOScale(_image.rectTransform.localScale / 0.8f, 0.3f).WaitForCompletion();
                _image.enabled = false;
                yield return new WaitForSeconds(1f);
            }
        }

        public void ShowDragDrop(Vector3 worldStart, Vector3 worldEnd, float speed = 10f)
        {
            KillHandCoroutine();
            _handCo = StartCoroutine(ShowDragDropProcess(worldStart, worldEnd, speed));
        }


        IEnumerator ShowDragDropProcess(Vector3 worldStart, Vector3 worldEnd, float speed)
        {
            Vector2 startPosUI = UIManager.Instance.GetCanvasPositionFromWorldPosition(worldStart, _canvasRect);
            Vector2 endPosUI = UIManager.Instance.GetCanvasPositionFromWorldPosition(worldEnd, _canvasRect);
            while (true)
            {
                _image.rectTransform.anchoredPosition = startPosUI;
                _image.enabled = true;
                yield return new WaitForSeconds(0.2f);
                yield return _image.rectTransform.DOScale(_image.rectTransform.localScale * 0.8f, 0.3f).WaitForCompletion();
                yield return _image.rectTransform.DOAnchorPos(endPosUI, speed).SetSpeedBased().WaitForCompletion();
                yield return _image.rectTransform.DOScale(_image.rectTransform.localScale / 0.8f, 0.3f).WaitForCompletion();
                _image.enabled = false;
                yield return new WaitForSeconds(1f);
            }
        }

        public void ShowHorizontalSwipe(Vector3 worldPos, float speed = 10f)
        {
            KillHandCoroutine();
            _handCo = StartCoroutine(ShowHorizontalSwipeProcess(worldPos, speed));
        }

        IEnumerator ShowHorizontalSwipeProcess(Vector3 worldPos, float speed)
        {
            Vector2 startPosUI = UIManager.Instance.GetCanvasPositionFromWorldPosition(worldPos, _canvasRect);
            while (true)
            {
                _image.rectTransform.anchoredPosition = startPosUI;
                _image.enabled = true;
                yield return new WaitForSeconds(0.2f);
                yield return _image.rectTransform.DOAnchorPosX(-100f, speed).SetRelative().SetSpeedBased().WaitForCompletion();
                for (int i = 0; i < 3; i++)
                {
                    yield return _image.rectTransform.DOAnchorPosX(200f, speed).SetRelative().SetSpeedBased().WaitForCompletion();
                    yield return _image.rectTransform.DOAnchorPosX(-200f, speed).SetRelative().SetSpeedBased().WaitForCompletion();
                }
                yield return new WaitForSeconds(0.2f);
                _image.enabled = false;
                yield return new WaitForSeconds(1f);
            }
        }

        public void ShowSineDraw(Vector3 worldPos, float speed = 10f)
        {
            KillHandCoroutine();
            _handCo = StartCoroutine(ShowSineDrawProcess(worldPos, speed));
        }

        IEnumerator ShowSineDrawProcess(Vector3 worldPos, float speed)
        {
            Vector2 startPosUI = UIManager.Instance.GetCanvasPositionFromWorldPosition(worldPos, _canvasRect);
            while (true)
            {
                _image.rectTransform.anchoredPosition = startPosUI;
                _image.enabled = true;
                yield return new WaitForSeconds(0.1f);
                //Vector2[] path = new Vector2[3];
                //path[0] = _image.rectTransform.anchoredPosition + new Vector2(80f, 100f);
                _image.rectTransform.DOAnchorPosY(150f, speed).SetSpeedBased().SetRelative().SetEase(Ease.OutSine);
                yield return _image.rectTransform.DOAnchorPosX(120f, speed).SetRelative().SetSpeedBased().WaitForCompletion();
                _image.rectTransform.DOAnchorPosY(-150f, speed).SetRelative().SetSpeedBased().SetEase(Ease.InSine);
                yield return _image.rectTransform.DOAnchorPosX(120f, speed).SetRelative().SetSpeedBased().WaitForCompletion();
                yield return new WaitForSeconds(0.1f);
                _image.enabled = false;
                yield return new WaitForSeconds(1f);
            }
        }

        public void ShowClick(Vector3 position, bool isWorldPos, float speed = 10f)
        {
            KillHandCoroutine();
            _handCo = StartCoroutine(ShowClickProcess(position + Vector3.down * 50, isWorldPos, speed));
        }

        IEnumerator ShowClickProcess(Vector3 position, bool isWorldPos, float speed)
        {
            Vector2 startPosUI;
            if (isWorldPos)
            {
                startPosUI = UIManager.Instance.GetCanvasPositionFromWorldPosition(position, _canvasRect);
            }
            else
            {
                startPosUI = AdjustPositionForAnchors(position, _image.rectTransform);
            }

            while (true)
            {
                _image.rectTransform.anchoredPosition = startPosUI;
                _image.enabled = true;
                yield return new WaitForSeconds(0.1f);
                yield return _image.rectTransform.DOScale(_image.rectTransform.localScale * 0.8f, speed).SetSpeedBased().WaitForCompletion();
                yield return _image.rectTransform.DOScale(_image.rectTransform.localScale / 0.8f, speed).SetSpeedBased().WaitForCompletion();
                //_image.enabled = false;
                //yield return new WaitForSeconds(1f);
            }
        }

        private Vector2 AdjustPositionForAnchors(Vector3 position, RectTransform rectTransform)
        {
            RectTransform parentRect = rectTransform.parent as RectTransform;
            if (parentRect == null) return position;

            // Convert position from world space to local space of the parent
            Vector2 localPosition = parentRect.InverseTransformPoint(position);
            
            // Adjust for anchors and pivots
            Vector2 adjustedPosition = localPosition;

            // Calculate anchor offsets for the rect transform
            Vector2 anchorMinOffset = rectTransform.anchorMin - new Vector2(0.5f, 0.5f);
            Vector2 anchorMaxOffset = rectTransform.anchorMax - new Vector2(0.5f, 0.5f);
            Vector2 pivotOffset = rectTransform.pivot - new Vector2(0.5f, 0.5f);

            // Apply anchor and pivot offsets to the position
            adjustedPosition.x += (anchorMinOffset.x + anchorMaxOffset.x + pivotOffset.x) * parentRect.rect.width;
            adjustedPosition.y += (anchorMinOffset.y + anchorMaxOffset.y + pivotOffset.y) * parentRect.rect.height;

            return adjustedPosition;
        }

    }