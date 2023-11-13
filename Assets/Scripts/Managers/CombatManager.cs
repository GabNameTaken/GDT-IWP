using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField] Camera battleCamera;
    [SerializeField] TurnOrderUI turnOrderUI;

    public List<EntityBase> entitiesOnField;
    public List<PlayableCharacter> playerParty = new();
    public List<Enemy> enemyParty = new();

    public void StartBattle(CombatZone combatZone)
    {
        playerParty.Clear();
        for (int i = 0; i < combatZone.teamSlots.Count; i++)
        {
            if (combatZone.teamSlots[i].childCount > 0)
            {
                playerParty.Add(combatZone.teamSlots[i].GetChild(0).GetComponent<PlayableCharacter>());
            }
        }

        entitiesOnField.AddRange(playerParty);
        entitiesOnField.AddRange(enemyParty);
        foreach (EntityBase entity in entitiesOnField)
            entity.turnMeter = 0;

        
        CombatUIManager.Instance.SetUpPlayerUI(playerParty);

        turnOrderUI.AddFighters(entitiesOnField);
        IncreaseTurnMeter(CalculateNumberOfIncrease());
        SetTurn();
    }

    float CalculateNextTurn(EntityBase entity)
    {
        float turnMeterRate = (float)entity.trueStats.speed / 100f;
        float numberOfIncrease = (100f - entity.turnMeter) / turnMeterRate;
        //Debug.Log(entity.name + ": " + numberOfIncrease);

        return numberOfIncrease;
    }

    float CalculateNumberOfIncrease()
    {
        float numberOfIncrease = -1;
        foreach (EntityBase entity in entitiesOnField)
        {
            entity.isMoving = false;
            if (entity.isDead)
                continue;

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
        {
            entity.turnMeter += (float)(entity.trueStats.speed / 100f) * numberOfIncrease;
            //Debug.Log(entity.name + ": " + entity.turnMeter);
        }
        turnOrderUI.UpdateTurnOrder();
    }

    void SetTurn()
    {
        bool anyEntityMoving = false;

        // Check if any entity has isMoving set to true
        foreach (EntityBase entity in entitiesOnField)
        {
            if (entity.isMoving && !entity.isDead)
            {
                anyEntityMoving = true;
                break; // Exit the loop early since we found one entity with isMoving = true
            }
        }

        if (!anyEntityMoving)
        {
            EntityBase entity = entitiesOnField.Where(entity => !entity.isDead).OrderByDescending(entity => entity.turnMeter).FirstOrDefault();

            if (entity.GetComponent<PlayableCharacter>())
            {
                battleCamera.transform.position = entity.transform.Find("CameraPosition").transform.position;
                battleCamera.transform.rotation = entity.transform.Find("CameraPosition").transform.rotation;

                entity.listOfTargets.Clear();
                foreach (Enemy enemy in enemyParty)
                {
                    if (!enemy.isDead)
                        entity.listOfTargets.Add(enemy);
                }
            }
            entity.TakeTurn();
        }
    }

    bool hasWon = false;
    public void EndTurn(EntityBase currentTurn)
    {
        currentTurn.isMoving = false;
        currentTurn.turnMeter = 0;

        CheckForEndBattle();

        if (hasWon)
        { 
            //end battle
        }
        IncreaseTurnMeter(CalculateNumberOfIncrease());
        SetTurn();
    }

    void CheckForEndBattle()
    {
        foreach (PlayableCharacter player in playerParty)
        {
            if (!player.isDead)
            {
                hasWon = true;
                break;
            }
        }

        foreach (EntityBase enemy in enemyParty)
        {
            if (!enemy.isDead)
            {
                hasWon = false;
            }
        }
    }
}
