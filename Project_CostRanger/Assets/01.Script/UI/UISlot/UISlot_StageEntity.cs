using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlot_StageEntity : UIBase
{
    private BattleEntityController controller;
    public void Init(BattleEntityController _controller)
    {
        controller = _controller;
        BindImage(typeof(Images));
        BindText(typeof(Texts));
        Managers.Resource.Load<Sprite>(controller.entity.data.name, (_sprite) => { GetImage((int)Images.Image_Illust).sprite = _sprite; });
        BindEvent(GetImage((int)Images.Image_Illust).gameObject, UseSkill);
    }
    public void FixedUpdate()
    {
        if (controller == null) return;
        CheckCooltime();
        CheckHpBar();   
    }

    public void UseSkill()
    {
        if (Managers.Screen.isSkillCasting) return;
        controller.entity.Skill(); 
        Debug.Log("Ω∫≈≥ ΩΩ∑‘ ¿€µøµ ");
    }

    public void CheckCooltime()
    {
        if (controller.isDead)
        {
            GetImage((int)Images.Image_Cooltime).fillAmount = 1;
            GetText((int)Texts.Text_Cooltime).color = Color.red;
            GetText((int)Texts.Text_Cooltime).text = "Dead";
            return;
        }

        GetImage((int)Images.Image_Cooltime).fillAmount = controller.battleEntityStatus.currentSkillCooltime / controller.entity.data.skillCooltime;
        GetText((int)Texts.Text_Cooltime).text = $"{(int)controller.battleEntityStatus.currentSkillCooltime}";
        if (controller.battleEntityStatus.currentSkillCooltime == 0)
        {
            GetText((int)Texts.Text_Cooltime).gameObject.SetActive(false);
            GetImage((int)Images.Image_Cooltime).gameObject.SetActive(false);
        }

        else 
        {
            if (!GetImage((int)Images.Image_Cooltime).gameObject.activeSelf)
                GetImage((int)Images.Image_Cooltime).gameObject.SetActive(true);
            if(!GetText((int)Texts.Text_Cooltime).gameObject.activeSelf)
                GetText((int)Texts.Text_Cooltime).gameObject.SetActive(true);
        }
    }

    public void CheckHpBar()
    {
        if (!controller.isDead)
            GetImage((int)Images.Image_HP).fillAmount = (float)controller.battleEntityStatus.CurrentHP / controller.battleEntityStatus.maxHP;
        else if (controller.isDead)
            GetImage((int)Images.Image_HP).fillAmount = 0;
    }

    private enum Images
    {
        Image_Cooltime, Image_Illust, Image_HP
    }
    private enum Texts
    {
        Text_Cooltime
    }
}
