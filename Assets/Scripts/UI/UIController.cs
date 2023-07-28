using Core;
using UnityEngine;

namespace UI
{
    public class UIController: MonoBehaviour
    {
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject timerMenu;
        [SerializeField] private GameObject gamePlayMenu;
        [SerializeField] private GameObject winMenu;
        [SerializeField] private GameObject loseMenu;
        
        public void PlayClick()
        {
            DeactiveAllWindows();
            GameController.Instance.OnGameStarted?.Invoke();
            
            timerMenu.SetActive(true);
            LevelController.Instance.OnLevelStarted += ActivateGameplayMenu;
            LevelController.Instance.OnLevelEnded += ActivateEndGameMenus;
        }

        private void ActivateEndGameMenus(bool isWin)
        {
            DeactiveAllWindows();
            
            winMenu.SetActive(isWin);
            loseMenu.SetActive(!isWin);
        }

        private void ActivateGameplayMenu()
        {
            DeactiveAllWindows();
            gamePlayMenu.SetActive(true);
        }

        private void DeactiveAllWindows()
        {
            mainMenu.SetActive(false);
            timerMenu.SetActive(false);
            gamePlayMenu.SetActive(false);
            loseMenu.SetActive(false);
            winMenu.SetActive(false);
        }
    }
}