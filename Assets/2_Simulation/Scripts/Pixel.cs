using System;
using System.Collections.Generic;
using UniSand.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UniSand
{
    [CreateAssetMenu(fileName = nameof(Pixel), menuName = Settings.MenuName+nameof(Pixel), order = 0)]
    public class Pixel : ScriptableObject
    {
        public bool isEmpty;
        public bool isSand;
        public float density;
        public Color color;
        public bool canCreateVariantColor;

        public List<Direction> movementBehaviour;

        public Color GetDrawColor()
        {
            if (!canCreateVariantColor)
            {
                return color;
            }
            
            Color.RGBToHSV(color, out var h, out var s, out var v);

            s = Mathf.Clamp01(s + Random.Range(-Settings.Instance.saturationVariation,
                Settings.Instance.saturationVariation));
            v = Mathf.Clamp01(v + Random.Range(-Settings.Instance.valueVariation, Settings.Instance.valueVariation));
            return Color.HSVToRGB(h, s, v);
        }
    }
}