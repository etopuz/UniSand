using MyBox;
using UnityEngine;

public class Settings : Singleton<Settings>
{
    public Pixel[] pixelTypes;
    
    [Header("Simulation")]
    public int step = 3;
    public int frameSkip = 3;
    public int chunkSize = 64;
    
    [Header("Rendering")]
    public float saturationVariation = 0.1f;
    public float valueVariation = 0.1f;
}
