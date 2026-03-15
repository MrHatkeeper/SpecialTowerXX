using System.Collections.Generic;
using UnityEngine;

public class GMScript : MonoBehaviour
{
    private Camera cam;
    public GameObject player;
    public Player ps;
    public List<GameObject> spawnedEnemies;
    public List<GameObject> enemyCatalog;
    public int wave = 1;
    public float spawnTimer;
    public float actSpawnTimer;
    public float money = 0;
    public float coins = 0;

    void Start()
    {
        spawnTimer = 0.5f;
        actSpawnTimer = spawnTimer;

        cam = Camera.main;

        player = Instantiate(player, new Vector2(0f, 0f), Quaternion.identity);
        ps = player.GetComponent<Player>();
        ps.gm = this;
        SpawnEnemy();
    }

    void Update()
    {
        ClearEnemies();
        ps.UpdatePlayer();
        if (actSpawnTimer <= -11111)
        {
            actSpawnTimer = spawnTimer;
            SpawnEnemy();
        }
        actSpawnTimer -= Time.deltaTime;
    }

    void SpawnEnemy()
    {
        GameObject enemyToSpawn = enemyCatalog[Random.Range(0, enemyCatalog.Count - 1)];
        Vector2 spawnPos = SpawnPos(enemyToSpawn);
        GameObject randEnemy = Instantiate(enemyToSpawn, spawnPos, Quaternion.identity);;
        spawnedEnemies.Add(randEnemy);
        randEnemy.GetComponent<Enemy>().player = player;
        randEnemy.GetComponent<Enemy>().gm = this;
    }

    public void ClearEnemy(GameObject enemy){
        spawnedEnemies.Remove(enemy);
        player.GetComponent<Player>().enemiesInRange.Remove(enemy);
    }

    void ClearEnemies(){
        List<GameObject> enemiesToClear = new();
        foreach (GameObject enemy in spawnedEnemies){
            Enemy es = enemy.GetComponent<Enemy>();
            if (es.hp <= 0){
                enemiesToClear.Add(enemy);
            }
        }
        foreach (GameObject enemy in enemiesToClear){
            ps.enemiesInRange.Remove(enemy);
            spawnedEnemies.Remove(enemy);
            Destroy(enemy);
        }
    }

    void WaveValue()
    {

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