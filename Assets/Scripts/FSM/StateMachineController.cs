using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineController : MonoBehaviour
{
    public EntityBase self;
    public List<EntityBase> listOfTargets;

    [SerializeField] private BaseState _initialState;

    private void Awake()
    {
        currentState = _initialState;
    }

    public BaseState currentState { get; set; }

    private void Update()
    {
        currentState.Execute(this);
    }
}
