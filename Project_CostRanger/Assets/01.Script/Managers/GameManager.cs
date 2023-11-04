using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Define;

public class GameManager : Singleton<GameManager>
{
    //���� ���� ������ �� ����
    public GameState state;
    public BattleInfo battleInfo;

    public void Awake()
    {
        Managers.Resource.LoadAllAsync<UnityEngine.Object>("Preload", _completeCallback: () => 
        {
            StartGame(() => 
            {
                state = GameState.BattleBefore;
                battleInfo = new BattleInfo();
            });
        });
    }

    //���� ����
    public void StartGame(Action _callback)
    {
        Managers.Data.LoadPreData(() => { Managers.Scene.LoadScene(Define.Scene.Main); _callback?.Invoke(); });
    }

    //���� ����
    public void SaveGame()
    {
        Managers.Data.SavePlayerData(Managers.Data.playerData);
    }

    private void Update()
    {
        if(battleInfo != null)
            battleInfo.Update();
    }

    public void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveGame();
    }
}

[System.Serializable]
public class BattleInfo
{
    //���� �������� �������� ������
    public StageData currentStage;
    public int clearStar;

    //���� �������� ���������� ��ġ�� �÷��̾� entity  �� ��ġ
    public BattleEntityData[] armyFront;
    public BattleEntityData[] armyCenter;
    public BattleEntityData[] armyRear;

    //���� �������� ���������� �÷��̾��� ����
    public int isCanUseBattleEntityCount;
    public int nowUseBattleEntityCount;
    public int armyCurrentHP;
    public int armyMaxHP;
    public int armyAttackForce;
    public int armybattleForce;
    public int allBattlePoint;

    //�������� ���� ���ĵ� �÷��̾� ��ƼƼ��
    public List<BattleEntityController> battleMVPPoints;

    //���� �������� ���������� ��ġ�� �� entity  �� ��ġ
    public BattleEntityData[] enemyFront;
    public BattleEntityData[] enemyCenter;
    public BattleEntityData[] enemyRear;

    //���� �������� ���������� ���� ����
    public int nowEnemyCount;
    public int enemyCurrentHP;
    public int enemyMaxHP;
    public int enemyAttackForce;
    public int enemybattleForce;

    //���� ���� ���� �� ����
    public bool isAutoSkill;
    public bool isFastSpeed;
    public float time;


    //�÷��̾� ��ƼƼ ��ġ
    public bool UseBattleEntity(BattleEntityData _data)
    {
        //�� ��ġ�� �� �ִ��� üũ
        if (nowUseBattleEntityCount == isCanUseBattleEntityCount) return false;
        nowUseBattleEntityCount++;

        //����ִ� �迭 üũ �� ����
        int nullIndex = armyFront.FindEmptyArrayIndex();
        if (nullIndex != -1)
        {
            armyFront[nullIndex] = _data;
            SetArmyBattleForceValue();
            return true;
        }

        nullIndex = armyCenter.FindEmptyArrayIndex();
        if(nullIndex != -1)
        {
            armyCenter[nullIndex] = _data;
            SetArmyBattleForceValue();
            return true;
        }

        nullIndex = armyRear.FindEmptyArrayIndex();
        if (nullIndex != -1)
        {
            armyRear[nullIndex] = _data;
            SetArmyBattleForceValue();
            return true;
        }

        //����ִ� �迭�� ���� �� False ��ȯ
        return false;
    }

    //������ ��ġ�� �÷��̾� ��ƼƼ ��ġ
    public bool UseBattleEntity(BattleEntityData _data, PlaceType _type)
    {
        if (nowUseBattleEntityCount == isCanUseBattleEntityCount) return false;
        nowUseBattleEntityCount++;

        int nullIndex = 0;
        switch (_type)
        {
            case PlaceType.Front:
                nullIndex = armyFront.FindEmptyArrayIndex();
                if (nullIndex != -1) 
                {
                    armyFront[nullIndex] = _data;
                    SetArmyBattleForceValue();                
                }
                return true;

            case PlaceType.Center:
                nullIndex = armyCenter.FindEmptyArrayIndex();
                if (nullIndex != -1)
                {
                    armyCenter[nullIndex] = _data;
                    SetArmyBattleForceValue();
                }
                return true;

            case PlaceType.Rear:
                nullIndex = armyRear.FindEmptyArrayIndex();
                if (nullIndex != -1)
                {
                    armyRear[nullIndex] = _data;
                    SetArmyBattleForceValue();
                }
                return true;
        }

        return false;
    }

    //�÷��̾� ��ġ ��� 
    public void UnUseBattleEntity(BattleEntityData _data)
    {
        //��ġ ī��Ʈ ����
        nowUseBattleEntityCount--;

        //�� ��ġ �˻� �� �� ��ġ�� ������ ��ġ ���
        for (int i = 0; i < armyFront.Length; i++)
        {
            if (armyFront[i] == _data)
            {
                armyFront[i] = null;
                SetArmyBattleForceValue();
                return;
            }
        }

        for (int i = 0; i < armyCenter.Length; i++)
        {
            if (armyCenter[i] == _data)
            {
                armyCenter[i] = null;
                SetArmyBattleForceValue();
                return;
            }
        }

        for (int i = 0; i < armyRear.Length; i++)
        {
            if (armyRear[i] == _data)
            {
                armyRear[i] = null;
                SetArmyBattleForceValue();
                return;
            }
        }
    }

    //�÷��̾� ��ġ �ʱ�ȭ
    public void UnUseAllBattleEntity()
    {
        armyFront = new BattleEntityData[3];
        armyCenter = new BattleEntityData[3];
        armyRear = new BattleEntityData[3];

        nowUseBattleEntityCount = 0;
        armybattleForce = 0;

        UpdateUI();
    }

    //��Ʋ ����Ʈ ����
    private void SetArmyBattleForceValue()
    {
        //��Ż ���� �ʱ�ȭ
        armyAttackForce = 0;
        armyMaxHP = 0;
        armybattleForce = 0;

        //�� ��ġ�� ������Ʈ�� �ִ��� üũ �� �ִٸ� �� ��Ż������ ����
        for (int i = 0; i < armyFront.Length; i++)
        {
            if (armyFront[i] != null)
            {
                armyAttackForce += armyFront[i].attackForce;
                armyMaxHP += armyFront[i].maxHP;
            }
        }

        for (int i = 0; i < armyCenter.Length; i++)
        {
            if (armyCenter[i] != null)
            {
                armyAttackForce += armyCenter[i].attackForce;
                armyMaxHP += armyCenter[i].maxHP;
            }
        }

        for (int i = 0; i < armyRear.Length; i++)
        {
            if (armyRear[i] != null)
            {
                armyAttackForce += armyRear[i].attackForce;
                armyMaxHP += armyRear[i].maxHP;
            }
        }

        //��Ʋ ����Ʈ ���
        armybattleForce = armyAttackForce + armyMaxHP;
        UpdateUI();
    }

    /// <summary>
    /// World�ʿ��� �������� ���ý� ȣ���
    /// </summary>
    /// <param name="_UID">�������� ������ �ε���</param>
    public void SetStageData(int _UID)
    {
        currentStage = Managers.Data.GetStageData(_UID);

        nowEnemyCount = 0;
        enemyAttackForce = 0;
        enemyMaxHP = 0;
        enemybattleForce = 0;
        time = 60;

        enemyFront = new BattleEntityData[3];

        for (int i = 0; i < currentStage.frontEnemyUIDs.Length; i++)
        {
            BattleEntityData data = Managers.Data.GetBattleEntityData(currentStage.frontEnemyUIDs[i], currentStage.frontEnemyLevels[i]);
            enemyFront[i] = data;
            nowEnemyCount++;
            enemyAttackForce += data.attackForce;
            enemyMaxHP += data.maxHP;
        }

        enemyCenter = new BattleEntityData[3];
        for (int i = 0; i < currentStage.centerEnemyUIDs.Length; i++)
        {
            BattleEntityData data = Managers.Data.GetBattleEntityData(currentStage.centerEnemyUIDs[i], currentStage.centerEnemyLevels[i]);
            enemyCenter[i] = data;
            nowEnemyCount++;
            enemyAttackForce += data.attackForce;
            enemyMaxHP += data.maxHP;
        }

        enemyRear = new BattleEntityData[3];
        for (int i = 0; i < currentStage.rearEnemyUIDs.Length; i++)
        {
            BattleEntityData data = Managers.Data.GetBattleEntityData(currentStage.rearEnemyUIDs[i], currentStage.rearEnemyLevels[i]);
            enemyRear[i] = data;
            nowEnemyCount++;
            enemyAttackForce += data.attackForce;
            enemyMaxHP += data.maxHP;
        }

        enemybattleForce = enemyMaxHP + enemyAttackForce;
    }

    //���������� ���ۉ� �� �ʱ�ȭ
    public void StartStage()
    {
        clearStar = 0;
        allBattlePoint = 0;

        armyCurrentHP = armyMaxHP;
        enemyCurrentHP = enemyMaxHP;

        battleMVPPoints.Clear();
        battleMVPPoints = Managers.Object.Armys;
        UpdateUI();
    }   

    //MVP����Ʈ�� ����� �� �ʱ�ȭ �� ���� ����Ʈ ������ �迭 ����
    public void UpdateMVPPoints()
    {
        List<BattleEntityController> tempList = new List<BattleEntityController>();
        bool[] isChecked = new bool[Managers.Object.Armys.Count];
        for (int i = 0; i < isChecked.Length; i++)
            isChecked[i] = false;
        int bestPoint = 0;
        int bestIndex = -1;

        for (int i = 0; i < Managers.Object.Armys.Count; i++)
        {
            bestPoint = 0;
            for (int j = 0; j < Managers.Object.Armys.Count; j++)
            {
                if (isChecked[j]) continue;
                if(bestPoint < Managers.Object.Armys[j].mvpPoint)
                {
                    bestPoint = Managers.Object.Armys[j].mvpPoint;
                    bestIndex = j;
                }
            }
            isChecked[bestIndex] = true;
            tempList.Add(Managers.Object.Armys[bestIndex]);
        }

        battleMVPPoints = tempList;
        allBattlePoint = 0;

        for (int i = 0; i < battleMVPPoints.Count; i++)
            allBattlePoint += battleMVPPoints[i].mvpPoint;

        UpdateUI();
    }

    //�� ü���� �޾��� �� �ʱ�ȭ
    public void UpdateTeamHP(VoidEventType _type)
    {
        if (_type != VoidEventType.OnChangeControllerStatus) return;
        if (Managers.Game.state != GameState.BattleProgress) return;
        armyCurrentHP = 0;
        enemyCurrentHP = 0;

        
        foreach (var item in Managers.Object.Armys)
            armyCurrentHP += item.status.CurrentHP;
        foreach (var item in Managers.Object.Enemys)
            enemyCurrentHP += item.status.CurrentHP;
        UpdateUI();
    }

    //���� ���ǵ� ���� ���� �� ó��
    public void ChangeFastSpeed()
    {
        isFastSpeed = !isFastSpeed;
        if (isFastSpeed) Time.timeScale = 1.5f;
        else Time.timeScale = 1.0f;
    }

    //���� ��ų ���� ����
    public void ChangeAutoSkill()
    {
        isAutoSkill = !isAutoSkill;
    }

    //UI ������Ʈ �̺�Ʈ ȣ��
    public void UpdateUI()
    {
        Managers.Event.OnVoidEvent?.Invoke(VoidEventType.OnChangeBattleInfo);
    }

    //���������� �������� �� �ð� ���� ó�� �� ���� ����
    public void CheckTime()
    {
        if (Managers.Game.state == GameState.BattleProgress)
        {
            time -= Time.deltaTime;
            UpdateUI();
            if (time <= 0)
            {
                time = 0;
                TimeOver();
            }
        }
    }

    //�¸� ó��
    public void Victory()
    {
        Managers.Game.state = Define.GameState.BattleAfter;
        Managers.Routine.StartCoroutine(VictoryRoutine());
    }

    //�й� ó��
    public void Lose()
    {
        Managers.Game.state = Define.GameState.BattleAfter;
        Managers.Routine.StartCoroutine(LoseRoutine());
    }

    //����� ���� ó��
    private void BaseEndStage()
    {
        //�� �ʱ�ȭ
        armyFront = new BattleEntityData[3];
        armyCenter = new BattleEntityData[3];
        armyRear = new BattleEntityData[3];
        nowUseBattleEntityCount = 0;
        armyCurrentHP = 0;
        armyMaxHP = 0;
        armyAttackForce = 0;
        armybattleForce = 0;
        Time.timeScale = 1.0f;

        //�� ������Ʈ�� ����
        for (int i = 0; i < Managers.Object.Armys.Count; i++)
        {
            Managers.Object.Armys[i].StopAllRoutine();
            Managers.Resource.Destroy(Managers.Object.Armys[i].gameObject);
        }
        Managers.Object.Armys.Clear();
        

        for (int i = 0; i < Managers.Object.Enemys.Count; i++)
        {
            Managers.Object.Enemys[i].StopAllRoutine();
            Managers.Resource.Destroy(Managers.Object.Enemys[i].gameObject);
        }
        Managers.Object.Enemys.Clear();
    }

    //�¸� ó�� �κ�
    private IEnumerator VictoryRoutine()
    {
        //�����ִ� ������Ʈ �¸� ����
        for (int i = 0; i < Managers.Object.Armys.Count; i++)
            Managers.Object.Armys[i].ChangeState(BattleEntityState.EndBattle);

        //�¸� ó�� �� UI ȣ��
        yield return new WaitForSeconds(2);
        Managers.Screen.FadeIn(0.5f, () =>
        {
            BaseEndStage();
            int aliveArmyCount = 0;
            for (int i = 0; i < Managers.Object.Armys.Count; i++)
                if (Managers.Object.Armys[i].state != BattleEntityState.Die)
                    aliveArmyCount++;

            if (aliveArmyCount == Managers.Object.Armys.Count)
                clearStar = 3;

            else if (aliveArmyCount >= ((float)Managers.Object.Armys.Count / 3) * 2)
                clearStar = 2;

            else clearStar = 1;

            Managers.UI.ShowPopupUI<UIPopup_Result>().Init(Define.GameResult.Victory);
            Managers.Screen.FadeOut(0.5f);
        });
    }


    //�¸� ó�� �κ�
    private IEnumerator LoseRoutine()
    {
        //�� ������Ʈ �¸� ����
        for (int i = 0; i < Managers.Object.Enemys.Count; i++)
            Managers.Object.Enemys[i].ChangeState(BattleEntityState.EndBattle);

        //�й� ó�� �� UI ȣ��
        yield return new WaitForSeconds(2);
        Managers.Screen.FadeIn(0.5f, () =>
        {
            BaseEndStage();
            Managers.UI.ShowPopupUI<UIPopup_Result>().Init(Define.GameResult.Lose);
            Managers.Screen.FadeOut(0.5f);
        });
    }

    //Ÿ�� ������ �й� ó��
    public void TimeOver()
    {
        Lose();
    }

    public void Update()
    {
        CheckTime();
    }

    public BattleInfo()
    {
        armyRear = new BattleEntityData[3];
        armyCenter = new BattleEntityData[3];
        armyFront = new BattleEntityData[3];

        enemyRear = new BattleEntityData[3];
        enemyCenter = new BattleEntityData[3];
        enemyFront = new BattleEntityData[3];

        isCanUseBattleEntityCount = 4;
        battleMVPPoints = new List<BattleEntityController>();
        isAutoSkill = false;
        isFastSpeed = false;

        Managers.Event.OnVoidEvent -= UpdateTeamHP;
        Managers.Event.OnVoidEvent += UpdateTeamHP;
    }
}