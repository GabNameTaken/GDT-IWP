using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TurnOrderUI : MonoBehaviour
{
    [SerializeField] Slider turnSlider;
    [SerializeField] GameObject turnMeterPrefab;

    List<EntityBase> unitsOnField = new();
    public void AddFighters(List<EntityBase> listOfFighters)
    {
        EmptyTurnOrder();

        foreach (EntityBase entity in listOfFighters)
        {
            GameObject turnMeterUI = Instantiate(turnMeterPrefab, transform);
            turnSlider.value = entity.turnMeter;
            TrackSlider(turnMeterUI);
            turnMeterUI.transform.GetChild(0).GetComponent<TMP_Text>().text = entity.entity.entityName;
            turnMeterUI.transform.GetChild(1).GetComponent<TMP_Text>().text = turnSlider.value.ToString() + "%";
            entity.turnMeterUI = turnMeterUI;
        }
        unitsOnField = listOfFighters;
    }

    void EmptyTurnOrder()
    {
        List<Transform> destroys = new();
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
        }
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
        foreach(EntityBase entity in unitsOnField)
        {
            if (entity.turnMeterUI != null)
            {
                turnSlider.value = Mathf.RoundToInt(entity.turnMeter);
                TrackSlider(entity.turnMeterUI);
                entity.turnMeterUI.transform.GetChild(1).GetComponent<TMP_Text>().text = turnSlider.value.ToString() + "%";
            }
        }
    }
}
