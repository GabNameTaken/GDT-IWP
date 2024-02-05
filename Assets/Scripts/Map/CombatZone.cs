using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatZone : MonoBehaviour
{
    public GameObject battleground;
    [SerializeField] Transform playerTransform, enemyTransform;

    [Header("Waves")]
    public List<Wave> enemyWaves;
    private Wave currentWave = null;

    List<PlayableCharacter> playableCharacters = null;

    [Header("Soundtrack")]
    [SerializeField] AudioClip soundtrack;

    private void Start()
    {
        currentWave = enemyWaves[0];

        InstantiateWave();

        CameraManager.Instance.MoveCamera(battleground, CAMERA_POSITIONS.PLAYER_TEAM_BACK, 0f);
        CombatManager combatManager = CombatManager.Instance;
        combatManager.WaveClearedEvent += OnWaveCleared;

        if (!soundtrack)
            AudioManager.Instance.PlayRandomCombatBGM();
        else
            AudioManager.Instance.PlayMusic(soundtrack, true);
    }

    private void InstantiateWave()
    {
        Vector3 playerToEnemyNormalized = (currentWave.EnemyPosition.position - currentWave.PlayerPosition.position).normalized;

        if (playableCharacters == null) InstantiatePlayableCharacters(playerToEnemyNormalized);
        else MovePlayableCharacters(playerToEnemyNormalized);
        InstantiateEnemies(-playerToEnemyNormalized);
    }

    private void InstantiatePlayableCharacters(Vector3 direction)
    {
        playableCharacters = new List<PlayableCharacter>();

        PlayerTeamManager playerTeamManager = PlayerTeamManager.Instance;
        float xPosition = currentWave.XPositionSpacing * ((playerTeamManager.teamPrefabs.Count - 1) / 2);
        foreach (GameObject characterPrefab in playerTeamManager.teamPrefabs)
        {
            GameObject playableCharacter = Instantiate(characterPrefab);
            playableCharacter.transform.position = currentWave.PlayerPosition.position + playableCharacter.transform.TransformDirection(xPosition, 0f, 0f);
            playableCharacter.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

            playableCharacter.transform.SetParent(playerTransform);

            playableCharacters.Add(playableCharacter.GetComponent<PlayableCharacter>());

            xPosition -= currentWave.XPositionSpacing;
        }
        CombatManager.Instance.PlayerParty = playableCharacters;
    }

    private void InstantiateEnemies(Vector3 direction)
    {
        List<Enemy> enemies = new List<Enemy>();

        float xPosition = currentWave.XPositionSpacing * ((currentWave.EnemyPrefabs.Count - 1) / 2);
        foreach (GameObject enemyPrefab in currentWave.EnemyPrefabs)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.position += currentWave.EnemyPosition.position + enemy.transform.TransformDirection(xPosition, 0f, 0f);
            enemy.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

            enemy.transform.SetParent(enemyTransform);

            enemies.Add(enemy.GetComponent<Enemy>());
            if (enemy.GetComponent<Enemy>().isBoss)
            {
                CombatUIManager.Instance.bossInfoUI.gameObject.SetActive(true);
                CombatUIManager.Instance.bossInfoUI.character = enemy.GetComponent<Enemy>();
                enemy.GetComponent<Enemy>().entityInfoUI = CombatUIManager.Instance.bossInfoUI;
                CombatUIManager.Instance.bossInfoUI.SetUpUI();
            }

            xPosition -= currentWave.XPositionSpacing;
        }
        CombatManager.Instance.EnemyParty = enemies;
    }

    private void MovePlayableCharacters(Vector3 direction)
    {
        //CombatManager combatManager = CombatManager.Instance;

        //float xPosition = currentWave.XPositionSpacing * ((combatManager.PlayerParty.Count - 1) / 2);
        //foreach (PlayableCharacter playableCharacter in combatManager.PlayerParty)
        //{
        //    playableCharacter.transform.position = currentWave.PlayerPosition.position + playableCharacter.transform.TransformDirection(xPosition, 0f, 0f);
        //    playableCharacter.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        //    xPosition -= currentWave.XPositionSpacing;
        //}
    }

    private void OnWaveCleared()
    {
        CombatUIManager.Instance.bossInfoUI.gameObject.SetActive(false);

        int waveNumber = enemyWaves.IndexOf(currentWave);
        if (waveNumber >= enemyWaves.Count - 1) // If waves finished, call battle ended (won)
        {
            CombatManager.Instance.CallBattleWon();
            CombatManager.Instance.WaveClearedEvent -= OnWaveCleared;
            return;
        }

        currentWave = enemyWaves[++waveNumber];
        InstantiateWave();
    }

    private void OnDestroy()
    {
        CombatManager.Instance.WaveClearedEvent -= OnWaveCleared;
    }
}

[System.Serializable]
public class Wave
{
    [SerializeField] Transform playerPosition, enemyPosition;
    public Transform PlayerPosition => playerPosition;
    public Transform EnemyPosition => enemyPosition;

    [SerializeField] float xPositionSpacing = 3f;
    public float XPositionSpacing => xPositionSpacing;

    [SerializeField] private List<GameObject> enemyPrefabs;
    public List<GameObject> EnemyPrefabs => enemyPrefabs;
}
