using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup_PrepareStage : UIPopup
{
    private List<UISlot_CanUseRanger> canUseRangerSlots;
    public List<UISlot_UseRanger> batchOne_UseRangerSlots;
    public List<UISlot_UseRanger> batchTwo_UseRangerSlots;
    public List<UISlot_PrepareEnemy> enemySlot;
    private Transform canUseRangerSlotParent;

    bool tempBool;
    private bool isDrawedEnemy = false;

    public override bool Init()
    {
        Managers.UI.SetCanvas(gameObject, true);
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        canUseRangerSlots = new List<UISlot_CanUseRanger>();
        canUseRangerSlotParent = Util.FindChild(gameObject, _name: "Content_CanUseSlot", _recursive: true).transform;

        BindEvent(GetButton((int)Buttons.Button_Back).gameObject, () => { Managers.UI.ClosePopupUI(this); Managers.Game.prepareStageSystem = null; });
        BindEvent(GetButton((int)Buttons.Button_Start).gameObject, OnClick_Start);

        //RedrawCanUseSlot();
        return true;
    }

    public void OnClick_Start()
    {
        Managers.Game.StartBattleStage((callback) => 
        {
            if(callback == Define.StartBattleStageEvent.RangerIsNotExist)
            {
                //레인저가 없다고 뜨기
                Debug.Log("레인저가 하나는 필요해");
                return;
            }
            ClosePopupUP();
        });
    }

    public override void RedrawUI()
    {
        RedrawCanUseSlot();
        RedrawUseSlot();
        DrawEnemy();
        GetText((int)Texts.Text_CostInfo).text = $"{Managers.Game.prepareStageSystem.currentCost}/{Managers.Game.prepareStageSystem.stageData.canUseCost}";
        //GetText((int)Texts.Text_UserGem).text = $"{Managers.Game.playerData.gem}";
        //GetText((int)Texts.Text_UserGold).text = $"{Managers.Game.playerData.gold}";
    }

    public void RedrawCanUseSlot()
    {
        for (int i = canUseRangerSlots.Count - 1; i >= 0; i--)
        {
            Managers.Resource.Destroy(canUseRangerSlots[i].gameObject);
        }

        canUseRangerSlots.Clear();
        //여기서 전부 다 다시 소환하는 게 아니라 사용하고 있는 녀석이라면 스킵을 해야함

        for (int i = 0; i < Managers.Game.playerData.hasRangers.Count; i++)
        {
            tempBool = false;

            for (int j = 0; j < Managers.Game.prepareStageSystem.rangerControllerData.Length; j++)
            {
                if (Managers.Game.prepareStageSystem.rangerControllerData[j] != null && Managers.Game.prepareStageSystem.rangerControllerData[j].UID == Managers.Game.playerData.hasRangers[i].UID)
                    tempBool = true;
            }

            if (tempBool) continue;
            UISlot_CanUseRanger slot = Managers.Resource.Instantiate("UISlot_CanUseRanger", canUseRangerSlotParent).GetOrAddComponent<UISlot_CanUseRanger>();
            slot.Init(Managers.Data.GetRangerControllerData(Managers.Game.playerData.hasRangers[i].UID), transform);
            canUseRangerSlots.Add(slot);
        }
    }

    public void RedrawUseSlot()
    {
        if(Managers.Game.prepareStageSystem.batch == Define.Batch.One)
        {
            for (int i = 0; i < batchOne_UseRangerSlots.Count; i++)
            {
                batchOne_UseRangerSlots[i].RedrawUI(Managers.Game.prepareStageSystem.rangerControllerData[i], transform);
            }
        }
    }

    public void DrawEnemy()
    {
        if (isDrawedEnemy) return;
        isDrawedEnemy = true;
        for (int i = 0; i < enemySlot.Count; i++)
        {
            enemySlot[i].SetEnemy();
        }
    }

    private enum Buttons
    {
        Button_Start, Button_Back
    }

    private enum Texts
    {
        Text_CostInfo
    }
}
