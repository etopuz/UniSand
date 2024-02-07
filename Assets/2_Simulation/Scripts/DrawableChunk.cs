using System;
using UnityEngine;

// TODO: Separate Rendering And Cell System
// TODO: Add continuous drawing support for mouse drag
// TODO: Create Cellular Automata Logic For Simulation
// TODO: Add more detailed classes for Pixel(like Sand, Water, etc.) and Pens(like Brush, Eraser, etc., they may have size). And separate them from DrawableChunk
// TODO: Add UI buttons for drawing on canvas (choose pixels, pens, or other functional things such as simulation start, pause, etc.)
// TODO: Add pause and resume support for simulation
// TODO: Add multiple chunk support for bigger simulations
// TODO: Add multithreading support for multiple chunks

namespace UniSand
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DrawableChunk : MonoBehaviour
    {
        [SerializeField] private int size = 16;
        
        
        [SerializeField] private Pixel[] pixelTypes; // TODO: needs to be removed on refactoring
        
        // References
        private Camera _mainCamera;
        private Sprite _drawableSprite; // TODO: needs to be removed on refactoring
        private Texture2D _drawableTexture; // TODO: needs to be removed on refactoring
        
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
                var x = (int) ((mousePos.x + 0.5f) / _cellScale);
                var y = (int) ((mousePos.y + 0.5f) / _cellScale);
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
// TODO: needs to be removed on refactoring
public struct Pixel
{
    public bool isSand;
    public Color color;
}