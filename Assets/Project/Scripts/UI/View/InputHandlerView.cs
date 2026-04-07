using System;
using LS.Events;
using LS.Gameplay;
using UnityEngine;

namespace LS.UI.View
{
    public class InputHandlerView : UIViewBase
    {
        private void Awake()
        {
            GameEvents.OnCameraTransitionComplete += Show;
            GameEvents.OnSessionEnded += OnSessionEnded;
        }

        private void OnDestroy()
        {
            GameEvents.OnCameraTransitionComplete -= Show;
            GameEvents.OnSessionEnded -= OnSessionEnded;
        }

        private void OnSessionEnded(GameplaySession gameplaySession)
        {
            Hide();
        }
        
    }
}
