using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class ObjectManager
{
    // 전체 스폰 위치
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

    //스폰된 아군
    public List<RangerController> Rangers { get; } = new List<RangerController>();

    // 아군 스폰 위치
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

    // 스폰된 적
    public List<EnemyController> Enemies { get; } = new List<EnemyController>();

    //적 스폰 위치
    public Transform EnemyTrnas
    {
        get
        {
            if (enemyTrnas == null)
            {
                GameObject go = GameObject.Find("@EnemyTrnas");
                if (go == null)
                {
                    go = new GameObject(name: "@EnemyTrnas");
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
        RangerStatus status = new RangerStatus(controller, data);
        Ranger ranger = null;
        Dictionary<RangerState, State<RangerController>> states = new Dictionary<RangerState, State<RangerController>>();

        Define.Ranger rangerEnum = Util.ParseEnum<Define.Ranger>(data.name);
        switch (rangerEnum)
        {
            default:
                ranger = new Rangers.Base(controller);
                states.Add(Define.RangerState.Stay, new RangerStates.Base.Stay());
                states.Add(Define.RangerState.Idle, new RangerStates.Base.Idle());
                states.Add(Define.RangerState.Move, new RangerStates.Base.Move());
                states.Add(Define.RangerState.Follow, new RangerStates.Base.Follow());
                states.Add(Define.RangerState.Attack, new RangerStates.Base.Attack());
                states.Add(Define.RangerState.SkillCast, new RangerStates.Base.SkillCast());
                states.Add(Define.RangerState.Die, new RangerStates.Base.Die());
                states.Add(Define.RangerState.EndBattle, new RangerStates.Base.EndBattle());
                break;
        }
        controller.SetPosition(_position);
        controller.Init(ranger, data, status, states);
        Rangers.Add(controller);
        return controller;
    }

    public EnemyController SpawnEnemy(int _UID, Vector2 _position = new Vector2())
    {
        EnemyControllerData data = Managers.Data.GetEnemyControllerData(_UID);
        EnemyController controller = Managers.Resource.Instantiate(data.name, EnemyTrnas).GetOrAddComponent<EnemyController>();
        EnemyStatus status = new EnemyStatus(controller,data);
        Enemy enemy = null;
        Dictionary<EnemyState, State<EnemyController>> states = new Dictionary<EnemyState, State<EnemyController>>();

        Define.Enemy rangerEnum = Util.ParseEnum<Define.Enemy>(data.name);
        switch (rangerEnum)
        {
            case Define.Enemy.WitchZombie:
                enemy = new Enemies.WitchZombie(controller);
                states.Add(Define.EnemyState.Stay, new EnemyStates.Base.Stay());
                states.Add(Define.EnemyState.Idle, new EnemyStates.Base.Idle());
                states.Add(Define.EnemyState.Move, new EnemyStates.Base.Move());
                states.Add(Define.EnemyState.Follow, new EnemyStates.Base.Follow());
                states.Add(Define.EnemyState.Attack, new EnemyStates.Base.Attack());
                states.Add(Define.EnemyState.SkillCast, new EnemyStates.Base.SkillCast());
                states.Add(Define.EnemyState.Die, new EnemyStates.Base.Die());
                states.Add(Define.EnemyState.EndBattle, new EnemyStates.Base.EndBattle());

                break;
            default:
                enemy = new Enemies.Base(controller);
                states.Add(Define.EnemyState.Stay, new EnemyStates.Base.Stay());
                states.Add(Define.EnemyState.Idle, new EnemyStates.Base.Idle());
                states.Add(Define.EnemyState.Move, new EnemyStates.Base.Move());
                states.Add(Define.EnemyState.Follow, new EnemyStates.Base.Follow());
                states.Add(Define.EnemyState.Attack, new EnemyStates.Base.Attack());
                states.Add(Define.EnemyState.SkillCast, new EnemyStates.Base.SkillCast());
                states.Add(Define.EnemyState.Die, new EnemyStates.Base.Die());
                states.Add(Define.EnemyState.EndBattle, new EnemyStates.Base.EndBattle());
                break;
        }
        controller.SetPosition(_position);
        controller.Init(enemy, data, status, states);
        Enemies.Add(controller);
        return controller;
    }

    public void ClearRangers()
    {
        for (int i = Rangers.Count-1; i > -1; i--)
            Managers.Resource.Destroy(Rangers[i].gameObject);
        Rangers.Clear();
    }

    public void ClearEnemies()
    {
        for (int i = Enemies.Count - 1; i > -1; i--)
            Managers.Resource.Destroy(Enemies[i].gameObject);
        Rangers.Clear();

    }
}
