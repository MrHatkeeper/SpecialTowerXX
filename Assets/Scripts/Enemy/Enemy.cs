using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp;
    public bool isAttacking = false;
    public float attackSpeed;
    public float attackCooldown;
    public float dmg;
    public float range;
    public float value;
    public float coinValue;
    public float speed;
    public GameObject player;
    public GMScript gm;

    public void MoveEnemy()
    {
        Vector2 dir = (player.transform.position - transform.position).normalized;
        transform.Translate(dir * speed * Time.deltaTime);
    }

    public void AttackPlayer()
    {
        if (isAttacking)
        {
            if (attackCooldown <= 0)
            {
                player.GetComponent<Player>().DamagePlayer(dmg);
                attackCooldown = attackSpeed;
            }
            attackCooldown -= Time.deltaTime;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            hp -= other.gameObject.GetComponent<Projectile>().dmg;
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Player")
        {
            isAttacking = true;
            speed = 0;
        }
    }

}