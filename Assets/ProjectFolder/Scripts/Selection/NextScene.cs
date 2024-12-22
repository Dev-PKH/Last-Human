using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    // �̱��� ����
    public static NextScene Instance;

    public ECharacter type; // ĳ���͸� �з��ϱ����� ���

    public void Awake()
    {
        // �ν��Ͻ� �Ҵ�
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

    // �� ��ȯ
    public void SceneChange()
    {
        AudioManager.instance.BGM_Change(1);
        SceneManager.LoadScene(1);
    }

    // �ش� ������Ʈ ����
    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
