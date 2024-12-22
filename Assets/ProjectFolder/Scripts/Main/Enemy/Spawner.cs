using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public Transform[] spawnPoint;

    int upgarade = 0;
	float timer;

    const int maxEnemy = 100;

	private void Start()
	{
		spawnPoint = GetComponentsInChildren<Transform>();
	}

	void Update()
    {
		timer += Time.deltaTime;

        if (timer > 2f && GameManager.instance.curEnemy < maxEnemy && !GameManager.instance.isOver && !GameManager.instance.isBoss && !GameManager.instance.isClear)
        {
            upgarade++;
            timer = 0;
			Spawn();
            if(upgarade >= 15)
            {
                GameManager.instance.UpgradeMoster();
                upgarade = 0;
            }
		}
    }

	void Spawn()
	{
		int RandomPoint = Random.Range(1, spawnPoint.Length);
		if (spawnPoint[RandomPoint].GetComponent<SpawnPoint>().isWall == false)
		{
			GameObject enemy = GameManager.instance.pool.Get(Random.Range(0, GameManager.instance.pool.prefabs.Length));
			enemy.transform.position = spawnPoint[RandomPoint].position;
		}
	}
}
