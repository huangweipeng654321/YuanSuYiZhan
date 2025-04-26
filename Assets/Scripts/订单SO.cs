using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "NewNestedStringList", menuName = "ScriptableObjects/Nested String List")]
public class NestedStringList : ScriptableObject
{
    public List<string> strings;
}