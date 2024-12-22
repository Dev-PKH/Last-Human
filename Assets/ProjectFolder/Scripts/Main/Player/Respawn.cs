using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 캐릭터 생성 위치
public class Respawn : MonoBehaviour
{
    public GameObject[] characterPrefabs; // 미리 저장된 프리펩들
    public GameObject player; // 생성된 프리펩을 가져오기위한 게임 오브젝트

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
