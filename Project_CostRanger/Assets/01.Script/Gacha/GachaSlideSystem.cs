using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GachaSlideSystem : MonoBehaviour
{
    public GachaSlideSequence sequence;

    public GachaEffect[] effects;

    public void NextSlide()
    {

    }

}

public class GachaSlideSequence
{
    public Queue<GachaSlide> slides;
}

public class GachaSlide
{

}

public class GachaEffect
{
    // 파티클 등 저장
}
