using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UISlot_PrepareRanger : UIBase
{
    public int slotIndex;
    public abstract void OnChanging();
}
