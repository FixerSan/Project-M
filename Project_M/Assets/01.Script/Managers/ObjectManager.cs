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

}
