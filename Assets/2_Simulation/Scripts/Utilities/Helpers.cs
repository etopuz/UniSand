using UnityEngine;

namespace UniSand.Utils
{
    public static class Helpers
    {
        public static Vector2Int GetFurthestPossiblePixelIndex(Node[,] grid, Vector2Int from, Vector2Int direction, int step)
        {
            var furthest = from;
            var currentPixelIndex = from;
            var currentStep = 0;
            var size = grid.GetLength(0);
            
            while (currentStep < step)
            {
                currentPixelIndex += direction;
                
                var isInBounds = currentPixelIndex.x >= 0 && currentPixelIndex.x < size &&
                                 currentPixelIndex.y >= 0 && currentPixelIndex.y < size;

                if (!isInBounds)
                {
                    break;
                }
                
                var isTargetNodesDensityGoodToMove = 
                    (grid[currentPixelIndex.x, currentPixelIndex.y].pixelData.density 
                     - grid[from.x, from.y].pixelData.density) * direction.y > 0 || 
                    grid[currentPixelIndex.x, currentPixelIndex.y].pixelData.density == 0;
                

                if (!isTargetNodesDensityGoodToMove)
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
