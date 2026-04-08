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

        private void Awake()
        {
            GameEvents.OnPullEnded += Show;
            GameEvents.OnCharacterDistanceUpdated += OnCharacterDistanceUpdated;                                                                                                                     
        }

        private void OnDestroy()
        {
            GameEvents.OnPullEnded -= Show;
            GameEvents.OnCharacterDistanceUpdated -= OnCharacterDistanceUpdated;                                                                                                                     
        }

        private void OnCharacterDistanceUpdated(float distance)
        {
            _distanceText.SetText($"{distance:F0}m");
        }
    }
}
