using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BattleEntityStates 
{
    namespace Base
    {
        public class Idle : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {
                _controller.Stop();
            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class Move : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {
                _controller.entity.Move();
                if (_controller.entity.CheckStop()) return;
            }
        }

        public class Follow : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {
                _controller.FindTarget();
            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

                if (_controller.entity.CheckCanUseSkill()) return;
                _controller.entity.Follow();
                if (_controller.entity.CheckAttack()) return;
            }
        }

        public class Attack : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {
                _controller.Stop();
                _controller.Attack();
            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {
                if (_controller.entity.CheckCanUseSkill()) return;
            }
        }

        public class SkillCast : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {
                _controller.Stop();
                _controller.Skill();
            }

            public override void ExitState(BattleEntityController _controller)
            {
                _controller.battleEntityStatus.currentSkillCooltime = _controller.entity.data.skillCooltime;
            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class Die : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {
                _controller.Die();
            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }
        public class EndBattle : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {
                _controller.StopAllRoutine();
            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }
    }

    namespace Zero
    {
        public class Idle : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class Move : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class Follow : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class Attack : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {
                _controller.Stop();
            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class SkillCast : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class Die : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class EndBattle : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }
    }

    namespace One
    {
        public class Idle : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class Move : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class Attack : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {
                _controller.Stop();

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class SkillCast : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class Die : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class EndBattle : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }
    }
    namespace Two
    {
        public class Idle : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class Move : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class Attack : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {
                _controller.Stop();

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class SkillCast : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class Die : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class EndBattle : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }
    }
    namespace Three
    {
        public class Idle : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class Move : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class Attack : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {
                _controller.Stop();
            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class SkillCast : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class Die : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class EndBattle : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }
    }
    namespace Four
    {
        public class Idle : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {
                Debug.Log("Idle 들어옴");
            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {
                Debug.Log("Idle 업데이트중");
            }
        }

        public class Move : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class Follow : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class Attack : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {
                _controller.Stop();

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class SkillCast : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class Die : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class EndBattle : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }
    }
    namespace Five
    {
        public class Idle : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {
                Debug.Log("Idle 들어옴");
            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {
                Debug.Log("Idle 업데이트중");
            }
        }

        public class Move : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class Follow : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class Attack : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {
                _controller.Stop();

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class SkillCast : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class Die : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }

        public class EndBattle : State<BattleEntityController>
        {
            public override void EnterState(BattleEntityController _controller)
            {

            }

            public override void ExitState(BattleEntityController _controller)
            {

            }

            public override void UpdateState(BattleEntityController _controller)
            {

            }
        }
    }

}
