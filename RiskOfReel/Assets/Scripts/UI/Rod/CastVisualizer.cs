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
        private Vector2 _mousePosition;
        public GameObject flickTextPrefab;
        public GameObject pullBackTextPrefab;
        private bool _attachedPullBack = false;
        private bool _attachedFlick = false;

        private GameObject _pullBackClone = null;
        public void EnablePullBackText()
        {
            _pullBackClone = Instantiate(pullBackTextPrefab, transform);
            _pullBackClone.transform.position = _mousePosition;
            _attachedPullBack = true;
        }

        public void DisablePullBackText()
        {
            _attachedPullBack = false;
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
            _flickClone.transform.position = _mousePosition;
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
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, InputManager.Instance.currentPosition, Camera.main, out _mousePosition);
            Debug.Log(InputManager.Instance.currentPosition);
            if (_attachedFlick)
            {
                Debug.Log("flicking");
                _flickClone.transform.position = transform.TransformPoint(_mousePosition);
            }

            if (_attachedPullBack)
            {
                Debug.Log("pulling back");
                _pullBackClone.transform.position = transform.TransformPoint(_mousePosition);
            }
        }
    }
}
