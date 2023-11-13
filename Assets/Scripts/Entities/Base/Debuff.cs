using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuff : ScriptableObject
{
    [SerializeField] string _name;
    public string debuffName => _name;

    [SerializeField] string _desc;
    public string desc => desc;

    [SerializeField] Sprite _icon;
    public Sprite icon => _icon;

    [SerializeField] float _multiplier;
    public float multiplier => _multiplier;

    protected float scalingAmount;

    public int numOfTurns;

    public virtual void Apply(EntityBase entity)
    {
        if (entity.isMoving)
        {
            entity.TakeDamage(CalculateDamage(entity));
            RemoveDebuff();
        }
    }

    public virtual float CalculateDamage(EntityBase entity)
    {
        return multiplier * scalingAmount;
    }

    void RemoveDebuff()
    {
        numOfTurns--;
        if (numOfTurns <= 0)
        {
            Destroy(this);
        }
    }
}
