using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
	public bool isWall;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Wall"))
			isWall = true;
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Wall"))
			isWall = false;
	}
}
