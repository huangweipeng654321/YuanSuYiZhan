using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tempreture : MonoBehaviour
{
    public GameObject targetTempView;
    [Header("温度计增加速度和温度计自减速度")]
    public float decreaseSpeed=0.1f;
    public float mouseSpeed=0.01f;
    [Header("起始温度和目标温度")]
    [Range(0,1)]
    public float originTemp;
    [Range(0,1)]
    public float targetTemp;
    [Header("保持在目标温度以上的频次")]
    public int targetCount;
    
    Image image;
    bool isOver;
    bool isSuccess;
    int judgeCount;
    private void Awake()
    {
        
        image = GetComponent<Image>();
    }
    private void OnEnable()
    {
        image.fillAmount = originTemp;
        isOver = false;
        isSuccess = false;
        judgeCount = 0;
        targetTempView.GetComponent<Image>().fillAmount = targetTemp;
        Debug.Log("enable");
        StartCoroutine("selfDecrease");
        StartCoroutine("judgement");
    }
    private void Start()
    {
        Debug.Log("Start");
        
    }

    void Update()
    {
        Debug.Log("judgecount:"+judgeCount);
        Debug.Log("Status:" + jugement());
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine("Add");
        }
        if(image.fillAmount<=0.01f)
        {
            isOver = true;
            isSuccess = false;
        }
        if(judgeCount >= targetCount)
        {
            isOver = true;
            isSuccess = true;
            Debug.Log("success");

        }
    }

    IEnumerator selfDecrease()
    {
        while(image.fillAmount>0.01f&&!isOver)
        {
            Debug.Log(image.fillAmount);
            image.fillAmount -= decreaseSpeed;
            yield return new WaitForSeconds(0.05f);
        }
    }
    IEnumerator Add()
    {
        
        if (!isOver)
        {
            image.fillAmount += mouseSpeed;
            yield return new WaitForSeconds(0.05f);
        }

    }

    IEnumerator judgement()
    {
        while(!isOver)
        {
            if(image.fillAmount>=targetTemp)
                judgeCount++;
            yield return new WaitForSeconds(0.5f);
        }
    }
    public int jugement()
    {
        if (isOver && isSuccess)
            return 1;//温度计处于目标温度之上达到频次
        else if (isOver && !isSuccess)
            return 2;//温度计归零
        else
            return 0;
    }
}
