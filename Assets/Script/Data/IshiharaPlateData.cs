using System;
using System.Collections.Generic;
using UnityEngine;

// IshiharaPlateData.cs
[System.Serializable]
public class IshiharaPlateData
{
    public int plateNumber;
    public string normalAnswer;
    public string partialAnswer;
    public string userInput; // This will store the user's answer for this plate
}

[System.Serializable]
public class IshiharaDataWrapper
{
    public List<IshiharaPlateData> plates;
}
