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

    // 아군 스폰 위치
    public Transform ArmyBattleEntityTrnas
    {
        get
        {
            if (armyBattleEntityTrnas == null)
            {
                GameObject go = GameObject.Find("@ArmyBattleEntityTrans");
                if (go == null)
                {
                    go = new GameObject(name: "@ArmyBattleEntityTrans");
                    go.transform.SetParent(EntityTrans);
                }
                armyBattleEntityTrnas = go.transform;
            }
            return armyBattleEntityTrnas;
        }
    }
    private Transform armyBattleEntityTrnas;

    //스폰된 아군 위치
    public List<BattleEntityController> Armys { get; } = new List<BattleEntityController>();
    public List<RangerController> Rangers { get; } = new List<RangerController>();
    public List<EnemyController> enemies { get; } = new List<EnemyController>();


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


    //적 스폰 위치
    public Transform EnemyBattleEntityTrnas
    {
        get
        {
            if (enemyBattleEntityTrnas == null)
            {
                GameObject go = GameObject.Find("@EnemyBattleEntityTrnas");
                if (go == null)
                {
                    go = new GameObject(name: "@EnemyBattleEntityTrnas");
                    go.transform.SetParent(EntityTrans);
                }
                enemyBattleEntityTrnas = go.transform;
            }
            return enemyBattleEntityTrnas;
        }
    }
    private Transform enemyBattleEntityTrnas;

    // 스폰된 적들 위치
    public List<BattleEntityController> Enemys { get; } = new List<BattleEntityController>();

    //아군 전투 오브젝트 생성 후 초기화
    public BattleEntityController SpawnArmyBattleEntity(Define.BattleEntity _entity, int _level,Vector2 _position = new Vector2())
    {
        //전투 오브젝트 공통 요소 생성

        BattleEntityController battleEntityController = Managers.Resource.Instantiate($"{_entity.ToString()}", _parent: ArmyBattleEntityTrnas).GetOrAddComponent<BattleEntityController>();
        battleEntityController.transform.position = _position;
        BattleEntity battleEntity = null;
        BattleEntityStatus status = null;
        BattleEntityData data = null;
        Dictionary<Define.BattleEntityState, State<BattleEntityController>> states = new Dictionary<BattleEntityState, State<BattleEntityController>>();

        //각 전투 오브젝트의 요소 생성
        switch (_entity)
        {
            case Define.BattleEntity.Warrior:
                states.Add(Define.BattleEntityState.Idle, new BattleEntityStates.Base.Idle());
                states.Add(Define.BattleEntityState.Move, new BattleEntityStates.Base.Move());
                states.Add(Define.BattleEntityState.Follow, new BattleEntityStates.Base.Follow());
                states.Add(Define.BattleEntityState.Attack, new BattleEntityStates.Base.Attack());
                states.Add(Define.BattleEntityState.SkillCast, new BattleEntityStates.Base.SkillCast());
                states.Add(Define.BattleEntityState.Die, new BattleEntityStates.Base.Die());
                states.Add(Define.BattleEntityState.EndBattle, new BattleEntityStates.Base.EndBattle());

                data = Managers.Data.GetBattleEntityData((int)_entity, _level);
                battleEntity = new BattleEntites.Warrior(battleEntityController, data);
                break;

            case Define.BattleEntity.Tank:
                states.Add(Define.BattleEntityState.Idle, new BattleEntityStates.Base.Idle());
                states.Add(Define.BattleEntityState.Move, new BattleEntityStates.Base.Move());
                states.Add(Define.BattleEntityState.Follow, new BattleEntityStates.Base.Follow());
                states.Add(Define.BattleEntityState.Attack, new BattleEntityStates.Base.Attack());
                states.Add(Define.BattleEntityState.SkillCast, new BattleEntityStates.Base.SkillCast());
                states.Add(Define.BattleEntityState.Die, new BattleEntityStates.Base.Die());
                states.Add(Define.BattleEntityState.EndBattle, new BattleEntityStates.Base.EndBattle());

                data = Managers.Data.GetBattleEntityData((int)_entity, _level);
                battleEntity = new BattleEntites.Tank(battleEntityController, data);
                break;

            case Define.BattleEntity.Wizard:
                states.Add(Define.BattleEntityState.Idle, new BattleEntityStates.Base.Idle());
                states.Add(Define.BattleEntityState.Move, new BattleEntityStates.Base.Move());
                states.Add(Define.BattleEntityState.Follow, new BattleEntityStates.Base.Follow());
                states.Add(Define.BattleEntityState.Attack, new BattleEntityStates.Base.Attack());
                states.Add(Define.BattleEntityState.SkillCast, new BattleEntityStates.Base.SkillCast());
                states.Add(Define.BattleEntityState.Die, new BattleEntityStates.Base.Die());
                states.Add(Define.BattleEntityState.EndBattle, new BattleEntityStates.Base.EndBattle());

                data = Managers.Data.GetBattleEntityData((int)_entity, _level);
                battleEntity = new BattleEntites.Wizard(battleEntityController, data);
                break;

            case Define.BattleEntity.EnemyOne:
                states.Add(Define.BattleEntityState.Idle, new BattleEntityStates.Base.Idle());
                states.Add(Define.BattleEntityState.Move, new BattleEntityStates.Base.Move());
                states.Add(Define.BattleEntityState.Follow, new BattleEntityStates.Base.Follow());
                states.Add(Define.BattleEntityState.Attack, new BattleEntityStates.Base.Attack());
                states.Add(Define.BattleEntityState.SkillCast, new BattleEntityStates.Base.SkillCast());
                states.Add(Define.BattleEntityState.Die, new BattleEntityStates.Base.Die());
                states.Add(Define.BattleEntityState.EndBattle, new BattleEntityStates.Base.EndBattle());

                data = Managers.Data.GetBattleEntityData((int)_entity, _level);
                battleEntity = new BattleEntites.Three(battleEntityController, data);
                break;

            case Define.BattleEntity.EnemyTwo:
                states.Add(Define.BattleEntityState.Idle, new BattleEntityStates.Base.Idle());
                states.Add(Define.BattleEntityState.Move, new BattleEntityStates.Base.Move());
                states.Add(Define.BattleEntityState.Follow, new BattleEntityStates.Base.Follow());
                states.Add(Define.BattleEntityState.Attack, new BattleEntityStates.Base.Attack());
                states.Add(Define.BattleEntityState.SkillCast, new BattleEntityStates.Base.SkillCast());
                states.Add(Define.BattleEntityState.Die, new BattleEntityStates.Base.Die());
                states.Add(Define.BattleEntityState.EndBattle, new BattleEntityStates.Base.EndBattle());

                data = Managers.Data.GetBattleEntityData((int)_entity, _level);
                battleEntity = new BattleEntites.Four(battleEntityController, data);
                break;
            case Define.BattleEntity.EnemyThree:
                states.Add(Define.BattleEntityState.Idle, new BattleEntityStates.Base.Idle());
                states.Add(Define.BattleEntityState.Move, new BattleEntityStates.Base.Move());
                states.Add(Define.BattleEntityState.Follow, new BattleEntityStates.Base.Follow());
                states.Add(Define.BattleEntityState.Attack, new BattleEntityStates.Base.Attack());
                states.Add(Define.BattleEntityState.SkillCast, new BattleEntityStates.Base.SkillCast());
                states.Add(Define.BattleEntityState.Die, new BattleEntityStates.Base.Die());
                states.Add(Define.BattleEntityState.EndBattle, new BattleEntityStates.Base.EndBattle());

                data = Managers.Data.GetBattleEntityData((int)_entity, _level);
                battleEntity = new BattleEntites.Five(battleEntityController, data);
                break;
        }

        //조합 및 초기화
        status = new BattleEntityStatus(battleEntityController, data.maxHP, data.maxHP, data.attackForce, data.skillCooltime, data.skillCooltime, data.moveSpeed, data.attackCycle);
        if (battleEntityController != null)
        {
            battleEntityController.Init(battleEntity, states, status, BattleEntityType.Army);
            Armys.Add(battleEntityController);
            return battleEntityController;        
        }

        //실패시 디버그
        Debug.Log("스폰을 실패하였습니다.");
        return null;
    }

    //적 전투 오브젝트 생성 후 초기화
    public BattleEntityController SpawnEnemyBattleEntity(Define.BattleEntity _entity, int _level, Vector2 _position = new Vector2())
    {
        //전투 오브젝트 공통 요소 생성
        BattleEntityController battleEntityController = Managers.Resource.Instantiate($"{_entity.ToString()}", _parent: EnemyBattleEntityTrnas).GetOrAddComponent<BattleEntityController>();
        battleEntityController.transform.position = _position;
        BattleEntity battleEntity = null;
        BattleEntityStatus status = null;
        BattleEntityData data = null;
        Dictionary<Define.BattleEntityState, State<BattleEntityController>> states = new Dictionary<BattleEntityState, State<BattleEntityController>>();

        //각 전투 오브젝트의 요소 생성
        switch (_entity)
        {
            case Define.BattleEntity.Warrior:
                states.Add(Define.BattleEntityState.Idle, new BattleEntityStates.Base.Idle());
                states.Add(Define.BattleEntityState.Move, new BattleEntityStates.Base.Move());
                states.Add(Define.BattleEntityState.Follow, new BattleEntityStates.Base.Follow());
                states.Add(Define.BattleEntityState.Attack, new BattleEntityStates.Base.Attack());
                states.Add(Define.BattleEntityState.SkillCast, new BattleEntityStates.Base.SkillCast());
                states.Add(Define.BattleEntityState.Die, new BattleEntityStates.Base.Die());
                states.Add(Define.BattleEntityState.EndBattle, new BattleEntityStates.Base.EndBattle());

                data = Managers.Data.GetBattleEntityData((int)_entity, _level);
                battleEntity = new BattleEntites.Warrior(battleEntityController, data);
                break;

            case Define.BattleEntity.Tank:
                states.Add(Define.BattleEntityState.Idle, new BattleEntityStates.Base.Idle());
                states.Add(Define.BattleEntityState.Move, new BattleEntityStates.Base.Move());
                states.Add(Define.BattleEntityState.Follow, new BattleEntityStates.Base.Follow());
                states.Add(Define.BattleEntityState.Attack, new BattleEntityStates.Base.Attack());
                states.Add(Define.BattleEntityState.SkillCast, new BattleEntityStates.Base.SkillCast());
                states.Add(Define.BattleEntityState.Die, new BattleEntityStates.Base.Die());
                states.Add(Define.BattleEntityState.EndBattle, new BattleEntityStates.Base.EndBattle());

                data = Managers.Data.GetBattleEntityData((int)_entity, _level);
                battleEntity = new BattleEntites.Tank(battleEntityController, data);
                break;

            case Define.BattleEntity.Wizard:
                states.Add(Define.BattleEntityState.Idle, new BattleEntityStates.Base.Idle());
                states.Add(Define.BattleEntityState.Move, new BattleEntityStates.Base.Move());
                states.Add(Define.BattleEntityState.Follow, new BattleEntityStates.Base.Follow());
                states.Add(Define.BattleEntityState.Attack, new BattleEntityStates.Base.Attack());
                states.Add(Define.BattleEntityState.SkillCast, new BattleEntityStates.Base.SkillCast());
                states.Add(Define.BattleEntityState.Die, new BattleEntityStates.Base.Die());
                states.Add(Define.BattleEntityState.EndBattle, new BattleEntityStates.Base.EndBattle());

                data = Managers.Data.GetBattleEntityData((int)_entity, _level);
                battleEntity = new BattleEntites.Wizard(battleEntityController, data);
                break;

            case Define.BattleEntity.EnemyOne:
                states.Add(Define.BattleEntityState.Idle, new BattleEntityStates.Base.Idle());
                states.Add(Define.BattleEntityState.Move, new BattleEntityStates.Base.Move());
                states.Add(Define.BattleEntityState.Follow, new BattleEntityStates.Base.Follow());
                states.Add(Define.BattleEntityState.Attack, new BattleEntityStates.Base.Attack());
                states.Add(Define.BattleEntityState.SkillCast, new BattleEntityStates.Base.SkillCast());
                states.Add(Define.BattleEntityState.Die, new BattleEntityStates.Base.Die());
                states.Add(Define.BattleEntityState.EndBattle, new BattleEntityStates.Base.EndBattle());

                data = Managers.Data.GetBattleEntityData((int)_entity, _level);
                battleEntity = new BattleEntites.Three(battleEntityController, data);
                break;

            case Define.BattleEntity.EnemyTwo:
                states.Add(Define.BattleEntityState.Idle, new BattleEntityStates.Base.Idle());
                states.Add(Define.BattleEntityState.Move, new BattleEntityStates.Base.Move());
                states.Add(Define.BattleEntityState.Follow, new BattleEntityStates.Base.Follow());
                states.Add(Define.BattleEntityState.Attack, new BattleEntityStates.Base.Attack());
                states.Add(Define.BattleEntityState.SkillCast, new BattleEntityStates.Base.SkillCast());
                states.Add(Define.BattleEntityState.Die, new BattleEntityStates.Base.Die());
                states.Add(Define.BattleEntityState.EndBattle, new BattleEntityStates.Base.EndBattle());

                data = Managers.Data.GetBattleEntityData((int)_entity, _level);
                battleEntity = new BattleEntites.Four(battleEntityController, data);
                break;
            case Define.BattleEntity.EnemyThree:
                states.Add(Define.BattleEntityState.Idle, new BattleEntityStates.Base.Idle());
                states.Add(Define.BattleEntityState.Move, new BattleEntityStates.Base.Move());
                states.Add(Define.BattleEntityState.Follow, new BattleEntityStates.Base.Follow());
                states.Add(Define.BattleEntityState.Attack, new BattleEntityStates.Base.Attack());
                states.Add(Define.BattleEntityState.SkillCast, new BattleEntityStates.Base.SkillCast());
                states.Add(Define.BattleEntityState.Die, new BattleEntityStates.Base.Die());
                states.Add(Define.BattleEntityState.EndBattle, new BattleEntityStates.Base.EndBattle());

                data = Managers.Data.GetBattleEntityData((int)_entity, _level);
                battleEntity = new BattleEntites.Five(battleEntityController, data);
                break;
        }

        //조합 및 초기화
        status = new BattleEntityStatus(battleEntityController,data.maxHP, data.maxHP, data.attackForce, data.skillCooltime, data.skillCooltime, data.moveSpeed, data.attackCycle);
        if (battleEntityController != null)
        {
            battleEntityController.Init(battleEntity, states, status, BattleEntityType.Enemy);
            Enemys.Add(battleEntityController);
            return battleEntityController;
        }

        //실패시 디버그
        Debug.Log("스폰을 실패하였습니다.");
        return null;
    }

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
        enemies.Add(controller);
        return controller;
    }
}
