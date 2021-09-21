using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewComparisonTable",menuName = "Config/ComparisonTable")]
public class BuffColorConfig : ScriptableObject
{
    [Tooltip("颜色对照表")] public Buff_ColorDictionary _comparisonTable;
}
