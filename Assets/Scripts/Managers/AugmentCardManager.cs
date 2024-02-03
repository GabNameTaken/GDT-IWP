using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common.DesignPatterns;
public class AugmentCardManager : Singleton<AugmentCardManager>
{
    [SerializeField] GameObject selectionPage;
    [SerializeField] GameObject listUI;
    [SerializeField] GameObject selectionPrefab;
    public void StartSelection()
    {
        if (listUI.transform.childCount > 0)
        {
            foreach (Transform child in listUI.transform){  Destroy(child.gameObject);  }
        }
        selectionPage.SetActive(true);
        SpawnBlessings(3);
    }

    void SpawnBlessings(int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject selection = Instantiate(selectionPrefab, listUI.transform);
            DelegateSelection(selection.GetComponent<Button>());
        }
    }

    void DelegateSelection(Button button)
    {
        button.onClick.AddListener(delegate { Select(); });
    }

    void Select()
    {
        selectionPage.SetActive(false);
        GameController.Instance.BeginBattle();
    }
}
