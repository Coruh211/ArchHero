using System;
using TMPro;
using UnityEngine;

namespace Game.Core.UI
{
    public class CoinsText: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        private void Start()
        {
            text.text = "Coins: " + CoinsManager.Instance.CoinsCount;
            CoinsManager.Instance.UpdateCount += UpdateText;
        }

        private void UpdateText(float obj)
        {
            text.text = "Coins: " + CoinsManager.Instance.CoinsCount;
        }
    }
}