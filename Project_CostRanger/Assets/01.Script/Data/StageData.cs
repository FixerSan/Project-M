using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Container/StageData")]
public class StageData : ScriptableObject
{
    public int UID;
    public string stageName;
    public Define.StageState stageState;
    public int oneStarReward;
    public int twoStarReward;
    public int threeStarReward;
    public int clearReward;
    public int canUseCost;

    public int[] frontEnemyUIDs;
    public int[] frontEnemyLevels;
    public int[] centerEnemyUIDs;
    public int[] centerEnemyLevels;
    public int[] rearEnemyUIDs;
    public int[] rearEnemyLevels;
}
