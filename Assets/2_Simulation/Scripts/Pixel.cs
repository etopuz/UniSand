using System;
using System.Collections.Generic;
using UniSand.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct Pixel
{
    public bool isEmpty;
    public bool isSand;
    public Color color;
    
    public const float SaturationVariation = 0.1f; // Intensity of saturation variation
    public const float ValueVariation = 0.1f;

    public List<Direction> movementBehaviour;
    
    
    public void VariantColor()
    {
        Color.RGBToHSV(color, out var h, out var s, out var v);
        
        s = Mathf.Clamp01(s + Random.Range(-SaturationVariation, SaturationVariation));
        v = Mathf.Clamp01(v + Random.Range(-ValueVariation, ValueVariation));
        color=Color.HSVToRGB(h, s, v);
    }
}