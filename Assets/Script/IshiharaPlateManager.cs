using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IshiharaPlateManager : MonoBehaviour
{
    public Texture[] plateTextures; 
    public Renderer plateRenderer;

    private int currentIndex = 0;

    void Start()
    {
        if (plateRenderer == null)
            plateRenderer = GetComponent<Renderer>();

        if (plateTextures.Length > 0)
            plateRenderer.material.mainTexture = plateTextures[currentIndex];
    }

    public void NextPlate()
    {
        if (plateTextures.Length == 0) return;

        currentIndex = (currentIndex + 1) % plateTextures.Length;
        plateRenderer.material.mainTexture = plateTextures[currentIndex];

        Debug.Log("Switched to Ishihara Plate: " + currentIndex);
    }
}
