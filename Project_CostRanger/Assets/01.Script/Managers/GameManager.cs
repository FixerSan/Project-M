using JetBrains.Annotations;
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
    public LoginSystem loginSystem;
    public PrepareStageSystem prepareStageSystem;
    public BattleStageSystem battleStageSystem;
    public BattleInfo battleInfo;
    public PlayerData playerData;


    public void Awake()
    {
        StartGame();
    }

    //���� ����
    public void StartGame()
    {
        Managers.Resource.LoadAllAsync<UnityEngine.Object>("Preload", _completeCallback: () =>
        {
            Managers.Data.LoadPreData(() =>
            {
                Managers.Screen.SetCameraPosition(Vector2.zero);
                Managers.Scene.LoadScene(Define.Scene.Login);
            });
        });
    }

    //���� ����
    public void SaveGame()
    {
        Managers.Data.SavePlayerData(Managers.Game.playerData);
    }

    public void StartBattleStage()
    {
        //battleInfo = new BattleInfo(Managers.Data.GetStageData(_UID));
        if (battleStageSystem == null)
            battleStageSystem = new BattleStageSystem();

        battleStageSystem.Init();
    }

    public void StartPrepare(int _stageUID)
    {
        if (prepareStageSystem == null)
            prepareStageSystem = new PrepareStageSystem();

        prepareStageSystem.Init(Managers.Data.GetStageData(_stageUID));
    }

    public void EndBattleStage()
    {
        battleInfo = null;
    }

    public void Login(string _ID, string _passward, Action<LoginEvent> _callback)
    {
        if(loginSystem == null)
            loginSystem = new LoginSystem();

        loginSystem.Login(_ID, _passward, _callback);
    }

    public void SignUp(string _ID, string _name, string _passward, string _passwardReCheck, Action<SignUpEvent> _callback)
    {
        if (loginSystem == null)
            loginSystem = new LoginSystem();
        loginSystem.SignUp(_ID, _name, _passward, _passwardReCheck, _callback);
    }

    private void Update()
    {
        if (battleInfo != null)
            battleInfo.Update();
    }

    public void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveGame();
    }

    public void OnApplicationQuit()
    {
        SaveGame();
    }
}

public class LoginSystem
{
    public void Login(string _ID, string _passward, Action<LoginEvent> _callback)
    {
        PlayerSaveData playerData = Managers.Data.GetPlayerSaveData(_ID);
        if(playerData == null)
        {
            _callback?.Invoke(LoginEvent.NotExistPlayerData);
            return;
        }    

        if(playerData.passward != _passward)
        {
            _callback.Invoke(LoginEvent.IncorrectPassward);
            return;
        }

        Managers.Game.playerData = Managers.Data.CreatePlayerData(_ID);
        _callback?.Invoke(LoginEvent.SuccessLogin);
    }

    public void SignUp(string _ID, string _name, string _passward, string _passwardReCheck, Action<SignUpEvent> _callback)
    {
        PlayerSaveData saveData = Managers.Data.GetPlayerSaveData(_ID);
        if(_ID == string.Empty)
        {
            _callback.Invoke(SignUpEvent.IDisNull);
            return;
        }

        if (_passward == string.Empty)
        {
            _callback.Invoke(SignUpEvent.PasswardIsNull);
            return;
        }

        if (saveData != null)
        {
            _callback?.Invoke(SignUpEvent.ExistSameID);
            return;
        }

        if(_passward != _passwardReCheck)
        {
            _callback?.Invoke(SignUpEvent.PasswardNotSame);
            return;
        }

        Managers.Data.CreatePlayerSaveData(_ID, _passward, _name, string.Empty);
        _callback?.Invoke(SignUpEvent.SuccessSignUp);
    }
}
[System.Serializable]
public class PrepareStageSystem
{
    public StageData stageData;
    public RangerControllerData[] rangers;
    public EnemyControllerData[] enemies;
    public Batch batch;

    //�ʱ� ����
    public void Init(StageData _stageData)
    {
        stageData = _stageData;
        //����� ������ ������ ����
        SetupCanUseRanger(); //����� ������ ���������� �� �� ���Ǵ� �������� ������ 
        SetupEnemy();

        UpdataUI();
    }

    //����� ������ �����¾ȿ� �� �ִ� �������� ������ ������ ���������� ī�带 ����
    public void SetupCanUseRanger()
    {

    }

    //�� ���� ��� ����
    public void SetupEnemy()
    {
        string[] enemyStringArray = stageData.enemyUIDs.Split(",");

        for (int i = 0; i < enemies.Length; i++)
            if(Int32.TryParse(enemyStringArray[i], out int enemyUID))
                enemies[i] = Managers.Data.GetEnemyControllerData(enemyUID);
    }

    public void UpdataUI()
    {
        Managers.Event.OnVoidEvent?.Invoke(VoidEventType.OnChangePrepare);
    }

    public PrepareStageSystem()
    {
        rangers = new RangerControllerData[6];
        enemies = new EnemyControllerData[9];
    }
}
public class BattleStageSystem
{
    //���� �������� ���������� �÷��̾��� ����
    public int canUseCost;
    public int nowUseCost;
    public int armyCurrentHP;
    public int armyMaxHP;
    public int armyAttackForce;
    public int armybattleForce;
    public int allDamage;

    //�������� ���� ���ĵ� �÷��̾� ��ƼƼ��
    public List<RangerController> battleMVPPoints;

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


    public void Init()
    {
        //���⼭ ������� �ý��� ������ �����ų ����
        nowUseCost = 0;
        armyCurrentHP = 0;
        armyMaxHP = 0;
        armyAttackForce = 0;
        armybattleForce = 0;
        allDamage = 0;

        battleMVPPoints.Clear();

        nowEnemyCount = 0;
        enemyCurrentHP = 0;
        enemyMaxHP = 0;
        enemyAttackForce = 0;
        enemybattleForce = 0;

        time = 0;
        SetEnemy();
    }

    public void SetEnemy()
    {
        //for (int i = 0; i < currentStage.frontEnemyUIDs.Length; i++)
        //    frontEnemy[i] = Managers.Data.GetEnemyControllerData(currentStage.frontEnemyUIDs[i]);

        //for (int i = 0; i < currentStage.centerEnemyUIDs.Length; i++)
        //    centerEnemy[i] = Managers.Data.GetEnemyControllerData(currentStage.centerEnemyUIDs[i]);

        //for (int i = 0; i < currentStage.rearEnemyUIDs.Length; i++)
        //    rearEnemy[i] = Managers.Data.GetEnemyControllerData(currentStage.rearEnemyUIDs[i]);
    }

    public void StartStage()
    {

    }

    public void Victory()
    {

    }

    public void Lose()
    {

    }

    public BattleStageSystem()
    {
        //���� �������� ���������� �÷��̾��� ����
        nowUseCost = 0;
        armyCurrentHP = 0;
        armyMaxHP = 0;
        armyAttackForce = 0;
        armybattleForce = 0;
        allDamage = 0;

        //�������� ���� ���ĵ� �÷��̾� ��ƼƼ��
        battleMVPPoints = new List<RangerController>();

        //���� �������� ���������� ���� ����
        nowEnemyCount = 0;
        enemyCurrentHP = 0;
        enemyMaxHP = 0;
        enemyAttackForce = 0;
        enemybattleForce = 0;

        //���� ���� ���� �� ����
        isAutoSkill = false;
        isFastSpeed = false;
        time = 0;
    }
}

//���ſ� ���� ��ü �ý���
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
    public int canUseCost;
    public int nowUseCost;
    public int armyCurrentHP;
    public int armyMaxHP;
    public int armyAttackForce;
    public int armybattleForce;
    public int allDamage;

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
        if (nowUseCost == canUseCost) return false;
        nowUseCost++;

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
        if (nowUseCost == canUseCost) return false;
        nowUseCost++;

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
        nowUseCost--;

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

        nowUseCost = 0;
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
        //currentStage = Managers.Data.GetStageData(_UID);

        //nowEnemyCount = 0;
        //enemyAttackForce = 0;
        //enemyMaxHP = 0;
        //enemybattleForce = 0;
        //time = 60;

        //enemyFront = new BattleEntityData[3];

        //for (int i = 0; i < currentStage.frontEnemyUIDs.Length; i++)
        //{
        //    BattleEntityData data = Managers.Data.GetBattleEntityData(currentStage.frontEnemyUIDs[i], currentStage.frontEnemyLevels[i]);
        //    enemyFront[i] = data;
        //    nowEnemyCount++;
        //    enemyAttackForce += data.attackForce;
        //    enemyMaxHP += data.maxHP;
        //}

        //enemyCenter = new BattleEntityData[3];
        //for (int i = 0; i < currentStage.centerEnemyUIDs.Length; i++)
        //{
        //    BattleEntityData data = Managers.Data.GetBattleEntityData(currentStage.centerEnemyUIDs[i], currentStage.centerEnemyLevels[i]);
        //    enemyCenter[i] = data;
        //    nowEnemyCount++;
        //    enemyAttackForce += data.attackForce;
        //    enemyMaxHP += data.maxHP;
        //}

        //enemyRear = new BattleEntityData[3];
        //for (int i = 0; i < currentStage.rearEnemyUIDs.Length; i++)
        //{
        //    BattleEntityData data = Managers.Data.GetBattleEntityData(currentStage.rearEnemyUIDs[i], currentStage.rearEnemyLevels[i]);
        //    enemyRear[i] = data;
        //    nowEnemyCount++;
        //    enemyAttackForce += data.attackForce;
        //    enemyMaxHP += data.maxHP;
        //}

        //enemybattleForce = enemyMaxHP + enemyAttackForce;
    }

    //���������� ���ۉ� �� �ʱ�ȭ
    public void StartStage()
    {
        clearStar = 0;
        allDamage = 0;

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
        allDamage = 0;

        for (int i = 0; i < battleMVPPoints.Count; i++)
            allDamage += battleMVPPoints[i].mvpPoint;

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
            armyCurrentHP += item.battleEntityStatus.CurrentHP;
        foreach (var item in Managers.Object.Enemys)
            enemyCurrentHP += item.battleEntityStatus.CurrentHP;
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
        nowUseCost = 0;
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

    public BattleInfo(StageData _stageData)
    {
        //���� �������� �������� ������
        currentStage = _stageData;
        clearStar = 0;

        //���� �������� ���������� ��ġ�� �÷��̾� entity  �� ��ġ
        armyRear = new BattleEntityData[3];
        armyCenter = new BattleEntityData[3];
        armyFront = new BattleEntityData[3];

        //���� �������� ���������� �÷��̾��� ����
        canUseCost = _stageData.canUseCost;
        nowUseCost = 0;
        armyCurrentHP = 0;
        armyMaxHP = 0;
        armyAttackForce = 0;
        armybattleForce = 0;
        allDamage = 0;

        //�������� ���� ���ĵ� �÷��̾� ��ƼƼ��
        battleMVPPoints = new List<BattleEntityController>();

        //���� �������� ���������� ��ġ�� �� entity  �� ��ġ
        enemyRear = new BattleEntityData[3];
        enemyCenter = new BattleEntityData[3];
        enemyFront = new BattleEntityData[3];

        //���� �������� ���������� ���� ����
        nowEnemyCount = 0;
        enemyCurrentHP = 0;
        enemyMaxHP = 0;
        enemyAttackForce = 0;
        enemybattleForce = 0;

        //���� ���� ���� �� ����
        isAutoSkill = false;
        isFastSpeed = false;
        time = 0;

        armyRear = new BattleEntityData[3];
        armyCenter = new BattleEntityData[3];
        armyFront = new BattleEntityData[3];

        enemyRear = new BattleEntityData[3];
        enemyCenter = new BattleEntityData[3];
        enemyFront = new BattleEntityData[3];

        canUseCost = 4;
        battleMVPPoints = new List<BattleEntityController>();
        isAutoSkill = false;
        isFastSpeed = false;

        Managers.Event.OnVoidEvent -= UpdateTeamHP;
        Managers.Event.OnVoidEvent += UpdateTeamHP;
    }
}