using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    public ControllerStatus status;

    public abstract void Hit(float _damage);
    public abstract void GetDamage(float _damage);
}
