using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RangerStates
{
    namespace Base
    {
        public class Idle : State<RangerController>
        {
            public override void EnterState(RangerController _entity)
            {
                Debug.Log("¾ÆÀÌµé µé¾î¿È");
            }

            public override void ExitState(RangerController _entity)
            {

            }

            public override void UpdateState(RangerController _entity)
            {

            }
        }

        public class Move : State<RangerController>
        {
            public override void EnterState(RangerController _entity)
            {
                Debug.Log("¹«ºê µé¾î¿È");
            }

            public override void ExitState(RangerController _entity)
            {

            }

            public override void UpdateState(RangerController _entity)
            {

            }
        }

        public class Follow : State<RangerController>
        {
            public override void EnterState(RangerController _entity)
            {
                Debug.Log("ÆÈ·Î¿ì µé¾î¿È");
            }

            public override void ExitState(RangerController _entity)
            {

            }

            public override void UpdateState(RangerController _entity)
            {

            }
        }

        public class Attack : State<RangerController>
        {
            public override void EnterState(RangerController _entity)
            {

            }

            public override void ExitState(RangerController _entity)
            {

            }

            public override void UpdateState(RangerController _entity)
            {

            }
        }

        public class SkillCast : State<RangerController>
        {
            public override void EnterState(RangerController _entity)
            {

            }

            public override void ExitState(RangerController _entity)
            {

            }

            public override void UpdateState(RangerController _entity)
            {

            }
        }

        public class Die : State<RangerController>
        {
            public override void EnterState(RangerController _entity)
            {

            }

            public override void ExitState(RangerController _entity)
            {

            }

            public override void UpdateState(RangerController _entity)
            {

            }
        }

        public class EndBattle : State<RangerController>
        {
            public override void EnterState(RangerController _entity)
            {

            }

            public override void ExitState(RangerController _entity)
            {

            }

            public override void UpdateState(RangerController _entity)
            {

            }
        }

    }

    namespace TestMosnter
    {

    }
}
