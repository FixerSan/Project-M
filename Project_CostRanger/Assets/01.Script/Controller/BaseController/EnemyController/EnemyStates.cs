using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyStates 
{
    namespace Base
    {
        public class Stay : State<EnemyController>
        {
            public override void EnterState(EnemyController _entity)
            {

            }

            public override void ExitState(EnemyController _entity)
            {

            }

            public override void UpdateState(EnemyController _entity)
            {

            }
        }

        public class Idle : State<EnemyController>
        {
            public override void EnterState(EnemyController _entity)
            {

            }

            public override void ExitState(EnemyController _entity)
            {

            }

            public override void UpdateState(EnemyController _entity)
            {
                if (_entity.enemy.CheckCanUseSkill()) return;
                if (_entity.enemy.CheckAttack()) return;
                if (_entity.enemy.CheckFollow()) return;
                _entity.enemy.CheckAttackCooltime();
                _entity.enemy.CheckSkillCooltime();
            }
        }

        public class Move : State<EnemyController>
        {
            public override void EnterState(EnemyController _entity)
            {

            }

            public override void ExitState(EnemyController _entity)
            {

            }

            public override void UpdateState(EnemyController _entity)
            {
                _entity.Follow();
            }
        }

        public class Follow : State<EnemyController>
        {
            public override void EnterState(EnemyController _entity)
            {

            }

            public override void ExitState(EnemyController _entity)
            {

            }

            public override void UpdateState(EnemyController _entity)
            {
                if (_entity.enemy.CheckCanUseSkill()) return;
                if (_entity.enemy.CheckAttack()) return;
                _entity.enemy.CheckAttackCooltime();
                _entity.enemy.CheckSkillCooltime();
                if (_entity.enemy.CheckStop()) return;
                _entity.Follow();
            }
        }

        public class Attack : State<EnemyController>
        {
            public override void EnterState(EnemyController _entity)
            {
                _entity.enemy.Attack();
            }

            public override void ExitState(EnemyController _entity)
            {

            }

            public override void UpdateState(EnemyController _entity)
            {
                _entity.enemy.CheckSkillCooltime();
            }
        }

        public class SkillCast : State<EnemyController>
        {
            public override void EnterState(EnemyController _entity)
            {
                _entity.enemy.Skill();
            }

            public override void ExitState(EnemyController _entity)
            {

            }

            public override void UpdateState(EnemyController _entity)
            {
                _entity.enemy.CheckAttackCooltime();
            }
        }

        public class Die : State<EnemyController>
        {
            public override void EnterState(EnemyController _entity)
            {
                _entity.Die();
            }

            public override void ExitState(EnemyController _entity)
            {

            }

            public override void UpdateState(EnemyController _entity)
            {

            }
        }

        public class EndBattle : State<EnemyController>
        {
            public override void EnterState(EnemyController _entity)
            {

            }

            public override void ExitState(EnemyController _entity)
            {

            }

            public override void UpdateState(EnemyController _entity)
            {

            }
        }
    }
}
