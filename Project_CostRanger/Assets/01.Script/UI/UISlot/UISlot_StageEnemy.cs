using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlot_StageEnemy : UIBase
{
    public int slotIndex;
    private GameObject enemy;
    public void SetEnemy()
    {
        if(Managers.Game.prepareStageSystem.enemies[slotIndex] != null)
        {
            enemy = Managers.UI.ShowPrepareEnemy(Managers.Game.prepareStageSystem.enemies[slotIndex].name);
            enemy.transform.SetParent(transform);
            enemy.transform.localPosition = Define.prepareEnemyOffset;
        }
    }
}
