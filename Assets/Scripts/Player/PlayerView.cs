using System;
using System.Net.NetworkInformation;
using Bots;
using Core;
using Enums;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Player
{
    [RequireComponent (typeof(NavMeshAgent))]
    public class PlayerView : MonoBehaviour
    {
        public Action OnObjectDestroy;
        public CharacteristicsSo CharacteristicsSo;
        public ViewState CurrentState;
        public Transform model;
        
        [SerializeField] private Transform bulletSpawnPoint;
        [SerializeField] private Image hpBar;

        private PlayerMove _playerMove;
        private PlayerAttack _playerAttack;
        private HealthController _healthController;

        private void Awake()
        {
            LevelController.Instance.OnLevelStarted += Initialize;
        }

        private void Initialize()
        {
            _playerMove = new PlayerMove(gameObject.transform,model, this);
            _playerAttack = new PlayerAttack(this, bulletSpawnPoint);
            _healthController = new HealthController(CharacteristicsSo.Hp, gameObject, hpBar);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bot"))
            {
                _healthController.TakeDamage(other.GetComponent<BotView>().CharacteristicsSo.inContactDamage);
            }
            else if (other.CompareTag("BotBullet"))
            {
                _healthController.TakeDamage(other.GetComponent<BulletController>().Damage);
                SimplePool.Takeobj(other.gameObject);
            }
            else if (other.CompareTag("Finish"))
            {
                LevelController.Instance.EndLevel(true);
            }
        }

        private void OnDestroy()
        {
            OnObjectDestroy?.Invoke();
        }
    }
}