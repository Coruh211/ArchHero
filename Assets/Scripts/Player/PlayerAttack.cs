using Core;
using Core.Tools;
using Enums;
using UniRx;
using UnityEngine;

namespace Player
{
    public class PlayerAttack
    {
        private PlayerView _playerView;
        private Transform _bulletSpawnPoint;
        private LevelController _levelController;
        private Vector3 _currentTargetPos;
        
        private bool _canAttack = true;
        private Ticker _ticker;

        public PlayerAttack(PlayerView playerView, Transform bulletSpawnPoint)
        {
            _playerView = playerView;
            _bulletSpawnPoint = bulletSpawnPoint;
            
            _levelController = LevelController.Instance;
            _ticker = Ticker.Instance;
            
            _ticker.UpdateTick += Tick;
            playerView.OnObjectDestroy += OnDestroy;
        }
        
        private void Tick()
        {
            if (_playerView.CurrentState == ViewState.Idle)
            {
                _currentTargetPos = _levelController.GetPlayerTarget(_playerView.transform.position);
                
                if (_currentTargetPos == Vector3.zero)
                {
                    return;
                }
                
                _playerView.model.LookAt(_currentTargetPos);
                
                Attack();
            }
        }

        private void Attack()
        {
            if (!_canAttack)
            {
                return;
            }

            _canAttack = false;
            
            var activeBullet = SimplePool.GiveObj(0);
            activeBullet.transform.position = _bulletSpawnPoint.position;
            activeBullet.SetActive(true);
            activeBullet.GetComponent<BulletController>().Initialize(_currentTargetPos, _playerView.CharacteristicsSo.Damage);
            
            StartDelay();
        }

        private void StartDelay()
        {
            Observable.Timer(_playerView.CharacteristicsSo.ShootInterval.sec()).TakeUntilDisable(_playerView)
                .Subscribe(
                    x =>
                    {
                        _canAttack = true;
                    });
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