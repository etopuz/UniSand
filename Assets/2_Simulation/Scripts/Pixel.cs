using System;
using System.Collections.Generic;
using UniSand.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UniSand
{
    [Serializable]
    public struct Pixel
    {
        public bool isEmpty;
        public bool isSand;
        public Color color;

        public List<Direction> movementBehaviour;
        public bool isMovedOnce;

        public void VariantColor()
        {
            Color.RGBToHSV(color, out var h, out var s, out var v);

            s = Mathf.Clamp01(s + Random.Range(-Settings.Instance.saturationVariation,
                Settings.Instance.saturationVariation));
            v = Mathf.Clamp01(v + Random.Range(-Settings.Instance.valueVariation, Settings.Instance.valueVariation));
            color = Color.HSVToRGB(h, s, v);
        }
    }
}