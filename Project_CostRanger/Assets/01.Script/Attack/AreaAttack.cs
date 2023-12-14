using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttack : MonoBehaviour
{
    private string[] layer;

    public void Awake()
    {
        layer = new string[1];
        layer[0] = "BattleEntity";
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    public void Attack(BaseController _attacker, Define.BattleEntityType _attackerType, float _damage)
    {
        if(_attackerType == Define.BattleEntityType.Ranger)
        {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0, LayerMask.GetMask(layer));
            for (int i = 0; i < collider2Ds.Length; i++)
            {
                EnemyController enemy = collider2Ds[i].GetComponent<EnemyController>();
                if (enemy != null)
                    Managers.Battle.AttackCalculation(_attacker, enemy, _damage);
            }
        }

        else if (_attackerType == Define.BattleEntityType.Enemy)
        {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0, LayerMask.GetMask(layer));
            for (int i = 0; i < collider2Ds.Length; i++)
            {
                RangerController ranger = collider2Ds[i].GetComponent<RangerController>();
                if (ranger != null)
                    Managers.Battle.AttackCalculation(_attacker, ranger, _damage);
            }
        }
    }
}
