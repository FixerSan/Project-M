using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class TestController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
           EnemyController controller =  Managers.Object.SpawnEnemy(0, transform.position);
            controller.SetAttackTarget(gameObject.GetOrAddComponent<RangerController>());
            controller.ChangeState(Define.EnemyState.Move);
        }
    }
}