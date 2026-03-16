using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHp;
    public float actHp;
    float hpRegen;
    float attackSpeed;
    float attackCooldown;
    float dmg;
    float range;
    public float velocity;

    public SoftUpgrades sUps;
    public HardUpgrades hUps;

    public GMScript gm;
    public GameObject rangeCircle;
    public GameObject projectile;
    public List<GameObject> enemiesInRange;
    public Camera cam;

    /*void Start()
    {
        cam = Camera.main;
    }*/

    public void UpdatePlayer()
    {
        UpdateStats();
        if (attackCooldown <= 0 && enemiesInRange.Count != 0)
        {
            Shoot();
            attackCooldown = attackSpeed;
        }
        attackCooldown -= Time.deltaTime;

    }

    void UpdateStats()
    {
        maxHp = hUps.maxHpHUpgrade * sUps.maxHpSUpgrade;
        hpRegen = hUps.hpRegenHUpgrade * sUps.hpRegenSUpgrade;
        attackSpeed = hUps.attackSpeedHUpgrade * sUps.attackSpeedSUpgrade;
        dmg = hUps.dmgHUpgrade * sUps.dmgSUpgrade;
        range = hUps.rangeHUpgrade * sUps.rangeSUpgrade;
    }

    void Shoot()
    {
        GameObject closetEnemy = FindClosestEnemy();
        GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);

        float height = cam.orthographicSize;
        float width = height * cam.aspect;

        Vector2 center = cam.transform.position;

        //Bounds limit
        float x = center.x + width + 10;
        float y = center.y + height + 10;
        newProjectile.GetComponent<Projectile>().SetProjectile(dmg, velocity, (closetEnemy.transform.position - transform.position).normalized, x, y);
    }

    GameObject FindClosestEnemy()
    {
        float closestDist = Vector2.Distance(transform.position, enemiesInRange[0].transform.position);
        GameObject closestEnemy = enemiesInRange[0];
        foreach (GameObject enemy in enemiesInRange)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (closestDist > dist)
            {
                closestDist = dist;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" && enemiesInRange.IndexOf(other.gameObject) == -1)
        {
            enemiesInRange.Add(other.gameObject);
        }
    }


    void SetRange()
    {
        rangeCircle.transform.localScale = new Vector2(range, range);
    }

    public void SetPlayer()
    {
        cam = Camera.main;

        maxHp = hUps.maxHpHUpgrade;
        hpRegen = hUps.hpRegenHUpgrade;
        actHp = maxHp;

        attackSpeed = hUps.attackSpeedHUpgrade;
        dmg = hUps.dmgHUpgrade;
        range = hUps.rangeHUpgrade;

        SetRange();
    }

    public void DamagePlayer(float dmg)
    {
        actHp -= dmg;
    }
}