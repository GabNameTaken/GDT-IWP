using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class TurnMeter : MonoBehaviour
{
    EntityBase owner;
    [SerializeField] GameObject details;
    [SerializeField] TMP_Text characterNameText, meterText; 

    public void AssignEntity(EntityBase entity)
    {
        owner = entity;

        AssignDetails();
        owner.IsDeadChangedEvent += OnOwnerStatusChanged;
        owner.TurnMeterChangedEvent += OnOwnerTurnMeterChanged;
    }

    void AssignDetails()
    {
        characterNameText.text = owner.entity.entityName;
        meterText.text = ToPercentage(owner.TurnMeter);
    }

    void OnOwnerStatusChanged(bool status)
    {
        details.SetActive(status);
    }

    void OnOwnerTurnMeterChanged(float turnMeter)
    {
        meterText.text = ToPercentage(owner.TurnMeter);
        if (gameObject && gameObject.activeInHierarchy) transform.DOMove(CombatUIManager.Instance.GetSliderButtonPosition(turnMeter), 1, false);
    }

    private void OnEnable()
    {
        if (!owner)
            return;
        owner.IsDeadChangedEvent += OnOwnerStatusChanged;
        owner.TurnMeterChangedEvent += OnOwnerTurnMeterChanged;
    }

    private void OnDestroy()
    {
        owner.IsDeadChangedEvent -= OnOwnerStatusChanged;
        owner.TurnMeterChangedEvent -= OnOwnerTurnMeterChanged;
    }

    string ToPercentage(float turnMeter)
    {
        return ((int)turnMeter).ToString() + "%";
    }
}
