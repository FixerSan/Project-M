using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;
using Unity.VisualScripting;
using static Define;

public class DataManager
{
    public PlayerData playerData;
    public Dictionary<string, PlayerSaveData> playerSaveDatas;
    public Dictionary<int, RangerControllerData> rangerControllerDatas;
    public Dictionary<int, RangerInfoData> rangerInfoDatas;
    public Dictionary<int, EnemyControllerData> enemyControllerDatas;
    public Dictionary<int, EnemyInfoData> enemyInfoDatas;
    public Dictionary<int, DialogData> dialogDatas;
    public Dictionary<int, StageData> stageDatas;
    public Dictionary<int, SpecialtyData> specialtyDatas;
    
    public readonly string PLAYERSAVEDATAPATH;
    public readonly string STAGEDATAPATH;

    public DataManager()
    {
        playerData = null;
        playerSaveDatas = new Dictionary<string, PlayerSaveData>();
        dialogDatas = new Dictionary<int, DialogData>();
        rangerInfoDatas = new Dictionary<int, RangerInfoData>();
        rangerControllerDatas = new Dictionary<int, RangerControllerData>();
        enemyControllerDatas = new Dictionary<int, EnemyControllerData>();
        enemyInfoDatas = new Dictionary<int, EnemyInfoData>();
        stageDatas = new Dictionary<int, StageData>();
        specialtyDatas = new Dictionary<int, SpecialtyData>();
        PLAYERSAVEDATAPATH = Path.Combine(Application.dataPath + "/05.Data/", "PlayerSaveData.txt");
        STAGEDATAPATH = Path.Combine(Application.dataPath + "/05.Data/", "StageData.txt");
    }

    #region Get
    public PlayerSaveData GetPlayerSaveData(string _ID)
    {
        if (playerSaveDatas.TryGetValue(_ID, out PlayerSaveData data)) return data;
        return null;
    }

    //스테이지 데이터 반환
    public StageData GetStageData(int _UID)
    {
        if (stageDatas.TryGetValue(_UID, out StageData _data)) return _data;
        return null;
    }

    //다이얼로그 데이터 반환
    public DialogData GetDialogData(int _UID)
    {
        if (dialogDatas.TryGetValue(_UID, out DialogData _data)) return _data;
        return null;
    }

    //엔티티 데이터 반환
    public BattleEntityData GetBattleEntityData(int _UID, int _level)
    {
        //if (battleEntityStatusDatas.TryGetValue(_UID, out Dictionary<int, BattleEntityData> datas)) if (datas.TryGetValue(_level, out BattleEntityData data)) return data;
        return null;

    }

    //레인저 인포 데이터 반환
    public RangerInfoData GetRangerInfoData(int _UID)
    {
        if (rangerInfoDatas.TryGetValue(_UID, out RangerInfoData data)) return data;
        return null;
    }


    //레인저 컨트롤러 데이터 반환
    public RangerControllerData GetRangerControllerData(int _UID)
    {
        if (rangerControllerDatas.TryGetValue(_UID, out RangerControllerData data)) return data;
        return null;
    }

    //적 인포 데이터 반환
    public EnemyInfoData GetEnemyInfoData(int _UID)
    {
        if (enemyInfoDatas.TryGetValue(_UID, out EnemyInfoData data)) return data;
        return null;
    }


    //적 컨트롤러 데이터 반환
    public EnemyControllerData GetEnemyControllerData(int _UID)
    {
        if (enemyControllerDatas.TryGetValue(_UID, out EnemyControllerData data)) return data;
        return null;
    }

    public SpecialtyData GetSpecialtyData(int _UID)
    {
        if (specialtyDatas.TryGetValue(_UID, out SpecialtyData data)) return data;
        return null;
    }

    #endregion
    #region Load
    //게임 기반 데이터 로드
    public void LoadPreData(Action _callback)
    {
        //GetPlayerData(Define.userUID);
        //LoadBattleEntityStatusData();
        LoadStageData();
        LoadRangerData();
        LoadEnemyData();
        LoadPlayerSaveData();
        _callback?.Invoke();
    }

    //플레이어 개인 설정 데이터 로드
    private void LoadPlayerSaveData()
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>("PlayerSaveData");
        if (textAsset == null) return;
        PlayerSaveDatas saveData = JsonUtility.FromJson<PlayerSaveDatas>(textAsset.text);

        if (saveData.playerSaveDatas == null) return;
        for (int i = 0; i < saveData.playerSaveDatas.Length; i++)
            if (!playerSaveDatas.TryAdd(saveData.playerSaveDatas[i].ID, saveData.playerSaveDatas[i])) Debug.LogError($"{i}번째 플레이어 세이브 데이터가 로드 되지 않았음");
    }

    //스테이지 데이터 로드
    public void LoadStageData()
    {
        StageDatas datas = JsonUtility.FromJson<StageDatas>(Managers.Resource.Load<TextAsset>("StageData").text);

        for (int i = 0; i < datas.stageDatas.Length; i++)
            stageDatas.Add(datas.stageDatas[i].UID, datas.stageDatas[i]);
    }

    public void LoadRangerData()
    {
        //데이터가 저장된 텍스트 파일을 데이터로 로드
        TextAsset textAsset = Managers.Resource.Load<TextAsset>("RangerData");
        RangerDatas rangerDatas = JsonUtility.FromJson<RangerDatas>(textAsset.text);

        //인포 데이터를 딕셔너리에 저장
        for (int i = 0; i < rangerDatas.infoDatas.Length; i++)
            if (!rangerInfoDatas.TryAdd(rangerDatas.infoDatas[i].UID, rangerDatas.infoDatas[i]))
                Debug.LogError($"{i}번째 레인저 인포 데이터 로드에 실패하였습니다. ");

        //컨트롤러 데이터를 딕셔너리에 저장
        for (int i = 0; i < rangerDatas.controllersData.Length; i++)
            if (!rangerControllerDatas.TryAdd(rangerDatas.controllersData[i].UID, rangerDatas.controllersData[i]))
                Debug.LogError($"{i}번째 레인저 컨트롤러 데이터 로드에 실패하였습니다. ");
    }

    //적 데이터를 로드
    public void LoadEnemyData()
    {
        //데이터가 저장된 텍스트 파일을 데이터로 로드
        TextAsset textAsset = Managers.Resource.Load<TextAsset>("EnemyData");
        EnemyDatas enemyDatas = JsonUtility.FromJson<EnemyDatas>(textAsset.text);

        //인포 데이터를 딕셔너리에 저장
        for (int i = 0; i < enemyDatas.infoDatas.Length; i++)
            if (!enemyInfoDatas.TryAdd(enemyDatas.infoDatas[i].UID, enemyDatas.infoDatas[i]))
                Debug.LogError($"{i}번째 적 인포 데이터 로드에 실패하였습니다. ");

        //컨트롤러 데이터를 딕셔너리에 저장
        for (int i = 0; i < enemyDatas.controllersData.Length; i++)
            if (!enemyControllerDatas.TryAdd(enemyDatas.controllersData[i].UID, enemyDatas.controllersData[i]))
                Debug.LogError($"{i}번째 적 컨트롤러 데이터 로드에 실패하였습니다. ");

        Debug.Log($"로드가 완료되었습니다. 적 인포 데이터: {enemyInfoDatas.Count} , 적 컨트롤로 데이터: {enemyControllerDatas.Count}");
    }

    #endregion
    #region Create

    //플레이어 세이브 데이터를 플레이어 데이터로 형변환 후 반환
    public PlayerData CreatePlayerData(string _ID)
    {
        //플레이어 세이브 데이터를 Get
        PlayerSaveData saveData = Managers.Data.GetPlayerSaveData(_ID);
        List<RangerInfoData> hasRangers = new List<RangerInfoData>();
        string[] rangerUIDs = saveData.hasRangerUID.Split(',');

        //플레이어 세이브 데이터의 들어있는 레인저 UID를 통해서 레인저 인포 데이터 Get
        if (rangerUIDs[0] != string.Empty)
        {
            for (int i = 0; i < rangerUIDs.Length; i++)
                hasRangers.Add(Managers.Data.GetRangerInfoData(Int32.Parse(rangerUIDs[i])));
        }

        //플레이어 데이터 생성 후 리턴
        PlayerData data = new PlayerData(saveData.ID, saveData.name, saveData.level, saveData.exp, saveData.gold, saveData.gem, saveData.upgradeCost, hasRangers, saveData.lastClearStageUID);
        return data;
    }

    //플레이에 세이브 데이터 생성
    public void CreatePlayerSaveData(string _ID, string _passward, string _name, string _hasRangerUID)
    {
        PlayerSaveData playerSaveData = new PlayerSaveData(_ID, _passward, _name, 1, 0, 0, 100, 0, _hasRangerUID, -1);
        playerSaveDatas.TryAdd(playerSaveData.ID, playerSaveData);
    }

    //스테이지 데이터 생성
    public void CreateStageData(int _stageUID, string _stageName, int _canUseCost,
                            string _enemyUIDs, Action<CreateStageEvent> _callback)
    {
        //이미 같은 UID가 존재할 때
        if (stageDatas.ContainsKey(_stageUID))
        {
            //존재한다는 메세지 반환 후 리턴
            _callback?.Invoke(CreateStageEvent.ExistSameUID);
            return;
        }

        //스테이지 데이터 생성 및 딕셔너리에 추가
        StageData stageData = new StageData(_stageUID, _stageName, _canUseCost, _enemyUIDs);
        stageDatas.Add(_stageUID, stageData);

        //데이터를 저장할 배열 재생성
        StageData[] saveDatas = new StageData[stageDatas.Count];
        int count = 0;

        //배열의 저장
        foreach (var data in stageDatas)
        {
            saveDatas[count] = data.Value;
            count++;
        }

        //저장한 배열로 제이슨 데이터 생성 및 파일 저장
        StageDatas saveData = new StageDatas(saveDatas);
        string saveDataJson = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(STAGEDATAPATH, saveDataJson);


        //성공 메세지 반환
        _callback?.Invoke(CreateStageEvent.SuccessCreate);
    }

    #endregion
    #region Save
    //플레이어 데이터 저장
    public void SavePlayerData(PlayerData _playerData)
    {
        //플레이어 데이터가 들어오지 않았으면 리턴
        if (_playerData == null) return;

        //처음 플레이한 플레이어가 아닐 경우
        if(playerSaveDatas.TryGetValue(_playerData.ID, out PlayerSaveData saveData))
        {
            //기존 데이터에 바뀐 데이터를 업데이트
            string hasRangerUIDs = string.Empty;
            for (int i = 0; i < _playerData.hasRangers.Count; i++)
            {
                if (i != _playerData.hasRangers.Count - 1)
                {
                    hasRangerUIDs += (_playerData.hasRangers[i].UID + ",");
                    continue;
                }
                hasRangerUIDs += (_playerData.hasRangers[i].UID);
            }
            saveData.name = _playerData.name;
            saveData.hasRangerUID = hasRangerUIDs;
            saveData.lastClearStageUID = _playerData.lastClearStageUID;
        }

        //데이터를 저장할 배열 재생성
        PlayerSaveData[] saveDataArray = new PlayerSaveData[playerSaveDatas.Count];
        int count = 0;

        //배열의 저장
        foreach (var data in playerSaveDatas)
        {
            saveDataArray[count] = data.Value;
            count++;
        }

        //저장한 배열로 제이슨 데이터 생성 및 파일 저장
        PlayerSaveDatas saveDatas = new PlayerSaveDatas(saveDataArray);
        string saveDataJson = JsonUtility.ToJson(saveDatas, true);
        File.WriteAllText(PLAYERSAVEDATAPATH, saveDataJson);
    }
    #endregion
}



public class Data
{

}

[System.Serializable]
public class PlayerData : Data
{
    public string ID;
    public string name;
    public int level;
    public int exp;
    public int gold;
    public int gem;
    public int upgradeCost;
    public List<RangerInfoData> hasRangers;
    public int lastClearStageUID;

    public PlayerData(string _ID, string _name, int _level, int _exp, int _gold, int _gem, int _upgradeCost, List<RangerInfoData> _hasRangers, int _lastClearStageUID)
    {
        ID = _ID;
        name = _name;
        level = _level;
        exp = _exp;
        gold = _gold;
        gem = _gem;
        upgradeCost = _upgradeCost;
        hasRangers = _hasRangers;
        lastClearStageUID = _lastClearStageUID;
    }
}

[System.Serializable]
public class PlayerSaveData : Data
{
    public string ID;
    public string passward;
    public string name;
    public int level;
    public int exp;
    public int gold;
    public int gem;
    public int upgradeCost;
    public string hasRangerUID;
    public int lastClearStageUID;

    public PlayerSaveData(string _ID, string _passward , string _name, int _level, int _exp, int _gold, int _gem, int _upgradeCost, string _hasRangerUID, int _lastClearStageUID)
    {
        ID = _ID;
        passward = _passward;
        name = _name;
        level = _level;
        exp = _exp;
        gold = _gold;
        gem = _gem;
        upgradeCost = _upgradeCost;
        hasRangerUID = _hasRangerUID;
        lastClearStageUID = _lastClearStageUID;
    }
}

[System.Serializable]
public class PlayerSaveDatas : Data
{
    public PlayerSaveData[] playerSaveDatas;

    public PlayerSaveDatas(PlayerSaveData[] _playerSaveDatas)
    {
        playerSaveDatas = _playerSaveDatas;
    }
}

[System.Serializable]
public class KnightageData
{
    public int level;
    public int maxBattleEntityCount;
}

[System.Serializable]
public class KnightageDatas
{
    public KnightageData[] knightageDatas;
}


[System.Serializable]
public class DialogData : Data
{
    public int dialogUID;
    public string speakerName;
    public string speakerImageKey;
    public string speakerType;
    public string sentence;
    public string buttonOneContent;
    public string buttonTwoContent;
    public string buttonThreeContent;
    public int nextDialogUID;
}

[System.Serializable]
public class BaseBattleEntityData : Data
{
    public int UID;
    public int level;

    public BaseBattleEntityData(int _UID, int _level)
    {
        UID = _UID;
        level = _level;
    }
}

[System.Serializable]
public class BattleEntityData : Data
{
    public int UID;
    public string name;
    public int level;
    public int maxHP;
    public int attackForce;
    public float skillCooltime;
    public int moveSpeed;
    public float canAttackDistance;
    public float attackCycle;
}

public class BattleEntityDatas : Data
{
    public BattleEntityData[] warrior;
    public BattleEntityData[] wizard;
    public BattleEntityData[] tank;
    public BattleEntityData[] enemyOne;
    public BattleEntityData[] enemyTwo;
    public BattleEntityData[] enemyThree;
}

[System.Serializable]
public class RangerDatas
{
    public RangerInfoData[] infoDatas;  
    public RangerControllerData[] controllersData;
}

[System.Serializable]
public class RangerInfoData : Data
{
    public int UID;                     //인덱스
    public string name;                 //이름
    public int cost;                    //코스트
    public string description;
    public string sentence;
}

[System.Serializable]
public class RangerControllerData : Data
{
    public int UID;                     //인덱스
    public string name;                 //이름
    public int cost;                    //코스트
    public string specialtyOne;            //소속One
    public string specialtyTwo;            //소속Two
    public float attackForce;           //공격력
    public float attackSpeed;           //공격속도
    public float attackDistance;        //공격 사거리
    public float criticalForce;         //크리티컬 배율
    public float criticalProbability;   //크리티컬 확률
    public float defenseForce;          //방어력
    public float hp;                    //체력
    public float moveSpeed;             //이동속도
    public float skillCooltime;         //스킬 쿨타임
}

[System.Serializable]
public class EnemyDatas
{
    public EnemyInfoData[] infoDatas;
    public EnemyControllerData[] controllersData;
}

[System.Serializable]
public class EnemyInfoData : Data
{
    public int UID;                     //인덱스
    public string name;                 //이름
    public int cost;                    //코스트
    public string description;
    public string sentence;
}

[System.Serializable]
public class EnemyControllerData : Data
{
    public int UID;                     //인덱스
    public string name;                 //이름
    public int cost;                    //코스트
    public float attackForce;           //공격력
    public float attackSpeed;           //공격속도
    public float attackDistance;        //공격 사거리
    public float criticalForce;         //크리티컬 배율
    public float criticalProbability;   //크리티컬 확률
    public float defenseForce;          //방어력
    public float hp;                    //체력
    public float moveSpeed;             //이동속도
    public float skillCooltime;         //스킬 쿨타임
}

[Serializable]
public class StageData : Data
{
    public int UID;
    public string stageName;
    public string stageNumber;    
    public int canUseCost;
    public string enemyUIDs;
    public int clearRewardGold;
    public int clearRewardEXP;
    public int clearRewardUpgradeCost;
    public int clearRewardGem;

    public StageData(int _UID, string _stageName, int _canUseCost, string _enemyUIDs)
    {
        UID = _UID;
        stageName = _stageName;
        canUseCost = _canUseCost;
        enemyUIDs = _enemyUIDs;
    }
}

[Serializable]
public class StageDatas : Data
{
    public StageData[] stageDatas;
    public StageDatas(StageData[] _stageDatas)
    {
        stageDatas = _stageDatas;
    }
}

[Serializable]
public class ClearRewardData : Data
{
    public int gold;
    public int exp;
    public string getRangerUIDs;
}

[Serializable]
public class ClearRewardDatas : Data
{
    public ClearRewardData[] clearRewardDatas;
    public ClearRewardDatas(ClearRewardData[] _clearRewardDatas)
    {
        clearRewardDatas = _clearRewardDatas;
    }
}

[Serializable]
public class SpecialtyData : Data
{
    public int UID;
    public int needSpecialtyCount;
    public string description;
}
