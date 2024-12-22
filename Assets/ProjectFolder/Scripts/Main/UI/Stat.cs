using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    public EStamina type;

    Image content;

    public float lerpSpeed;

    float currentFill;
    public float maxValue;
    float currentValue;

    public float CurrentValue
    {
        get
        {
            return currentValue;
        }

        set
        {
            if (value > maxValue) currentValue = maxValue;
            else if (value < 0) currentValue = 0;
            else currentValue = value;

            currentFill = currentValue / maxValue;
          
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        content = GetComponent<Image>();

        if (type == EStamina.StaminaS)
        {
            StartCoroutine("Gauge", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(currentFill != content.fillAmount)
            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * 10);

    }

    public void Init(float maxVal, float curVal)
    {
        maxValue = maxVal;
        currentValue = curVal;
        CurrentValue = currentValue;
    }

    public void Hit(float damage)
    {
        CurrentValue -= damage;
    }

    public IEnumerator Gauge()
    {
        if (CurrentValue < maxValue) { //변경
            yield return new WaitForSeconds(1);
            CurrentValue += lerpSpeed;
            //CurrentValue = 30; // 스킬테스트할려고

            StartCoroutine("Gauge", 1);
        }
    }

    public void PlusHp()
    {
        CurrentValue += 20;
    }
}
