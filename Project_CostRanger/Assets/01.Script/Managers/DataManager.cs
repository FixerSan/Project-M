using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;


public class DataManager
{
    public PlayerData playerData;

    public Dictionary<int, RangerControllerData> rangerControllerDatas;
    public Dictionary<int, RangerInfoData> rangerInfoDatas;
    public Dictionary<int, KnightageData> knightageDatas;
    public Dictionary<int, DialogData> dialogDatas;
    public Dictionary<int, StageData> stageDatas;
    
    public readonly string PLAYERSAVEDATA_PATH;

    //플레이어 데이터 반환
    public PlayerData GetPlayerData(int _UID)
    {
        return LoadPlayerData(_UID);
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

    //게임 기반 데이터 로드
    public void LoadPreData(Action _callback)
    {
        //GetPlayerData(Define.userUID);
        //LoadBattleEntityStatusData();
        //LoadStageData();
        LoadRangerData();
        _callback?.Invoke();
    }

    //플레이어 데이터 로드
    private PlayerData LoadPlayerData(int _UID)
    {
        if(playerData == null)
        {
            TextAsset textAsset = Managers.Resource.Load<TextAsset>("PlayerSaveData");
            PlayerSaveData saveData = JsonUtility.FromJson<PlayerSaveData>(textAsset.text);

            string[] entityUIDs = saveData.hasEntityUID.Split(',');
            string[] entityLevels = saveData.hasEntityLevel.Split(',');

            List<BaseBattleEntityData> entityDatas = new List<BaseBattleEntityData>();

            for (int i = 0; i < entityUIDs.Length; i++)
            {
                BaseBattleEntityData entityData = new BaseBattleEntityData(Int32.Parse(entityUIDs[i]), Int32.Parse(entityLevels[i]));
                entityDatas.Add(entityData);
            }

            playerData = new PlayerData(saveData.UID, saveData.name, saveData.knightageLevel, entityDatas);
        }
        return playerData;
    }


    //스테이지 데이터 로드
    public void LoadStageData()
    {
        StageDatas datas = Managers.Resource.Load<StageDatas>("OneChapterStageData.Data");
        for (int i = 0; i < datas.normal.Length; i++)
            stageDatas.Add(datas.normal[i].UID, datas.normal[i]);

        for (int i = 0; i < datas.hard.Length; i++)
            stageDatas.Add(datas.hard[i].UID, datas.hard[i]);
    }

    //플레이어 데이터 저장
    public void SavePlayerData(PlayerData _playerData)
    {
        string hasEntityUIDs = string.Empty;
        string hasEntityLevel = string.Empty;

        for (int i = 0; i < _playerData.hasEntites.Count; i++)
        {
            if(i == _playerData.hasEntites.Count-1)
            {
                hasEntityUIDs += (_playerData.hasEntites[i].UID);
                hasEntityLevel += (_playerData.hasEntites[i].level);
                continue;
            }
            hasEntityUIDs += (_playerData.hasEntites[i].UID + ",");
            hasEntityLevel += (_playerData.hasEntites[i].level + ",");
        }

        PlayerSaveData saveData = new PlayerSaveData(_playerData.UID, _playerData.name, _playerData.knightageLevel, hasEntityUIDs, hasEntityLevel);
        string saveDataJson = JsonUtility.ToJson(saveData);

        File.WriteAllText(PLAYERSAVEDATA_PATH, saveDataJson);
    }

    public void LoadRangerData()
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>("RangerData");
        RangerDatas rangerDatas = JsonUtility.FromJson<RangerDatas>(textAsset.text);

        for (int i = 0; i < rangerDatas.infoDatas.Length; i++)
            if (!rangerInfoDatas.TryAdd(rangerDatas.infoDatas[i].UID, rangerDatas.infoDatas[i]))
                Debug.LogError($"{i}번째 레인저 인포 데이터 로드에 실패하였습니다. ");

        for (int i = 0; i < rangerDatas.controllersData.Length; i++)
            if (!rangerControllerDatas.TryAdd(rangerDatas.controllersData[i].UID, rangerDatas.controllersData[i]))
                Debug.LogError($"{i}번째 레인저 컨트롤러 데이터 로드에 실패하였습니다. ");
    }

    public DataManager()
    {
        playerData = null;
        dialogDatas = new Dictionary<int, DialogData> ();
        rangerInfoDatas = new Dictionary<int, RangerInfoData> ();
        rangerControllerDatas = new Dictionary<int, RangerControllerData>();
        knightageDatas = new Dictionary<int,  KnightageData>();
        stageDatas = new Dictionary<int, StageData>();
        PLAYERSAVEDATA_PATH = Path.Combine(Application.dataPath + "/04.Datas/", "PlayerSaveData.txt");
    }
}



public class Data
{

}

[System.Serializable]
public class PlayerData : Data
{
    public int UID;
    public string name;
    public int knightageLevel;
    public List<BaseBattleEntityData> hasEntites;

    public PlayerData(int _UID, string _name, int _knightageLevel, List<BaseBattleEntityData> _hasEntites)
    {
        UID = _UID;
        name = _name;
        knightageLevel = _knightageLevel;
        hasEntites = _hasEntites;
    }
}

[System.Serializable]
public class PlayerSaveData : Data
{
    public int UID;
    public string name;
    public string hasEntityUID;
    public string hasEntityLevel;
    public int knightageLevel;

    public PlayerSaveData(int _UID, string _name, int _knightageLevel, string _hasEntityUID, string _hasEntityLevel)
    {
        UID = _UID;
        name = _name;
        knightageLevel = _knightageLevel;
        hasEntityUID = _hasEntityUID;
        hasEntityLevel = _hasEntityLevel;
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
}

[System.Serializable]
public class RangerControllerData : Data
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