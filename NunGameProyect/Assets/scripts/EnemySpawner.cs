using UnityEngine;
using System.Collections;
using System;  

public class EnemySpawner : MonoBehaviour
{
    
    public event Action OnTodosEnemigosEliminados;

    [SerializeField] private GameObject swarmerPrefab;
    [SerializeField] private GameObject bigSwarmerPrefab;
    [SerializeField] private GameObject rangeEnemyPrefab;
    [SerializeField] private float swarmerInterval = 3.5f;
    [SerializeField] private float bigSwarmerInterval = 7f;
    [SerializeField] private float rangeEnemyInterval = 6f;

    [Header("Duración por nivel")]
    [SerializeField] private float[] spawnDurationPerLevel = { 30f, 45f, 60f };

    [Header("Rango de Spawn")]
    [SerializeField] private float spawnRangeX = 5f;
    [SerializeField] private float spawnRangeY = 6f;
    [SerializeField] private bool spawnRelativoAlSpawner = true;

    [Header("Tiempo sin enemigos para abrir puerta")]
    [SerializeField] private float tiempoSinEnemigos = 3f;

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
            Instantiate(enemy, GetSpawnPosition(), Quaternion.identity);
        }
    }

    private Vector3 GetSpawnPosition()
    {
        float baseX = spawnRelativoAlSpawner ? transform.position.x : 0f;
        float baseY = spawnRelativoAlSpawner ? transform.position.y : 0f;
        return new Vector3(
            baseX + UnityEngine.Random.Range(-spawnRangeX, spawnRangeX),
            baseY + UnityEngine.Random.Range(-spawnRangeY, spawnRangeY),
            0f
        );
    }

    private IEnumerator StopSpawningAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        StopSpawning();
        Debug.Log("Spawner Detenido.");
        StartCoroutine(EsperarSinEnemigos());
    }

   
    private IEnumerator EsperarSinEnemigos()
    {
        float timer = 0f;

        while (timer < tiempoSinEnemigos)
        {
            EnemyController[] enemigosVivos = FindObjectsByType<EnemyController>
                                              (FindObjectsSortMode.None);
            if (enemigosVivos.Length == 0)
                timer += Time.deltaTime;
            else
                timer = 0f;

            yield return null;
        }

       
        OnTodosEnemigosEliminados?.Invoke();
        Debug.Log("Todos los enemigos eliminados — puerta abierta!");
    }

    public void StopSpawning()
    {
        if (swarmerCoroutine    != null) StopCoroutine(swarmerCoroutine);
        if (bigSwarmerCoroutine != null) StopCoroutine(bigSwarmerCoroutine);
        if (rangeEnemyCoroutine != null) StopCoroutine(rangeEnemyCoroutine);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 center = spawnRelativoAlSpawner ? transform.position : Vector3.zero;
        Gizmos.DrawWireCube(center, new Vector3(spawnRangeX * 2, spawnRangeY * 2, 0));
    }
}