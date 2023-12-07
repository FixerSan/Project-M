using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScene_Stage : UIScene
{
    private List<UISlot_StageRanger> rangerSlots = new List<UISlot_StageRanger>();
    private List<UISlot_StageEnemy> enemySlots = new List<UISlot_StageEnemy>();
    public override bool Init()
    {
        if(!base.Init()) return false;
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        BindEvent(GetButton((int)Buttons.Button_FastSpeed).gameObject, OnClick_FastSpeed);
        BindEvent(GetButton((int)Buttons.Button_AutoSkill).gameObject, OnClick_AutoSkill);

        Transform tempTrans = Util.FindChild<Transform>(gameObject, _name: "Trans_RangerSlot");
        UISlot_StageRanger rangerSlot;
        for (int i = 0; i < Managers.Object.Rangers.Count; i++)
        {
            rangerSlot = Managers.UI.CreateStageRangerSlot(tempTrans);
            rangerSlot.Init(Managers.Object.Rangers[i]);
            rangerSlots.Add(rangerSlot);
        }

        UISlot_StageEnemy enemySlot;
        tempTrans = Util.FindChild<Transform>(gameObject, _name: "Trans_EnemySlot");
        for (int i = 0; i < Managers.Object.Enemies.Count; i++)
        {
            enemySlot = Managers.UI.CreateStageEnemySlot(tempTrans);
            enemySlot.Init(Managers.Object.Enemies[i]);
            enemySlots.Add(enemySlot);
        }

        RedrawUI();
        return true;
    }

    public override void RedrawUI()
    {
        GetText((int)Texts.Text_Timer).text = $"{(int)Managers.Game.battleStageSystem.time}";
        GetButton((int)Buttons.Button_FastSpeed).interactable = !Managers.Game.battleStageSystem.isFastSpeed;
        GetButton((int)Buttons.Button_AutoSkill).interactable = !Managers.Game.battleStageSystem.isAutoSkill;
        GetImage((int)Images.Image_RangersHPBar).fillAmount = Managers.Game.battleStageSystem.rangersTotalCurrentHP / Managers.Game.battleStageSystem.rangersTotalMaxHP;
        GetImage((int)Images.Image_EnemiesHPBar).fillAmount = Managers.Game.battleStageSystem.enemiesTotalCurrentHP / Managers.Game.battleStageSystem.enemiesTotalMaxHP;

        for (int i = 0; i < rangerSlots.Count; i++)
        {
            rangerSlots[i].Redraw();
        }

        for (int i = 0; i < enemySlots.Count; i++)
        {
            enemySlots[i].Redraw();
        }
    }

    public void OnClick_FastSpeed()
    {
        Managers.Game.battleStageSystem.SetFastSpeed(!Managers.Game.battleStageSystem.isFastSpeed);
    }

    public void OnClick_AutoSkill()
    {
        Managers.Game.battleStageSystem.SetAutoSkill(!Managers.Game.battleStageSystem.isAutoSkill);
    }

    private enum Texts
    {
        Text_Timer
    }

    private enum Buttons
    {
        Button_FastSpeed, Button_AutoSkill
    }

    private enum Images
    {
        Image_RangersHPBar, Image_EnemiesHPBar
    }
}
