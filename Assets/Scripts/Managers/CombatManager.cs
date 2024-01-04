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

    private List<PlayableCharacter> playerParty = new();
    public List<PlayableCharacter> PlayerParty
    {
        get { return playerParty; }
        set { playerParty = value; AssignEntitiesOnField(); }
    }
    private List<Enemy> enemyParty = new();
    public List<Enemy> EnemyParty
    {
        get { return enemyParty; }
        set { enemyParty = value; AssignEntitiesOnField(); }
    }

    public event System.Action<EntityBase> EntityStartTurnEvent;
    public void CallEntityStartTurnEvent(EntityBase entity) => EntityStartTurnEvent?.Invoke(entity);

    public event System.Action<EntityBase> EntityEndTurnEvent;
    public void CallEntityEndTurnEvent(EntityBase entity) => EntityEndTurnEvent?.Invoke(entity);

    public event System.Action<EntityBase> EntityDeadEvent;
    public void CallEntityDeadEvent(EntityBase entity) => EntityDeadEvent?.Invoke(entity);

    public event System.Action WaveClearedEvent;
    private event System.Action<bool> battleEndedEvent;

    public void StartBattle(CombatZone combatZone)
    {
        foreach (EntityBase entity in entitiesOnField)
            entity.TurnMeter = UnityEngine.Random.Range(0, 10);

        OnBattleStart();
        CombatUIManager.Instance.SetUpPlayerUI(playerParty);
        IncreaseTurnMeter(CalculateNumberOfIncrease());
    }

    void AssignEntitiesOnField()
    {
        List<EntityBase> newEntitiesOnField = new List<EntityBase>();
        newEntitiesOnField.AddRange(playerParty);
        newEntitiesOnField.AddRange(enemyParty);

        var entitiesToAdd = newEntitiesOnField.Except(entitiesOnField);
        var entitiesToRemove = entitiesOnField.Except(newEntitiesOnField);

        // Remove entities not present in newEntitiesOnField
        entitiesOnField.RemoveAll(entity => entitiesToRemove.Contains(entity));

        // Add new entities not present in entitiesOnField
        entitiesOnField.AddRange(entitiesToAdd);

        turnOrderUI.AssignFighters(entitiesOnField);
    }

    float CalculateNextTurn(EntityBase entity)
    {
        float turnMeterRate = (float)entity.trueStats.speed / 100f;
        float numberOfIncrease = (100f - entity.TurnMeter) / turnMeterRate;
        //Debug.Log(entity.name + ": " + numberOfIncrease);

        return numberOfIncrease;
    }

    float CalculateNumberOfIncrease()
    {
        float numberOfIncrease = -1;
        foreach (EntityBase entity in entitiesOnField)
        {
            entity.isMoving = false;
            if (entity.IsDead)
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
            if (entity.IsDead) continue;
            entity.TurnMeter += (float)(entity.trueStats.speed / 100f) * numberOfIncrease;
        }
        UpdateTurnOrderUI();
        SetTurn();
    }

    public void UpdateTurnOrderUI()
    {
        turnOrderUI.UpdateTurnOrder();
    }

    public bool isPlayerTurn = false;
    void SetTurn()
    {
        bool anyEntityMoving = false;

        // Check if any entity has isMoving set to true
        foreach (EntityBase entity in entitiesOnField)
        {
            if (entity.isMoving && !entity.IsDead)
            {
                anyEntityMoving = true;
                break; // Exit the loop early since we found one entity with isMoving = true
            }
        }

        if (!anyEntityMoving)
        {
            EntityBase entity = entitiesOnField.Where(entity => !entity.IsDead).OrderByDescending(entity => entity.TurnMeter).FirstOrDefault();

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
        currentTurn.TurnMeter = currentTurn.excessTurnMeter; currentTurn.excessTurnMeter = 0;

        isPlayerTurn = false;

        CallEntityEndTurnEvent(currentTurn);
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
            if (!player.IsDead)
            {
                playersAlive = true;
                break;
            }
        }

        foreach (EntityBase enemy in enemyParty)
        {
            if (!enemy.IsDead)
            {
                enemiesAlive = true;
                break;
            }
        }

        if (!enemiesAlive || !playersAlive)
            WaveEnded(playersAlive);
    }

    void WaveEnded(bool cleared)
    {
        turnOrderUI.EmptyTurnOrder();
        if (cleared)
        {
            enemyParty.ForEach(enemy => Destroy(enemy.gameObject)); enemyParty.Clear();
            WaveClearedEvent?.Invoke(); // If wave successfully cleared
        }
        else battleEndedEvent?.Invoke(false); // If wave failed
    }

    public void CallBattleWon()
    {
        battleEndedEvent?.Invoke(true);
    }

    void OnEndBattle(bool won)
    {
        OnBattleEnd();

        UIManager uiManager = UIManager.Instance;
        if (won) // Win condition
        {
            uiManager.SetWinScreenActive(true);
        }
        else // Lose condition
        {
            uiManager.SetLoseScreenActive(true);
        }
    }

    void OnBattleStart()
    {
        foreach (EntityBase entity in entitiesOnField)
            entity.OnBattleStart();

        battleEndedEvent += OnEndBattle;
    }

    void OnBattleEnd()
    {
        foreach (EntityBase entity in entitiesOnField)
            entity.OnBattleEnd();

        battleEndedEvent -= OnEndBattle;
    }
}
