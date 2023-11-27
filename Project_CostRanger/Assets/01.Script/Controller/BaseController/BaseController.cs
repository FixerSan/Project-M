using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    public ControllerStatus status;
    public Dictionary<string, Coroutine> routines;
    public Define.Direction direction;

    public abstract void Hit(float _damage);
    public abstract void GetDamage(float _damage);

    public void SetPosition(Vector3 _position)
    {
        transform.position = _position;
    }

    public void ChangeDirection(Define.Direction _direction)
    {
        if (direction == _direction) return;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        direction = _direction;
    }

    public abstract void CheckDie();
    public abstract void Die();
}

public abstract class ControllerStatus
{
    protected BaseController controller;
    //���ݷ�
    public float defaultAttackForce;
    protected float currentAttackForce;
    public float CurrentAttackForce
    {
        get
        {
            return currentAttackForce;
        }

        set
        {
            currentAttackForce = value;
        }
    }

    //���ݼӵ�
    public float defaultAttackSpeed;
    protected float currentAttackSpeed;
    public float CurrentAttackSpeed
    {
        get
        {
            return currentAttackSpeed;
        }

        set
        {
            currentAttackSpeed = value;
        }
    }

    //���� ��Ÿ�
    public float defaultAttackDistance;
    protected float currentAttackDistance;
    public float CurrentAttackDistance
    {
        get
        {
            return currentAttackDistance;
        }

        set
        {
            currentAttackDistance = value;
        }
    }

    //ġ��Ÿ ����
    public float defaultCriticalForce;
    protected float currentCriticalForce;
    public float CurrentCriticalForce
    {
        get
        {
            return currentCriticalForce;
        }

        set
        {
            currentCriticalForce = value;
        }
    }

    //ġ��Ÿ Ȯ��
    public float defaultCriticalProbability;
    protected float currentCriticalProbability;
    public float CurrentCriticalProbability
    {
        get
        {
            return currentCriticalProbability;
        }

        set
        {
            currentCriticalProbability = value;
        }
    }

    //����
    public float defaultDefenseForce;
    protected float currentDefenseForce;
    public float CurrentDefenseForce
    {
        get
        {
            return currentDefenseForce;
        }

        set
        {
            currentDefenseForce = value;
        }
    }

    //ü��
    public float defaultHP;
    protected float curretnMaxHP;
    public float CurrentMaxHP
    {
        get
        {
            return curretnMaxHP;
        }

        set
        {
            curretnMaxHP = value;
        }
    }
    protected float currentHP;
    public float CurrentHP
    {
        get
        {
            return currentHP;
        }

        set
        {
            if (currentHP == 0) return;
            currentHP = value;
            if (currentHP <= 0)
                currentHP = 0;
            controller.CheckDie();
        }
    }

    //�̵��ӵ�
    public float defaultMoveSpeed;
    protected float currentMoveSpeed;
    public float CurrentMoveSpeed
    {
        get
        {
            return currentMoveSpeed;
        }

        set
        {
            currentMoveSpeed = value;
        }
    }

    //��ų ��Ÿ��
    public float defaultSkillCooltime;
    protected float currentSkillCooltime;
    public float CurrentSkillCooltime
    {
        get
        {
            return currentSkillCooltime;
        }

        set
        {
            currentSkillCooltime = value;
        }
    }

    protected float checkAttackCooltime;
    public float CheckAttackCooltime 
    {
        get
        {
            return checkAttackCooltime;
        }

        set
        {
            checkAttackCooltime = value;
        }
    }
    protected float checkSkillCooltime;
    public float CheckSkillCooltime
    {
        get
        {
            return checkSkillCooltime;
        }

        set
        {
            checkSkillCooltime = value;
        }
    }
}
