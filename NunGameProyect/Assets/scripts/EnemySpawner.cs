using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject swarmerPrefab;
    [SerializeField] private GameObject bigSwarmerPrefab;      
    [SerializeField] private float swarmerInterval = 3.5f;     
    [SerializeField] private float bigSwarmerInterval = 7f;   

    [SerializeField] private float[] spawnDurationPerLevel = { 30f, 45f, 60f }; 

    private Coroutine swarmerCoroutine;    
    private Coroutine bigSwarmerCoroutine;
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

        StartCoroutine(StopSpawningAfter(duration));
    }

    private IEnumerator SpawnEnemyInterval(float interval, GameObject enemy)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            Instantiate(
                enemy,
                new Vector3(Random.Range(-5f, 5f), Random.Range(-6f, 6f), 0),
                Quaternion.identity
            );
        }
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
    }
}