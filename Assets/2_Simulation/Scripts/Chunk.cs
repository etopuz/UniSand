using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace UniSand
{
    public class Chunk : MonoBehaviour
    {
        [SerializeField] private int width = 512;
        [SerializeField] private int height = 512;
        [SerializeField] private float scale;

        [SerializeField] private Pixel[] PixelTypes;    
            
        private Pixel[,] _cellularGrid;
        private void Awake()
        {
            _cellularGrid = new Pixel[width, height];
            
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    _cellularGrid[x, y] = PixelTypes[0];
                }
            }
        }

        [MyBox.ButtonMethod]
        public void MakeRandomCellSand()
        {
            var randomX = UnityEngine.Random.Range(0, width);
            var randomY = UnityEngine.Random.Range(0, height);
            _cellularGrid[randomX, randomY] = PixelTypes[1];
        }
        

        private void OnDrawGizmos()
        {
            if (_cellularGrid == null) return;
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var pixel = _cellularGrid[x, y];
                    Gizmos.color = pixel.color;
                    Gizmos.DrawCube(new Vector3(-(width*scale)/2 + x*scale, -(height*scale)/2 + y*scale, 0), Vector3.one*scale);
                }
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