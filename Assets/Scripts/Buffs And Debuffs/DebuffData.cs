using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffData : ScriptableObject
{
    [SerializeField] string _name;
    public string debuffName => _name;

    [SerializeField] string _desc;
    public string desc => desc;

    [SerializeField] Sprite _icon;
    public Sprite icon => _icon;

    [SerializeField] float _multiplier;
    public float multiplier => _multiplier;

    public virtual void ApplyEffect(EntityBase source, EntityBase dest)
    {

    }
}
