using System;
using Manager.Input;
using TMPro;
using UnityEngine;

namespace UI.Rod
{
    public class CastVisualizer : MonoBehaviour
    {
        #region Singleton
        public static CastVisualizer Instance;
        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this.gameObject);
            else
                Instance = this;
        }
        #endregion
        private Vector2 _mouseCanvasPosition;
        public GameObject flickTextPrefab;
        public GameObject pullBackTextPrefab;
        private bool _attachedPullBack = false;
        private bool _attachedFlick = false;

        private GameObject _pullBackClone = null;
        public void EnablePullBackText()
        {
            _pullBackClone = Instantiate(pullBackTextPrefab, transform);
            _pullBackClone.transform.position = _mouseCanvasPosition;
            _attachedPullBack = true;
        }

        public void DetachPullBackText()
        {
            _attachedPullBack = false;
        }
        public void DisablePullBackText()
        {
            DetachPullBackText();
            Destroy(_pullBackClone);
        }

        public void SetPullBack(float value)
        {
            if(_pullBackClone != null)
                _pullBackClone.GetComponent<TextMeshProUGUI>().text = value.ToString();
        }

        private GameObject _flickClone = null;
        public void EnableFlickText()
        {
            _flickClone = Instantiate(flickTextPrefab, transform);
            _flickClone.transform.position = _mouseCanvasPosition;
            _attachedFlick = true;
        }

        public void DisableFlickText()
        {
            _attachedFlick = false;
            Destroy(_flickClone);
        }

        public void SetFlick(Vector2 value)
        {
            if(_flickClone != null)
                _flickClone.GetComponent<TextMeshProUGUI>().text = value.ToString();
        }

        private void Start()
        {
            InputManager.OnPointerMove += OnPointerMove;
        }

        private void OnPointerMove()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, InputManager.Instance.currentPosition, ReferenceScript.Instance.uiCamera, out _mouseCanvasPosition);
            if (_attachedFlick)
            {
                _flickClone.transform.localPosition = _mouseCanvasPosition;
            }

            if (_attachedPullBack)
            {
                _pullBackClone.transform.localPosition = _mouseCanvasPosition;
            }
        }
    }
}
