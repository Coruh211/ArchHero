using System;
using Core;
using Enums;
using Player;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Bots
{
    public class BotView: MonoBehaviour
    {
        public Action OnObjectDestroy;
        public CharacteristicsSo CharacteristicsSo;
        public ViewState CurrentState;
        public Transform model;
        
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private Transform bulletSpawnPoint;
        [SerializeField] private Image hpBar;
        
        private BotMove _botMove;
        private HealthController _healthController;
        private BotAttack _attackLogic;

        private void Awake()
        {
            LevelController.Instance.OnLevelStarted += Initialize;
        }

        private void Initialize()
        {
            _botMove = new BotMove(navMeshAgent, FindObjectOfType<PlayerView>().transform,  this);
            _healthController = new HealthController(CharacteristicsSo.Hp, gameObject, hpBar);
            _attackLogic = new BotAttack(this, bulletSpawnPoint);
            
            _healthController.OnDie += () => LevelController.Instance.RemoveBot(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PlayerBullet"))
            {
                _healthController.TakeDamage(other.GetComponent<BulletController>().Damage);
                SimplePool.Takeobj(other.gameObject);
            }
        }

        private void OnDestroy()
        {
            OnObjectDestroy?.Invoke();
        }
    }
}