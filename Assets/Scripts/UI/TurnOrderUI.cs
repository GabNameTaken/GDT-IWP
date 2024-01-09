using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Linq;

public class TurnOrderUI : MonoBehaviour
{
    [SerializeField] Slider turnSlider;
    [SerializeField] GameObject turnMeterPrefab;

    List<EntityBase> unitsOnField = new();
    public void AssignFighters(List<EntityBase> listOfFighters)
    {
        foreach (EntityBase entity in listOfFighters)
        {
            if (unitsOnField.Contains(entity)) continue;

            GameObject turnMeterUI = Instantiate(turnMeterPrefab, transform);
            turnMeterUI.GetComponent<TurnMeter>().AssignEntity(entity);
            entity.turnMeterUI = turnMeterUI;
        }
        unitsOnField = new List<EntityBase>(listOfFighters);
    }

    public void EmptyTurnOrder()
    {
        transform.Cast<Transform>().ToList().ForEach(child =>
        {
            if (child != null)
            {
                Destroy(child.gameObject);
            }
        });
        unitsOnField = new();
    }

    void TrackSlider(GameObject go)
    {
        // Get the position of the Slider's handle (thumb) in screen space
        Vector3 handlePosition = turnSlider.handleRect.position;

        // Convert the handle's position to the local position of this GameObject
        Vector2 localPosition = transform.parent.InverseTransformPoint(handlePosition);

        // Set the position of this GameObject to match the handle's position
        go.transform.DOLocalMove(localPosition, 1, false);
    }

    public void UpdateTurnOrder()
    {
        foreach (EntityBase entity in unitsOnField)
        {
            if (entity.turnMeterUI != null)
            {
                turnSlider.value = Mathf.RoundToInt(entity.TurnMeter);
            }
        }
    }
}
