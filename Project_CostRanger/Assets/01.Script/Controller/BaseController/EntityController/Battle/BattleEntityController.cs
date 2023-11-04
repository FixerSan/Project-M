using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Rendering;
using static Define;

public class BattleEntityController : EntityController, IAttackable, IHittable
{
    //�� ����
    public BattleEntity entity;
    public StateMachine<BattleEntityController> stateMachine;
    public BattleEntityStatus status;
    public Dictionary<BattleEntityState, State<BattleEntityController>> states;
    public Rigidbody2D rb;

    //��Ʈ�ѷ� ����
    public BattleEntityType entityType;
    public BattleEntityState state;
    public bool isDead;
    private bool init = false;
    public int mvpPoint;
    
    //��Ÿ ó�� �뵵 ����
    public Dictionary<string , Coroutine> routines;
    public BattleEntityController attackTarget;
    public Transform moveTarget;
    public Vector3 textOffset;

    //�ʱ�ȭ
    public void Init(BattleEntity _entity, Dictionary<BattleEntityState, State<BattleEntityController>> _states, BattleEntityStatus _status, BattleEntityType _entityType)
    {
        //���� ����
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
        
        //�ʱ�ȭ
        mvpPoint = 0;
        hpBar.Init(this);
        isDead = false;
        init = true;
    }

    //���� ó��
    public void Attack()
    {
        if (isDead) return;
        entity.Attack();
    }

    //������ ó��
    public void GetDamage(int _damage)
    {
        if (isDead) return;
        entity.GetDamage(_damage);
    }

    //�ǰ� ó��
    public void Hit(int _damage)
    {
        if (isDead) return;
        entity.Hit(_damage);
    }
    
    //��ų ó��
    public void Skill()
    {
        if (isDead) return;
        entity.Skill();
    }

    //���� ó��
    public void Die()
    {
        if (isDead) return;
        isDead = true;
        StopAllRoutine();
    }

    /// <summary>
    /// StopAllCoroutine�� �۵��� �ȵǴ� �� ���Ƽ� ���� ���� ���
    /// ��� �ڷ�ƾ ����
    /// </summary>
    public void StopAllRoutine()
    {
        foreach (var routine in routines)
            StopCoroutine(routine.Value);
        routines.Clear();
    }

    //����
    public void Stop()
    {
        rb.velocity = Vector3.zero;
    }

    //���� ��� ã��
    public void FindTarget()
    {
        //�ʱ�ȭ
        BattleEntityController tempTrans = null;
        float minDistace = 10000000;
        float currentDistance = 0;
        List<BattleEntityController> tempHashSet = null; ;

        //��Ʈ�ѷ��� Ÿ�Կ� ���� �� ���� �� ���� ����� �� ã��
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
    
    //Ÿ�� ����
    public void SetMoveTarget(Transform _target)
    {
        moveTarget = _target;
    }

    /// <summary>
    /// ���� ������ ��� ���� ���¿��� bool�� _isChangeSameState�� ���� ó������ ���ΰ� ���� 
    /// </summary>
    /// <param name="_nextState">������ ����</param>
    /// <param name="_isChangeSameState">���� ���¿��� ������ �� ������</param>
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

    //������ ���� �� ���� ����
    public void ChangeStateWithDelay(BattleEntityState _nextState, float _delay,bool _isChangeSameState = false)
    {
        if (!init) return;
        routines.Add("ChangeStateWithDelay", StartCoroutine(ChangeStateWithDelayRoutine(_nextState, _delay, _isChangeSameState)));
    }

    //������ ���� ���� ���� ó��
    private IEnumerator ChangeStateWithDelayRoutine(BattleEntityState _nextState, float _delay, bool _isChangeSameState = false)
    {
        yield return new WaitForSeconds(_delay);
        ChangeState(_nextState, _isChangeSameState);
        routines.Remove("ChangeStateWithDelay");
    }

    //ü�� ȸ�� ó��
    public void Heal(int _healValue)
    {
        if (status.CurrentHP >= status.maxHP)
            return;
        status.CurrentHP += _healValue;
        Managers.UI.MakeWorldText($"+ {_healValue}", transform.position + textOffset, TextType.Heal);
    }

    //���ݼӵ� �� ó��
    public void SetBuff_PlusSpeed(float _time, float _plusAttackSpeed)
    {
        status.buff.StartPlusAttackSpeed(_time, _plusAttackSpeed);
        Managers.UI.MakeWorldText($"AttackSpeed + 25%", transform.position + textOffset, TextType.Normal);
    }

    //��� ī��Ʈ ���� ó��
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


