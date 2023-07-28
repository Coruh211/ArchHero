using Core.Tools;
using Enums;
using TPSShooter;
using UI;
using UnityEngine;

namespace Player
{
    public class PlayerMove
    {
        private readonly float _angleLerpFactor;
        private readonly float _moveSpeed;
        private readonly Transform _target;
        private readonly Joystick _joystick;
        private readonly Transform _rotateTarget;
        private readonly PlayerView _playerView;
        private readonly Ticker _ticker;

        public PlayerMove(Transform target, Transform rotateTarget, PlayerView playerView) 
        {
            _moveSpeed = playerView.CharacteristicsSo.MoveSpeed;
            _angleLerpFactor = playerView.CharacteristicsSo.AngleLerpFactor;
            _playerView = playerView;
            _target = target;
            _rotateTarget = rotateTarget;
            
            _joystick = UIHolder.Instance.Joystick;
            _ticker = Ticker.Instance;
            
            _ticker.UpdateTick += Tick;
            playerView.OnObjectDestroy += OnDestroy;
        }
        
        private void Tick()
        {
            if (!_joystick.IsTouched)
            {
                _playerView.CurrentState = ViewState.Idle;
                return;
            }
            
            _playerView.CurrentState = ViewState.Move;
            Vector3 direction = Vector3.forward * _joystick.Vertical + Vector3.right * _joystick.Horizontal;
            Vector3 newPosition = _target.position + direction.normalized * (_moveSpeed * Time.deltaTime);

            float angle = Mathf.LerpAngle(_rotateTarget.localEulerAngles.y,
                Mathf.Atan2(_joystick.Horizontal, _joystick.Vertical) * Mathf.Rad2Deg, Time.deltaTime * _angleLerpFactor * direction.sqrMagnitude);
            
            _rotateTarget.localEulerAngles = new Vector3(0f, angle, 0f);

            _target.position = newPosition;
        }
        
        private void OnDestroy()
        {
            if (_ticker != null)
            {
                _ticker.UpdateTick -= Tick;
            }
        }

    }
}