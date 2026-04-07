using System;
using LS.Events;
using LS.Gameplay;
using TMPro;
using UnityEngine;

namespace LS.UI.View
{
    public class DistanceView : UIViewBase
    {
        [SerializeField] private TMP_Text _distanceText;

        private GameplaySession _session;

        private void Awake()
        {
            GameEvents.OnPullEnded += Show;
            GameEvents.OnSessionStarted += OnSessionStarted;
            GameEvents.OnPullUpdated += OnPullUpdated;                                                                                                                     
        }

        private void OnDestroy()
        {
            GameEvents.OnPullEnded -= Show;
            GameEvents.OnSessionStarted -= OnSessionStarted;
            GameEvents.OnPullUpdated -= OnPullUpdated;                                                                                                                     
        }

        private void OnPullUpdated(float value)
        {
            if (_session == null || !_session.IsActive) return;
            _distanceText.SetText("{0}m", (int)_session.TravelledDistance);
        }

        private void OnSessionStarted(GameplaySession session)
        {
            _session = session;
        }
    }
}
