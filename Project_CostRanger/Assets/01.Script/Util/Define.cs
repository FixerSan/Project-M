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
    public static readonly float sceneStartDelay = 3;
    public static readonly float skillScreenTime = 1;
    public static readonly Color stageinfoDrawedSlotColor = new Color(1,1,1,1);
    public static readonly Color stageinfoNotDrawedSlotColor = new Color(1,1,1,0.5f);

    //공격 시간
    public static readonly float magicAndNormalAttackBeforeTime = 0.24f;
    public static readonly float magicAndNormalAttackAfterTime = 0.16f;

    public static readonly float bowAttackBeforeTime = 0.28f;
    public static readonly float bowAttackAfterTime = 0.12f;

    //스킬 시간
    public static readonly float normalSkillBeforeTime = 0.32f;
    public static readonly float normalSkillAfterTime = 0.28f;

    public static readonly float magicSkillBeforeTime = 0.45f;
    public static readonly float magicSkillAfterTime = 0.15f;

    public static readonly float bowSkillBeforeTime = 0.43f;
    public static readonly float bowSkillAfterTime = 0.17f;


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
        Ranger, Enemy
    }

    public enum BattleEntityState
    {
        Idle, Move, Follow, Attack, SkillCast, Die, EndBattle
    }
    
    public enum Ranger
    {
        TestRanger, UnemployedKnight, BoringSpearman, DullAxeman, StrangeAssassin, GoofyHammeman,
        ScaredThug, ClumsyArcher, LenientNinja, FieryArcher,
    }

    public enum RangerState
    {
        Stay, Idle, Move, Follow, Attack, SkillCast, Die, EndBattle
    }

    public enum Enemy
    {
        TestEnemyZero, TestEnemyOne, TestEnemyTwo, TestEnemyThree , TestEnemyFour , TestEnemyFive , TestEnemySix , TestEnemySeven , TestEnemyEight,
        DazedZombie, FarmerZombie, RedKnightZombie, BlackKnightZombie, SuperPowerZombie, HeroZombie, BerserkZombie, WitchZombie
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

        // 가챠 씬
        ,UIPopup_Gacha, UIPopup_GachaResult, UIPopup_GachaSlide, UIPopup_GachaChart, UIPopup_GachaAnimation
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

    public enum SoundType
    {
        BGM, UI, Effect, Max    
    }
}
