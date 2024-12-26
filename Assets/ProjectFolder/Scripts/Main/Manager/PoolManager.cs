using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;

    List<GameObject>[] pools;

    public GameObject[] bossEnemy;

    public int curCnt = 0;
    public int maxCnt = 20;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for(int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        foreach(GameObject item in pools[index])
            if(!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }

        if(!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }

    public void LevelUp()
    {
        if (curCnt < maxCnt)
        {
            curCnt++;
            if (curCnt % 2 == 1)
            {
                GameManager.instance.zombieDaamge += 1;
            }
        }

        foreach (GameObject enemy in pools[0])
        {
            enemy.GetComponent<Enemy>().maxHp += 50;
            enemy.GetComponent<Enemy>().PlusDamage(GameManager.instance.zombieDaamge);
        }
    }

    public void GenerateBoss(int num)
    {
        GameObject boss = Instantiate(bossEnemy[num], transform);
        Transform target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        boss.transform.position = target.position + new Vector3(2, 0, 2);

        if (num == 2) AudioManager.instance.PlaySound(EAudio.BossAppear); 
    }


    //public void l

}
