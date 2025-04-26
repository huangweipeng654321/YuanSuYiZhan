using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightFire : MonoBehaviour
{
    public GameObject LimitLeft;
    public GameObject LimitRight;
    public GameObject Block;
    [Header("设置左右边界")]
    public float borderLeft;
    public float borderRight;
    
    [Range(0, 1.5f)][Header("速度生成范围")]
    public float minSpeed=0.5f;
    [Range(0, 1.5f)]
    public float maxSpeed=1.5f;
    [Header("修改动画机")]
    public GameObject lightAnim;
    [Header("初始位置")]
    [Range(90,270)]
    public float origin = 180f;
    [Header("只读")][Range(0, 1f)][SerializeField]
    private float fillAmountLeft;
    [Range(0, 1f)][SerializeField]
    private float fillAmountRight;
    [SerializeField]
    private float speed;

    private bool isClockWise=false;

    bool isStop=false;
    float rightAngle;
    float leftAngle;

    Transform blockTransform;
    Vector3 direction;
    Animator anim;
    private void Awake()
    {
        LimitLeft.GetComponent<Image>().fillAmount= fillAmountLeft ;
        LimitRight.GetComponent<Image>().fillAmount= fillAmountRight;
        anim = lightAnim.GetComponent<Animator>();
        rightAngle = 360*fillAmountRight;
        leftAngle =360*(1- fillAmountLeft);
        
        blockTransform = Block.transform;
        
        if (isClockWise)
        {
            direction = Vector3.back;
        }
        else
        {
            direction = Vector3.forward;
        }

        Debug.Log("left:" + leftAngle + "right:" + rightAngle);
        
    }

    private void OnEnable()
    {
        reLoad();
        randomLoad();
        StartCoroutine("qte");
        isStop = false;
        Debug.Log("on");
    }

    private void OnDisable()
    {
        StopCoroutine("qte");
        //reLoad();
    }
    private void Update()
    {
        
        if (Input.GetButtonDown("Stop"))
        {
            Debug.Log("Stop");
            Debug.Log(blockTransform.rotation.eulerAngles.z);
            isStop = true;
            //StopCoroutine("qte");
            setAnim();
        }
    }
    public bool jugement()
    {
        if (blockTransform.rotation.eulerAngles.z < leftAngle && blockTransform.rotation.eulerAngles.z > rightAngle)
        {
            Debug.Log("Target");
            return true;
        }
        Debug.Log("no target");
        return false;
    }

    void reLoad()
    {
        while(Mathf.Abs(blockTransform.rotation.eulerAngles.z-origin)>0.5f)
        {
            blockTransform.RotateAround(transform.position, direction, blockTransform.rotation.eulerAngles.z/Mathf.Abs(blockTransform.rotation.eulerAngles.z));
        }
    }

    void setAnim()
    {
        if (jugement())
            anim.SetInteger("Status", 2);
        else
            anim.SetInteger("Status", 1);
    }

    void randomLoad()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        fillAmountLeft = Random.Range(0.3f, 0.7f);
        fillAmountRight = Random.Range(1-fillAmountLeft-0.1f,0.3f);
        LimitLeft.GetComponent<Image>().fillAmount = fillAmountLeft;
        LimitRight.GetComponent<Image>().fillAmount = fillAmountRight;
        rightAngle = 360 * fillAmountRight;
        leftAngle = 360 * (1 - fillAmountLeft);
    }
    IEnumerator qte()
    {
        while (true)
        {
            yield return StartCoroutine("forward");

            yield return StartCoroutine("backward");

        }

    }

    IEnumerator forward()
    {
        while ((Mathf.Abs(blockTransform.rotation.eulerAngles.z - borderRight)>=5f))
        {
            //Debug.Log("forward");
            if(!isStop)
            blockTransform.RotateAround(transform.position, direction, speed* (-1));
            yield return null;
        }
        //yield break;
    }

    IEnumerator backward()
    {
        while (Mathf.Abs(blockTransform.rotation.eulerAngles.z - borderLeft) >= 5f)
        {
            //Debug.Log("backward");
            if(!isStop)
            blockTransform.RotateAround(transform.position, direction, speed );
            yield return null;
        }
        //yield break;
    }

    
}
