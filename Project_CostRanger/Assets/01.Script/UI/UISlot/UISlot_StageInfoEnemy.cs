using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlot_StageInfoEnemy : UIBase
{
    public EnemyInfoData enemyInfoData;
    private int count = 1;
    public override bool Init()
    {
        if(!base.Init()) return false;
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        GetImage((int)Images.Image_EnemyCount).gameObject.SetActive(false);
        return true;
    }

    public void DrawSlot(EnemyInfoData _enemyInfoData)
    {
        //if (enemyInfoData.UID == -1) return;
        isDrawed = true;
        GetImage((int)Images.Image_EnemyCount).gameObject.SetActive(true);
        enemyInfoData = _enemyInfoData;
        Managers.Resource.Load<Sprite>(enemyInfoData.name, (_sprite) => { GetImage((int)Images.Image_Illust).sprite = _sprite; });
    }

    public void AddCount()
    {
        count++;
        GetImage((int)Images.Image_EnemyCount).gameObject.SetActive(true);
        GetText((int)Texts.Text_EnemyCount).text = count.ToString();
    }

    private enum Images 
    {
        Image_Illust, Image_EnemyCount, Image_EnemySlot
    }

    private enum Texts
    {
        Text_EnemyCount
    }
}
