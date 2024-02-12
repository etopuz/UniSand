using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;

public class Settings : Singleton<Settings>
{
    public Pixel[] pixelTypes;
    
    [Header("Simulation")]
    public int step = 3;
    public float simulationSpeed;
    public int chunkSize = 64;
    
    [Header("Rendering")]
    public float saturationVariation = 0.1f; // Intensity of saturation variation
    public float valueVariation = 0.1f;
}
