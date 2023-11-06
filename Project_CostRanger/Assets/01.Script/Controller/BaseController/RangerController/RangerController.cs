using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerController : BaseController
{
    public Ranger ranger;
    public RangerStatus status;
    public StateMachine<Ranger> stateMachine;

    public void Init()
    {
        //status = new RangerStatus(this, )
    }
}

public class RangerStatus
{
    private RangerController controller;
    //public RangerStatus(RangerController _controller, )
    //{
    //    controller = _controller;
    //}
}
