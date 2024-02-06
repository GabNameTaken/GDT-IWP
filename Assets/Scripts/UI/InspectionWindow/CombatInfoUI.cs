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

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    [Header("GameObjects")]
    [SerializeField] GameObject turnOrderGO;
    [SerializeField] GameObject inspectionGO;

    private void OnEnable()
    {
        turnOrderGO.SetActive(true);
        inspectionGO.SetActive(true);
        UIManager.Instance.DisplayToMenuPrompt(false);
        InitButtons();
    }

    void InitButtons()
    {
        ClearTurnOrder();
        List<EntityBase> turnOrder = turnOrderUI.unitsOnField
            .OrderByDescending(entity => entity.TurnMeter)
            .ToList();

        foreach (EntityBase entity in turnOrder)
        {
            GameObject buttonGO = Instantiate(turnOrderPrefab, scrollContentGO.transform);
            SetUpButton(buttonGO.GetComponent<Button>(), entity);
            if (entity.IsDead)
                buttonGO.SetActive(false);
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
        button.transform.GetChild(0).GetComponent<TMP_Text>().text = entity.entity.entityName;
        button.transform.GetChild(1).GetComponent<TMP_Text>().text = Mathf.Round(entity.TurnMeter) + "%";
        button.onClick.AddListener(delegate { inspectionUI.InitTabs(entity); });
    }
}
