using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropZone : MonoBehaviour
{
    public Transform[] dropSlots;
    public TextMeshProUGUI answerText;

    private bool[] slotOccupied;

    void Awake()
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
        UpdateAnswerDisplay();
    }

    public void UnassignBlock(Transform block)
    {
        for (int i = 0; i < dropSlots.Length; i++)
        {
            if (dropSlots[i].childCount > 0 && dropSlots[i].GetChild(0) == block)
            {
                slotOccupied[i] = false;
                UpdateAnswerDisplay();
                return;
            }
        }
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
