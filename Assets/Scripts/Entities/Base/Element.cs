using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Base/Element")]
public class Element : ScriptableObject
{
    enum ELEMENT
    {
        FIRE,
        WATER,
        WIND,
        VOID
    }

    [SerializeField] string _elementName;
    public string elementName => _elementName;

    [SerializeField] Sprite _elementImage;
    public Sprite elementImage => _elementImage;
}
