using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    // 싱글톤 패턴
    public static NextScene Instance;

    public ECharacter type; // 캐릭터를 분류하기위한 상수

    public void Awake()
    {
        // 인스턴스 할당
        if (Instance  == null) Instance = this;
        else if (Instance != null) return;
        DontDestroyOnLoad(this.gameObject);
    }

    public void Girl()
    {
        type = ECharacter.Girl;
    }

    public void Police()
    {
        type = ECharacter.Police;
    }

    public void Bat()
    {
        type = ECharacter.Boy;
    }

    // 씬 전환
    public void SceneChange()
    {
        AudioManager.instance.BGM_Change(1);
        SceneManager.LoadScene(1);
    }

    // 해당 오브젝트 삭제
    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
