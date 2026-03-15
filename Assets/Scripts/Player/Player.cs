using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float hp;
    public float attackSpeed;
    public float attackCooldown;
    public float dmg;
    public float range;
    public float velocity;
    public GMScript gm;
    public GameObject rangeCircle;
    public GameObject projectile;
    public List<GameObject> enemiesInRange;
    public Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    public void UpdatePlayer()
    {
        SetRange();
        if (attackCooldown <= 0 && enemiesInRange.Count != 0)
        {
            Shoot();
            attackCooldown = attackSpeed;
        }
        attackCooldown -= Time.deltaTime;

    }

    void Shoot()
    {
        GameObject closetEnemy = FindClosestEnemy();
        GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);

        float height = cam.orthographicSize;
        float width = height * cam.aspect;

        Vector2 center = cam.transform.position;

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
}
