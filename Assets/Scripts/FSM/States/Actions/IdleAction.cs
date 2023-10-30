using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Action/Idle")]
public class IdleAction : Action
{
    public override void Execute(StateMachineController stateMachine)
    {
        stateMachine.self.animator.Play("idle");
    }
}
