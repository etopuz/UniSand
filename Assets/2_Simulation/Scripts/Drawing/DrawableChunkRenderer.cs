using System;
using UniSand;
using UnityEngine;

namespace UniSand
{
    [RequireComponent(typeof(DrawableChunk))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class DrawableChunkRenderer : MonoBehaviour
    {
        private DrawableChunk _drawableChunk;
        private SpriteRenderer _spriteRenderer;
        private Sprite _drawableSprite;
        private Texture2D _drawableTexture;
        private Color32[] _currentColors;

        private int Size => Settings.Instance.chunkSize;

        private void Awake()
        {
            _drawableChunk = GetComponent<DrawableChunk>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = Sprite.Create(new Texture2D(Size, Size), new Rect(0, 0, Size, Size), Vector2.one * 0.5f, 64);
            _drawableSprite = _spriteRenderer.sprite;
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

        private void Draw(Node[,] pixelGrid)
        {
            for (var x = 0; x < Size; x++)
            {
                for (var y = 0; y < Size; y++)
                {
                    _currentColors[x + y * Size] = pixelGrid[x, y].variantColor;
                }
            }

            _drawableTexture.SetPixels32(_currentColors);
            _drawableTexture.Apply();
        }
    }
}
