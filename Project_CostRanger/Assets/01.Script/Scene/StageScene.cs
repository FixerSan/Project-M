using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StageScene : BaseScene
{
    private WaitForSeconds sceneStartDelay;
    private Dictionary<int, Transform> enemySpawnTranses;
    
    public override void Init(Action _callback)
    {
        Managers.UI.ShowSceneUI<UIScene_Stage>();

        Managers.Object.Rangers.Clear();
        Managers.Object.Enemies.Clear();
        sceneStartDelay = new WaitForSeconds(Define.sceneStartDelay);
        enemySpawnTranses = new Dictionary<int, Transform>();

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
        //�� ��Ʈ�ѷ� ���� ��ġ ĳ��
        GameObject go = GameObject.Find("EnemySpawnTransforms");
        if(go == null)
        {
            Debug.Log("�� ���� ��ġ�� �������� ����");
            return;
        }
        string[] stringArray = Enum.GetNames(typeof(EnemyTrans)); 
        for (int i = 0; i < stringArray.Length; i++)
            enemySpawnTranses.Add(i, go.transform.Find(stringArray[i]));
        
        //�� ��Ʈ�ѷ� ����
        string[] enemyUIDArray = Managers.Game.battleStageSystem.currentStageData.enemyUIDs.Split(",");
        for (int i = 0; i < enemyUIDArray.Length; i++)
            if (Int32.TryParse(enemyUIDArray[i], out int enemyUID))
                Managers.Object.SpawnEnemy(enemyUID, enemySpawnTranses[i].position);


        StartCoroutine(SceneEventZeroRoutine());
    }

    private IEnumerator SceneEventZeroRoutine()
    {
        yield return sceneStartDelay;
        Managers.Game.battleStageSystem.StartStage();
    }

    public override void Clear()
    {

    }


    private enum EnemyTrans
    {
        Trans_EnemyOne = 0,
        trans_EnemyTwo = 1,
        trans_EnemyThree = 2,
        trans_EnemyFour = 3,
        trans_EnemyFive = 4,
        trans_EnemySix = 5,
        trans_EnemySeven = 6,
        trans_EnemyEight = 7,
        trans_EnemyNine = 8,
    }
}
