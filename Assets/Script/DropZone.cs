using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Required for TextMeshProUGUI

public class DropZone : MonoBehaviour
{
    public Transform[] dropSlots;
    public TextMeshProUGUI answerText; // Assign this from Inspector

    private bool[] slotOccupied;

    private void Awake()
    {
        slotOccupied = new bool[dropSlots.Length];
    }

    public Transform GetAvailableSlot()
    {
        for (int i = 0; i < dropSlots.Length; i++)
        {
            if (!slotOccupied[i])
            {
                slotOccupied[i] = true;
                UpdateAnswerDisplay(); // update text when block snapped
                return dropSlots[i];
            }
        }
        return null;
    }

    public void ResetSlots()
    {
        for (int i = 0; i < slotOccupied.Length; i++)
        {
            slotOccupied[i] = false;
        }

        UpdateAnswerDisplay(); // Clear answer text
    }

    public string GetSubmittedAnswer()
    {
        string answer = "";

        for (int i = 0; i < dropSlots.Length; i++)
        {
            Transform slot = dropSlots[i];

            if (slot.childCount > 0)
            {
                NumberBlock block = slot.GetChild(0).GetComponent<NumberBlock>();
                if (block != null)
                {
                    answer += block.numberValue.ToString();
                }
            }
        }

        return answer;
    }

    public void UpdateAnswerDisplay()
    {
        string currentAnswer = GetSubmittedAnswer();
        if (answerText != null)
        {
            answerText.text = currentAnswer;
        }
    }
}
