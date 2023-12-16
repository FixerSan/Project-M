using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "StageProfle", menuName = "Container/StageProfle.", order = 0)]
public class StageProfile : ScriptableObject
{
    [Header("StageUIDs")]
    [Description("�������� 1 UID")]
    public int stageOneUID;
    [Description("�������� 2 UID")]
    public int stageTwoUID;
    [Description("�������� 3 UID")]
    public int stageThreeUID;
    [Description("�������� 4 UID")]
    public int stageFourUID;
    [Description("�������� 5 UID")]
    public int stageFiveUID;
}
