using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Transition")]
public class Transition : ScriptableObject
{
    public Decision Decision;
    public BaseState TrueState;
    public BaseState FalseState;

    public void Execute(StateMachineController stateMachine)
    {
        if (Decision.Decide(stateMachine) && !(TrueState is FreezeState))
            stateMachine.currentState = TrueState;
        else if (!(FalseState is FreezeState))
            stateMachine.currentState = FalseState;
    }
}
