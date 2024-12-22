using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public float maxDamage;

    public float bouncingPower;


    private void Start()
    {
        Destroy(gameObject, 4f);
    }

    public void PlusDamage(float num)
    {
        damage += num;
        if (maxDamage <= damage) damage = maxDamage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            if(damage == 0) AudioManager.instance.PlaySound(EAudio.Cartridge);
            Destroy(gameObject, 3f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
