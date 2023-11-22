using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static Define;

public class StageManager
{
    public void CreateStage(string _stageUID, string _stageName, string _canUseCost,
                            string _enemyOneUID, string _enemyTwoUID, string _enemyThreeUID,
                            string _enemyFourUID, string _enemyFiveUID, string _enemySixUID,
                            string _enemySevenUID, string _enemyEightUID, string _enemyNineUID, Action<CreateStageEvent> _callback)
    {
        if (_stageUID == string.Empty)
        {
            _callback?.Invoke(CreateStageEvent.NotInputUID);
            return;
        }

        if(!Int32.TryParse(_stageUID, out int intStageUID))
        {
            _callback?.Invoke(CreateStageEvent.UIDIsNotInt);
            return;
        }

        if(_stageName == string.Empty)
        {
            _callback?.Invoke(CreateStageEvent.NotInputName);
            return;
        }

        if(_canUseCost == string.Empty)
        {
            _callback?.Invoke(CreateStageEvent.NotInputCanUseCost);
            return;
        }

        if(!Int32.TryParse(_canUseCost, out int intCanUseCost))
        {
            _callback?.Invoke(CreateStageEvent.CostIsNotInt);
            return;
        }


        StringBuilder stringBuilder = new StringBuilder($"{_enemyOneUID},{_enemyTwoUID},{_enemyThreeUID},{_enemyFourUID},{_enemyFiveUID},{_enemySixUID},{_enemySevenUID},{_enemyEightUID},{_enemyNineUID}");
        Managers.Data.CreateStageData(intStageUID, _stageName, intCanUseCost, stringBuilder.ToString(), _callback); ;
    }

    public void ClearStage()
    {
        //조건에 맞는 리워드 함수 호출
    }

    public void GetClearRewardInfo(int _UID)
    {
        switch(_UID)
        {
            case 0:
                break;
        }
    }

    public void ClearReward(int _UID)
    {
        switch (_UID)
        {
            case 0:
                break;
        }
    }

    //public void OneStarReward(int _UID)
    //{
    //    switch (_UID)
    //    {
    //        default:

    //            break;
    //    }
    //}

    //public void TwoStarReward(int _UID)
    //{
    //    switch (_UID)
    //    {
    //        default:

    //            break;
    //    }
    //}


    //public void ThreeStarReward(int _UID)
    //{
    //    switch (_UID)
    //    {
    //        default:

    //            break;
    //    }
    //}
}
