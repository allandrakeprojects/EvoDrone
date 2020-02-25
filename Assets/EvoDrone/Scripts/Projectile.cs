﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the damage and defines whether the projectile belongs to the ‘Enemy’ or to the ‘Player’, whether the projectile is destroyed in the collision, or not and amount of damage.
/// </summary>

public class Projectile : MonoBehaviour
{

    [Tooltip("Damage which a projectile deals to another object. Integer")]
    public int damage;

    [Tooltip("Whether the projectile belongs to the ‘Enemy’ or to the ‘Player’")]
    public bool enemyBullet;

    [Tooltip("Whether the projectile is destroyed in the collision, or not")]
    public bool destroyedByCollision;

    [Tooltip("Whether the projectile is using 'pooling', or not")]
    public bool isPooled;

    private void OnTriggerEnter2D(Collider2D collision) //when a projectile collides with another object
    {
        if (enemyBullet && collision.tag == "SideKick")
        {
            SideKick.instance.GetDamage(1);
            print("testtesttest");

            if (destroyedByCollision)
                Destruction();

            return;
        }

        if (enemyBullet && collision.tag == "Player") //if anoter object is 'player' or 'enemy sending the command of receiving the damage
        {
            Player.instance.GetDamage(damage);
            if (destroyedByCollision)
                Destruction();
        }
        else if (!enemyBullet && collision.tag == "Enemy")
        {
            try
            {
                collision.GetComponent<Enemy>().GetDamage(damage);
                if (destroyedByCollision)
                    Destruction();
            }
            catch (Exception err)
            {
                collision.GetComponent<Boss>().GetDamage(damage);
                if (destroyedByCollision)
                    Destruction();
            }
        }
    }

    void Destruction()  //if the object is using 'pooling' disactivate it. If it isn't destroy it
    {
        if (isPooled)
            gameObject.SetActive(false);
        else
            Destroy(gameObject);
    }
}