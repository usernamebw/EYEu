using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultSceneManager : MonoBehaviour
{
    public GameObject normalVisionCanvas;
    public GameObject partialVisionCanvas;
    public GameObject fullVisionCanvas;

    void Start()
    {
        switch (GameResultData.visionResult)
        {
            case "normal":
                normalVisionCanvas.SetActive(true);
                break;
            case "partial":
                partialVisionCanvas.SetActive(true);
                break;
            case "full":
                fullVisionCanvas.SetActive(true);
                break;
        }
    }
}
