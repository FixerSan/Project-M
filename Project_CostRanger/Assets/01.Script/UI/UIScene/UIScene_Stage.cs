using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UIScene_Stage : UIScene
{
    public override bool Init()
    {
        if(!base.Init()) return false;
        
        return true;
    }
}
