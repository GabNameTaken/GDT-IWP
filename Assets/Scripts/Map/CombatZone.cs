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
    public List<Transform> teamSlots;

    private void Start()
    {
        CombatManager.Instance.enemyParty = wave1Enemies;
        for (int i = 0; i < PlayerTeamManager.Instance.teamPrefabs.Count; i++)
        {
            GameObject member = Instantiate(PlayerTeamManager.Instance.teamPrefabs[i], teamSlots[i]);
            member.transform.position = teamSlots[i].position;

            // Calculate the rotation angle by adding 180 degrees to the Y-axis rotation of wave1Enemies
            float newRotationY = wave1Enemies[0].transform.rotation.eulerAngles.y + 180f;

            // Apply the new rotation to the player team member
            member.transform.rotation = Quaternion.Euler(0, newRotationY, 0);
        }
        CombatManager.Instance.StartBattle(this);
    }
}
