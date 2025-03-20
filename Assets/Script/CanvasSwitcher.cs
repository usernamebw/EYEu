using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSwitcher : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject instructionCanvas;

    void Start()
    {
        // Ensure main menu is active and instruction screen is hidden at start
        mainMenuCanvas.SetActive(true);
        instructionCanvas.SetActive(false);
    }

    public void ShowInstructions()
    {
        mainMenuCanvas.SetActive(false);
        instructionCanvas.SetActive(true);
    }
}
