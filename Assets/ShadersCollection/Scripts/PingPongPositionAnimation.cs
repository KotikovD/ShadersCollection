using System;
using DG.Tweening;
using UnityEngine;


public sealed class PingPongPositionAnimation : MonoBehaviour
{
    [SerializeField] private Vector3 _endPosition;
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private float _time = 1.5f;
    [SerializeField] private Ease _ease = Ease.InOutCubic;
  
    private Tweener _tween;
    
    private void OnValidate()
    {
        transform.localPosition = _startPosition;
    }

    private void Start()
    {
        _tween = DOTween.To(() => _startPosition, pos => transform.localPosition = pos, _endPosition, _time)
            .SetEase(_ease)
            .SetLoops(-1, LoopType.Yoyo);

        _tween.Play();
    }

    private void OnDisable()
    {
        _tween?.Kill();
    }
}