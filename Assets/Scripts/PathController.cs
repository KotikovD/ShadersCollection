using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public sealed class PathController : MonoBehaviour
{
    [SerializeField] private GameObject _roadSegmentPrefab;
    [Min(2)]
    [SerializeField] private int _segmentsCount;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private float _speed;
    
    private List<GameObject> _roads;
    private float _prefabLength;
    private Tweener _tween;
    
    private void Start()
    {
        _roads = new List<GameObject>();
        _prefabLength = _roadSegmentPrefab.GetComponent<BoxCollider>().size.z;
        
        for (var i = 0; i < _segmentsCount; i++)
        {
          var road =  Instantiate(_roadSegmentPrefab, _startPoint);
          road.name = $"{_roadSegmentPrefab.name}_{i + 1}";
          road.transform.localPosition = new Vector3(0f, 0f, _roads.Count * _prefabLength);
          _roads.Add(road);
        }

        for (var i = 0; i < _roads.Count; i++)
        {
            Animate(i);
        }
    }


    private void Animate(int roadIndex)
    {
        var road = _roads[roadIndex];
        var roadTransform = road.transform;
        var endPoint = new Vector3(0f, 0f, -_prefabLength);
        _tween = DOTween.To(() => roadTransform.localPosition, pos => roadTransform.localPosition = pos, endPoint, _speed * (roadIndex + 1))
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                _roads.RemoveAt(0);
                _roads.Insert(_roads.Count, road);
                roadTransform.localPosition = new Vector3(0f, 0f, (_roads.Count - 1) * (_prefabLength - 1));
                Animate(_roads.Count - 1);
            })
            .SetAutoKill();

        _tween.Play();
    }

}
