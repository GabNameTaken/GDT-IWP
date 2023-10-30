using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/State")]
sealed class State : BaseState
{
    [SerializeField] List<Action> actions;

    public override void Execute(StateMachineController stateMachine)
    {
        foreach (Action action in actions)
            action.Execute(stateMachine);
    }
}
