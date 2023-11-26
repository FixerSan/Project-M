using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract class Enemy
{
    protected EnemyController controller;
    public List<Define.SpecialtyType> specialties;
    protected WaitForSeconds attackWaitForSeceonds;
    protected WaitForSeconds skillWaitForSeconds;


    public virtual void Follow()
        {
            if (controller.attackTarget != null)
            {
                if (Vector2.Distance(controller.attackTarget.transform.position, controller.transform.position) < controller.status.CurrentAttackDistance)
                    return;
                Vector2 dir = (controller.attackTarget.transform.position - controller.transform.position).normalized;
            if (dir.x <= 0) controller.ChangeDirection(Define.Direction.Left);
            else controller.ChangeDirection(Define.Direction.Right);
            controller.transform.Translate((dir * controller.status.CurrentMoveSpeed) * Time.deltaTime);
            }
        }

}

namespace Enemies
{
    public class TestEnemy : Enemy
    {
        public TestEnemy(EnemyController _controller)
        {
            controller = _controller;
            attackWaitForSeceonds = new WaitForSeconds(Define.attackAnimationTime);
            skillWaitForSeconds = new WaitForSeconds(Define.skillAnimationTime);
            specialties = new List<Define.SpecialtyType>();
        }
    }
}
