using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;
using static Define;

public class GameManager : Singleton<GameManager>
{
    //현재 게임 데이터 및 상태
    public GameState state;
    public LoginSystem loginSystem;
    public PrepareStageSystem prepareStageSystem;
    public BattleStageSystem battleStageSystem;
    public PlayerData playerData;

    public void Awake()
    {
        StartGame();
    }

    //게임 시작
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

    //게임 저장
    public void SaveGame()
    {
        Managers.Data.SavePlayerData(Managers.Game.playerData);
    }


    public void StartPrepare(int _stageUID)
    {
        if (prepareStageSystem == null)
            prepareStageSystem = new PrepareStageSystem();

        state = GameState.BattleBefore;
        prepareStageSystem.Init(Managers.Data.GetStageData(_stageUID));
    }
    public void StartBattleStage(Action<Define.StartBattleStageEvent> _callback)
    {
        //여기서 검증한 후 시작할 수 없는 상황이면 콜백
        if(prepareStageSystem.rangerControllerData.NullCount() == prepareStageSystem.rangerControllerData.Length)
        {
            _callback?.Invoke(StartBattleStageEvent.RangerIsNotExist);
            return;
        }

        if (battleStageSystem == null)
            battleStageSystem = new BattleStageSystem();

        battleStageSystem.Init(prepareStageSystem);
        Managers.Scene.LoadScene(Define.Scene.Stage);
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

    //private void Update()
    //{
    //    if (battleStageSystem != null)
    //        battleStageSystem.Update();
    //}

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
    public RangerControllerData[] rangerControllerData;
    public EnemyControllerData[] enemies;

    public Dictionary<SpecialtyType ,Specialty> specialties;
    public Batch batch;

    private SpecialtyType tempSpecialtyType;

    public PrepareStageSystem()
    {
        rangerControllerData = new RangerControllerData[6];
        enemies = new EnemyControllerData[9];
        specialties = new Dictionary<SpecialtyType, Specialty>();
    }

    //초기 설정
    public void Init(StageData _stageData)
    {
        stageData = _stageData;

        Array.Clear(rangerControllerData, 0 , rangerControllerData.Length);
        Array.Clear(enemies, 0 , enemies.Length);
        specialties.Clear();

        //저장된 레인저 프리셋 설정
        SetupEnemy();
        RedrawUI();
    }

    public void SetUseRanger(int _rangerIndex, int _slotIndex)
    {
        rangerControllerData[_slotIndex] = Managers.Data.GetRangerControllerData(_rangerIndex);
        SetSpecialty();
        RedrawUI();
    }
    public void CancelUseRanger(int _slotIndex)
    {
        if (_slotIndex == -1) return;
        rangerControllerData[_slotIndex] = null;
        SetSpecialty();
        RedrawUI();
    }

    public void SetSpecialty()
    {
        return;
        specialties.Clear();
        for (int i = 0; i < rangerControllerData.Length; i++)
        {
            if (rangerControllerData[i] != null)
            {
                tempSpecialtyType = Util.ParseEnum<SpecialtyType>(rangerControllerData[i].specialtyOne);
                if (!specialties.ContainsKey(tempSpecialtyType))
                    CreateSpecialty(tempSpecialtyType);
                specialties[tempSpecialtyType].AddCount();

                tempSpecialtyType = Util.ParseEnum<SpecialtyType>(rangerControllerData[i].specialtyTwo);
                if (!specialties.ContainsKey(tempSpecialtyType))
                    CreateSpecialty(tempSpecialtyType);
                specialties[tempSpecialtyType].AddCount();
            }
        }
    }

    public void CreateSpecialty(SpecialtyType type)
    {
        Specialty specialty = null;
        if (type == SpecialtyType.Test) specialty = new Specialties.Test();
        specialties.Add(type, specialty);
    }

    //적 정보 대로 생성
    public void SetupEnemy()
    {
        string[] enemyStringArray = stageData.enemyUIDs.Split(",");

        for (int i = 0; i < enemies.Length; i++)
            if(Int32.TryParse(enemyStringArray[i], out int enemyUID))
                enemies[i] = Managers.Data.GetEnemyControllerData(enemyUID);
    }

    public void RedrawUI()
    {
        Managers.Event.OnVoidEvent?.Invoke(VoidEventType.OnChangePrepare);
    }

}
public class BattleStageSystem
{
    public StageData currentStageData;
    public StageScene scene;
    public Batch batch;

    public RangerControllerData[] rangerControllerData;
    public List<Specialty> specialties;

    //현재 진행중인 스테이지중 플레이어의 상태
    public int canUseCost;
    public int nowUseCost;
    public int armyCurrentHP;
    public int armyMaxHP;
    public int armyAttackForce;
    public int armybattleForce;
    public int allDamage;

    //데미지의 맞춰 정렬된 플레이어 엔티티들
    public List<RangerController> battleMVPPoints;

    //현재 진행중인 스테이지중 적의 상태
    public int nowEnemyCount;
    public int enemyCurrentHP;
    public int enemyMaxHP;
    public int enemyAttackForce;
    public int enemybattleForce;

    //게임 진행 설정 및 정보
    public bool isAutoSkill;
    public bool isFastSpeed;
    public float time;

    private bool tempBool;

    public BattleStageSystem()
    {
        //현재 진행중인 스테이지중 플레이어의 상태
        nowUseCost = 0;
        armyCurrentHP = 0;
        armyMaxHP = 0;
        armyAttackForce = 0;
        armybattleForce = 0;
        allDamage = 0;

        //데미지의 맞춰 정렬된 플레이어 엔티티들
        battleMVPPoints = new List<RangerController>();
        specialties = new List<Specialty>();

        //현재 진행중인 스테이지중 적의 상태
        nowEnemyCount = 0;
        enemyCurrentHP = 0;
        enemyMaxHP = 0;
        enemyAttackForce = 0;
        enemybattleForce = 0;

        //게임 진행 설정 및 정보
        isAutoSkill = false;
        isFastSpeed = false;
        time = 0;

        Managers.Event.AddUpdate(Update);
        Managers.Event.AddVoidEvent(VoidEventType.OnPlayerDead, CheckLose);
        Managers.Event.AddVoidEvent(VoidEventType.OnEnemyDead, CheckVictory);
    }

    public void Init(PrepareStageSystem _prepareSystem)
    {
        currentStageData = _prepareSystem.stageData;
        scene = Managers.Scene.GetActiveScene<StageScene>();
        batch = _prepareSystem.batch;
        rangerControllerData = _prepareSystem.rangerControllerData;

        battleMVPPoints.Clear();
        specialties.Clear();    
        
        //여기서 프리페어 시스템 정보를 적용시킬 것임
        nowUseCost = 0;
        armyCurrentHP = 0;
        armyMaxHP = 0;
        armyAttackForce = 0;
        armybattleForce = 0;
        allDamage = 0;

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
            Managers.Object.Enemies[i].ChangeState(Define.EnemyState.Idle);

        for (int i = 0; i < Managers.Object.Rangers.Count; i++)
            Managers.Object.Rangers[i].ChangeState(Define.RangerState.Idle);
    }

    public void Update()
    {
        if (Managers.Game.state != GameState.BattleProgress) return;
        CheckTime();
    }

    public void CheckTime()
    {
        time -= Time.deltaTime;
        RedrawUI();
        if (time <= 0)
        {
            time = 0;
            Lose();
        }
    }

    public void SetAutoSkill(bool _setBool)
    {
        isAutoSkill = _setBool;
        RedrawUI();
    }

    public void SetFastSpeed(bool _setBool)
    {
        isFastSpeed = _setBool;
        if (isFastSpeed) Time.timeScale = Define.fastSpeed;
        else Time.timeScale = 1.0f;
        RedrawUI();
    }


    public void CheckVictory()
    {
        tempBool = true;
        for (int i = 0; i < Managers.Object.Enemies.Count; i++)
        {
            if (!tempBool)
                break;
            if (Managers.Object.Enemies[i].currentState != EnemyState.Die)
                tempBool = false;
        }

        if (tempBool)
            Victory();
    }

    public void CheckLose()
    {
        tempBool = true;
        for (int i = 0; i < Managers.Object.Rangers.Count; i++)
        {
            if (!tempBool)
                break;
            if (Managers.Object.Rangers[i].currentState != RangerState.Die)
                tempBool = false;
        }

        if (tempBool)
            Victory();
    }

    public void Victory()
    {
        Managers.Game.state = GameState.BattleAfter;
        Managers.Object.ClearRangers();
        Managers.Object.ClearEnemies();
        Managers.Stage.ClearReward(currentStageData.UID);
        Managers.Screen.FadeIn(0.25f, () => 
        {
            UIPopup_Result ui = Managers.UI.ShowPopupUI<UIPopup_Result>();
            ui.Init(GameResult.Victory, currentStageData.UID);
            Managers.Screen.FadeOut(0.25f);
        });
    }

    public void Lose()
    {
        Managers.Game.state = GameState.BattleAfter;
    }

    public void RedrawUI()
    {
        Managers.Event.InvokeVoidEvent(VoidEventType.OnChangeBattle);
    }

    ~BattleStageSystem()
    {
        Managers.Event.RemoveUpdate(Update);
        Managers.Event.RemoveVoidEvent(VoidEventType.OnPlayerDead, CheckLose);
        Managers.Event.RemoveVoidEvent(VoidEventType.OnEnemyDead, CheckVictory);
    }
}