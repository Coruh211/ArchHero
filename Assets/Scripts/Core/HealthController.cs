using System;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Bots
{
    public class HealthController
    {
        public Action OnDie;
        private float _currentHp;
        private GameObject _gameObject;
        private Image _hpBar;
        private float _maxHp;

        public HealthController(float currentHp, GameObject gameObject, Image hpBar)
        {
            _currentHp = currentHp;
            _maxHp = currentHp;
            _gameObject = gameObject;
            _hpBar = hpBar;
        }

        public void TakeDamage(float damage)
        {
            _currentHp -= damage;
            _hpBar.fillAmount = _currentHp / _maxHp;
            
            if (_currentHp <= 0)
            {
                OnDie?.Invoke();
                Object.Destroy(_gameObject);
            }
        }
    }
}