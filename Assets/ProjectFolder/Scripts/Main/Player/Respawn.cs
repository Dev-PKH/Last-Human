using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ĳ���� ���� ��ġ
public class Respawn : MonoBehaviour
{
    public GameObject[] characterPrefabs; // �̸� ����� �������
    public GameObject player; // ������ �������� ������������ ���� ������Ʈ

    public FollwingCamera followCamera;

    void Awake()
    {
        if (NextScene.Instance == null) return;

        player = Instantiate(characterPrefabs[(int) NextScene.Instance.type]);
        player.transform.position = transform.position;

        followCamera.GetPlayer(player.transform);
        NextScene.Instance.DestroyObject();
    }
    /*
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }*/
}
