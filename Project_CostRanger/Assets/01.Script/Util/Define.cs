using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{
    public static readonly int currentBattleEntityCount = 6;
    public static readonly int currentBattleEntityMaxLevel = 5;
    public static readonly int userUID = 0;
    public static readonly Vector2 prepareEnemyOffset = new Vector2(0, -40);
    public static readonly Vector2 prepareRangerOffset = new Vector2(0, -60);
    public static readonly float attackAnimationTime = 1;
    public static readonly float fastSpeed = 3;
    public static readonly float normalAttackAnimationTime = 0.417f;
    public static readonly float skillAnimationTime = 1;
    public static readonly float normalSkillAnimationTime = 0.5f;
    public static readonly float sceneStartDelay = 3;
    public static readonly float skillScreenTime = 1;
    public static readonly Color stageinfoDrawedSlotColor = new Color(1,1,1,1);
    public static readonly Color stageinfoNotDrawedSlotColor = new Color(1,1,1,0.5f);

    public enum GameState
    {
        BattleBefore,
        BattleProgress,
        BattleAfter
    }

    public enum GameResult
    {
        Victory, Lose
    }
    
    public enum BattleEntity
    {
        Warrior = 0,
        Wizard = 1,
        Tank = 2,
        EnemyOne = 3,
        EnemyTwo = 4,
        EnemyThree = 5
    }

    public enum Direction
    {
        Left = -1,
        Right = 1
    }

    public enum BattleEntityType
    {
        Army, Enemy
    }

    public enum BattleEntityState
    {
        Idle, Move, Follow, Attack, SkillCast, Die, EndBattle
    }
    
    public enum Ranger
    {
        TestRanger
    }

    public enum RangerState
    {
        Stay, Idle, Move, Follow, Attack, SkillCast, Die, EndBattle
    }

    public enum Enemy
    {
        TestEnemyZero, TestEnemyOne, TestEnemyTwo, TestEnemyThree , TestEnemyFour , TestEnemyFive , TestEnemySix , TestEnemySeven , TestEnemyEight
    }

    public enum EnemyState
    {
        Stay, Idle, Move, Follow, Attack, SkillCast, Die, EndBattle
    }

    public enum SpecialtyType
    {
        Test
    }

    public enum StageState
    {
        ZeroStar, OneStar, TwoStar, ThreeStar
    }

    public enum VoidEventType
    {
        OnChangePrepare, OnChangeControllerStatus, OnChangeBattle, OnChangePlayerInfo, OnPlayerDead, OnEnemyDead
    }
    public enum IntEventType
    {

    }

    public enum LoginEvent
    {
        NotExistPlayerData, IncorrectPassward, SuccessLogin
    }

    public enum SignUpEvent
    {
        IDisNull, PasswardIsNull, ExistSameID, ExistSameNickName, PasswardNotSame, SuccessSignUp
    }

    public enum CreateStageEvent
    {
        NotInputUID, UIDIsNotInt , ExistSameUID , NotInputName, NotInputCanUseCost, CostIsNotInt, SuccessCreate
    }

    public enum StartBattleStageEvent
    {
        SuccessStart, RangerIsNotExist
    }

    public enum UIEventType
    {
        Click,
        Pressed,
        PointerDown,
        PointerUp,
        BeginDrag,
        Drag,
        EndDrag,
        Drop
    }

    public enum UIType
    {
        UIPopup_PrepareStage, UIPopup_Result, UIPopup_SignUp, UIPopup_WorldMap_ChapterOne, UIScene_CreateStage, UIScene_Login, UIScene_Main, UIScene_Stage, UIPopup_StageInfo
    }

    public enum RewardType
    {
        Gold, EXP, Gem, UpgradeCost
    }

    public enum SpeakerType
    {
        OneButton, 
        TwoButton, 
        ThreeButton
    }

    public enum Scene
    {
        Login, Main, Stage
    }

    public enum Buff
    {
        PlusAttackSpeed
    }

    public enum TextType
    {
        Damage, Heal, Normal
    }

    public enum PlaceType
    {
        Front, Center, Rear
    }

    public enum Batch
    {
        One, Two
    }
}
