using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject swarmerPrefab;
    [SerializeField] private GameObject bigSwarmerPrefab;
    [SerializeField] private GameObject rangeEnemyPrefab;  
    [SerializeField] private float swarmerInterval = 3.5f;
    [SerializeField] private float bigSwarmerInterval = 7f;
    [SerializeField] private float rangeEnemyInterval = 6f;

    [Header("Duración por nivel")]
    [SerializeField] private float[] spawnDurationPerLevel = { 30f, 45f, 60f };

    
    [Header("Rango de Spawn")]
    [SerializeField] private float spawnRangeX = 5f;  // mitad del ancho
    [SerializeField] private float spawnRangeY = 6f;  // mitad del alto
    [SerializeField] private bool spawnRelativoAlSpawner = true; // ← si es true, el rango es relativo a la posición del spawner

    private Coroutine swarmerCoroutine;
    private Coroutine bigSwarmerCoroutine;
    private Coroutine rangeEnemyCoroutine;
    void Start()
    {
        StartSpawning(0);
    }

    public void StartSpawning(int level)
    {
        float duration = (level < spawnDurationPerLevel.Length)
            ? spawnDurationPerLevel[level]
            : spawnDurationPerLevel[spawnDurationPerLevel.Length - 1];

        swarmerCoroutine    = StartCoroutine(SpawnEnemyInterval(swarmerInterval, swarmerPrefab));
        bigSwarmerCoroutine = StartCoroutine(SpawnEnemyInterval(bigSwarmerInterval, bigSwarmerPrefab));
        rangeEnemyCoroutine = StartCoroutine(SpawnEnemyInterval(rangeEnemyInterval, rangeEnemyPrefab));

        StartCoroutine(StopSpawningAfter(duration));
    }

    private IEnumerator SpawnEnemyInterval(float interval, GameObject enemy)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            if (enemy == null) continue; 

            Vector3 spawnPos = GetSpawnPosition();
            Instantiate(enemy, spawnPos, Quaternion.identity);
        }
    }

        private Vector3 GetSpawnPosition()
    {
        float baseX = spawnRelativoAlSpawner ? transform.position.x : 0f;
        float baseY = spawnRelativoAlSpawner ? transform.position.y : 0f;

        return new Vector3(
            baseX + Random.Range(-spawnRangeX, spawnRangeX),
            baseY + Random.Range(-spawnRangeY, spawnRangeY),
            0f
        );
    }

    private IEnumerator StopSpawningAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        StopSpawning();
        Debug.Log("Spawner Detenido.");
    }

    public void StopSpawning()
    {
        if (swarmerCoroutine    != null) StopCoroutine(swarmerCoroutine);
        if (bigSwarmerCoroutine != null) StopCoroutine(bigSwarmerCoroutine);
        if (rangeEnemyCoroutine != null) StopCoroutine(rangeEnemyCoroutine);
    }

    //  Dibuja el rango de spawn en la Scene View
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 center = spawnRelativoAlSpawner ? transform.position : Vector3.zero;
        Gizmos.DrawWireCube(center, new Vector3(spawnRangeX * 2, spawnRangeY * 2, 0));
    }
}