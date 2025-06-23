using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Manager.Input;
using UnityEngine;

namespace Mechanic.Rod.States
{
    public class RodCastState : IRodState
    {
        private InputManager _inputManager;
        private readonly Flick _flick;
        private readonly PullBack _pullBack;
        private IInputType _currentInput;
        private float _pullBackPower;
        private Vector2 _flickVelocity;
        private readonly Rod _rod;
        private bool _isPointerDown = false;
        private Coroutine _runningCoroutine;
        private bool _isCasting = false;

        public RodCastState(Rod rod)
        {
            _flick = new Flick();
            _pullBack = new PullBack();
            _rod = rod;
        }

        private void ChangeState(IInputType inputType)
        {
            _currentInput = inputType;
        }

        private void StartTracking()
        {
            _currentInput.StartTracking(_inputManager.currentPosition);
        }

        public void Enter()
        {
            _inputManager = InputManager.Instance;
            ChangeState(_pullBack);
            InputManager.OnPointerPress += HandlePointerPress;
        }

        public void UpdateState()
        {
            if (_isPointerDown && _currentInput == _pullBack && _inputManager.currentPosition.y > _inputManager.previousPosition.y)
            {
                TransitionToFlick();
            }
        }

        private void TransitionToFlick()
        {
            if (!_pullBack.IsPulling) return;
            var canEnd = _pullBack.TryEndTracking(_inputManager.currentPosition, out Vector2 temp);
            if (!canEnd) return;
            _pullBackPower = temp.magnitude;
            ChangeState(_flick);
            StartTracking();
        }

        private void Cast()
        {
            if (_isCasting) return;
            _isCasting = true;
            _runningCoroutine = _rod.StartCoroutine(CastCoroutine());
        }
        private IEnumerator CastCoroutine()
        {
            // Do something
            Debug.Log($"Casting rod with power {_pullBackPower} and velocity {_flickVelocity}");
            yield return _rod.StartCoroutine(_rod.attachedBobber.ThrowCoroutine());
            _rod.ChangeState(_rod.WaitState);
            
            _runningCoroutine = null;
            _isCasting = false;
        }
        public void Exit()
        {
            InputManager.OnPointerPress -= HandlePointerPress;
            if (_runningCoroutine != null)
            {
                _rod.StopCoroutine(_runningCoroutine);
                _runningCoroutine = null;
            }
        }
        
        private void HandlePointerPress(bool isPressed)
        {
            if (isPressed)
            {
                _isPointerDown = true;
                StartTracking();
            }
            else
            {
                if (!_isPointerDown) return;
                _isPointerDown = false;

                if (_currentInput == _pullBack)
                {
                    var _ = _currentInput.TryEndTracking(_inputManager.currentPosition, out var _);
                    Fail();
                }
                else if (_currentInput == _flick)
                {
                    var isEnd = _currentInput.TryEndTracking(_inputManager.currentPosition, out var temp);
                    if (!isEnd) return;
                    _flickVelocity = temp;
                    Cast();
                }
            }
        }

        private void Fail()
        {
            Exit();
            Enter();
        }
    }
}