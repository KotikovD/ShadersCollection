using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(Renderer))]
public sealed class Paint : MonoBehaviour
{
    
    [SerializeField] private Texture2D _textureDecal;
    [Space(15)]
    [SerializeField] private FilterMode _filterMode;
    [Space(15)]
    [SerializeField] private Camera _camera;
    [SerializeField] private Collider _collider;
    
    private Material _material;
    private Texture _textureSource;
    private HashSet<Pixel> _decalDataCache;
    private Texture2D _textureCopy;
    private Renderer _renderer;
    private MaterialPropertyBlock _block;


    private void Awake()
    {
      _renderer = GetComponent<Renderer>();
      _material = _renderer.material;
      _textureSource = _material.mainTexture;
      _block = new MaterialPropertyBlock();
      
      _renderer.GetPropertyBlock(_block);
      
        _decalDataCache = new HashSet<Pixel>();
       
        for (int w = 0; w < _textureDecal.width; w++)
        {
            for (int h = 0; h < _textureDecal.height; h++)
            {
               var pixelColor = _textureDecal.GetPixel(w, h);
               _decalDataCache.Add(new Pixel(pixelColor, w, h));
            }
        }
        
        _textureCopy = new Texture2D(_textureSource.width, _textureSource.height);
        Graphics.CopyTexture(_textureSource, _textureCopy);
        
        _textureCopy.wrapMode = _textureSource.wrapMode;
        _textureCopy.filterMode = _filterMode;
        _material.mainTexture = _textureCopy;
        _textureCopy.Apply();
        
        _block.SetTexture("_MainTex", _textureCopy);
        _renderer.SetPropertyBlock(_block);
    }
    
    private void Update()
    {
        if(!Input.GetMouseButton(0))
            return;

        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (_collider.Raycast(ray, out var hit, 100f))
        {
            var rayX = (int) (hit.textureCoord.x * _textureCopy.width);
            var rayY = (int) (hit.textureCoord.y * _textureCopy.height);

            foreach (var pixel in _decalDataCache)
            {
                var x = pixel.Place.x + rayX - _textureDecal.width / 2;
                var y = pixel.Place.y + rayY - _textureDecal.height / 2;
                _textureCopy.SetPixel(x, y, pixel.Color);
            }

            _textureCopy.Apply();
        }
    }
    
    private void OnApplicationQuit()
    {
        _material.mainTexture = _textureSource;
    }
    
    private readonly struct Pixel
    {
        public readonly Color Color;
        public readonly Vector2Int Place;

        public Pixel(Color color, int x, int y)
        {
            Color = color;
            Place = new Vector2Int(x, y);
        }
    }
}



