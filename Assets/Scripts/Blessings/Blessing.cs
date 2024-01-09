using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blessing : ScriptableObject
{
    [SerializeField] string _blessingName;
    public string blessingName => _blessingName;


}
