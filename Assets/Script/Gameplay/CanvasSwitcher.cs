using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSwitcher : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject instructionCanvas;
    public GameObject aboutCanvas;

    void Start()
    {
        // Show main menu by default
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenuCanvas.SetActive(true);
        instructionCanvas.SetActive(false);
        aboutCanvas.SetActive(false);
    }

    public void ShowInstructions()
    {
        mainMenuCanvas.SetActive(false);
        instructionCanvas.SetActive(true);
        aboutCanvas.SetActive(false);
    }

    public void ShowAbout()
    {
        mainMenuCanvas.SetActive(false);
        instructionCanvas.SetActive(false);
        aboutCanvas.SetActive(true);
    }
}
