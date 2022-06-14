using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public sealed class PingPongMaterialAnimation : MonoBehaviour
{
    [SerializeField] private string _propertyName;
    [SerializeField] private float _startValue = 1f;
    [SerializeField] private float _endValue = 1f;
    [SerializeField] private float _time = 4f;
    [SerializeField] private Ease ease = Ease.Linear;
  
    private Tweener _tween;
    private Material _material;
    
    private void Start()
    {
        _material = GetComponent<Renderer>().material;

        var seq = DOTween.Sequence();
        _tween = DOTween.To(() => _startValue, 
                value => _material.SetFloat(_propertyName, value), 
                _endValue, 
                _time)
            .SetEase(ease)
            .SetLoops(-1, LoopType.Yoyo);

        _tween.Play();
    }

    private void OnDisable()
    {
        _tween?.Kill();
    }
}