using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using UnityEditor.Rendering;

public class GachaSystem
{
    // 현재 탭에서 접근할 가챠 테이블
    private GachaTable[] gachaTables;
    public GachaTable currentTable;

    // 결과창 UI용 뽑은 개수 정리
    private Queue<string> obtainedRangers;
    private int[] lastObtainedRangers;

    public bool Init()
    {
        obtainedRangers = new Queue<string>(10);
        lastObtainedRangers = new int[10];

        currentTable = new GachaTable();
        currentTable.Init(Managers.Resource.Load<TextAsset>("RecruitmentRegularData"));

        Debug.Log("가챠 시스템 초기화 완료");

        return true;
    }

    // _gachaCount 만큼 뽑기가 가능한지 체크하고 실행
    public bool TryGacha(int _gachaCount = 1)
    {
        // 재화 개수 검사
        bool isEnable = true; // _gachaCount * 120 <= Managers.Data.playerData.gem ? true : false;

        if (isEnable)
        {
            return true;
        }

        return false;
    }

    // 뽑기 실행 ( 레어도 확정부터 유닛 뽑기까지 )
    public void StartGacha(int _gachaCount = 1)
    {
        double randomSeed = UnityEngine.Random.Range(0, 100f);
        var rarities = Enum.GetValues(typeof(RangerRarity));

        for (int i = 0; i < rarities.Length; i++)
        {
            if (i == 0)
            {
                // 처음 값이 0일 경우 예외
                if (currentTable.globalProbabilities[i] == 0)
                    continue;

            }
            else
            {
                // 이전 값과 현재 값이 같을 경우 예외
                if (currentTable.globalProbabilities[i - 1] == currentTable.globalProbabilities[i])
                    continue;

            }

            if (randomSeed <= currentTable.globalProbabilities[i])
            {
                // Debug.Log($"1. 나온 숫자 : {randomSeed}, 얻은 확률 : {currentTable.globalProbabilities[i]}");
                StartGachaByRarity((RangerRarity)rarities.GetValue(i));
                break;
            }

        }
    }

    // _gachaCount 만큼 뽑기 실행
    public void StartGachaAll(int _gachaCount)
    {
        for (int i = 0; i < _gachaCount; i++)
        {
            StartGacha();
        }

        CompleteGacha();
    }

    // 풀에서 뽑기 실행
    public void StartGachaByRarity(RangerRarity _rarity)
    {
        GachaPool rarityPool = null;

        switch (_rarity)
        {
            case RangerRarity.Common:
                rarityPool = currentTable.commonPool;
                break;
            case RangerRarity.Rare:
                rarityPool = currentTable.rarePool;
                break;
            case RangerRarity.Epic:
                rarityPool = currentTable.epicPool;
                break;
            case RangerRarity.Legendary:
                rarityPool = currentTable.legendaryPool;
                break;
        }

        // 난수 생성
        double randomSeed = UnityEngine.Random.Range(0, 100f);

        for (int i = 0; i < rarityPool.datas.Length; i++)
        {
            if (randomSeed <= rarityPool.datas[i].probability)
            {
                // obtainedRangers.Enqueue(Managers.Data.GetRangerInfoData(rarityPool.datas[i].UID));
                // Debug.Log($"2. 나온 숫자 : {randomSeed}, 얻은 확률 : {rarityPool.datas[i].probability}");
                obtainedRangers.Enqueue(rarityPool.datas[i].UID.ToString());
                break;
            }
        }
    }

    public void CompleteGacha()
    {
        // 마지막 가챠 배열 초기화
        for (int i = 0; i < lastObtainedRangers.Length; i++)
        {
            lastObtainedRangers[i] = 0;
        }

        int imax = obtainedRangers.Count;

        // obtainedRangers 에 등록된 만큼 완료 처리
        for (int i = 0; i < imax; i++)
        {
            lastObtainedRangers[i] = Int32.Parse(obtainedRangers.Peek());
            Debug.Log($"{i + 1}번째 뽑기 : {obtainedRangers.Dequeue()} 획득!");
        }
    }

    public int[] GetGachaResult()
    {
        return lastObtainedRangers;
    }
}

[System.Serializable]
public class GachaTable
{
    public GachaTableDatas data;

    public double[] globalProbabilities;
    public float globalCommonProbability = 0;
    public float globalRareProbability = 0;
    public float globalEpicProbability = 0;
    public float globalLegendaryProbability = 0;

    // 각각의 유닛 풀에 들어가는 유닛과 확률 목록
    public GachaPool commonPool;
    public GachaPool rarePool;
    public GachaPool epicPool;
    public GachaPool legendaryPool;

    public bool Init(TextAsset textAsset)
    {
        // TextAsset textAsset = Managers.Resource.Load<TextAsset>("RegularRecruitmentData");
        if (textAsset == null)
        {
            Debug.Log(textAsset + "is null");
            return false;

        }
        else
            data = JsonUtility.FromJson<GachaTableDatas>(textAsset.text);

        // 1. 전역 확률 저장
        globalCommonProbability = data.globalData[0].commonProbability;
        globalRareProbability = data.globalData[0].rareProbability;
        globalEpicProbability = data.globalData[0].epicProbability;
        globalLegendaryProbability = data.globalData[0].legendaryProbability;

        var rarities = Enum.GetValues(typeof(RangerRarity));

        globalProbabilities = new double[rarities.Length];

        globalProbabilities[0] = globalCommonProbability;
        globalProbabilities[1] = globalProbabilities[0] + globalRareProbability;
        globalProbabilities[2] = globalProbabilities[1] + globalEpicProbability;
        globalProbabilities[3] = globalProbabilities[2] + globalLegendaryProbability;

        // 전역 확률 무결성 검사
        if (globalCommonProbability + globalRareProbability + globalEpicProbability + globalLegendaryProbability != 100)
        {
            Debug.LogError($"Error : {data.globalData[0].name} Register Failed. \n전역 확률의 총합이 100이 아닙니다. \n총합 : {globalCommonProbability + globalRareProbability + globalEpicProbability + globalLegendaryProbability}");
            return false;
        }

        // 2. 유닛 시트의 유닛 저장
        int commonLength = 0;
        int rareLength = 0;
        int epicLength = 0;
        int legendaryLength = 0;

        float percent = 100f;
        float globalCommonProbabilityCheck = percent;
        float globalRareProbabilityCheck = percent;
        float globalEpicProbabilityCheck = percent;
        float globalLegendaryProbabilityCheck = percent;

        List<GachaPoolData> commonPickupList = new List<GachaPoolData>();
        List<GachaPoolData> rarePickupList = new List<GachaPoolData>();
        List<GachaPoolData> epicPickupList = new List<GachaPoolData>();
        List<GachaPoolData> legendaryPickupList = new List<GachaPoolData>();

        // 픽업 유닛 확률 등록
        foreach (var item in data.poolDatas)
        {
            RangerRarity rarity;
            bool isAvailable = Enum.TryParse(item.rarityPool, out rarity);

            // 레어도 명시가 잘못되어 있을 경우 실패
            if (!isAvailable)
            {
                return false;
            }

            // 픽업캐 처리
            switch (rarity)
            {
                case RangerRarity.Common:
                    if (item.pickupProbability > 0)
                        commonPickupList.Add(new GachaPoolData(item.UID, percent - (globalCommonProbabilityCheck - item.pickupProbability)));

                    globalCommonProbabilityCheck -= item.pickupProbability;
                    commonLength++;
                    break;
                case RangerRarity.Rare:
                    if (item.pickupProbability > 0)
                        rarePickupList.Add(new GachaPoolData(item.UID, percent - (globalRareProbabilityCheck - item.pickupProbability)));

                    globalRareProbabilityCheck -= item.pickupProbability;
                    rareLength++;
                    break;
                case RangerRarity.Epic:
                    if (item.pickupProbability > 0)
                        epicPickupList.Add(new GachaPoolData(item.UID, percent - (globalEpicProbabilityCheck - item.pickupProbability)));

                    globalEpicProbabilityCheck -= item.pickupProbability;
                    epicLength++;
                    break;
                case RangerRarity.Legendary:
                    if (item.pickupProbability > 0)
                        legendaryPickupList.Add(new GachaPoolData(item.UID, percent - (globalLegendaryProbabilityCheck - item.pickupProbability)));

                    globalLegendaryProbabilityCheck -= item.pickupProbability;
                    legendaryLength++;
                    break;
            }

            // 남은 확률이 0 이하일 경우 실패
            if (globalCommonProbabilityCheck <= 0 || globalRareProbabilityCheck <= 0 || globalEpicProbabilityCheck <= 0 || globalLegendaryProbabilityCheck <= 0)
            {
                Debug.LogError($"Error : {data.globalData[0].name} Register Failed. \n{rarity}의 전역 확률이 0보다 적습니다. \n마지막으로 확인된 유닛 확률 : {item.UID}의 {item.pickupProbability}");
                return false;
            }
        }

        commonPool = new GachaPool(commonLength);
        rarePool = new GachaPool(rareLength);
        epicPool = new GachaPool(epicLength);
        legendaryPool = new GachaPool(legendaryLength);

        // 각 픽업 캐릭터를 먼저 확률에 넣기
        for (int i = 0; i < commonPickupList.Count; i++)
        {
            commonPool.datas[i] = commonPickupList[i];
        }

        for (int i = 0; i < rarePickupList.Count; i++)
        {
            rarePool.datas[i] = rarePickupList[i];
        }

        for (int i = 0; i < epicPickupList.Count; i++)
        {
            epicPool.datas[i] = epicPickupList[i];
        }

        for (int i = 0; i < legendaryPickupList.Count; i++)
        {
            legendaryPool.datas[i] = legendaryPickupList[i];
        }

        int commonRemain = commonLength - commonPickupList.Count;
        int rareRemain = rareLength - rarePickupList.Count;
        int epicRemain = epicLength - epicPickupList.Count;
        int legendaryRemain = legendaryLength - legendaryPickupList.Count;

        double eachCommonProbability = globalCommonProbabilityCheck / commonRemain;
        double eachRareProbability = globalRareProbabilityCheck / rareRemain;
        double eachEpicProbability = globalEpicProbabilityCheck / epicRemain;
        double eachLegendaryProbability = globalLegendaryProbabilityCheck / legendaryRemain;

        foreach (var item in data.poolDatas)
        {
            // 일반 캐 처리
            if (item.pickupProbability == 0)
            {
                RangerRarity rarity;
                bool isAvailable = Enum.TryParse(item.rarityPool, out rarity);

                double probability = 0d;

                switch (rarity)
                {
                    case RangerRarity.Common:
                        if (commonLength - commonRemain == 0)
                        {
                            commonPool.datas[0] = new GachaPoolData(item.UID, eachCommonProbability);
                            commonRemain--;
                            continue;
                        }

                        probability = commonPool.datas[commonLength - commonRemain - 1].probability + eachCommonProbability;
                        commonPool.datas[commonLength - commonRemain] = new GachaPoolData(item.UID, probability);
                        commonRemain--;
                        break;
                    case RangerRarity.Rare:
                        if (rareLength - rareRemain == 0)
                        {
                            rarePool.datas[0] = new GachaPoolData(item.UID, eachRareProbability);
                            rareRemain--;
                            continue;
                        }

                        probability = rarePool.datas[rareLength - rareRemain - 1].probability + eachRareProbability;
                        rarePool.datas[rareLength - rareRemain] = new GachaPoolData(item.UID, probability);
                        rareRemain--;
                        break;
                    case RangerRarity.Epic:
                        if (epicLength - epicRemain == 0)
                        {
                            epicPool.datas[0] = new GachaPoolData(item.UID, eachEpicProbability);
                            epicRemain--;
                            continue;
                        }

                        probability = epicPool.datas[epicLength - epicRemain - 1].probability + eachEpicProbability;
                        epicPool.datas[epicLength - epicRemain] = new GachaPoolData(item.UID, probability);
                        epicRemain--;
                        break;
                    case RangerRarity.Legendary:
                        if (legendaryLength - legendaryRemain == 0)
                        {
                            legendaryPool.datas[0] = new GachaPoolData(item.UID, eachLegendaryProbability);
                            legendaryRemain--;
                            continue;
                        }

                        probability = legendaryPool.datas[legendaryLength - legendaryRemain - 1].probability + eachLegendaryProbability;
                        legendaryPool.datas[legendaryLength - legendaryRemain] = new GachaPoolData(item.UID, probability);
                        legendaryRemain--;
                        break;
                }
            }
        }

        return true;
    }
}

[System.Serializable]
public class GachaTableDatas
{
    public GachaTableGlobalData[] globalData;
    public GachaTablePoolData[] poolDatas;
}

[System.Serializable]
public class GachaTableGlobalData : Data
{
    public int UID;                             //인덱스 (가챠 테이블)
    public string name;                         //이름 (가챠 테이블)
    public string description;                  //설명 (가챠 테이블)

    // 각 레어도의 합은 100이 되도록 작성
    public float commonProbability;            //Common 전역 확률  
    public float rareProbability;              //Rare 전역 확률
    public float epicProbability;              //Epic 전역 확률
    public float legendaryProbability;         //Legendary 전역 확률
}

[System.Serializable]
public class GachaTablePoolData : Data
{
    public int UID;                            //인덱스
    public string rarityPool;                  //레어도 풀
    public float pickupProbability;            //픽업 확률 (디폴트 : 0)
}

[System.Serializable]
public class GachaPool
{
    public GachaPoolData[] datas;

    public GachaPool(int _length)
    {
        datas = new GachaPoolData[_length];
    }
}

[System.Serializable]
public class GachaPoolData
{
    public int UID;
    public double probability;

    public GachaPoolData(int _UID, double _probability)
    {
        UID = _UID;
        probability = _probability;
    }
}

[System.Serializable]
public class GachaChart
{
    public GachaChartData[] chartDatas;
}

[System.Serializable]
public class GachaChartData : Data
{
    public int UID;                            //인덱스
    public string rarityPool;                  //레어도 풀
    public float probabilityForUser;           //유저에게 보이는 확률(소숫점 3자리까지)
}

public enum RangerRarity
{
    Common, Rare, Epic, Legendary
}