using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    public Animator animator;

    public Stats stats;
    public float turnMeter;
    public bool isMoving;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
}
