using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;

public class MossSys : MonoBehaviour
{
    public GameObject displayBox;
    public GameObject codeBox;
    public GameObject passBox;
    
    [Header("自定义文本(e,r,t)")]
    public string alphbet;
    public int alphabetSize=3;
    [Header("成功键入后的颜色")]
    public string color = "<color=#2EF7E2>";
    [Header("当前字母序号(只读)")]
    [SerializeField]
    private int order;
    
    public const int AlphaSize = 2;
    const int shortKeycode = 1;
    const int longKeycode = 2;
    List<int> inputCode = new List<int>();
    int[,] alphacode =
    {
        {shortKeycode,shortKeycode },//E
        {shortKeycode,longKeycode },//R
        {longKeycode,longKeycode },//T
    };
    int[] currentAlphacode = new int[AlphaSize];

    int currentKeycode = 0;
    bool isSuccess = false;
    bool isOver = false;
    bool isDone = false;
    bool isFail = false;
    void complieCode()
    {
        if (alphbet[order].Equals('E') || alphbet[order].Equals('e'))
        {
            for (int i = 0; i < AlphaSize; i++)
            {
                currentAlphacode[i] = alphacode[0, i];
            }
        }
        else if (alphbet[order].Equals('R') || alphbet[order].Equals('r'))
        {
            for (int i = 0; i < AlphaSize; i++)
                currentAlphacode[i] = alphacode[1, i];
        }
        else if (alphbet[order].Equals('T') || alphbet[order].Equals('t'))
        {
            for (int i = 0; i < AlphaSize; i++)
                currentAlphacode[i] = alphacode[2, i];
        }
        else
        {
            currentAlphacode = null;
        }
    }
    private void OnEnable()
    {
        isFail = false;
        randomLoad();
        reStart();
        
    }

    private void Update()
    {
        //Debug.Log("if is over:" + jugement());
        eventTrigger();
        if(!isOver)
        {
            updateCode();
            if (isSuccess)
            {
                isSuccess = false;
                Debug.Log("Success");
                StartCoroutine(WaitForSeconds(0.5f, () =>
                {
                    moveNext();
                })); 
            }
            if (Input.anyKeyDown)
            {
                Debug.Log(currentKeycode);
                if(!(Input.GetMouseButtonDown(0)|| Input.GetMouseButtonDown(1)|| Input.GetMouseButtonDown(2)))
                    inputCode.Add(currentKeycode);
                Debug.Log("inputcode:");
                for (int i = 0; i < inputCode.Count; i++)
                {
                    Debug.Log(inputCode[i]);
                }
                if (inputCode.Count == AlphaSize)
                {
                    int SuccessCount = 0;
                    for (int i = 0; i < AlphaSize; i++)
                    {
                        if (inputCode[i] == currentAlphacode[i])
                        {
                            changeCodeColor(i);
                            SuccessCount++; 
                        }
                    }

                    if (SuccessCount == AlphaSize)
                    {
                        isSuccess = true;
                        changeAlphaColor();
                    }
                    else
                    {
                        Debug.Log("code fail");
                        isFail = true;
                        reStart();
                    }

                }
                if(inputCode.Count<AlphaSize)
                {
                    for (int i = 0; i < inputCode.Count; i++)
                    {
                        if (inputCode[i] == currentAlphacode[i])
                        {
                            Debug.Log("code color change");
                            changeCodeColor(i);
                            
                        }
                    }
                }
            }

        }
    }

   void updateCode()
   {
        if (Input.anyKeyDown)
        {
            currentKeycode = 0;
            if (Input.GetButtonDown("Short"))
            {
                currentKeycode = shortKeycode;
            }
            if (Input.GetButtonDown("Long"))
            {
                currentKeycode = longKeycode;
            }
        }
  
   }
    void moveNext()
    {
       
        if(alphbet.Length-1==order)
        {
            order = 0;
            Debug.Log("Done");

            codeBox.GetComponent<Text>().text = "";
            passBox.GetComponent<Text>().text = "success";
            isOver = true;
            isDone = true;
            return;
        }
        else
        {
            order++;
        }
        codeBox.GetComponent<Text>().text = "";
        complieCode();
        displayCode();
        inputCode.Clear();
    }

    void displayCode()
    {
        for(int i=0;i<AlphaSize;i++)
        {
            if(currentAlphacode[i]==shortKeycode)
            {
                codeBox.GetComponent<Text>().text += ".";
            }
            else if (currentAlphacode[i] == longKeycode)
            {
                codeBox.GetComponent<Text>().text += "-";
            }
        }
    }

    void changeAlphaColor()
    {
        Debug.Log("current to change:" + alphbet[order]);
        if(displayBox.GetComponent<Text>().text.Contains(alphbet[order].ToString().ToUpper()))
        {

            string target = alphbet[order].ToString().ToUpper();
            string frontString = alphbet.ToUpper().Substring(0, order);
            string frontResult=new string("");
            for(int i=0;i<order;i++)
            {
               
                frontResult += frontString.Substring(i, 1).Replace(frontString.Substring(i, 1), color + frontString.Substring(i, 1) + "</color>");
                
            }
            displayBox.GetComponent<Text>().text =frontResult+ 
                                        alphbet.ToUpper().Substring(order,1).Replace(target, color+alphbet[order].ToString().ToUpper()+"</color>")
                                        + alphbet.ToUpper().Substring(order+1);
            //string table = displayBox.GetComponent<Text>().text;
            //char[] array = table.ToCharArray();
            Debug.Log("changed");
        }
    }

    void changeCodeColor(int Codeorder)
    {
        int j,k;
        string target = new string("");
        for(int i=0;i<AlphaSize;i++)
        {
            if (currentAlphacode[i] == shortKeycode)
                target += ".";
            else if (currentAlphacode[i] == longKeycode)
                target += "-";
            else
                target += "";
        }
        string result=new string("");
        for(j=0;j<=Codeorder;j++)
        {
           
            result += target[j].ToString().Replace(target[j].ToString(), color + target[j].ToString() + "</color>");
        }

        for(k=j;k<AlphaSize;k++)
        {
                result += target[k].ToString();
        }   
        codeBox.GetComponent<Text>().text = result;
        
        Debug.Log("code color changed");
    }
    void reStart()
    {
        isSuccess = false;
        isOver = false;
        isDone = false;
        
        order = 0;
        inputCode.Clear();
        passBox.GetComponent<Text>().text = "";
        codeBox.GetComponent<Text>().text = "";
        displayBox.GetComponent<Text>().text = alphbet.ToUpper();
        complieCode();
        displayCode(); 
    }
    
    public int jugement()
    {
        if (isFail)
            return 1;//输错了
        if (isDone)
            return 2;//全对
        return 0;
    }
    void eventTrigger()
    {
        if (jugement() == 1)
        { 
            Debug.Log("code fail!");
            CustomEvent.Trigger(this.gameObject, "fail");
        }
        else if (jugement() == 2)
        { 
            Debug.Log("all pass");
            CustomEvent.Trigger(this.gameObject, "success");
        }
        else
        { 
            //Debug.Log("none"); 
        }
    }

    void randomLoad()
    {
        alphbet = "";
        Debug.Log("alphabetSize" + alphabetSize);
        for (int i = 0; i < alphabetSize; i++)
        { 
            int randomFlag = UnityEngine.Random.Range(1, alphabetSize+1);
            
            Debug.Log("randomFloat" + UnityEngine.Random.Range(1, alphabetSize+1));
            Debug.Log("randomFlag"+randomFlag);
            generateRandomFlag(randomFlag);
        }
    }
    void generateRandomFlag(int randomFlag)
    {
        switch (randomFlag)
        {
            case 1: alphbet += "e"; break;
            case 2: alphbet += "r"; break;
            case 3: alphbet += "t"; break;
            default:
                {
                    randomFlag = UnityEngine.Random.Range(1, alphabetSize + 1);
                    generateRandomFlag(randomFlag); 
                    break; 
                }
        }
    }
    public static IEnumerator WaitForSeconds(float duration, Action action = null)
    {
        yield return new WaitForSeconds(duration);
        action?.Invoke();
    }
}
