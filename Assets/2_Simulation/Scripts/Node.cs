using UniSand;
using UnityEngine;
using UnityEngine.Serialization;

namespace UniSand
{
    public struct Node
    {
        public Pixel pixelData;
        public Color variantColor;
        public bool isMovedOnce;

        public Node(Pixel pixelData, bool isMovedOnce)
        {
            this.variantColor = pixelData.VariantColor();
            this.pixelData = pixelData;
            this.isMovedOnce = isMovedOnce;
        }
    }
}
