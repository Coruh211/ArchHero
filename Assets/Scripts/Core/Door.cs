using UnityEngine;

namespace Core
{
    public class Door : MonoBehaviour
    {
        private void Start()
        {
            LevelController.Instance.OnKillAllBots += DeactiveDoors;
        }

        private void DeactiveDoors()
        {
            gameObject.SetActive(false);
        }
    }
}