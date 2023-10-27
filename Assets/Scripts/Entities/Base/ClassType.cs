using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Base/Class")]
public class ClassType : ScriptableObject
{
    enum ClASS
    {
        KNIGHT,
        RANGER,
        THIEF,
        MAGE
    }

    [SerializeField] string _className;
    public string className => _className;

    [SerializeField] Sprite _classImage;
    public Sprite classImage => _classImage;
}
