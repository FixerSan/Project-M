using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class ObjectManager
{
    // ��ü ���� ��ġ
    public Transform EntityTrans
    {
        get
        {
            if (entityTrans == null)
            {
                GameObject go = GameObject.Find("@EntityTrans");
                if (go == null)
                    go = new GameObject(name: "@EntityTrans");
                entityTrans = go.transform;
            }
            return entityTrans;
        }
    }
    private Transform entityTrans;

    //������ �Ʊ�
    public List<RangerController> Rangers { get; } = new List<RangerController>();

    // �Ʊ� ���� ��ġ
    public Transform RangerTrans
    {
        get
        {
            if (rangerTrans == null)
            {
                GameObject go = GameObject.Find("@RangerTrans");
                if (go == null)
                {
                    go = new GameObject(name: "@RangerTrans");
                    go.transform.SetParent(EntityTrans);
                }
                rangerTrans = go.transform;
            }
            return rangerTrans;
        }
    }
    private Transform rangerTrans;

    // ������ ��
    public List<EnemyController> Enemies { get; } = new List<EnemyController>();

    //�� ���� ��ġ
    public Transform EnemyTrnas
    {
        get
        {
            if (enemyTrnas == null)
            {
                GameObject go = GameObject.Find("@enemyTrnas");
                if (go == null)
                {
                    go = new GameObject(name: "@enemyTrnas");
                    go.transform.SetParent(EntityTrans);
                }
                enemyTrnas = go.transform;
            }
            return enemyTrnas;
        }
    }
    private Transform enemyTrnas;


    public RangerController SpawnRanger(int _UID, Vector2 _position = new Vector2())
    {
        RangerControllerData data = Managers.Data.GetRangerControllerData(_UID);
        RangerController controller = Managers.Resource.Instantiate(data.name, RangerTrans).GetOrAddComponent<RangerController>();
        RangerStatus status = new RangerStatus(data);
        Ranger ranger = null;
        Dictionary<RangerState, State<RangerController>> states = new Dictionary<RangerState, State<RangerController>>();

        Define.Ranger rangerEnum = Util.ParseEnum<Define.Ranger>(data.name);
        switch (rangerEnum)
        {
            case Define.Ranger.TestRanger:
                ranger = new Rangers.TestRanger();
                states.Add(Define.RangerState.Idle, new RangerStates.Base.Idle());
                states.Add(Define.RangerState.Move, new RangerStates.Base.Move());
                states.Add(Define.RangerState.Follow, new RangerStates.Base.Follow());
                states.Add(Define.RangerState.Attack, new RangerStates.Base.Attack());
                states.Add(Define.RangerState.SkillCast, new RangerStates.Base.SkillCast());
                states.Add(Define.RangerState.Die, new RangerStates.Base.Die());
                states.Add(Define.RangerState.EndBattle, new RangerStates.Base.EndBattle());
                break;
        }

        controller.Init(ranger, data, status, states);
        Rangers.Add(controller);
        return controller;
    }

    public EnemyController SpawnEnemy(int _UID, Vector2 _position = new Vector2())
    {
        EnemyControllerData data = Managers.Data.GetEnemyControllerData(_UID);
        EnemyController controller = Managers.Resource.Instantiate(data.name, RangerTrans).GetOrAddComponent<EnemyController>();
        EnemyStatus status = new EnemyStatus(data);
        Enemy enemy = null;
        Dictionary<EnemyState, State<EnemyController>> states = new Dictionary<EnemyState, State<EnemyController>>();

        Define.Enemy rangerEnum = Util.ParseEnum<Define.Enemy>(data.name);
        switch (rangerEnum)
        {
            case Define.Enemy.TestEnemyZero:
                enemy = new Enemies.TestEnemy(controller);
                states.Add(Define.EnemyState.Idle, new EnemyStates.Base.Idle());
                states.Add(Define.EnemyState.Move, new EnemyStates.Base.Move());
                states.Add(Define.EnemyState.Follow, new EnemyStates.Base.Follow());
                states.Add(Define.EnemyState.Attack, new EnemyStates.Base.Attack());
                states.Add(Define.EnemyState.SkillCast, new EnemyStates.Base.SkillCast());
                states.Add(Define.EnemyState.Die, new EnemyStates.Base.Die());
                states.Add(Define.EnemyState.EndBattle, new EnemyStates.Base.EndBattle());
                break;
        }

        controller.Init(enemy, data, status, states);
        Enemies.Add(controller);
        return controller;
    }
}
