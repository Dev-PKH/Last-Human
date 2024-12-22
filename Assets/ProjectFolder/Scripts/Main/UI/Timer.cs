using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	Text myText;

	public float Sec = 0f;
	public int Min = 0;

    public bool bossTime;

	// Start is called before the first frame update

	private void Awake()
	{
		myText = GetComponent<Text>();
	}

    // Update is called once per frame
    void Update()
    {
		if (!GameManager.instance.isBoss && !GameManager.instance.isOver && !GameManager.instance.isClear)
			TimeGo();
	}

	public void TimeGo()
	{
		Sec += Time.deltaTime;

		if ((int)Sec > 59)
		{
			Sec = 0;
			Min++;
            if(!bossTime)
            {
                // 0 = Slime, 1 = Quill, 2 = Boss
                if (Min == 3) GenerateBoss(0);
                else if (Min == 6) GenerateBoss(1);
                else if (Min == 9)
                {
                    GenerateBoss(0);
                    GenerateBoss(1);
                }
                else if (Min == 10) GenerateBoss(2);
            }
		}

		myText.text = string.Format("{0:D2}:{1:D2}", Min, (int)Sec);
	}

    public void GenerateBoss(int num)
    {
        GameManager.instance.Boss(num);
    }
}
