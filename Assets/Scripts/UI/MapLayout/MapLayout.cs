using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapLayout : MonoBehaviour
{
    [SerializeField] GameObject layoutGO;
    [SerializeField] MapNode mapNodePrefab;

    List<MapNode> mapNodes = new();
    int currentNodeNum = -1;

    private void Awake()
    {
        for (int i = 0; i < MapManager.Instance.map.Count; i++)
        {
            MapNode node = Instantiate(mapNodePrefab, layoutGO.transform);
            mapNodes.Add(node);
            if (MapManager.Instance.map[i].GetComponent<CombatZone>() && MapManager.Instance.map[i].GetComponent<CombatZone>().isBossZone)
                node.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void ActivateNextNode()
    {
        currentNodeNum = MapManager.Instance.currentMapNum;

        if (currentNodeNum - 1 >= 0)
            mapNodes[currentNodeNum - 1].Completed();

        if (currentNodeNum < mapNodes.Count)
            mapNodes[currentNodeNum].Activate();
    }

    RectTransform rectTransform;
    float slideDuration = 0.5f;

    public void SlideOut()
    {
        rectTransform = GetComponent<RectTransform>();
        OnSlideOut();
    }

    void OnSlideOut()
    {
        // Calculate the position to slide out to (above the screen)
        Vector2 targetPosition = new Vector2(rectTransform.anchoredPosition.x, Screen.height);

        // Slide the image out to the top of the screen
        rectTransform.DOAnchorPosY(targetPosition.y, slideDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(DeactivateGameObject) // Call DeactivateGameObject method when animation completes
            .SetUpdate(UpdateType.Normal, true); // Set timeScaleIndependence to true
    }

    void DeactivateGameObject()
    {
        // Deactivate the GameObject
        gameObject.SetActive(false);
    }
}
