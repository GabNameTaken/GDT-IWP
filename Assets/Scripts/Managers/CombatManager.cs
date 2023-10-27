using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CombatManager : MonoBehaviour
{
    public List<EntityBase> entitiesOnField;

    void StartBattle()
    {
        foreach (EntityBase entity in entitiesOnField)
            entity.turnMeter = 0;
        IncreaseTurnMeter(CalculateNumberOfIncrease());
        SetTurn();
    }

    float CalculateNextTurn(EntityBase entity)
    {
        float turnMeterRate = entity.stats.speed / 100;
        float numberOfIncrease = (100 - entity.turnMeter) / turnMeterRate;

        return numberOfIncrease;
    }

    float CalculateNumberOfIncrease()
    {
        float numberOfIncrease = -1;
        foreach (EntityBase entity in entitiesOnField)
        {
            if (numberOfIncrease == -1)
            {
                numberOfIncrease = CalculateNextTurn(entity);
                continue;
            }

            float a = CalculateNextTurn(entity);
            if (a < numberOfIncrease)
                numberOfIncrease = a;

        }
        return numberOfIncrease;
    }

    void IncreaseTurnMeter(float numberOfIncrease)
    {
        foreach (EntityBase entity in entitiesOnField)
            entity.turnMeter += (entity.stats.speed / 100) * numberOfIncrease;
    }

    void SetTurn()
    {
        bool anyEntityMoving = false;

        // Check if any entity has isMoving set to true
        foreach (EntityBase entity in entitiesOnField)
        {
            if (entity.isMoving)
            {
                anyEntityMoving = true;
                break; // Exit the loop early since we found one entity with isMoving = true
            }
        }

        if (!anyEntityMoving)
        {
            foreach (EntityBase entity in entitiesOnField)
            {
                if (entity.turnMeter >= 100)
                {
                    entity.isMoving = true;
                    break; // Exit the loop after setting isMoving
                }
            }
        }
    }
}
