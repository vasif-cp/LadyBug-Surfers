using System;
using LS.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LS.UI.View
{
    public class PullIndicatorView : UIViewBase
    {
        [Header("Scene Dependencies")]
        [SerializeField] private TMP_Text _percentText;
        [SerializeField] private Image _fillBar;

        private void Awake()
        {
            GameEvents.OnPullStarted += Show;
            GameEvents.OnPullEnded += Hide;
            GameEvents.OnPullUpdated += UpdatePullPercent;
        }


        private void OnDestroy()
        {
            GameEvents.OnPullStarted -= Show;
            GameEvents.OnPullEnded -= Hide;
            GameEvents.OnPullUpdated -= UpdatePullPercent;
        }


        private void UpdatePullPercent(float normalizedValue)
        {
            float pullPercentage = normalizedValue * 100;
            _percentText.SetText($"{pullPercentage:F0}%");
 
            if (_fillBar != null)
            {
                _fillBar.fillAmount = normalizedValue;
            }
        }
    }
}
