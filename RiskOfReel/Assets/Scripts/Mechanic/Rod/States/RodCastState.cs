using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Manager.Input;
using UI.Rod;
using UnityEngine;
using UnityEngine.InputSystem;

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
        private Vector2 _maxVelocity = new Vector2(500, 500);
        private CastVisualizer _castVisualizer;

        public RodCastState(Rod rod)
        {
            _flick = new Flick();
            _pullBack = new PullBack();
            _rod = rod;
        }

        private void StartTracking()
        {
            _currentInput.StartTracking(_inputManager.currentPosition);
        }

        public void Enter()
        {
            _inputManager = InputManager.Instance;
            _castVisualizer = CastVisualizer.Instance;
            _currentInput = _pullBack;
            InputManager.OnPointerPress += HandlePointerPress;
        }

        public void UpdateState()
        {
            if (_isPointerDown && _currentInput == _pullBack && _inputManager.currentPosition.y > _inputManager.previousPosition.y)
            {
                TransitionToFlick();
            }

            if (_currentInput == _pullBack)
            {
                var distance = _pullBack.GetPullBackMagnitude(_inputManager.currentPosition);
                _castVisualizer.SetPullBack(distance);
            }
        }

        private void TransitionToFlick()
        {
            if (!_pullBack.IsPulling) return;
            var canEnd = _pullBack.TryEndTracking(_inputManager.currentPosition, out Vector2 temp);
            if (!canEnd) return;
            _rod.StartCoroutine(EndPullBackCoroutine());
            _castVisualizer.EnableFlickText();
            _pullBackPower = temp.magnitude;
            _currentInput = _flick;
            StartTracking();
        }

        private void Cast()
        {
            if (_isCasting) return;
            _isCasting = true;
            _runningCoroutine = _rod.StartCoroutine(CastCoroutine());
        }

        private Vector2 CalculateNormalizedVelocity(Vector2 velocity)
        {
            var normalizedX = Mathf.Clamp(velocity.x/_maxVelocity.x, -1, 1);
            var normalizedY = Mathf.Clamp(velocity.y/_maxVelocity.y, 0, 1);
            return new Vector2(normalizedX, normalizedY);
        }

        private float CalculateNormalizedPower(float power)
        {
            
            return Mathf.Clamp(power / _rod.GetPlayer().playerData.strength, 0, 1);
        }
        private IEnumerator CastCoroutine()
        {
            Debug.Log($"Casting rod with power {_pullBackPower} and velocity {_flickVelocity}");
            var normalizedVelocity = CalculateNormalizedVelocity(_flickVelocity);
            var normalizedPower = CalculateNormalizedPower(_pullBackPower);
            yield return _rod.StartCoroutine(_rod.attachedBobber.ThrowCoroutine(normalizedVelocity, normalizedPower));
            _rod.ChangeState(_rod.CastState);
            
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
            if (isPressed && _currentInput == _pullBack)
            {
                _isPointerDown = true;
                StartTracking(); // Should only be called for pullback
                _castVisualizer.EnablePullBackText();
            }
            else
            {
                if (!_isPointerDown) return;
                _isPointerDown = false;

                if (_currentInput == _pullBack)
                {
                    var _ = _currentInput.TryEndTracking(_inputManager.currentPosition, out var _);
                    _rod.StartCoroutine(EndPullBackCoroutine());
                    Fail();
                }
                else if (_currentInput == _flick)
                {
                    var isEnd = _flick.TryEndTracking(_inputManager.currentPosition, out var temp);
                    if (isEnd)
                    {
                        _rod.StartCoroutine(EndFlickCoroutine());
                        _flickVelocity = temp;
                        var velocity = _flick.GetVelocity(_inputManager.currentPosition);
                        _castVisualizer.SetFlick(velocity);
                        Cast();
                    }
                    else
                    {
                        Fail();
                    }
                }
            }
        }

        private IEnumerator EndPullBackCoroutine()
        {
            yield return new WaitForSeconds(1);
            _castVisualizer.DisablePullBackText();
        }

        private IEnumerator EndFlickCoroutine()
        {
            yield return new WaitForSeconds(1);
            _castVisualizer.DisableFlickText();
        }

        private void Fail()
        {
            Debug.Log("Cast failed");
            Exit();
            Enter();
        }
    }
}