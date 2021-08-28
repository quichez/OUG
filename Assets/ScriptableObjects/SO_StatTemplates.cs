using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "SO_StatTemplates", menuName = "StatTemplates", order = 0)]
public class SO_StatTemplates : ScriptableObject
{
    [System.Serializable]
    public class StatTemplate
    {
        public string Class = "Skelly";
        public float Attack = 3f;
        public float Defense = 1f;
    }

    public List<StatTemplate> StatTemplates = new List<StatTemplate>();
}

