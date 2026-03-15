using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float dmg;
    public float vel;
    Vector2 dir;
    public float a;
    float maxX;
    float maxY;

    void Update()
    {
        MoveBullet();
    }

    void MoveBullet()
    {
        transform.Translate(dir * vel * Time.deltaTime);
        OutOfBounds();
    }

    public void SetProjectile(float damage, float velocity, Vector2 direction, float maxX, float maxY)
    {
        dmg = damage;
        dir = direction;
        vel = velocity;
        this.maxX = maxX;
        this.maxY = maxY;
    }

    public void OutOfBounds()
    {
        if (Mathf.Abs(transform.position.x) >= maxX || Mathf.Abs(transform.position.y) >= maxY)
        {
            Destroy(this.gameObject);
        }
    }
}
