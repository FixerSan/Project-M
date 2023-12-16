using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "StageProfle", menuName = "Container/StageProfle.", order = 0)]
public class StageProfile : ScriptableObject
{
    [Header("StageUIDs")]
    [Description("스테이지 1 UID")]
    public int stageOneUID;
    [Description("스테이지 2 UID")]
    public int stageTwoUID;
    [Description("스테이지 3 UID")]
    public int stageThreeUID;
    [Description("스테이지 4 UID")]
    public int stageFourUID;
    [Description("스테이지 5 UID")]
    public int stageFiveUID;
}
