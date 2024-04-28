using System;
using System.Collections;
using System.Collections.Generic;
using UniSand;
using UnityEngine;

[RequireComponent(typeof(DrawableChunk))]
public abstract class DrawableChunkRenderer : MonoBehaviour
{
    protected  Color32[] _currentColors;
    protected DrawableChunk _drawableChunk;
    protected int Size => Settings.Instance.chunkSize;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        _drawableChunk.onDraw += Draw;
    }
        
    private void OnDisable()
    {
        _drawableChunk.onDraw -= Draw;
    }

    protected abstract void Init();

    protected abstract void Draw(Node[,] pixelGrid);
    
}
