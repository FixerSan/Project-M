using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyStates 
{
    namespace Base
    {
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
                _entity.Follow();
            }
        }

        public class Attack : State<EnemyController>
        {
            public override void EnterState(EnemyController _entity)
            {
                throw new System.NotImplementedException();
            }

            public override void ExitState(EnemyController _entity)
            {
                throw new System.NotImplementedException();
            }

            public override void UpdateState(EnemyController _entity)
            {
                throw new System.NotImplementedException();
            }
        }

        public class SkillCast : State<EnemyController>
        {
            public override void EnterState(EnemyController _entity)
            {
                throw new System.NotImplementedException();
            }

            public override void ExitState(EnemyController _entity)
            {
                throw new System.NotImplementedException();
            }

            public override void UpdateState(EnemyController _entity)
            {
                throw new System.NotImplementedException();
            }
        }

        public class Die : State<EnemyController>
        {
            public override void EnterState(EnemyController _entity)
            {
                throw new System.NotImplementedException();
            }

            public override void ExitState(EnemyController _entity)
            {
                throw new System.NotImplementedException();
            }

            public override void UpdateState(EnemyController _entity)
            {
                throw new System.NotImplementedException();
            }
        }

        public class EndBattle : State<EnemyController>
        {
            public override void EnterState(EnemyController _entity)
            {
                throw new System.NotImplementedException();
            }

            public override void ExitState(EnemyController _entity)
            {
                throw new System.NotImplementedException();
            }

            public override void UpdateState(EnemyController _entity)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
