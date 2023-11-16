using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Define;

public class GameManager : Singleton<GameManager>
{
    //현재 게임 데이터 및 상태
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

    //게임 시작
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

    //게임 저장
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

    //초기 설정
    public void Init(StageData _stageData)
    {
        stageData = _stageData;
        //저장된 레인저 프리셋 설정
        SetupCanUseRanger(); //저장된 레인저 프리셋으로 인 해 사용되는 레인저를 제외한 
        SetupEnemy();

        UpdataUI();
    }

    //저장된 레인저 프리셋안에 들어가 있는 레인저를 제외한 나머지 레인저들의 카드를 생성
    public void SetupCanUseRanger()
    {

    }

    //적 정보 대로 생성
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


    public void Init()
    {
        //여기서 프리페어 시스템 정보를 적용시킬 것임
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
        //현재 진행중인 스테이지중 플레이어의 상태
        nowUseCost = 0;
        armyCurrentHP = 0;
        armyMaxHP = 0;
        armyAttackForce = 0;
        armybattleForce = 0;
        allDamage = 0;

        //데미지의 맞춰 정렬된 플레이어 엔티티들
        battleMVPPoints = new List<RangerController>();

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
    }
}

//과거에 쓰던 전체 시스템
[System.Serializable]
public class BattleInfo
{
    //현재 진행중인 스테이지 데이터
    public StageData currentStage;
    public int clearStar;

    //현재 진행중인 스테이지의 배치된 플레이어 entity  및 위치
    public BattleEntityData[] armyFront;
    public BattleEntityData[] armyCenter;
    public BattleEntityData[] armyRear; 

    //현재 진행중인 스테이지중 플레이어의 상태
    public int canUseCost;
    public int nowUseCost;
    public int armyCurrentHP;
    public int armyMaxHP;
    public int armyAttackForce;
    public int armybattleForce;
    public int allDamage;

    //데미지의 맞춰 정렬된 플레이어 엔티티들
    public List<BattleEntityController> battleMVPPoints;

    //현재 진행중인 스테이지의 배치된 적 entity  및 위치
    public BattleEntityData[] enemyFront;
    public BattleEntityData[] enemyCenter;
    public BattleEntityData[] enemyRear;

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


    //플레이어 엔티티 배치
    public bool UseBattleEntity(BattleEntityData _data)
    {
        //더 배치할 수 있는지 체크
        if (nowUseCost == canUseCost) return false;
        nowUseCost++;

        //비어있는 배열 체크 및 적용
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

        //비어있는 배열이 없을 시 False 반환
        return false;
    }

    //지정된 위치에 플레이어 엔티티 배치
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

    //플레이어 배치 취소 
    public void UnUseBattleEntity(BattleEntityData _data)
    {
        //배치 카운트 감소
        nowUseCost--;

        //각 배치 검사 후 그 배치에 있으면 배치 취소
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

    //플레이어 배치 초기화
    public void UnUseAllBattleEntity()
    {
        armyFront = new BattleEntityData[3];
        armyCenter = new BattleEntityData[3];
        armyRear = new BattleEntityData[3];

        nowUseCost = 0;
        armybattleForce = 0;

        UpdateUI();
    }

    //배틀 포인트 정리
    private void SetArmyBattleForceValue()
    {
        //토탈 변수 초기화
        armyAttackForce = 0;
        armyMaxHP = 0;
        armybattleForce = 0;

        //각 배치에 오브젝트가 있는지 체크 후 있다면 각 토탈변수에 적립
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

        //배틀 포인트 계산
        armybattleForce = armyAttackForce + armyMaxHP;
        UpdateUI();
    }

    /// <summary>
    /// World맵에서 스테이지 선택시 호출됨
    /// </summary>
    /// <param name="_UID">스테이지 데이터 인덱스</param>
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

    //스테이지가 시작됄 시 초기화
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

    //MVP포인트가 변경될 시 초기화 및 높은 포인트 순으로 배열 정렬
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

    //팀 체력이 달았을 시 초기화
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

    //게임 스피드 상태 변경 및 처리
    public void ChangeFastSpeed()
    {
        isFastSpeed = !isFastSpeed;
        if (isFastSpeed) Time.timeScale = 1.5f;
        else Time.timeScale = 1.0f;
    }

    //오토 스킬 상태 변경
    public void ChangeAutoSkill()
    {
        isAutoSkill = !isAutoSkill;
    }

    //UI 업데이트 이벤트 호출
    public void UpdateUI()
    {
    }

    //스테이지가 진행중일 때 시간 감소 처리 및 상태 변경
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

    //승리 처리
    public void Victory()
    {
        Managers.Game.state = Define.GameState.BattleAfter;
        Managers.Routine.StartCoroutine(VictoryRoutine());
    }

    //패배 처리
    public void Lose()
    {
        Managers.Game.state = Define.GameState.BattleAfter;
        Managers.Routine.StartCoroutine(LoseRoutine());
    }

    //공통된 종료 처리
    private void BaseEndStage()
    {
        //값 초기화
        armyFront = new BattleEntityData[3];
        armyCenter = new BattleEntityData[3];
        armyRear = new BattleEntityData[3];
        nowUseCost = 0;
        armyCurrentHP = 0;
        armyMaxHP = 0;
        armyAttackForce = 0;
        armybattleForce = 0;
        Time.timeScale = 1.0f;

        //각 오브젝트들 삭제
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

    //승리 처리 부분
    private IEnumerator VictoryRoutine()
    {
        //남아있는 오브젝트 승리 포즈
        for (int i = 0; i < Managers.Object.Armys.Count; i++)
            Managers.Object.Armys[i].ChangeState(BattleEntityState.EndBattle);

        //승리 처리 및 UI 호출
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


    //승리 처리 부분
    private IEnumerator LoseRoutine()
    {
        //적 오브젝트 승리 포즈
        for (int i = 0; i < Managers.Object.Enemys.Count; i++)
            Managers.Object.Enemys[i].ChangeState(BattleEntityState.EndBattle);

        //패배 처리 및 UI 호출
        yield return new WaitForSeconds(2);
        Managers.Screen.FadeIn(0.5f, () =>
        {
            BaseEndStage();
            Managers.UI.ShowPopupUI<UIPopup_Result>().Init(Define.GameResult.Lose);
            Managers.Screen.FadeOut(0.5f);
        });
    }

    //타임 오버시 패배 처리
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
        //현재 진행중인 스테이지 데이터
        currentStage = _stageData;
        clearStar = 0;

        //현재 진행중인 스테이지의 배치된 플레이어 entity  및 위치
        armyRear = new BattleEntityData[3];
        armyCenter = new BattleEntityData[3];
        armyFront = new BattleEntityData[3];

        //현재 진행중인 스테이지중 플레이어의 상태
        canUseCost = _stageData.canUseCost;
        nowUseCost = 0;
        armyCurrentHP = 0;
        armyMaxHP = 0;
        armyAttackForce = 0;
        armybattleForce = 0;
        allDamage = 0;

        //데미지의 맞춰 정렬된 플레이어 엔티티들
        battleMVPPoints = new List<BattleEntityController>();

        //현재 진행중인 스테이지의 배치된 적 entity  및 위치
        enemyRear = new BattleEntityData[3];
        enemyCenter = new BattleEntityData[3];
        enemyFront = new BattleEntityData[3];

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