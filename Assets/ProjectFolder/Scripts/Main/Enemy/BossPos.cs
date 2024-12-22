using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPos : MonoBehaviour
{
    public GameObject bullet;

    void Start()
    {
        
    }

    public void FireBall()
    {
        // 총알 생성 및 발사
        GameObject instanceBullet = Instantiate(bullet, transform.position, transform.rotation);
        Rigidbody bulletRigid = instanceBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = transform.forward * 50;

    }

}
