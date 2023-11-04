using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UIScene_Stage : UIScene
{
    public List<UISlot_MVPPoint> mvpSlots = new List<UISlot_MVPPoint>();
    public override bool Init()
    {
        if (!base.Init()) return false;
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindObject(typeof(Objects));
        GetButton((int)Buttons.Button_AutoSkill).interactable = !Managers.Game.battleInfo.isAutoSkill;
        GetButton((int)Buttons.Button_FastSpeed).interactable = !Managers.Game.battleInfo.isFastSpeed;
        BindEvent(GetButton((int)Buttons.Button_FastSpeed).gameObject, () => 
        {
            Managers.Game.battleInfo.ChangeFastSpeed();
            GetButton((int)Buttons.Button_FastSpeed).interactable = !Managers.Game.battleInfo.isFastSpeed;
        });

        BindEvent(GetButton((int)Buttons.Button_AutoSkill).gameObject, () =>
        {
            Managers.Game.battleInfo.ChangeAutoSkill();
            GetButton((int)Buttons.Button_AutoSkill).interactable = !Managers.Game.battleInfo.isAutoSkill;
        });

        Managers.Event.OnVoidEvent -= UpdateUI;
        Managers.Event.OnVoidEvent += UpdateUI;

        GetText((int)Texts.Text_Stage).text = Managers.Game.battleInfo.currentStage.stageName;
        for (int i = 0; i < Managers.Object.Armys.Count; i++)
        {
            UISlot_MVPPoint slot = Managers.Resource.Instantiate("Slot_MVPPoint", GetObject((int)Objects.Trans_MvpSlot).transform).GetOrAddComponent<UISlot_MVPPoint>();
            slot.Init(i);
            mvpSlots.Add(slot);
        }

        for (int i = 0; i < Managers.Object.Armys.Count; i++)
        {
            UISlot_StageEntity slot = Managers.Resource.Instantiate("Slot_StageEntity", GetObject((int)Objects.Bundle_EntitySlot).transform).GetOrAddComponent<UISlot_StageEntity>();
            slot.Init(Managers.Object.Armys[i]);
        }
        return true;
    }

    public void UpdateUI(Define.VoidEventType _type)
    {
        if (_type != Define.VoidEventType.OnChangeBattleInfo) return;

        GetImage((int)Images.Image_ArmyHP).fillAmount = (float)Managers.Game.battleInfo.armyCurrentHP / Managers.Game.battleInfo.armyMaxHP;
        GetImage((int)Images.Image_EnemyHP).fillAmount = (float)Managers.Game.battleInfo.enemyCurrentHP / Managers.Game.battleInfo.enemyMaxHP;
        GetText((int)Texts.Text_Time).text = $"00:{((int)Managers.Game.battleInfo.time)}";
        for (int i = 0; i < mvpSlots.Count; i++)
        {
            mvpSlots[i].UpdateValue();
        }
    }

    private void OnDestroy()
    {
        Managers.Event.OnVoidEvent -= UpdateUI;
    }

    public enum Buttons
    {
        Button_AutoSkill, Button_FastSpeed
    }
    private enum Texts
    {
        Text_Stage, Text_Time
    }
    private enum Images
    {
        Image_ArmyHP, Image_EnemyHP
    }
    private enum Objects
    {
        Trans_MvpSlot, Bundle_EntitySlot
    }
}
