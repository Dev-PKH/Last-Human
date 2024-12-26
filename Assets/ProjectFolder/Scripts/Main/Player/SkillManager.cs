using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;
    public GameObject[] skillItem;

    ECharacter type;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

    }

    public void SpecialSkill(ECharacter playerType, Transform pos)
    {
        type = playerType;

        switch (type)
        {
            case ECharacter.Girl:
                GenerateBird(type);
                break;
            case ECharacter.Boy:
                ThrowBall(type);
                break;
            case ECharacter.Police:
                ThrowHandCurffs(type);
                break;
            default:
                break;
        }
    }

    public void GenerateBird(ECharacter type)
    {
        GameObject bird = Instantiate(skillItem[(int)type],
                                        transform.position,
                                        transform.rotation);

        AudioManager.instance.PlaySound(EAudio.Call_bird);

        bird.transform.position += new Vector3(0, 3, 0); 


        Rigidbody rigidBird = bird.GetComponent<Rigidbody>();
        
        
    }

    public void ThrowBall(ECharacter type)
    {
        GameObject ball = Instantiate(skillItem[(int) type],
                                         transform.position,
                                        transform.rotation);

        AudioManager.instance.PlaySound(EAudio.Kick);

        Rigidbody rigidBall = ball.GetComponent<Rigidbody>();
        Vector3 ballVec = transform.forward * Random.Range(10, 20) + Vector3.up * Random.Range(10, 15);
        rigidBall.AddForce(ballVec, ForceMode.Impulse);
        rigidBall.AddTorque(Vector3.up * 10, ForceMode.Impulse);
    }

    public void ThrowHandCurffs(ECharacter type)
    {
        GameObject hancCuffs = Instantiate(skillItem[(int)type],
                                         transform.position,
                                        transform.rotation);

        AudioManager.instance.PlaySound(EAudio.HandCuffs_throw);


        Rigidbody rigidCuffs = hancCuffs.GetComponent<Rigidbody>();
        Vector3 cuffsVec = transform.forward * Random.Range(15, 25) + Vector3.up * 5f;
        rigidCuffs.AddForce(cuffsVec, ForceMode.Impulse);
        rigidCuffs.AddTorque(Vector3.up * 10, ForceMode.Impulse);

    }
}
