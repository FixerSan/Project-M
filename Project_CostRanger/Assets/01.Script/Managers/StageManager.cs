using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager 
{
    public void CreateStage(string _stageName, string _canUseCost, 
                            string _)
    {
        





        Managers.Data.CreateStageData();
    }

    public void ClearStage()
    {
        //���ǿ� �´� ������ �Լ� ȣ��
    }

    public void ClearReward(int _UID)
    {

    }

    public void OneStarReward(int _UID)
    {
        switch (_UID)
        {
            default:

                break;
        }
    }

    public void TwoStarReward(int _UID)
    {
        switch (_UID)
        {
            default:

                break;
        }
    }


    public void ThreeStarReward(int _UID)
    {
        switch (_UID)
        {
            default:

                break;
        }
    }
}
