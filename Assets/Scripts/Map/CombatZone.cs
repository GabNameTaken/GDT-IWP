using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatZone : MonoBehaviour
{
    [Header("Wave 1")]
    public List<Enemy> wave1Enemies;

    [Header("Wave 2")]
    public List<Enemy> wave2Enemies;

    [Header("Player team")]
    public List<GameObject> teamSlots;

    private void Awake()
    {
        CombatManager.Instance.enemyParty = wave1Enemies;
        for (int i = 0; i < PlayerTeamManager.Instance.playerTeam.Count; i++)
        {
            PlayerTeamManager.Instance.playerTeam[i].transform.position = teamSlots[i].transform.position;

            // Calculate the rotation angle by adding 180 degrees to the Y-axis rotation of wave1Enemies
            float newRotationY = wave1Enemies[i].transform.rotation.eulerAngles.y + 180f;

            // Apply the new rotation to the player team member
            PlayerTeamManager.Instance.playerTeam[i].transform.rotation = Quaternion.Euler(0, newRotationY, 0);
        }
        CombatManager.Instance.StartBattle();
    }
}
