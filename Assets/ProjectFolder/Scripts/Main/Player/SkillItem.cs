using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillItem : MonoBehaviour
{
    public ESkill eSkill;

    public float damage;

    public GameObject effectObj;
    public GameObject meshObj;
    public GameObject playerSkill;

    Rigidbody rigid;
    Animator anim;

    public Vector3 preView;

    int cnt = 0;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        switch(eSkill)
        {
            case ESkill.ESpawn:
                anim = GetComponent<Animator>();
                StartCoroutine(Bird());
                break;
            case ESkill.EBall:
                StartCoroutine(Ball(SkillManager.Instance.transform));
                break;
            case ESkill.ECuffs:
                StartCoroutine(HandCuffs(SkillManager.Instance.transform));
                break;
            default:
                break;
        }
    }

    public IEnumerator Bird()
    {
        if (cnt == 0)
        {
            yield return new WaitForSeconds(1f);
            cnt++;
            StartCoroutine(Bird());
        }
        else if (cnt > 10)
        {
            StopCoroutine(Bird());
            Destroy(gameObject);
        }
        else
        {
            anim.SetBool("isAttack", false);

            yield return new WaitForSeconds(1f);
            GameObject birdBullet = Instantiate(playerSkill,
                                             transform.position,
                                            transform.rotation);
            AudioManager.instance.PlaySound(EAudio.Twitter);


            //birdBullet.transform.position = new Vector3(0, 1, 0);
            Rigidbody rigidBullet = birdBullet.GetComponent<Rigidbody>();
            rigidBullet.velocity = birdBullet.transform.forward * 50;

            cnt++;

            anim.SetBool("isAttack", true);
            //speed = 0f; 

            StartCoroutine("Bird", 1);
        }
    }
    
    /*IEnumerator BirdAttack()
    {
        yield return null;
    }*/

    IEnumerator Ball(Transform pos)
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject, 3f);
        AudioManager.instance.PlaySound(EAudio.Kick);

    }

    public IEnumerator HandCuffs (Transform pos)
    {
        yield return new WaitForSeconds(2f);
        meshObj.SetActive(false);

        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position,
                            15f,
                            Vector3.up,
                            0f,
                            LayerMask.GetMask("Enemy"));

        foreach (RaycastHit hit in rayHits)
        {
            hit.transform.GetComponent<Enemy>().Sturn();
        }
        MeshOn();

        AudioManager.instance.PlaySound(EAudio.HandCuffs_tie);

        Destroy(gameObject, 3f);
    }

    public void MeshOn()
    {
        effectObj.SetActive(true);
        meshObj.SetActive(false);
    }

}
