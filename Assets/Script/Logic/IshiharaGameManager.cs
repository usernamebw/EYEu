using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class IshiharaGameManager : MonoBehaviour
{
    [Header("Plate Settings")]
    public List<IshiharaPlateData> plateDataList;
    public IshiharaPlateManager plateManager;

    [Header("Answer Input")]
    public List<Transform> answerSlots;

    [Header("UI Elements")]
    public Text plateNumberText;
    public Button submitButton;
    public Text feedbackText;
    public Scrollbar timerScrollbar; // Timer UI scrollbar
    public GameObject testOverCanvas;  // Reference to the "Test Over" Canvas
    public Button goToResultButton;    // Reference to the button to transition to the ResultScene


    private int currentPlateIndex = 0;
    private List<string> userAnswers = new List<string>();

    private float timePerPlate = 18f;
    private float currentTimer;
    private bool timerRunning = false;

    public string filePath;

    void Start()
    {
        filePath = Application.persistentDataPath + "/userData.json";
        LoadData();
        submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(SubmitAnswer);
        goToResultButton.onClick.AddListener(GoToResultScene);

        UpdatePlate();
    }

    void Update()
    {
        if (timerRunning)
        {
            currentTimer -= Time.deltaTime;

            if (timerScrollbar != null)
                timerScrollbar.size = currentTimer / timePerPlate;

            if (currentTimer <= 0)
            {
                timerRunning = false;
                AutoSubmit();
            }
        }
    }

    private void StartTimer()
    {
        currentTimer = timePerPlate;
        timerRunning = true;

        if (timerScrollbar != null)
            timerScrollbar.size = 1f;
    }

    private void AutoSubmit()
    {
        string randomAnswer = Random.Range(1, 10).ToString();

        ResetAllBlocksAndSlots();

        if (currentPlateIndex < plateDataList.Count)
        {
            plateDataList[currentPlateIndex].userInput = randomAnswer;
            SaveData();
        }

        currentPlateIndex++;

        if (currentPlateIndex >= plateDataList.Count)
        {
            EvaluateResult();
            timerRunning = false;
        }
        else
        {
            UpdatePlate();
        }
    }

    private void ResetAllBlocksAndSlots()
    {
        // Reset all number blocks to their original positions
        DragDropBlock[] blocks = FindObjectsOfType<DragDropBlock>();
        foreach (var block in blocks)
        {
            block.ResetBlock();
        }

        // Reset all drop zones slots and answer text
        DropZone[] dropZones = FindObjectsOfType<DropZone>();
        foreach (var zone in dropZones)
        {
            zone.ResetSlots();
        }
    }

    private void LoadData()
    {
        string persistentFilePath = Path.Combine(Application.persistentDataPath, "userData.json");

        if (File.Exists(persistentFilePath))
        {
            string json = File.ReadAllText(persistentFilePath);
            IshiharaDataWrapper data = JsonUtility.FromJson<IshiharaDataWrapper>(json);
            plateDataList = data.plates;
            Debug.Log("[LoadData] Loaded from userData.json");
        }
        else
        {
            string streamingFilePath = Path.Combine(Application.streamingAssetsPath, "ishihara_plate.json");
            if (File.Exists(streamingFilePath))
            {
                string json = File.ReadAllText(streamingFilePath);
                IshiharaDataWrapper data = JsonUtility.FromJson<IshiharaDataWrapper>(json);
                plateDataList = data.plates;
                Debug.Log("[LoadData] Loaded from ishihara_plate.json (default)");
            }
            else
            {
                Debug.LogError("[LoadData] Could not find ishihara_plate.json in StreamingAssets.");
                plateDataList = new List<IshiharaPlateData>();
            }
        }

        Debug.Log($"[LoadData] Total Plates Loaded: {plateDataList.Count}");
    }

    private void SaveData()
    {
        string persistentFilePath = Path.Combine(Application.persistentDataPath, "userData.json");
        IshiharaDataWrapper dataWrapper = new IshiharaDataWrapper { plates = plateDataList };
        string json = JsonUtility.ToJson(dataWrapper, true);
        File.WriteAllText(persistentFilePath, json);
        Debug.Log("[SaveData] Data saved to userData.json");
    }

    public void SubmitAnswer()
    {
        Debug.Log($"[SubmitAnswer] Called. CurrentPlateIndex: {currentPlateIndex}");

        string answer = GetCurrentAnswer();
        Debug.Log($"[SubmitAnswer] Retrieved answer: '{answer}'");

        if (string.IsNullOrEmpty(answer))
        {
            Debug.LogWarning("[SubmitAnswer] Tried to submit empty answer. Ignored.");
            return;
        }

        userAnswers.Add(answer);

        if (currentPlateIndex < plateDataList.Count)
        {
            plateDataList[currentPlateIndex].userInput = answer;
            SaveData();
        }

        currentPlateIndex++;

        if (currentPlateIndex >= plateDataList.Count)
        {
            EvaluateResult();
            timerRunning = false; // No more plates
            return;
        }

        ResetAllBlocksAndSlots();

        UpdatePlate(); // This will restart the timer via StartTimer()
    }

    private void EvaluateResult()
    {
        int normal = 0, partial = 0, full = 0;

        for (int i = 0; i < plateDataList.Count; i++)
        {
            var plate = plateDataList[i];
            Debug.Log($"[EvaluateResult] Plate {i + 1} - UserInput: '{plate.userInput}', Normal: '{plate.normalAnswer}', Partial: '{plate.partialAnswer}'");

            if (plate.userInput == plate.normalAnswer)
                normal++;
            else if (plate.userInput == plate.partialAnswer)
                partial++;
            else
                full++;
        }

        float total = plateDataList.Count;
        float normalRatio = normal / total;
        float partialRatio = partial / total;

        // Save result into GameResultData
        if (normalRatio >= 0.8f)
        {
            GameResultData.visionResult = "normal";
        }
        else if (partialRatio >= 0.5f)
        {
            GameResultData.visionResult = "partial";
        }
        else
        {
            GameResultData.visionResult = "full";
        }

        // If test is over, show the "Test Over" canvas
        ShowTestOverCanvas();
    }

    private void ShowTestOverCanvas()
    {
        // Activate the "Test Over" Canvas
        if (testOverCanvas != null)
        {
            testOverCanvas.SetActive(true);
        }
    }

    public void GoToResultScene()
    {
        // Load the Result Scene
        SceneLoader.Instance.sceneToLoad = "ResultScene";
        SceneLoader.Instance.LoadSceneWithFade();
    }

    private string GetCurrentAnswer()
    {
        string number = "";
        foreach (var slot in answerSlots)
        {
            if (slot.childCount > 0)
            {
                string blockName = slot.GetChild(0).name.Replace("(Clone)", "").Replace("NumberBlock ", "").Trim();
                if (blockName == "X") return "";
                number += blockName;
            }
        }
        return number;
    }

    private void UpdatePlate()
    {
        Debug.Log($"[UpdatePlate] Switching to plate {currentPlateIndex}");

        if (plateManager != null)
        {
            plateManager.SetPlate(currentPlateIndex);
        }
        else
        {
            Debug.LogWarning("[UpdatePlate] plateManager is not assigned!");
        }

        plateNumberText.text = $"Plate {currentPlateIndex + 1}";
        StartTimer();
    }
}
