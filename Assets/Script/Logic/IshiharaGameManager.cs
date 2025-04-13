using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private int currentPlateIndex = 0;
    private List<string> userAnswers = new List<string>();

    void Start()
    {
        submitButton.onClick.RemoveAllListeners(); // Prevent double-assigning
        submitButton.onClick.AddListener(SubmitAnswer);

        UpdatePlate();
    }

    void Update()
    {
        if (submitButton == null)
        {
            Debug.LogWarning("[Update] SubmitButton is missing!");
            return;
        }

        if (submitButton.onClick.GetPersistentEventCount() > 1)
        {
            Debug.LogWarning("[Update] SubmitButton has multiple listeners!");
        }

        submitButton.interactable = IsAnswerComplete();
    }


    private bool IsAnswerComplete()
    {
        return !string.IsNullOrEmpty(GetCurrentAnswer());
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

        currentPlateIndex++;

        Debug.Log($"[SubmitAnswer] New currentPlateIndex: {currentPlateIndex}");

        UpdatePlate();

        if (currentPlateIndex >= plateDataList.Count)
        {
            EvaluateResult();
        }

    //BLOCK RESET CODE

        DropZone[] dropZones = FindObjectsOfType<DropZone>();
        foreach (var zone in dropZones)
        {
            zone.ResetSlots();
        }

        DragDropBlock[] blocks = FindObjectsOfType<DragDropBlock>();
        foreach (var block in blocks)
        {
            block.ResetBlock();
        }
    }


    private void UpdatePlate()
    {
        Debug.Log($"[UpdatePlate] Switching to plate {currentPlateIndex}");

        if (plateManager != null)
        {
            Debug.Log("[UpdatePlate] Calling plateManager.SetPlate...");
            plateManager.SetPlate(currentPlateIndex);
        }
        else
        {
            Debug.LogWarning("[UpdatePlate] plateManager is not assigned!");
        }

        plateNumberText.text = $"Plate {currentPlateIndex + 1}";
        ClearAnswerSlots();
    }


    public void ClearAnswerSlots()
    {
        foreach (Transform slot in answerSlots)
        {
            foreach (Transform child in slot)
            {
                var block = child.GetComponent<DragDropBlock>();
                if (block != null)
                {
                    block.ResetBlock(); // Move it back instead of destroying
                }
            }
        }

    }

    private void EvaluateResult()
    {
        int normal = 0, partial = 0, full = 0;
        for (int i = 0; i < plateDataList.Count; i++)
        {
            string userAnswer = userAnswers[i];
            var plate = plateDataList[i];
            if (plate.normalAnswer == userAnswer) normal++;
            else if (plate.partialColorBlindAnswer == userAnswer) partial++;
            else full++;
        }
        string result = $"Normal: {normal}\nPartial Colorblind: {partial}\nFull Colorblind: {full}";
        feedbackText.text = result;
        Debug.Log("[Evaluation] " + result);
    }
}
