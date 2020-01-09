using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;

    private Vector2 dir;

    public bool isFromPlayer;

    public void Init(Vector2 myDir, float mySpeed, bool _isFromPlayer)
    {
        dir = myDir;
        speed = mySpeed;

        isFromPlayer = _isFromPlayer;
    }

    public void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);

        if (transform.position.y >= GameManager.topRight.y)
        {
            Destruction();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) //when a projectile collides with another object
    {
        if (collision.tag == "Player") //if anoter object is 'player' or 'enemy sending the command of receiving the damage
        {
            Player.instance.GetDamage(0);
            Destruction();
        }
    }

    void Destruction()
    {
        Destroy(gameObject);
    }
}
