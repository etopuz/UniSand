using System;
using UnityEngine;

namespace UniSand
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DrawableChunk : MonoBehaviour
    {
        [SerializeField] private int size = 16;
        
        // TODO: needs to be removed on refactoring
        [SerializeField] private Pixel[] pixelTypes;
        
        // References
        private Camera _mainCamera;
        private Sprite _drawableSprite;
        private Texture2D _drawableTexture;
        
        // GridData
        private Pixel[,] _cellularGrid;
        private Color32[] _currentColors;

        private float _cellScale;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Awake()
        {
            _cellularGrid = new Pixel[size, size];
            _cellScale = 1f / size;
            
            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    _cellularGrid[x, y] = pixelTypes[0];
                }
            }
            _drawableSprite = GetComponent<SpriteRenderer>().sprite;
            _drawableTexture = _drawableSprite.texture;
            _drawableTexture.Reinitialize(size, size);
            _currentColors = _drawableTexture.GetPixels32();
        }
        
        private void SetCurrentColorsFromGrid()
        {
            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    _currentColors[x + y * size] = _cellularGrid[x, y].color;
                }
            }
        }

        private void ApplyGridToTexture()
        {
            _drawableTexture.SetPixels32(_currentColors);
            _drawableTexture.Apply();
        }

        [MyBox.ButtonMethod]
        public void MakeRandomCellSand()
        {
            var randomX = UnityEngine.Random.Range(0, size);
            var randomY = UnityEngine.Random.Range(0, size);
            _cellularGrid[randomX, randomY] = pixelTypes[1];
        }

        private void Update()
        {
            var mouseHeldDown = Input.GetMouseButton(0);
            
            if (mouseHeldDown)
            {
                var mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                var x = (int) ((mousePos.x + (size*_cellScale)/2) / _cellScale);
                var y = (int) ((mousePos.y + (size*_cellScale)/2) / _cellScale);
                x = Mathf.Clamp(x, 0, size - 1);
                y = Mathf.Clamp(y, 0, size - 1);
                _cellularGrid[x, y] = pixelTypes[1];
                SetCurrentColorsFromGrid();
                ApplyGridToTexture();
            }  
        }
        
        
        
    }
}

[Serializable]
public struct Pixel
{
    public bool isSand;
    public Color color;
}