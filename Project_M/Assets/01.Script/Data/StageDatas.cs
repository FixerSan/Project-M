using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StageDatas", menuName = "Container/StageDatas")]
public class StageDatas : ScriptableObject
{
    public StageData[] normal;
    public StageData[] hard;
}


