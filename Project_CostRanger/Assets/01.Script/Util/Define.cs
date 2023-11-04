using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{
    public static readonly int currentBattleEntityCount = 6;
    public static readonly int currentBattleEntityMaxLevel = 5;
    public static readonly int userUID = 0;

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

    public enum BattleEntityType
    {
        Army, Enemy
    }

    public enum BattleEntityState
    {
        Idle, Move, Follow, Attack, SkillCast, Die, EndBattle
    }

    public enum MainSpecialty
    {

    }
    public enum SubSpecialty
    {

    }

    public enum StageState
    {
        ZeroStar, OneStar, TwoStar, ThreeStar
    }

    public enum VoidEventType
    {
        OnChangeBattleInfo, OnChangeControllerStatus
    }

    public enum IntEventType
    {

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

    public enum SpeakerType
    {
        OneButton, 
        TwoButton, 
        ThreeButton
    }

    public enum Scene
    {
        Main, Stage
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
}
