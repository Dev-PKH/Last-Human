using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public EWeapon type;
    public int weaponValue;

    public float damage;
    public float maxDamage;
    public float rate;
    public int curMagazine;
    public int maxMagazine;

    public float bouncingPower;

    // Melee
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;

    // Gun
    public Transform bulletPos;
    public GameObject bullet;
    public Transform casePos;
    public GameObject bulletCase;

    public void Use()
    {
        if(type != EWeapon.Gun)
        {
            StopCoroutine(Swing());
            StartCoroutine(Swing());
        }
        else if(curMagazine > 0)
        {
            curMagazine--;
            StartCoroutine(Shot());
        }
    }

    public void PlusDamage(float num)
    {
        damage += num;
        if (damage >= maxDamage) damage = maxDamage;
    }

    public void PlusGun(float num)
    {
        bullet.GetComponent<Bullet>().PlusDamage(num);
    }

    IEnumerator Swing()
    {
        // ����� ����Ʈ Ȱ��ȭ

        yield return new WaitForSeconds(0.15f);
        meleeArea.enabled = true;
        trailEffect.enabled = true;
        if(type == EWeapon.Bat) AudioManager.instance.PlaySound(EAudio.Swing);
        else if(type == EWeapon.Knife) AudioManager.instance.PlaySound(EAudio.Knife);

        // ���� ��Ȱ��ȭ
        yield return new WaitForSeconds(0.15f);
        meleeArea.enabled = false;

        // ����Ʈ ��Ȱ��ȭ
        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;
    }

    IEnumerator Shot()
    {
        // �Ѿ� ���� �� �߻�
        GameObject instanceBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        instanceBullet.SetActive(true);

        Rigidbody bulletRigid = instanceBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50;
        AudioManager.instance.PlaySound(EAudio.Shot);

        yield return null;
        // ź�� ����
        GameObject instanceCase = Instantiate(bulletCase, casePos.position, casePos.rotation);
        Rigidbody caseRigid = instanceCase.GetComponent<Rigidbody>();
        Vector3 caseVec = casePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3); // forward�� �ݴ����
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
        // ź�� ������ �� ���ϱ�
    }
}
