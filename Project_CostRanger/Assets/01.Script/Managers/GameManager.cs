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
                Managers.Screen.SetCameraPosition(new Vector3(0,0,-10f));
                Managers.Scene.LoadScene(Define.Scene.Login);
            });
        });
    }

    //���� ����
    public void SaveGame()
    {
        Managers.Data.SavePlayerData(Managers.Game.playerData);
    }

    public void StartBattleStage(Action<Define.StartBattleStageEvent> _callback)
    {
        //���⼭ ������ �� ������ �� ���� ��Ȳ�̸� �ݹ�
        if(prepareStageSystem.rangers.NullCount() == prepareStageSystem.rangers.Length)
        {
            _callback?.Invoke(StartBattleStageEvent.RangerIsNotExist);
            return;
        }

        if (battleStageSystem == null)
            battleStageSystem = new BattleStageSystem();

        battleStageSystem.Init(prepareStageSystem);
        Managers.Scene.LoadScene(Define.Scene.Stage);
    }

    public void StartPrepare(int _stageUID)
    {
        if (prepareStageSystem == null)
            prepareStageSystem = new PrepareStageSystem();

        prepareStageSystem.Init(Managers.Data.GetStageData(_stageUID));
    }

    public void EndBattleStage()
    {
        battleStageSystem = null;
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
        if (battleStageSystem != null)
            battleStageSystem.Update();
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
        SetupEnemy();

        UpdataUI();
    }

    public void SetUseRanger(int _rangerIndex, int _slotIndex)
    {
        rangers[_slotIndex] = Managers.Data.GetRangerControllerData(_rangerIndex);

        UpdataUI();
    }

    public void CancelUseRanger(int _slotIndex)
    {
        if (_slotIndex == -1) return;
        rangers[_slotIndex] = null;

        UpdataUI();
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
    public StageData currentStageData;
    public StageScene scene;

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


    public void Init(PrepareStageSystem _prepareSystem)
    {
        currentStageData = _prepareSystem.stageData;
        scene = Managers.Scene.GetActiveScene<StageScene>();

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

        time = 60;
    }

    public void StartStage()
    {
        Managers.Game.state = GameState.BattleProgress;

        for (int i = 0; i < Managers.Object.Enemies.Count; i++)
            Managers.Object.Enemies[i].ChangeState(Define.EnemyState.Follow);
    }

    public void Update()
    {
        if (Managers.Game.state != GameState.BattleProgress) return;
        CheckTime();
    }

    public void CheckTime()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = 0;
            Lose();
        }
    }

    public void Victory()
    {
        Managers.Game.state = GameState.BattleAfter;
    }

    public void Lose()
    {
        Managers.Game.state = GameState.BattleAfter;
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