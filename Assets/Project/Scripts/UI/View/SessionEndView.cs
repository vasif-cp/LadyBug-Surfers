using System;
using LS.Events;
using LS.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LS.UI.View
{
    public class SessionEndView : UIViewBase
    {
        [SerializeField] private TMP_Text _collectedCoinsText;
        [SerializeField] private Button _continueButton;

        private void Awake()
        {
            GameEvents.OnSessionEnded += OnGameplaySessionEnd;
            _continueButton.onClick.AddListener(OnContinueButtonClick);
        }

        private void OnDestroy()
        {
            GameEvents.OnSessionEnded -= OnGameplaySessionEnd;
        }

        private void OnGameplaySessionEnd(GameplaySession gameplaySession)
        {
            _collectedCoinsText.SetText("{0} <sprite index=0>", gameplaySession.CollectedCoins);
            Show();
        }

        private void OnContinueButtonClick()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
