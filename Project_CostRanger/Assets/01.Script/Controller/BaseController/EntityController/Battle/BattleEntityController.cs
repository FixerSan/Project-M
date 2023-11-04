using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Rendering;
using static Define;

public class BattleEntityController : EntityController, IAttackable, IHittable
{
    //각 파츠
    public BattleEntity entity;
    public StateMachine<BattleEntityController> stateMachine;
    public BattleEntityStatus status;
    public Dictionary<BattleEntityState, State<BattleEntityController>> states;
    public Rigidbody2D rb;

    //컨트롤러 상태
    public BattleEntityType entityType;
    public BattleEntityState state;
    public bool isDead;
    private bool init = false;
    public int mvpPoint;
    
    //기타 처리 용도 변수
    public Dictionary<string , Coroutine> routines;
    public BattleEntityController attackTarget;
    public Transform moveTarget;
    public Vector3 textOffset;

    //초기화
    public void Init(BattleEntity _entity, Dictionary<BattleEntityState, State<BattleEntityController>> _states, BattleEntityStatus _status, BattleEntityType _entityType)
    {
        //파츠 조립
        if (init) return;
        entity = _entity;
        states = _states;
        status = _status;
        entityType = _entityType;
        state = BattleEntityState.Idle;
        rb = gameObject.GetOrAddComponent<Rigidbody2D>();
        routines = new Dictionary<string, Coroutine>();
        stateMachine = new StateMachine<BattleEntityController>(this, states[BattleEntityState.Idle]);
        UIHPBar hpBar =  Managers.Resource.Instantiate("UIHPbar", _pooling:true).GetOrAddComponent<UIHPBar>();
        
        //초기화
        mvpPoint = 0;
        hpBar.Init(this);
        isDead = false;
        init = true;
    }

    //공격 처리
    public void Attack()
    {
        if (isDead) return;
        entity.Attack();
    }

    //데미지 처리
    public void GetDamage(int _damage)
    {
        if (isDead) return;
        entity.GetDamage(_damage);
    }

    //피격 처리
    public void Hit(int _damage)
    {
        if (isDead) return;
        entity.Hit(_damage);
    }
    
    //스킬 처리
    public void Skill()
    {
        if (isDead) return;
        entity.Skill();
    }

    //죽음 처리
    public void Die()
    {
        if (isDead) return;
        isDead = true;
        StopAllRoutine();
    }

    /// <summary>
    /// StopAllCoroutine이 작동이 안되는 것 같아서 새로 만든 기능
    /// 모든 코루틴 종료
    /// </summary>
    public void StopAllRoutine()
    {
        foreach (var routine in routines)
            StopCoroutine(routine.Value);
        routines.Clear();
    }

    //정지
    public void Stop()
    {
        rb.velocity = Vector3.zero;
    }

    //공격 대상 찾기
    public void FindTarget()
    {
        //초기화
        BattleEntityController tempTrans = null;
        float minDistace = 10000000;
        float currentDistance = 0;
        List<BattleEntityController> tempHashSet = null; ;

        //컨트롤러의 타입에 따른 적 구별 및 제일 가까운 적 찾기
        if (entityType == BattleEntityType.Army) tempHashSet = Managers.Object.Enemys;
        else tempHashSet = Managers.Object.Armys;
        foreach (var item in tempHashSet)
        {
            if(item.isDead) continue;
            currentDistance = Vector2.Distance(transform.position, item.transform.position);
            if (currentDistance < minDistace)
            {
                minDistace = currentDistance;
                tempTrans = item;
            }
        }
        attackTarget = tempTrans;
    }
    
    //타겟 설정
    public void SetMoveTarget(Transform _target)
    {
        moveTarget = _target;
    }

    /// <summary>
    /// 같은 상태일 경우 같은 상태여도 bool가 _isChangeSameState에 따라 처리할지 여부가 결정 
    /// </summary>
    /// <param name="_nextState">변경할 상태</param>
    /// <param name="_isChangeSameState">같은 상태여도 변경을 할 것인지</param>
    public void ChangeState(BattleEntityState _nextState , bool _isChangeSameState = false)
    {
        if (!init) return;
        if (state == _nextState)
        {
            if (_isChangeSameState)
                stateMachine.ChangeState(states[_nextState]);
            return;
        }
        state = _nextState;
        stateMachine.ChangeState(states[_nextState]);
    }

    //지연을 가진 후 상태 변경
    public void ChangeStateWithDelay(BattleEntityState _nextState, float _delay,bool _isChangeSameState = false)
    {
        if (!init) return;
        routines.Add("ChangeStateWithDelay", StartCoroutine(ChangeStateWithDelayRoutine(_nextState, _delay, _isChangeSameState)));
    }

    //지연을 가진 상태 변경 처리
    private IEnumerator ChangeStateWithDelayRoutine(BattleEntityState _nextState, float _delay, bool _isChangeSameState = false)
    {
        yield return new WaitForSeconds(_delay);
        ChangeState(_nextState, _isChangeSameState);
        routines.Remove("ChangeStateWithDelay");
    }

    //체력 회복 처리
    public void Heal(int _healValue)
    {
        if (status.CurrentHP >= status.maxHP)
            return;
        status.CurrentHP += _healValue;
        Managers.UI.MakeWorldText($"+ {_healValue}", transform.position + textOffset, TextType.Heal);
    }

    //공격속도 업 처리
    public void SetBuff_PlusSpeed(float _time, float _plusAttackSpeed)
    {
        status.buff.StartPlusAttackSpeed(_time, _plusAttackSpeed);
        Managers.UI.MakeWorldText($"AttackSpeed + 25%", transform.position + textOffset, TextType.Normal);
    }

    //방어 카운트 증가 처리
    public void SetBuff_SetMissCount(int _count)
    {
        status.buff.SetMissCount(_count);
    }

    public void Update()
    {
        if (!init) return;
        stateMachine.UpdateState();
        status.buff.CheckBuff();
    }

    public void OnDisable()
    {
        StopAllRoutine();
    }
}


