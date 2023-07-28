using System;
using System.Collections.Generic;
using Bots;
using Core.Tools;
using Player;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Core
{
    public class LevelController: Singleton<LevelController>
    {
        public Action OnLevelStarted;
        public Action OnKillAllBots;
        public Action<bool> OnLevelEnded;
        public float ActualTime { get; private set; }

        [SerializeField] private float startTimer;
        [Header("Player")]
        [SerializeField] private Transform playerSpawnPoint;
        [SerializeField] private GameObject playerPrefab;
        [Header("Bots")]
        [SerializeField] private int spawnBotsCount;
        [SerializeField] private Transform[] botsSpawnPoints;
        [SerializeField] private GameObject[] botsPrefabs;

        private List<BotView> _botViews = new List<BotView>();
        private int _spawnPos;
        private IDisposable _dispose;
        
        private void Start()
        {
            GameController.Instance.OnGameStarted += Initialize;
        }
        
        private void Initialize()
        {
            SpawnPlayer();
            SpawnBots();
            
            StartTimer();
        }

        private void StartTimer()
        {
            ActualTime = startTimer;
            
            _dispose = Observable.Interval(1f.sec()).TakeUntilDisable(gameObject).Subscribe(x =>
            {
                ActualTime--;
                if (ActualTime == 0)
                {
                    _dispose?.Dispose();
                    OnLevelStarted?.Invoke();
                }
            });
        }

        private void SpawnPlayer()
        {
            var player = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity, gameObject.transform);
            player.GetComponent<PlayerView>().OnObjectDestroy += () => EndLevel(false);
        }
        
        private void SpawnBots()
        {
            for (int i = 0; i < spawnBotsCount; i++)
            {
                int botType = Random.Range(0, botsPrefabs.Length);
                GameObject bot = Instantiate(botsPrefabs[botType], botsSpawnPoints[_spawnPos].position, botsSpawnPoints[_spawnPos].rotation, gameObject.transform);
                _botViews.Add(bot.GetComponent<BotView>());
                _spawnPos++;
            }
        }

        public Vector3 GetPlayerTarget(Vector3 pos)
        {
            if (_botViews.Count == 0)
            {
                return Vector3.zero;
            }
            
            float minDistance = 1000000;
            int currentBotNumber = 0;
            
            for (int i = 0; i < _botViews.Count; i++)
            {
                var distance = Vector3.Distance(pos, _botViews[i].transform.position);
                
                if (distance < minDistance)
                {
                    minDistance = distance;
                    currentBotNumber = i;
                }
            }

            return _botViews[currentBotNumber].transform.position;
        }

        public void RemoveBot(BotView botView)
        {
            _botViews.Remove(botView);

            CheckLevelEnd();
        }

        private void CheckLevelEnd()
        {
            if (_botViews.Count <= 0)
            {
                OnKillAllBots?.Invoke();
            }
        }

        public void EndLevel(bool isWin)
        {
            OnLevelEnded?.Invoke(isWin);
        }
    }
}