using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoad = "PlayScene"; // Change this to your scene name

    private void OnMouseDown() // Detects mouse clicks
    {
        LoadScene();
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
