using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageScene : BaseScene
{
    public Dictionary<string, Transform> poses;
    public override void Init(Action _callback = null)
    {
        poses = new Dictionary<string, Transform>();
        string[] names = Enum.GetNames(typeof(Trans));
        Transform posesTrans = GameObject.Find("@Trans").transform;

        Managers.Event.OnVoidEvent -= CheckEndStage;
        Managers.Event.OnVoidEvent += CheckEndStage;

        for (int i = 0; i < names.Length; i++)
            poses.Add(names[i], Util.FindChild<Transform>(posesTrans.gameObject, _name: names[i], true));
        Managers.UI.ShowSceneUI<UIScene_Stage>();

        SceneEvent(0, () => 
        {
            SceneEvent(1);
        });

        foreach (var item in Managers.Object.Armys)
            item.ChangeState(Define.BattleEntityState.Move);

        foreach (var item in Managers.Object.Enemys)
            item.ChangeState(Define.BattleEntityState.Move);
    }

    public void CreateArmyBattleEntities()
    {
        #region ArmyFront
        List<BattleEntityController> becs = new List<BattleEntityController>();
        for (int i = 0; i < Managers.Game.battleInfo.armyFront.Length; i++)
        {
            if (Managers.Game.battleInfo.armyFront[i] != null)
                becs.Add(Managers.Object.SpawnArmyBattleEntity((Define.BattleEntity)Managers.Game.battleInfo.armyFront[i].UID, Managers.Game.battleInfo.armyFront[i].level));
        }

        if(becs.Count == 1)
        {
            for (int i = 0; i < becs.Count; i++)
            {
                becs[i].transform.position = poses[Trans.Trans_ArmyFront_OneOrThree_Spawn.ToString()].GetChild(1).position;
                becs[i].SetMoveTarget(poses[Trans.Trans_ArmyFront_OneOrThree_Stop.ToString()].GetChild(1));
            }
        }

        else if(becs.Count == 2)
        {
            for (int i = 0; i < becs.Count; i++)
            {
                becs[i].transform.position = poses[Trans.Trans_ArmyFront_Two_Spawn.ToString()].GetChild(i).position;
                becs[i].SetMoveTarget(poses[Trans.Trans_ArmyFront_Two_Stop.ToString()].GetChild(i));
            }
        }

        else if (becs.Count == 3)
        {
            for (int i = 0; i < becs.Count; i++)
            {
                becs[i].transform.position = poses[Trans.Trans_ArmyFront_OneOrThree_Spawn.ToString()].GetChild(i).position;
                becs[i].SetMoveTarget(poses[Trans.Trans_ArmyFront_OneOrThree_Stop.ToString()].GetChild(i));
            }
        }
        #endregion

        #region ArmyCenter
        becs.Clear();
        for (int i = 0; i < Managers.Game.battleInfo.armyCenter.Length; i++)
        {
            if (Managers.Game.battleInfo.armyCenter[i] != null)
                becs.Add(Managers.Object.SpawnArmyBattleEntity((Define.BattleEntity)Managers.Game.battleInfo.armyCenter[i].UID, Managers.Game.battleInfo.armyCenter[i].level));
        }

        if (becs.Count == 1)
        {
            for (int i = 0; i < becs.Count; i++)
            {
                becs[i].transform.position = poses[Trans.Trans_ArmyCenter_OneOrThree_Spawn.ToString()].GetChild(1).position;
                becs[i].SetMoveTarget(poses[Trans.Trans_ArmyCenter_OneOrThree_Stop.ToString()].GetChild(1));
            }
        }

        else if (becs.Count == 2)
        {
            for (int i = 0; i < becs.Count; i++)
            {
                becs[i].transform.position = poses[Trans.Trans_ArmyCenter_Two_Spawn.ToString()].GetChild(i).position;
                becs[i].SetMoveTarget(poses[Trans.Trans_ArmyCenter_Two_Stop.ToString()].GetChild(i));
            }
        }

        else if (becs.Count == 3)
        {
            for (int i = 0; i < becs.Count; i++)
            {
                becs[i].transform.position = poses[Trans.Trans_ArmyCenter_OneOrThree_Spawn.ToString()].GetChild(i).position;
                becs[i].SetMoveTarget(poses[Trans.Trans_ArmyCenter_OneOrThree_Stop.ToString()].GetChild(i));
            }
        }
        #endregion

        #region ArmyRear
        becs.Clear();
        for (int i = 0; i < Managers.Game.battleInfo.armyRear.Length; i++)
        {
            if (Managers.Game.battleInfo.armyRear[i] != null)
                becs.Add(Managers.Object.SpawnArmyBattleEntity((Define.BattleEntity)Managers.Game.battleInfo.armyRear[i].UID, Managers.Game.battleInfo.armyRear[i].level));
        }

        if (becs.Count == 1)
        {
            for (int i = 0; i < becs.Count; i++)
            {
                becs[i].transform.position = poses[Trans.Trans_ArmyRear_OneOrThree_Spawn.ToString()].GetChild(1).position;
                becs[i].SetMoveTarget(poses[Trans.Trans_ArmyRear_OneOrThree_Stop.ToString()].GetChild(1));
            }
        }

        else if (becs.Count == 2)
        {
            for (int i = 0; i < becs.Count; i++)
            {
                becs[i].transform.position = poses[Trans.Trans_ArmyRear_Two_Spawn.ToString()].GetChild(i).position;
                becs[i].SetMoveTarget(poses[Trans.Trans_ArmyRear_Two_Stop.ToString()].GetChild(i));
            }
        }

        else if (becs.Count == 3)
        {
            for (int i = 0; i < becs.Count; i++)
            {
                becs[i].transform.position = poses[Trans.Trans_ArmyRear_OneOrThree_Spawn.ToString()].GetChild(i).position;
                becs[i].SetMoveTarget(poses[Trans.Trans_ArmyRear_OneOrThree_Stop.ToString()].GetChild(i));
            }
        }
        #endregion
    }
    public void CreateEnemyBattleEntities()
    {
        #region Front
        List<BattleEntityController> becs = new List<BattleEntityController>();
        for (int i = 0; i < Managers.Game.battleInfo.enemyFront.Length; i++)
        {
            if (Managers.Game.battleInfo.enemyFront[i] != null)
                becs.Add(Managers.Object.SpawnEnemyBattleEntity((Define.BattleEntity)Managers.Game.battleInfo.enemyFront[i].UID, Managers.Game.battleInfo.enemyFront[i].level));
        }

        if (becs.Count == 1)
        {
            for (int i = 0; i < becs.Count; i++)
            {
                becs[i].transform.position = poses[Trans.Trans_EnemyFront_OneOrThree_Spawn.ToString()].GetChild(1).position;
                becs[i].SetMoveTarget(poses[Trans.Trans_EnemyFront_OneOrThree_Stop.ToString()].GetChild(1));
            }
        }

        else if (becs.Count == 2)
        {
            for (int i = 0; i < becs.Count; i++)
            {
                becs[i].transform.position = poses[Trans.Trans_EnemyFront_Two_Spawn.ToString()].GetChild(i).position;
                becs[i].SetMoveTarget(poses[Trans.Trans_EnemyFront_Two_Stop.ToString()].GetChild(i));
            }
        }

        else if (becs.Count == 3)
        {
            for (int i = 0; i < becs.Count; i++)
            {
                becs[i].transform.position = poses[Trans.Trans_EnemyFront_OneOrThree_Spawn.ToString()].GetChild(i).position;
                becs[i].SetMoveTarget(poses[Trans.Trans_EnemyFront_OneOrThree_Stop.ToString()].GetChild(i));
            }
        }
        #endregion

        #region Center
        becs.Clear();
        for (int i = 0; i < Managers.Game.battleInfo.enemyCenter.Length; i++)
        {
            if (Managers.Game.battleInfo.enemyCenter[i] != null)
                becs.Add(Managers.Object.SpawnEnemyBattleEntity((Define.BattleEntity)Managers.Game.battleInfo.enemyCenter[i].UID, Managers.Game.battleInfo.enemyCenter[i].level));
        }

        if (becs.Count == 1)
        {
            for (int i = 0; i < becs.Count; i++)
            {
                becs[i].transform.position = poses[Trans.Trans_EnemyCenter_OneOrThree_Spawn.ToString()].GetChild(1).position;
                becs[i].SetMoveTarget(poses[Trans.Trans_EnemyCenter_OneOrThree_Stop.ToString()].GetChild(1));
            }
        }

        else if (becs.Count == 2)
        {
            for (int i = 0; i < becs.Count; i++)
            {
                becs[i].transform.position = poses[Trans.Trans_EnemyCenter_Two_Spawn.ToString()].GetChild(i).position;
                becs[i].SetMoveTarget(poses[Trans.Trans_EnemyCenter_Two_Stop.ToString()].GetChild(i));
            }
        }

        else if (becs.Count == 3)
        {
            for (int i = 0; i < becs.Count; i++)
            {
                becs[i].transform.position = poses[Trans.Trans_EnemyCenter_OneOrThree_Spawn.ToString()].GetChild(i).position;
                becs[i].SetMoveTarget(poses[Trans.Trans_EnemyCenter_OneOrThree_Stop.ToString()].GetChild(i));
            }
        }
        #endregion

        #region Rear
        becs.Clear();
        for (int i = 0; i < Managers.Game.battleInfo.enemyRear.Length; i++)
        {
            if (Managers.Game.battleInfo.enemyRear[i] != null)
                becs.Add(Managers.Object.SpawnEnemyBattleEntity((Define.BattleEntity)Managers.Game.battleInfo.enemyRear[i].UID, Managers.Game.battleInfo.enemyRear[i].level));
        }

        if (becs.Count == 1)
        {
            for (int i = 0; i < becs.Count; i++)
            {
                becs[i].transform.position = poses[Trans.Trans_EnemyRear_OneOrThree_Spawn.ToString()].GetChild(1).position;
                becs[i].SetMoveTarget(poses[Trans.Trans_EnemyRear_OneOrThree_Stop.ToString()].GetChild(1));
            }
        }

        else if (becs.Count == 2)
        {
            for (int i = 0; i < becs.Count; i++)
            {
                becs[i].transform.position = poses[Trans.Trans_EnemyRear_Two_Spawn.ToString()].GetChild(i).position;
                becs[i].SetMoveTarget(poses[Trans.Trans_EnemyRear_Two_Stop.ToString()].GetChild(i));
            }
        }

        else if (becs.Count == 3)
        {
            for (int i = 0; i < becs.Count; i++)
            {
                becs[i].transform.position = poses[Trans.Trans_EnemyRear_OneOrThree_Spawn.ToString()].GetChild(i).position;
                becs[i].SetMoveTarget(poses[Trans.Trans_EnemyRear_OneOrThree_Stop.ToString()].GetChild(i));
            }
        }
        #endregion
    }

    public void CheckEndStage(Define.VoidEventType _type)
    {
        if (_type != Define.VoidEventType.OnChangeBattleInfo) return;
        if (Managers.Game.state != Define.GameState.BattleProgress) return;

        if (Managers.Game.battleInfo.armyCurrentHP == 0)
            Managers.Game.battleInfo.Lose();

        if (Managers.Game.battleInfo.enemyCurrentHP == 0)
            Managers.Game.battleInfo.Victory();

    }

    public override void SceneEvent(int _eventIndex, Action _callback = null)
    {
        switch (_eventIndex)
        {
            case 0:
                CreateArmyBattleEntities();
                CreateEnemyBattleEntities();
                Managers.Game.battleInfo.StartStage();
                break;

            case 1:
                StartCoroutine(SceneEvent_1Routine());
                break;
        }
        _callback?.Invoke();
    }

    public IEnumerator SceneEvent_1Routine()
    {
        yield return new WaitForSeconds(3);
        foreach (var item in Managers.Object.Armys)
        {
            item.FindTarget();
            item.ChangeState(Define.BattleEntityState.Follow);
        }

        foreach (var item in Managers.Object.Enemys)
        {
            item.FindTarget();
            item.ChangeState(Define.BattleEntityState.Follow);
        }
        Managers.Game.state = Define.GameState.BattleProgress;
    }

    public override void Clear()
    {
        Managers.Event.OnVoidEvent -= CheckEndStage;
    }

    private enum Trans
    {
        Trans_ArmyFront_OneOrThree_Spawn,
        Trans_ArmyFront_Two_Spawn, 
        Trans_ArmyCenter_OneOrThree_Spawn,
        Trans_ArmyCenter_Two_Spawn,
        Trans_ArmyRear_OneOrThree_Spawn,
        Trans_ArmyRear_Two_Spawn,
        Trans_ArmyFront_OneOrThree_Stop,
        Trans_ArmyFront_Two_Stop,
        Trans_ArmyCenter_OneOrThree_Stop,
        Trans_ArmyCenter_Two_Stop, 
        Trans_ArmyRear_OneOrThree_Stop,
        Trans_ArmyRear_Two_Stop,

        Trans_EnemyFront_OneOrThree_Spawn,
        Trans_EnemyFront_Two_Spawn,
        Trans_EnemyCenter_OneOrThree_Spawn,
        Trans_EnemyCenter_Two_Spawn,
        Trans_EnemyRear_OneOrThree_Spawn,
        Trans_EnemyRear_Two_Spawn,
        Trans_EnemyFront_OneOrThree_Stop,
        Trans_EnemyFront_Two_Stop,
        Trans_EnemyCenter_OneOrThree_Stop,
        Trans_EnemyCenter_Two_Stop,
        Trans_EnemyRear_OneOrThree_Stop,
        Trans_EnemyRear_Two_Stop,
    }
}
