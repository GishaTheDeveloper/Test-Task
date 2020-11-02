using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Object Data", menuName = "ScriptableObjects/Geometry Object Data")]
public class GeometryObjectData : ScriptableObject
{
    public List<ClickColorData> clicksData;
}

[System.Serializable]
public class ClickColorData
{
    public int minClicksCount;
    public int maxClicksCount;
    public Color Color;
}