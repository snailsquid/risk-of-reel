using System.Collections.Generic;
using Manager.Input;
using UnityEngine;

namespace Mechanic.Rod.States
{
    public class RodCastState : IRodState
    {
        private InputManager _inputManager;
        private Flick _flick;
        private PullBack _pullBack;
        private IInputType _currentInput;
        private float _pullBackPower;
        private Vector2 _flickVelocity;
        private readonly Rod _rod;

        public RodCastState(Rod rod)
        {
            _rod = rod;
            InputManager.OnPointerPress += HandlePointerPress;
        }

        private void ChangeState(IInputType inputType)
        {
            _currentInput = inputType;
        }

        private void StartTracking()
        {
            _currentInput.StartTracking(_inputManager.currentPosition);
        }

        private Vector2 EndTracking()
        {
            return _currentInput.EndTracking(_inputManager.currentPosition);
        }
        public void Enter()
        {
            _flick = new Flick();
            _pullBack = new PullBack();
            _inputManager = InputManager.Instance;
            _currentInput = _pullBack;
        }

        public void UpdateState()
        {
            if (_currentInput == _pullBack && _inputManager.currentPosition.y > _inputManager.previousPosition.y)
            {
                _pullBackPower = _pullBack.EndTracking(_inputManager.currentPosition, ()=>
                {
                    ChangeState(_flick);
                    StartTracking();
                }).magnitude;
            }
        }

        private void Exit()
        {
            // ChangeState(_pullBack);
            _rod.ChangeState(_rod.ReelState);
        }
        
        private void HandlePointerPress(bool value)
        {
            if(value && _currentInput == _pullBack)
                StartTracking();
            else if (!value)
            {
                _pullBack.EndTracking(_inputManager.currentPosition);
                _flickVelocity = EndTracking();
                Exit();
            }
        }
        
    }
}