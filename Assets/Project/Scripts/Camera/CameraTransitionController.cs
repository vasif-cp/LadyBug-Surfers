using System;
using DG.Tweening;
using LS.Events;
using Unity.Cinemachine;
using UnityEngine;

namespace LS.Camera
{
    public class CameraTransitionController : MonoBehaviour
    {                                                                                                                                             
        [Header("Camera State Settings")]
        [SerializeField] private Vector3 _menuTargetOffset;                                                                                                                 
        [SerializeField] private float _menuCameraDistance;
        
        [SerializeField] private Vector3 _gameplayTargetOffset;                                                                                                             
        [SerializeField] private float _gameplayCameraDistance;
        
        [SerializeField] private float _transitionDuration = 1f;
        
        private CinemachinePositionComposer _positionComposer;
        private Sequence _transitionSequence;                                                                                                                               

        private void Awake()
        {
            _positionComposer = GetComponent<CinemachinePositionComposer>();

            _positionComposer.TargetOffset = _menuTargetOffset;
            _positionComposer.CameraDistance = _menuCameraDistance;

            GameEvents.OnGameStartRequested += OnGameStartRequested;
        }

        private void OnDestroy()
        {
            _transitionSequence?.Kill();
            GameEvents.OnGameStartRequested -= OnGameStartRequested;
        }

        private void OnGameStartRequested()
        {
            _transitionSequence = DOTween.Sequence()
                .Join(DOTween.To(
                    () => _positionComposer.TargetOffset,                                                                                                                   
                    x  => _positionComposer.TargetOffset = x,
                    _gameplayTargetOffset,                                                                                                                                  
                    _transitionDuration).SetEase(Ease.Linear))
                .Join(DOTween.To(                                                                                                                                           
                    () => _positionComposer.CameraDistance,
                    x  => _positionComposer.CameraDistance = x,                                                                                                             
                    _gameplayCameraDistance,
                    _transitionDuration).SetEase(Ease.Linear))
                .OnComplete(() => GameEvents.OnCameraTransitionComplete?.Invoke());
        }
    }
}
