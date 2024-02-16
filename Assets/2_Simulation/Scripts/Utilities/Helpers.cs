using UnityEngine;

namespace UniSand.Utils
{
    public static class Helpers
    {
        public static Vector2Int GetFurthestEmptyPixelIndex(Node[,] grid, Vector2Int from, Vector2Int direction, int step)
        {
            var furthest = from;
            var currentPixelIndex = from;
            var currentStep = 0;
            var size = grid.GetLength(0);
            
            while (currentStep < step)
            {
                currentPixelIndex += direction;
                var canDraw = currentPixelIndex.x >= 0 && currentPixelIndex.x < size &&
                              currentPixelIndex.y >= 0 && currentPixelIndex.y < size &&
                              grid[currentPixelIndex.x, currentPixelIndex.y].pixelData.isEmpty;

                if (!canDraw)
                {
                    break;
                }
                
                furthest = currentPixelIndex;
                currentStep++;
            }

            return furthest;
        }
        
    }
}
