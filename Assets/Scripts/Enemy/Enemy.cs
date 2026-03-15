using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp;
    public float attackSpeed;
    public float actattackCooldown;
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


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            hp -= other.gameObject.GetComponent<Projectile>().dmg;
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Player")
        {
            speed = 0;
            player.GetComponent<Player>().hp -= dmg;
        }
    }

}