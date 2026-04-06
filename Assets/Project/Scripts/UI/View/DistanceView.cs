using System;
using LS.Events;
using LS.Gameplay;
using TMPro;
using UnityEngine;

namespace LS.UI.View
{
    public class DistanceView : UIViewBase
    {
        [SerializeField] private GameFlowController _gameFlowController;
        [SerializeField] private TMP_Text _distanceText;

        private void Awake()
        {
            GameEvents.OnPullEnded += Show;
        }

        private void OnDestroy()
        {
            GameEvents.OnPullEnded -= Show;
        }

        private void LateUpdate()
        {
            if (!_isActive || !_gameFlowController.GameplaySession.IsActive) return;
            _distanceText.SetText("{0}m", (int)_gameFlowController.GameplaySession.TravelledDistance);
        }
    }
}
