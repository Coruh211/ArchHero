using Core;
using Core.Tools;
using Enums;
using Player;
using UniRx;
using UnityEngine;

namespace Bots
{
    public class BotAttack
    {
        private readonly Transform _bulletSpawnPoint;
        private readonly Transform _target;
        private readonly Ticker _ticker;
        private readonly BotView view;
        
        private bool _canAttack = true;
        
        public BotAttack(BotView view, Transform bulletSpawnPoint)
        {
            this.view = view;
            _bulletSpawnPoint = bulletSpawnPoint;
            _target = GameObject.FindGameObjectWithTag("Player").transform;
            _ticker = Ticker.Instance;
            
            _ticker.UpdateTick += Tick;
            view.OnObjectDestroy += OnDestroy;
        }

        private void Tick()
        {
            if (view.CurrentState == ViewState.Idle)
            {
                Attack();
            }
        }

        private void Attack()
        {
            if (_target == null || view == null)
            {
                return;
            }
            
            if (!_canAttack)
            {
                return;
            }

            _canAttack = false;

            var activeBullet = SimplePool.GiveObj(1);
            activeBullet.transform.position = _bulletSpawnPoint.position;
            activeBullet.SetActive(true);
            activeBullet.GetComponent<BulletController>()
                .Initialize(_target.position, view.CharacteristicsSo.Damage);

            StartDelay();
        }

        private void StartDelay()
        {
            Observable.Timer(view.CharacteristicsSo.ShootInterval.sec()).TakeUntilDisable(view)
                .Subscribe(
                    x => { _canAttack = true; });
        }

        private void OnDestroy()
        {
            _ticker.UpdateTick -= Tick;
        }
    }
}