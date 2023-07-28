using Core;
using Core.Tools;
using DG.Tweening;
using Enums;
using ScriptableObjects;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace Bots
{
    public class BotMove
    {
        private readonly NavMeshAgent _botAgent;
        private readonly Transform _target;
        private readonly BotView _botView;
        private readonly Ticker _ticker;
        
        private float _moveDistance;
        private float _timeOfImmobility;
        private float _moveSpeed;
        private float _startDistance;
        
        public BotMove(NavMeshAgent meshAgent, Transform target, BotView botView)
        {
            _botAgent = meshAgent;
            _target = target;
            _botView = botView;
            _ticker = Ticker.Instance;
            
            SetInfo(botView.CharacteristicsSo);
            
            _ticker.UpdateTick += Tick;
            botView.OnObjectDestroy += OnDestroy;
            Move();
        }
        
        private void SetInfo(CharacteristicsSo characteristicsSo)
        {
            _moveDistance = characteristicsSo.MoveDistance;
            _moveSpeed = characteristicsSo.MoveSpeed;
            _timeOfImmobility = characteristicsSo.TimeOfImmobility;
            
            _botAgent.speed = _moveSpeed;
        }

        private void Move()
        {
            if (_botView == null)
            {
                return;
            }
            
            _botView.CurrentState = ViewState.Idle;
            Observable.Timer(_timeOfImmobility.sec()).TakeUntilDisable(_botAgent).Subscribe(x =>
            {
                if (_target == null)
                {
                    return;
                }
                
                _startDistance = Vector3.Distance(_target.position, _botView.transform.position);
                _botAgent.SetDestination(_target.position);
                _botView.CurrentState = ViewState.Move;
            });
        }
        
        private void Tick()
        {
            if (_botView.CurrentState != ViewState.Move || _target == null)
            {
                return;
            }

            float distanceTraveled = _startDistance - Vector3.Distance(_target.position, _botView.transform.position);

            if (!_botAgent.hasPath || distanceTraveled >= _moveDistance)
            {
                StopMove();
            }
        }

        private void StopMove()
        {
            if (_botView == null)
            {
                return;
            }
            
            _botView.CurrentState = ViewState.Idle;
            _botAgent.ResetPath();
             
            Move();
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