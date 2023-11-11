using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class UIPopup_BattleBefore : UIPopup
{
    private List<UISlot_CanUseBattleEntity> canUseSlots = new List<UISlot_CanUseBattleEntity>();
    private List<UISlot_UseBattleEntity> useSlots = new List<UISlot_UseBattleEntity>();
    public override bool Init()
    {
        if (!base.Init())
            return false;

        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindObject(typeof(Objects));

        Managers.Event.OnVoidEvent -= OnChangeBattleInfo;
        Managers.Event.OnVoidEvent += OnChangeBattleInfo;
        BindEvent(GetButton((int)Buttons.Button_Clear).gameObject, Managers.Game.battleInfo.UnUseAllBattleEntity );
        BindEvent(GetButton((int)Buttons.Button_ClosePopup).gameObject, () => { Managers.UI.ClosePopupUI(this); });
        BindEvent(GetButton((int)Buttons.Button_Start).gameObject, () => { CheckStart(); });

        BindEvent(GetImage((int)Images.Image_ArmyBattleEntitySpace_Front).gameObject, _dropCallback: (_callback) => 
        {
            GameObject dropGameObjet = _callback.pointerDrag;
            BattleEntityData data = dropGameObjet.GetComponent<UISlot_UseBattleEntity>().data;
            Managers.Game.battleInfo.UnUseBattleEntity(data);
            Managers.Game.battleInfo.UseBattleEntity(data, Define.PlaceType.Front);
            Managers.Resource.Destroy(dropGameObjet);
        } , _type: Define.UIEventType.Drop);

        BindEvent(GetImage((int)Images.Image_ArmyBattleEntitySpace_Center).gameObject, _dropCallback: (_callback) =>
        {
            GameObject dropGameObjet = _callback.pointerDrag;
            BattleEntityData data = dropGameObjet.GetComponent<UISlot_UseBattleEntity>().data;
            Managers.Game.battleInfo.UnUseBattleEntity(data);
            Managers.Game.battleInfo.UseBattleEntity(data, Define.PlaceType.Center);
            Managers.Resource.Destroy(dropGameObjet);
        }, _type: Define.UIEventType.Drop);

        BindEvent(GetImage((int)Images.Image_ArmyBattleEntitySpace_Rear).gameObject, _dropCallback: (_callback) =>
        {
            GameObject dropGameObjet = _callback.pointerDrag;
            BattleEntityData data = dropGameObjet.GetComponent<UISlot_UseBattleEntity>().data;
            Managers.Game.battleInfo.UnUseBattleEntity(data);
            Managers.Game.battleInfo.UseBattleEntity(data, Define.PlaceType.Rear);
            Managers.Resource.Destroy(dropGameObjet);
        }, _type: Define.UIEventType.Drop);






        //for (int i = 0; i < Managers.Data.playerData.hasEntites.Count; i++)
        //    CreateCanUseSlot(Managers.Data.playerData.hasEntites[i].UID, Managers.Data.playerData.hasEntites[i].level);

        CreateEnemyUseSlots();

        GetText((int)Texts.Text_Stage).text = $"Stage {Managers.Game.battleInfo.currentStage.stageName}";

        GetText((int)Texts.Text_CanSpawnArmyEntityCount).text = $"{Managers.Game.battleInfo.nowUseCost} / {Managers.Game.battleInfo.canUseCost}";
        GetText((int)Texts.Text_ArmyBattleForce).text = $"{Managers.Game.battleInfo.armybattleForce}";

        GetText((int)Texts.Text_CanSpawnEnemyEntityCount).text = $"{Managers.Game.battleInfo.nowEnemyCount} / {Managers.Game.battleInfo.nowEnemyCount}";
        GetText((int)Texts.Text_EnemyBattleForce).text = $"{Managers.Game.battleInfo.enemybattleForce}";
        return true;
    }

    public void OnChangeBattleInfo(Define.VoidEventType _type)
    {
        if (_type != Define.VoidEventType.OnChangeBattleInfo) return;

        GetText((int)Texts.Text_CanSpawnArmyEntityCount).text = $"{Managers.Game.battleInfo.nowUseCost} / {Managers.Game.battleInfo.canUseCost}";
        GetText((int)Texts.Text_ArmyBattleForce).text = $"{Managers.Game.battleInfo.armybattleForce}";
        ClearArmyUseSlots();
        CreateArmyUseSlots();
        UpdateCanUseSlots();
    }
    public void CreateCanUseSlot(int _UID, int _level)
    {
        UISlot_CanUseBattleEntity slot = Managers.Resource.Instantiate("Slot_CanUseBattleEntity", _parent: GetObject((int)Objects.Panel_Slot).transform).GetOrAddComponent<UISlot_CanUseBattleEntity>();
        BattleEntityData data = Managers.Data.GetBattleEntityData(_UID, _level);
        slot.Init(data, UISlot_CanUseBattleEntity.SlotState.UnUsed);
        canUseSlots.Add(slot);
    }

    public void UpdateCanUseSlots()
    {
        for (int i = 0; i < canUseSlots.Count; i++)
        {
            canUseSlots[i].UpdateState(UISlot_CanUseBattleEntity.SlotState.UnUsed);
        }

        for (int i = 0; i < canUseSlots.Count; i++)
        {
            for (int j = 0; j < Managers.Game.battleInfo.armyFront.Length; j++)
            {
                if (canUseSlots[i].data == Managers.Game.battleInfo.armyFront[j])
                    canUseSlots[i].UpdateState(UISlot_CanUseBattleEntity.SlotState.Used);
            }

            for (int j = 0; j < Managers.Game.battleInfo.armyCenter.Length; j++)
            {
                if (canUseSlots[i].data == Managers.Game.battleInfo.armyCenter[j])
                    canUseSlots[i].UpdateState(UISlot_CanUseBattleEntity.SlotState.Used);
            }

            for (int j = 0; j < Managers.Game.battleInfo.armyRear.Length; j++)
            {
                if (canUseSlots[i].data == Managers.Game.battleInfo.armyRear[j])
                    canUseSlots[i].UpdateState(UISlot_CanUseBattleEntity.SlotState.Used);
            }
        }
    }


    public void ClearArmyUseSlots()
    {
        for (int i = 0; i < useSlots.Count; i++)
        {
            Managers.Resource.Destroy(useSlots[i].gameObject);
        }
        useSlots.Clear();
    }

    public void CreateArmyUseSlots()
    {
        for (int i = 0; i < Managers.Game.battleInfo.armyFront.Length; i++)
        {
            if (Managers.Game.battleInfo.armyFront[i] != null)
            {
                UISlot_UseBattleEntity slot = Managers.Resource.Instantiate("Slot_UseBattleEntity", GetImage((int)Images.Image_ArmyBattleEntitySpace_Front).transform).GetOrAddComponent<UISlot_UseBattleEntity>();
                slot.Init(Managers.Game.battleInfo.armyFront[i], UISlot_UseBattleEntity.SlotType.Army);
                useSlots.Add(slot);
            }
        }

        for (int i = 0; i < Managers.Game.battleInfo.armyCenter.Length; i++)
        {
            if (Managers.Game.battleInfo.armyCenter[i] != null)
            {
                UISlot_UseBattleEntity slot = Managers.Resource.Instantiate("Slot_UseBattleEntity", GetImage((int)Images.Image_ArmyBattleEntitySpace_Center).transform).GetOrAddComponent<UISlot_UseBattleEntity>();
                slot.Init(Managers.Game.battleInfo.armyCenter[i], UISlot_UseBattleEntity.SlotType.Army);
                useSlots.Add(slot);
            }
        }

        for (int i = 0; i < Managers.Game.battleInfo.armyRear.Length; i++)
        {
            if (Managers.Game.battleInfo.armyRear[i] != null)
            {
                UISlot_UseBattleEntity slot = Managers.Resource.Instantiate("Slot_UseBattleEntity", GetImage((int)Images.Image_ArmyBattleEntitySpace_Rear).transform).GetOrAddComponent<UISlot_UseBattleEntity>();
                slot.Init(Managers.Game.battleInfo.armyRear[i], UISlot_UseBattleEntity.SlotType.Army);
                useSlots.Add(slot);
            }
        }
    }

    public void CreateEnemyUseSlots()
    {
        for (int i = 0; i < Managers.Game.battleInfo.enemyFront.Length; i++)
        {
            if (Managers.Game.battleInfo.enemyFront[i] != null)
            {
                UISlot_UseBattleEntity slot = Managers.Resource.Instantiate("Slot_UseBattleEntity", GetImage((int)Images.Image_EnemyBattleEntitySpace_Front).transform).GetOrAddComponent<UISlot_UseBattleEntity>();
                slot.Init(Managers.Game.battleInfo.enemyFront[i], UISlot_UseBattleEntity.SlotType.Enemy);
            }
        }

        for (int i = 0; i < Managers.Game.battleInfo.enemyCenter.Length; i++)
        {
            if (Managers.Game.battleInfo.enemyCenter[i] != null)
            {
                UISlot_UseBattleEntity slot = Managers.Resource.Instantiate("Slot_UseBattleEntity", GetImage((int)Images.Image_EnemyBattleEntitySpace_Center).transform).GetOrAddComponent<UISlot_UseBattleEntity>();
                slot.Init(Managers.Game.battleInfo.enemyCenter[i], UISlot_UseBattleEntity.SlotType.Enemy);
            }
        }

        for (int i = 0; i < Managers.Game.battleInfo.enemyRear.Length; i++)
        {
            if (Managers.Game.battleInfo.enemyRear[i] != null)
            {
                UISlot_UseBattleEntity slot = Managers.Resource.Instantiate("Slot_UseBattleEntity", GetImage((int)Images.Image_EnemyBattleEntitySpace_Rear).transform).GetOrAddComponent<UISlot_UseBattleEntity>();
                slot.Init(Managers.Game.battleInfo.enemyRear[i], UISlot_UseBattleEntity.SlotType.Enemy);
            }
        }
    }

    public void CheckStart()
    {
        if(Managers.Game.battleInfo.nowUseCost == 0)
        {
            Managers.UI.ShowToast("You have to put in a unit.");
            return;
        }
        Managers.Scene.LoadScene(Define.Scene.Stage);
    }

    private enum Buttons
    {
        Button_Clear, Button_ClosePopup, Button_Start
    }
    
    private enum Images
    {
        Image_ArmyBattleEntitySpace_Front, Image_ArmyBattleEntitySpace_Center, Image_ArmyBattleEntitySpace_Rear , Image_EnemyBattleEntitySpace_Front, Image_EnemyBattleEntitySpace_Center, Image_EnemyBattleEntitySpace_Rear
    }

    private enum Texts
    {
        Text_CanSpawnArmyEntityCount, Text_ArmyBattleForce, Text_Stage , Text_CanSpawnEnemyEntityCount, Text_EnemyBattleForce
    }

    public enum Objects
    {
        Panel_Slot
    }

    public void OnDestroy()
    {
        Managers.Event.OnVoidEvent -= OnChangeBattleInfo;
    }
}
