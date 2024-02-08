using System;
using MyBox;
using UnityEngine;

// TODO: Separate Rendering And Cell System
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
        private Vector2Int _lastPenHoldPosition = Vector2Int.zero;

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
            SetCurrentColorsFromGrid();
            ApplyGridToTexture();
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

                var currentPos = new Vector2Int(x, y);
                
                if (_lastPenHoldPosition == Vector2Int.zero)
                {
                    DrawSinglePixel(currentPos);
                }

                else
                {
                    DrawLerpLine(currentPos);
                }
                
                _lastPenHoldPosition = new Vector2Int(x, y);
                
                SetCurrentColorsFromGrid();
                ApplyGridToTexture();
            }

            else
            {
                _lastPenHoldPosition = Vector2Int.zero;
            }
        }

        private void DrawLerpLine(Vector2Int currentPos)
        {
            float distance = Vector2Int.Distance(_lastPenHoldPosition, currentPos);

            var currentPosition = _lastPenHoldPosition;

            // Calculate how many times we should interpolate between start_point and end_point based on the amount of time that has passed since the last update

            for (int lerp = 0; lerp <= distance; lerp += 1)
            {
                currentPosition = Vector2.Lerp(_lastPenHoldPosition, currentPos, lerp / distance).ToVector2Int();
                DrawSinglePixel(currentPosition);
            }
        }

        private void DrawSinglePixel(Vector2Int currentPos)
        {
            _cellularGrid[currentPos.x, currentPos.y] = pixelTypes[1];
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