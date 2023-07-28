using Core;
using Core.Tools;
using TMPro;
using UnityEngine;

namespace UI
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUgui;
        
        private LevelController _levelController;
        private Ticker _ticker;

        private void OnEnable()
        {
            _levelController = LevelController.Instance;
            _ticker = Ticker.Instance;
            
            _ticker.UpdateTick += Tick;
        }

        private void Tick()
        {
            _textMeshProUgui.text = _levelController.ActualTime.ToString();
        }

        private void OnDisable()
        {
            if (_ticker != null)
            {
                _ticker.UpdateTick -= Tick;
            }
        }
    }
}