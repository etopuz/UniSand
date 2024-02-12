using System;
using System.Collections;
using MyBox;
using UniSand.Utils;
using UnityEngine;

// TODO: Add velocity and acceleration support for pixels
// TODO: Add density support for pixels
// TODO: Create Cellular Automata Logic For Simulation
// TODO: Add more detailed classes for Pixel(like Sand, Water, etc.) and Pens(like Brush, Eraser, etc., they may have size). And separate them from DrawableChunk
// TODO: Add UI buttons for drawing on canvas (choose pixels, pens, or other functional things such as simulation start, pause, etc.)
// TODO: Add pause and resume support for simulation
// TODO: Add multiple chunk support for bigger simulations
// TODO: Add multithreading support for multiple chunks

namespace UniSand
{
    public class DrawableChunk : MonoBehaviour
    {
        public Action<Pixel[,]> onDraw;
        
        private Camera _mainCamera;
        private Pixel[,] _cellularGrid;
        private Vector2Int _lastPenHoldPosition = Vector2Int.zero;
        private float _cellScale;
        private Settings _settings;
        private int Size => _settings.chunkSize;
        private void Start()
        {
            _mainCamera = Camera.main;
            onDraw?.Invoke(_cellularGrid);
            StartCoroutine(UpdateAfterFrameSkip());
        }

        private void Awake()
        {
            _settings = Settings.Instance;
            _cellularGrid = new Pixel[Size, Size];
            _cellScale = 1f / Size;
            
            for (var x = 0; x < Size; x++)
            {
                for (var y = 0; y < Size; y++)
                {
                    _cellularGrid[x, y] = _settings.pixelTypes[0];
                }
            }
        }

        private void Update()
        {
            DrawOnInput();
        }
        
        private IEnumerator UpdateAfterFrameSkip()
        {
            while (true)
            {
                yield return new WaitForSeconds(_settings.frameSkip * Time.deltaTime);
                UpdateGrid();
            }
        }

        private void UpdateGrid()
        {
            var isAnythingChanged = false;
            
            for (var x = 0; x < Size; x++)
            {
                for (var y = 0; y < Size; y++)
                {
                    if (_cellularGrid[x,y].isSand)
                    {
                        foreach (var directionEnum in _cellularGrid[x,y].movementBehaviour)
                        {
                            var direction = Directions.GetDirectionVector2Int(directionEnum);
                            var targetIndex = Helpers.GetFurthestEmptyPixelIndex(_cellularGrid, new Vector2Int(x, y), direction, _settings.step);
                            if (Vector2Int.Distance(targetIndex, new Vector2Int(x, y)) > 0)
                            {
                                MovePixel(new Vector2Int(x, y), targetIndex);
                                isAnythingChanged = true;
                                break;
                            }                            
                        }
                    }
                }
            }

            if (isAnythingChanged)
            {
                onDraw?.Invoke(_cellularGrid);
            }
        }

        // TODO: Move this method to a separate class(like pen class xD)
        private void DrawOnInput()
        {
                        
            var mouseHeldDown = Input.GetMouseButton(0);
            
            if (mouseHeldDown)
            {
                var mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                var x = (int) ((mousePos.x + 0.5f) / _cellScale);
                var y = (int) ((mousePos.y + 0.5f) / _cellScale);
                x = Mathf.Clamp(x, 0, Size - 1);
                y = Mathf.Clamp(y, 0, Size - 1);

                var currentPos = new Vector2Int(x, y);
                
                if (_lastPenHoldPosition == Vector2Int.zero || _lastPenHoldPosition == currentPos)
                {
                    DrawSinglePixel(currentPos);
                }

                else
                {
                    DrawLerpLine(currentPos);
                }
                
                _lastPenHoldPosition = new Vector2Int(x, y);
                
                onDraw?.Invoke(_cellularGrid);
            }
            
            else if (Input.GetMouseButtonUp(0))
            {
                _lastPenHoldPosition = Vector2Int.zero;
            }
        }

        private void DrawLerpLine(Vector2Int currentPos)
        {
            var distance = Vector2Int.Distance(_lastPenHoldPosition, currentPos);

            Vector2Int currentPosition;

            for (var move = 0; move <= distance; move += 1)
            {
                currentPosition = Vector2.Lerp(_lastPenHoldPosition, currentPos, move / distance).ToVector2Int();
                DrawSinglePixel(currentPosition);
            }
        }

        private void DrawSinglePixel(Vector2Int currentPos)
        {
            _cellularGrid[currentPos.x, currentPos.y] = _settings.pixelTypes[1];
            _cellularGrid[currentPos.x, currentPos.y].VariantColor();
        }
        
        private void MovePixel(Vector2Int from, Vector2Int to)
        {
            _cellularGrid[to.x, to.y] = _cellularGrid[from.x, from.y];
            _cellularGrid[from.x, from.y] = _settings.pixelTypes[0];
        }
    }
}
