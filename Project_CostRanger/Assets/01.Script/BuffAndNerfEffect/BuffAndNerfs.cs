using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffAndNerfEffect 
{
    protected BaseController controller;
    public abstract void EffectOn();
    public abstract void EffectOff();
    public IEnumerator EffectOffRoutine(float _effectDuration)
    {
        yield return new WaitForSeconds(_effectDuration);
        EffectOff();
    }
}

namespace BuffsAndNerfs
{
    public class BoringSpearmanSkill : BuffAndNerfEffect
    {
        public BoringSpearmanSkill(BaseController _controller, float _effectDuration)
        {
            controller = _controller;
            EffectOn();
            _controller.routines.Add(typeof(BoringSpearmanSkill).Name, _controller.StartCoroutine(EffectOffRoutine(_effectDuration)));
        }
        public override void EffectOn()
        {
            controller.status.PlusAttackSpeed += -0.35f;
            controller.isSkillUsing = true;
        }
        public override void EffectOff()
        {
            controller.status.PlusAttackSpeed -= -0.35f;
            controller.RemoveBuffAndNerf(this);
            controller.routines.Remove(typeof(BoringSpearmanSkill).Name);
            controller.isSkillUsing = false;
        }
    }

    public class DullAxemanSkill : BuffAndNerfEffect
    {
        public DullAxemanSkill(BaseController _controller, float _effectDuration)
        {
            controller = _controller;
            EffectOn();
            _controller.routines.Add(typeof(DullAxemanSkill).Name, _controller.StartCoroutine(EffectOffRoutine(_effectDuration)));
        }
        public override void EffectOn()
        {
            controller.status.CurrentCriticalProbability += 5;
            controller.isSkillUsing = true;
        }

        public override void EffectOff()
        {
            controller.status.CurrentCriticalProbability -= 5;
            controller.RemoveBuffAndNerf(this);
            controller.routines.Remove(typeof(DullAxemanSkill).Name);
            controller.isSkillUsing = false;
        }
    }
    public class StrangeAssassinSkill : BuffAndNerfEffect
    {
        public StrangeAssassinSkill(BaseController _controller, float _effectDuration)
        {
            controller = _controller;
            EffectOn();
            _controller.routines.Add(typeof(StrangeAssassinSkill).Name, _controller.StartCoroutine(EffectOffRoutine(_effectDuration)));
        }
        public override void EffectOn()
        {
            controller.status.plustAttackForce += controller.status.defaultAttackForce * 1.5f;
            controller.isSkillUsing = true;
        }

        public override void EffectOff()
        {
            controller.status.plustAttackForce -= controller.status.defaultAttackForce * 1.5f;
            controller.RemoveBuffAndNerf(this);
            controller.routines.Remove(typeof(StrangeAssassinSkill).Name);
            controller.isSkillUsing = false;
        }
    }

    public class ScaredThugSkill : BuffAndNerfEffect
    {
        public ScaredThugSkill(BaseController _controller, float _effectDuration)
        {
            controller = _controller;
            EffectOn();
            _controller.routines.Add(typeof(StrangeAssassinSkill).Name, _controller.StartCoroutine(EffectOffRoutine(_effectDuration)));
        }

        public override void EffectOn()
        {
            controller.isSkillUsing = true;
        }

        public override void EffectOff()
        {
            controller.RemoveBuffAndNerf(this);
            controller.routines.Remove(typeof(StrangeAssassinSkill).Name);
            controller.isSkillUsing = false;
        }
    }

    public class LenientNinjaSkill : BuffAndNerfEffect
    {
        public LenientNinjaSkill(BaseController _controller, float _effectDuration)
        {
            controller = _controller;
            EffectOn();
            _controller.routines.Add(typeof(StrangeAssassinSkill).Name, _controller.StartCoroutine(EffectOffRoutine(_effectDuration)));
        }

        public override void EffectOn()
        {
            controller.status.CurrentCriticalForce += 1;
            controller.isSkillUsing = true;
        }

        public override void EffectOff()
        {
            controller.RemoveBuffAndNerf(this);
            controller.routines.Remove(typeof(StrangeAssassinSkill).Name);
            controller.status.CurrentCriticalForce -= 1;
            controller.isSkillUsing = false;
        }
    }
}
