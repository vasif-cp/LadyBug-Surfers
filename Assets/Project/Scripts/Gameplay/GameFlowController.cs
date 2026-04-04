using System;
using UnityEngine;

namespace LS.Gameplay
{
    public class GameFlowController : MonoBehaviour
    {
        [SerializeField] private Transform _characterTransform;
        
        private GameplaySession _gameplaySession;
        
        public GameplaySession GameplaySession => _gameplaySession;
        
        private void Start()
        {
            _gameplaySession = new GameplaySession(_characterTransform);
            _gameplaySession.OnStart();
        }

        private void Update()
        {
            _gameplaySession.OnUpdate();
        }
    }
}
