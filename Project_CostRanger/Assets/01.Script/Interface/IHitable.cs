using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittable
{
    public abstract void Hit(int _damage);
    public abstract void GetDamage(int _damage);
}
