using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCheck : ScriptableObject
{
    public virtual bool CheckState()
    {
        return false;
    }
}
