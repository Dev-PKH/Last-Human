using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	public AudioSource SFX_Source;
	public AudioSource BGM_Source;

	[Header("Sounds")]
	public AudioClip[] EffectAudio;
	public AudioClip[] BGMAudio;

	// Start is called before the first frame update
	void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
	}

	public void PlaySound(EAudio EA)
	{
		SFX_Source.PlayOneShot(EffectAudio[(int)EA]);
	}

	public void BGM_Change(int num)
	{
		BGM_Source.clip = BGMAudio[num];
		BGM_Source.Play();
	}
}
