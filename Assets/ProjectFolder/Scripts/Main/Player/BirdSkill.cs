using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSkill : MonoBehaviour
{
    public bool arroundEnemy; // 적이 근처에 있는지 체크

    Transform view;
    public Vector3 offset;

    private void Update()
    {
        if (view != null) transform.LookAt(view);
        transform.position = Vector3.Lerp(transform.position
                            , SkillManager.Instance.transform.position +offset
                            , Time.deltaTime * 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" || !arroundEnemy)
        {
            view = other.gameObject.transform;
            //transform.LookAt(other.gameObject.transform);
            arroundEnemy = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy" || arroundEnemy)
        {     
            arroundEnemy = false;
        }
    }
}
