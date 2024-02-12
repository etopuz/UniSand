using System;
using UniSand;
using UnityEngine;

namespace UniSand
{
    [RequireComponent(typeof(DrawableChunk))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class DrawableChunkRenderer : MonoBehaviour
    {
        private Sprite _drawableSprite;
        private Texture2D _drawableTexture;
        private Color32[] _currentColors;
        private DrawableChunk _drawableChunk;
        private int Size => Settings.Instance.chunkSize;

        private void Awake()
        {
            _drawableChunk = GetComponent<DrawableChunk>();
            _drawableSprite = GetComponent<SpriteRenderer>().sprite;
            _drawableTexture = _drawableSprite.texture;
            _drawableTexture.Reinitialize(Size, Size);
            _currentColors = _drawableTexture.GetPixels32();
        }
        
        

        private void OnEnable()
        {
            _drawableChunk.onDraw += Draw;
        }
        
        private void OnDisable()
        {
            _drawableChunk.onDraw -= Draw;
        }

        private void Draw(Pixel[,] pixelGrid)
        {
            for (var x = 0; x < Size; x++)
            {
                for (var y = 0; y < Size; y++)
                {
                    _currentColors[x + y * Size] = pixelGrid[x, y].color;
                }
            }

            _drawableTexture.SetPixels32(_currentColors);
            _drawableTexture.Apply();
        }
    }
}
