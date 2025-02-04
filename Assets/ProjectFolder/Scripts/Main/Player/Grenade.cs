using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject meshObj;
    public GameObject effectObj;

    Rigidbody rigid;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Explosion());
    }

	IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3f);

        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        meshObj.SetActive(false);
        effectObj.SetActive(true);

        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position,
                            15,
                            Vector3.up,
                            0f,
                            LayerMask.GetMask("Enemy"));

        foreach(RaycastHit hit in rayHits)
        {
            hit.transform.GetComponent<Enemy>().Bomb(transform.position);
        }

        Destroy(gameObject, 5);
    }

    
}
