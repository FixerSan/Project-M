using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StageScene : BaseScene
{
    private WaitForSeconds sceneStartDelay;
    private Dictionary<int, Transform> enemySpawnTranses;
    private Dictionary<int, Transform> rangerSpawnTranses;
    
    public override void Init(Action _callback)
    {
        Managers.UI.ShowSceneUI<UIScene_Stage>();

        Managers.Object.Rangers.Clear();
        Managers.Object.Enemies.Clear();
        sceneStartDelay = new WaitForSeconds(Define.sceneStartDelay);
        enemySpawnTranses = new Dictionary<int, Transform>();
        rangerSpawnTranses= new Dictionary<int, Transform>();

        SceneEvent(0);
    }


    public override void SceneEvent(int _eventIndex, Action _callback = null)
    {
        switch (_eventIndex)
        {
            case 0:
                SceneEventZero();
                break;
        }
        _callback?.Invoke();
    }

    private void SceneEventZero()
    {
        Transform transforms = GameObject.Find("EnemySpawnTransforms").transform;
        string[] stringArray = Enum.GetNames(typeof(EnemyTrans)); 

        //적 컨트롤러 생성 위치 캐싱
        if(transforms == null)
        {
            Debug.Log("적 생성 위치가 존재하지 않음");
            return;
        }
        for (int i = 0; i < stringArray.Length; i++)
            enemySpawnTranses.Add(i, transforms.Find(stringArray[i]));

        //레인저 컨트롤러 생성 위치 캐싱
        switch(Managers.Game.battleStageSystem.batch)
        {
            case Define.Batch.One:
                transforms = GameObject.Find("RangerSpawnTransforms_BatchOne").transform;
                break;
            case Define.Batch.Two:
                transforms = GameObject.Find("RangerSpawnTransforms_BatchTwo").transform;
                break;
        }

        stringArray = Enum.GetNames(typeof(RangerTrans));
        for (int i = 0; i < stringArray.Length; i++)
            rangerSpawnTranses.Add(i, transforms.Find(stringArray[i]));

        //레인저 컨트롤러 생성
        for (int i = 0; i < Managers.Game.battleStageSystem.rangerControllerData.Length; i++)
            if (Managers.Game.battleStageSystem.rangerControllerData[i] != null)
                Managers.Object.SpawnRanger(Managers.Game.battleStageSystem.rangerControllerData[i].UID, rangerSpawnTranses[i].position);

        //적 컨트롤러 생성
        string[] enemyUIDArray = Managers.Game.battleStageSystem.currentStageData.enemyUIDs.Split(",");
        for (int i = 0; i < enemyUIDArray.Length; i++)
            if (Int32.TryParse(enemyUIDArray[i], out int enemyUID))
                Managers.Object.SpawnEnemy(enemyUID, enemySpawnTranses[i].position);


        for (int i = 0; i < Managers.Object.Rangers.Count; i++)
            Managers.Object.Rangers[i].ChangeDirection(Define.Direction.Right);
        StartCoroutine(SceneEventZeroRoutine());
    }

    private IEnumerator SceneEventZeroRoutine()
    {
        yield return sceneStartDelay;
        Managers.Game.battleStageSystem.StartStage();
        for (int i = 0; i < Managers.Object.Rangers.Count; i++)
            Managers.Object.Rangers[i].ChangeState(Define.RangerState.Idle);
    }

    public override void Clear()
    {

    }

    private enum RangerTrans
    {
        Trans_RangerOne,
        Trans_RangerTwo,
        Trans_RangerThree,
        Trans_RangerFour,
        Trans_RangerFive,
        Trans_RangerSix
    }

    private enum EnemyTrans
    {
        Trans_EnemyOne = 0,
        Trans_EnemyTwo = 1,
        Trans_EnemyThree = 2,
        Trans_EnemyFour = 3,
        Trans_EnemyFive = 4,
        Trans_EnemySix = 5,
        Trans_EnemySeven = 6,
        Trans_EnemyEight = 7,
        Trans_EnemyNine = 8,
    }
}
