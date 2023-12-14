using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongAttack : MonoBehaviour
{
    private BaseController attacker;
    private BaseController hiter;
    private Vector3 dir;
    private Vector3 rotationVec;
    private Quaternion rotation;
    private float angle;
    private bool isUsing = false;
    private Transform spriteTrans;
    private float damage;

    public Vector3 hitPointOffset;
    public float moveSpeed;
    public float rotationSpeed;


    public void Attack(BaseController _attacker, BaseController _hiter, float _damage = -1)
    {
        spriteTrans = Util.FindChild<Transform>(gameObject, "Sprite", true);
        transform.localPosition = Vector3.zero;
        attacker = _attacker;
        hiter = _hiter;
        damage = _damage;
        isUsing = true;
    }

    public void Follow()
    {
        dir = (hiter.transform.position + hitPointOffset) - transform.position;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
        transform.rotation = rotation;
        if(rotationSpeed != 0)
        {
            rotationVec = spriteTrans.eulerAngles;
            rotationVec.z  -= rotationSpeed * Time.deltaTime; 
            spriteTrans.eulerAngles = rotationVec;
        }

        transform.position = Vector2.MoveTowards(transform.position, hiter.transform.position + hitPointOffset, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, hiter.transform.position + hitPointOffset) <= 0.1f)
            AttackEffect();
    }

    public void Update()
    {
        if (!isUsing) return;
        Follow();
    }

    public void AttackEffect()
    {
        Managers.Battle.AttackCalculation(attacker, hiter, damage);
        Managers.Resource.Destroy(gameObject);
    }
}
