using System.Collections.Generic;
using UnityEngine;

public class GMScript : MonoBehaviour
{
    private Camera cam;
    public GameObject player;
    private Player ps;
    public List<GameObject> spawnedEnemies;
    public List<GameObject> enemyCatalog;
    private List<GameObject> enemiesToSpawn = new();

    public int wave = 1;
    public float waveCooldown;
    private float actWaveCooldown;

    public float spawnTimer;
    public float actSpawnTimer;
    public float money = 0;
    public float coins = 0;

    void Start()
    {
        spawnTimer = 0.5f;
        actSpawnTimer = spawnTimer;
        actWaveCooldown = 1f;

        cam = Camera.main;

        player = Instantiate(player, new Vector2(0f, 0f), Quaternion.identity);
        ps = player.GetComponent<Player>();
        ps.gm = this;
        SetUpWave();
    }

    void Update()
    {
        if (enemiesToSpawn.Count == 0 && spawnedEnemies.Count == 0 && waveCooldown <= 0)
        {
            enemiesToSpawn = SetUpWave();
        }
        foreach (GameObject enemy in spawnedEnemies)
        {
            enemy.GetComponent<Enemy>().MoveEnemy();
        }

        if (actSpawnTimer <= 0 && enemiesToSpawn.Count != 0)
        {
            actSpawnTimer = spawnTimer;
            SpawnEnemy();
        }
        ClearEnemies();
        ps.UpdatePlayer();
        actSpawnTimer -= Time.deltaTime;
        actWaveCooldown -= Time.deltaTime;
    }

    void SpawnEnemy()
    {
        GameObject enemyToSpawn = enemiesToSpawn[0];
        Vector2 spawnPos = SpawnPos(enemyToSpawn);
        GameObject randEnemy = Instantiate(enemyToSpawn, spawnPos, Quaternion.identity); ;
        spawnedEnemies.Add(randEnemy);
        enemiesToSpawn.Remove(enemyToSpawn);
        enemiesToSpawn.RemoveAt(0);
        randEnemy.GetComponent<Enemy>().player = player;
        randEnemy.GetComponent<Enemy>().gm = this;
    }

    public void ClearEnemy(GameObject enemy)
    {
        spawnedEnemies.Remove(enemy);
        player.GetComponent<Player>().enemiesInRange.Remove(enemy);
    }

    void ClearEnemies()
    {
        List<GameObject> enemiesToClear = new();
        foreach (GameObject enemy in spawnedEnemies)
        {
            Enemy es = enemy.GetComponent<Enemy>();
            if (es.hp <= 0)
            {
                enemiesToClear.Add(enemy);
                money += es.value;
                coins += es.coinValue;
            }
        }
        foreach (GameObject enemy in enemiesToClear)
        {
            ps.enemiesInRange.Remove(enemy);
            spawnedEnemies.Remove(enemy);
            Destroy(enemy);
        }
    }

    List<GameObject> SetUpWave()
    {
        List<GameObject> output = new();
        float baseWaveValue = 5;
        float coeficient = Random.Range(1f, 2f);
        float finalWaveValue = baseWaveValue * coeficient * wave;

        while (finalWaveValue > 0)
        {
            print(finalWaveValue);
            List<GameObject> allowedEnemies = new();
            foreach (var enemy in enemyCatalog)
            {
                if (enemy.GetComponent<Enemy>().value <= finalWaveValue)
                {
                    allowedEnemies.Add(enemy);
             
                }
            }
            if (allowedEnemies.Count == 0){
                break;
            }
            GameObject addedEnemy = allowedEnemies[Random.Range(0, allowedEnemies.Count - 1)];
            output.Add(addedEnemy);
            finalWaveValue -= addedEnemy.GetComponent<Enemy>().value;
        }
        return output;

    }

    Vector2 SpawnPos(GameObject enemy)
    {
        float height = cam.orthographicSize;
        float width = height * cam.aspect;

        Vector2 center = cam.transform.position;

        float x = center.x + width + enemy.transform.localScale.x;
        float y = center.y + height + enemy.transform.localScale.x;

        int whichSwitch = Random.Range(0, 2);

        int isRight = Random.Range(0, 2) == 1 ? 1 : -1;
        int isTop = Random.Range(0, 2) == 1 ? 1 : -1;

        if (whichSwitch == 0)
        {
            float randVal = Random.Range(0, y);
            return new Vector2(isRight * x, isTop * randVal);
        }
        else
        {
            float randVal = Random.Range(0, x);
            return new Vector2(isRight * randVal, isTop * y);
        }
    }
}