using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class CombatInfoUI : MonoBehaviour
{
    [Header("Turn Order UI")]
    [SerializeField] TurnOrderUI turnOrderUI;
    [SerializeField] GameObject scrollContentGO;
    [SerializeField] GameObject turnOrderPrefab;

    [Header("Inspection UI")]
    [SerializeField] InspectionUI inspectionUI;

    private void OnEnable()
    {
        InitButtons();
    }

    void InitButtons()
    {
        ClearTurnOrder();
        List<EntityBase> turnOrder = turnOrderUI.unitsOnField.OrderByDescending(entity => entity.TurnMeter).ToList();

        foreach (EntityBase entity in turnOrder)
        {
            GameObject buttonGO = Instantiate(turnOrderPrefab, scrollContentGO.transform);
            SetUpButton(buttonGO.GetComponent<Button>(), entity);
        }
        inspectionUI.InitTabs(turnOrder[0]);
    }

    void ClearTurnOrder()
    {
        //Empty tabs
        if (scrollContentGO.transform.childCount > 0)
        {
            for (int i = scrollContentGO.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = scrollContentGO.transform.GetChild(i);
                Destroy(child.gameObject);
            }
        }
    }

    void SetUpButton(Button button, EntityBase entity)
    {
        button.transform.GetChild(0).GetComponent<TMP_Text>().text = entity.entity.name;
        button.onClick.AddListener(delegate { inspectionUI.InitTabs(entity); });
    }
}
