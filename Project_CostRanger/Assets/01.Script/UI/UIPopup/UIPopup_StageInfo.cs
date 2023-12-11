using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup_StageInfo : UIPopup
{
    private StageData stageData;
    private UISlot_StageInfoEnemy[] enemySlots;
    private UISlot_RewardInfo[] rewardSlots;

    public void Init(int _stageUID)
    {
        stageData = Managers.Data.GetStageData(_stageUID);

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        Transform tempTrans = Util.FindChild<Transform>(gameObject, "Content_Enemy", true);
        enemySlots = tempTrans.GetComponentsInChildren<UISlot_StageInfoEnemy>();

        tempTrans = Util.FindChild<Transform>(gameObject, "Content_Reward", true);    
        rewardSlots = tempTrans.GetComponentsInChildren<UISlot_RewardInfo>();

        GetText((int)Texts.Text_StageNumber).text = stageData.stageNumber;
        GetText((int)Texts.Text_StageName).text = stageData.stageName;

        BindEvent(GetButton((int)Buttons.Button_Start).gameObject, OnClick_Start) ;
        BindEvent(GetButton((int)Buttons.Button_Close).gameObject, ClosePopupUP);
        DrawSlots();
    }

    private void DrawSlots()
    {
        List<EnemyInfoData> enemies = new List<EnemyInfoData>();
        string[] enemyStringArray = stageData.enemyUIDs.Split(",");
        for (int i = 0; i < enemyStringArray.Length; i++)
        {
            if (Int32.TryParse(enemyStringArray[i], out int _enemyIndex) && _enemyIndex != 0)
                enemies.Add(Managers.Data.GetEnemyInfoData(_enemyIndex));
        }

        bool isAdded = false ;
        for (int i = 0; i < enemies.Count; i++)
        {
            isAdded = false;
            for (int j = 0; j < enemySlots.Length; j++)
            {
                if (enemySlots[j].enemyInfoData == null) continue;
                if (enemySlots[j].enemyInfoData.UID == enemies[i].UID)
                {
                    enemySlots[j].AddCount();
                    isAdded = true;
                    break;
                }
            }

            if (isAdded)
                continue;
            enemySlots.FindNotDrawed().DrawSlot(enemies[i]);
        }

        if (stageData.clearRewardGold != 0)
            rewardSlots.FindNotDrawed().DrawSlot(Define.RewardType.Gold, stageData.clearRewardGold);

        if (stageData.clearRewardEXP != 0)
            rewardSlots.FindNotDrawed().DrawSlot(Define.RewardType.EXP, stageData.clearRewardEXP);

        if (stageData.clearRewardGem != 0)
            rewardSlots.FindNotDrawed().DrawSlot(Define.RewardType.Gem, stageData.clearRewardGem);

        if (stageData.clearRewardUpgradeCost != 0)
            rewardSlots.FindNotDrawed().DrawSlot(Define.RewardType.UpgradeCost, stageData.clearRewardUpgradeCost);
    }

    private void OnClick_Start()
    {
        ClosePopupUP();
        Managers.Game.StartPrepare(stageData.UID);
    }

    private enum Texts
    {
        Text_StageNumber, Text_StageName
    }

    private enum Buttons
    {
        Button_Close, Button_Start
    }
}
