using System;
using UniSand;
using UnityEngine;

public class CanvasGenerator : MonoBehaviour
{
    [SerializeField] private GameObject pixelPrefab;

    private void Awake()
    {
        transform.position = Vector3.zero;
        CreateCanvas();
    }

    private void CreateCanvas()
    {
        int edgeBound = Settings.Instance.chunkAmountPerEdge/2;
        
        for (int x = -edgeBound; x < edgeBound+1; x++)
        {
            for (int y = -edgeBound; y < edgeBound+1; y++)
            {
                Instantiate(pixelPrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
    }
}
