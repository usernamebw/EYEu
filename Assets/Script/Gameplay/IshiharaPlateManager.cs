using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IshiharaPlateManager : MonoBehaviour
{
    public Texture[] plateTextures; 
    public Renderer plateRenderer;

    void Start()
    {
        if (plateRenderer == null)
            plateRenderer = GetComponent<Renderer>();

        // Optional: show first plate
        if (plateTextures.Length > 0)
            plateRenderer.material.mainTexture = plateTextures[0];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetPlate(1); // Does pressing Space update the texture?
        }
    }

    public void SetPlate(int index)
    {
        Debug.Log("[SetPlate] Called with index: " + index);

        if (plateTextures == null || plateTextures.Length == 0)
        {
            Debug.LogWarning("[SetPlate] No textures assigned.");
            return;
        }

        if (index >= 0 && index < plateTextures.Length)
        {
            plateRenderer.material.mainTexture = plateTextures[index];
            Debug.Log("[SetPlate] Texture switched successfully to plate " + index);
        }
        else
        {
            Debug.LogWarning("[SetPlate] Invalid index: " + index);
        }
    }

}
