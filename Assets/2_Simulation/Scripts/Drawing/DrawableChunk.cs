using System;
using System.Collections;
using MyBox;
using UniSand.Utils;
using UnityEngine;



namespace UniSand
{
    public class DrawableChunk : MonoBehaviour
    {
        public Action<Node[,]> onDraw; // TODO : maybe use Color32[,] instead of Node[,]
                                       // ?? Which one is better for performance? Or is it really matter
        
        private Camera _mainCamera;
        private Node[,] _cellularGrid;
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
            _cellularGrid = new Node[Size, Size];
            _cellScale = 1f / Size;
            
            for (var x = 0; x < Size; x++)
            {
                for (var y = 0; y < Size; y++)
                {
                    _cellularGrid[x, y] = new Node(_settings.emptyPixel, false);
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
                    if (_cellularGrid[x,y].pixelData.density!=0 && !_cellularGrid[x,y].isMovedOnce)
                    {
                        foreach (var directionEnum in _cellularGrid[x,y].pixelData.movementBehaviour)
                        {
                            var direction = Directions.GetDirectionVector2Int(directionEnum);
                            var targetIndex = Helpers.GetFurthestPossiblePixelIndex(_cellularGrid, new Vector2Int(x, y), direction, _settings.step);
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
                ClearMovedOnce();
            }

        }

        private void ClearMovedOnce()
        {
            for (var x = 0; x < Size; x++)
            {
                for (var y = 0; y < Size; y++)
                {
                    _cellularGrid[x, y].isMovedOnce = false;
                }
            }
        }

        // TODO: Move this method to a separate class(like pen class xD)
        private void DrawOnInput()
        {
            var mouseHeldDown = Input.GetMouseButton(0);
            
            if (mouseHeldDown)
            {
                var mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                var testPos = new Vector3(transform.position.x, transform.position.y, -10);
                
                var x = (int) ((mousePos.x - testPos.x + 0.5f) / _cellScale);
                var y = (int) ((mousePos.y - testPos.y + 0.5f) / _cellScale);

                bool isMouseInBounds = Vector3.Distance(mousePos, testPos) < 0.5f;
                
                Debug.Log("testPos:" + testPos + "mousePos:" + mousePos + "isMouseInBounds:" + isMouseInBounds + "x: " + x + " y: " +y);
                
                if (!isMouseInBounds)
                {
                    return;
                }
                
                /*x = Mathf.Clamp(x, 0, Size - 1);
                y = Mathf.Clamp(y, 0, Size - 1);*/

                var currentPos = new Vector2Int(x, y);
                
                if (_lastPenHoldPosition == Vector2Int.zero || _lastPenHoldPosition == currentPos)
                {
                    DrawSinglePixel(currentPos, _settings.pixelTypes[0]); // TODO remove hardcoded pixel type
                }

                else
                {
                    DrawLerpLine(currentPos, _settings.pixelTypes[0]); // TODO remove hardcoded pixel type
                }
                
                _lastPenHoldPosition = new Vector2Int(x, y);
                
                onDraw?.Invoke(_cellularGrid);
            }
            
            else if (Input.GetMouseButtonUp(0))
            {
                _lastPenHoldPosition = Vector2Int.zero;
            }
        }

        private void DrawLerpLine(Vector2Int currentPos, Pixel pixel)
        {
            var distance = Vector2Int.Distance(_lastPenHoldPosition, currentPos);

            Vector2Int currentPosition;

            for (var move = 0; move <= distance; move += 1)
            {
                currentPosition = Vector2.Lerp(_lastPenHoldPosition, currentPos, move / distance).ToVector2Int();
                DrawSinglePixel(currentPosition, pixel);
            }
        }

        private void DrawSinglePixel(Vector2Int currentPos, Pixel pixel)
        {
            _cellularGrid[currentPos.x, currentPos.y] = new Node(pixel, false);
        }
        
        private void MovePixel(Vector2Int from, Vector2Int to)
        {
            (_cellularGrid[to.x, to.y], _cellularGrid[from.x, from.y])  = (_cellularGrid[from.x, from.y], _cellularGrid[to.x, to.y]);
            _cellularGrid[to.x, to.y].isMovedOnce = true;
        }
    }
}


