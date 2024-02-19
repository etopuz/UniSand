using System;
using MyBox;
using UnityEngine;

namespace UniSand
{
    public class Settings : Singleton<Settings>
    {
        public const string MenuName = "UniSand/";
        
        public Pixel[] pixelTypes;
        public Pixel emptyPixel;
        
        [Header("Simulation Speed")]
        public int step = 3;
        public int frameSkip = 3;
        
        [Header("Chunk Settings")]
        public int chunkSize = 64;
        public int chunkAmountPerEdge = 4;

        [Header("Rendering")] 
        public Camera targetCamera;
        public float saturationVariation = 0.1f;
        public float valueVariation = 0.1f;


        public void Awake()
        {
            targetCamera.orthographicSize = 0.5f * chunkAmountPerEdge + 0.1f;
        }
    }  
}
