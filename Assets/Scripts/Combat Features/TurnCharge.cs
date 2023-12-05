using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnCharge : MonoBehaviour
{
    [SerializeField] int maxEther = 30;
    int turnCharge;
    int ether;

    [SerializeField] Slider slider;
    private void Awake()
    {
        slider.maxValue = maxEther;
    }

    void ConsumeCharge(int charge)
    {
        if (ether < 10)
            return;
        ConsumeEther(charge);

        //take turn
        CombatManager.Instance.stealTurn = true;
    }

    public void AddEther(int amount)
    {
        ether += amount;
        if (ether > maxEther)
            ether = maxEther;
        turnCharge = ether / 10;
        slider.value = ether;
    }

    public void ConsumeEther(int amount)
    {
        ether -= amount;
        if (ether < 0)
            ether = 0;
        turnCharge = ether / 10;
        slider.value = ether;
    }
}
