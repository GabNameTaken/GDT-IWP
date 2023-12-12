using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Common.DesignPatterns;

public class CombatManager : Singleton<CombatManager>
{
    [SerializeField] TurnOrderUI turnOrderUI;
    public TurnCharge turnCharge;

    public List<EntityBase> entitiesOnField;
    public List<PlayableCharacter> playerParty = new();
    public List<Enemy> enemyParty = new();

    public event System.Action<EntityBase> EntityDeadEvent;
    public void CallEntityDeadEvent(EntityBase entity) => EntityDeadEvent?.Invoke(entity);

    public event System.Action WaveClearedEvent;

    public void StartBattle(CombatZone combatZone)
    {
        entitiesOnField.AddRange(playerParty);
        entitiesOnField.AddRange(enemyParty);
        foreach (EntityBase entity in entitiesOnField)
            entity.turnMeter = UnityEngine.Random.Range(0, 10);

        OnBattleStart();
        
        CombatUIManager.Instance.SetUpPlayerUI(playerParty);

        turnOrderUI.AddFighters(entitiesOnField);
        IncreaseTurnMeter(CalculateNumberOfIncrease());
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
        if (stealTurn)
            return;
        foreach (EntityBase entity in entitiesOnField)
        {
            entity.turnMeter += (float)(entity.trueStats.speed / 100f) * numberOfIncrease;
            //Debug.Log(entity.name + ": " + entity.turnMeter);
        }
        turnOrderUI.UpdateTurnOrder();
        SetTurn();
    }

    public bool isPlayerTurn = false;
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
                CameraManager.Instance.MoveCamera(entity.gameObject, CAMERA_POSITIONS.LOW_BACK, 1f);
                isPlayerTurn = true;
            }
            entity.TakeTurn();
        }
    }

    bool hasWon = false;
    public void EndTurn(EntityBase currentTurn)
    {
        currentTurn.isMoving = false;
        currentTurn.turnMeter = currentTurn.excessTurnMeter; currentTurn.excessTurnMeter = 0;

        isPlayerTurn = false;

        CheckForEndBattle();

        if (hasWon)
        { 
            //end battle
        }
        if (stealTurn)
            turnCharge.SelectTurn();
        else
            IncreaseTurnMeter(CalculateNumberOfIncrease());
    }

    public bool stealTurn = false;
    public void StealNextTurn(EntityBase entity)
    {
        CameraManager.Instance.MoveCamera(entity.gameObject, CAMERA_POSITIONS.LOW_BACK, 1f);
        entity.TakeTurn();

        isPlayerTurn = true;
        stealTurn = false;
    }

    
    void CheckForEndBattle()
    {
        bool enemiesAlive = false, playersAlive = false;
        foreach (var player in playerParty)
        {
            if (!player.isDead)
            {
                playersAlive = true;
                break;
            }
        }

        foreach (EntityBase enemy in enemyParty)
        {
            if (!enemy.isDead)
            {
                enemiesAlive = true;
                break;
            }
        }

        if (!enemiesAlive || !playersAlive)
            EndBattle(playersAlive);
    }

    void EndBattle(bool cleared)
    {
        OnBattleEnd();

        if (cleared) WaveClearedEvent?.Invoke();
    }

    void OnBattleStart()
    {
        foreach (EntityBase entity in entitiesOnField)
            entity.OnBattleStart();
    }

    void OnBattleEnd()
    {
        foreach (EntityBase entity in entitiesOnField)
            entity.OnBattleEnd();
    }
}
