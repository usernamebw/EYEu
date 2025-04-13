using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IshiharaPlateData
{
    public string plateName;
    public string normalAnswer;
    public string partialColorBlindAnswer;
}


[System.Serializable]
public class IshiharaPlateDataList
{
    public List<IshiharaPlateData> plates;
}
