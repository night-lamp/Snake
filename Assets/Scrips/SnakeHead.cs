using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SnakeHead : MonoBehaviour
{
    public List<Transform> bodyList = new List<Transform>();
    private Transform canvas;
    public GameObject bodyPrefab;
    public Sprite[] bodySprites = new Sprite[2];

    private float speed = 0.2f;
    public int step;
    private int x;
    private int y;
    private Vector3 headPos;

    private bool isDie = false;
    public GameObject dieEffect;

    public AudioClip eatClip;
    public AudioClip dieClip;

    void Awake()
    {
        canvas = GameObject.Find("Canvas").transform;
        //通过Resources.Load(string path)方法加载资源，path的书写不需要加Resources/以及文件名
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("sh", "sh02"));
        bodySprites[0] = Resources.Load<Sprite>(PlayerPrefs.GetString("sb01", "sb0201"));
        bodySprites[1] = Resources.Load<Sprite>(PlayerPrefs.GetString("sb02", "sb0202"));
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Move", 0, speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && MainUIController.Instance.isPause == false && isDie == false)
        {
            CancelInvoke();
            InvokeRepeating("Move", 0, speed - 0.15f);
        }
        if (Input.GetKeyUp(KeyCode.Space) && MainUIController.Instance.isPause == false && isDie == false)
        {
            CancelInvoke();
            InvokeRepeating("Move", 0, speed);
        }
        if (Input.GetKey(KeyCode.W) && y!= -step && MainUIController.Instance.isPause == false && isDie == false)
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            x = 0; y = step;
        }
        if (Input.GetKey(KeyCode.S) && y != step && MainUIController.Instance.isPause == false && isDie == false)
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);
            x = 0; y = -step;
        }
        if (Input.GetKey(KeyCode.A) && x != step && MainUIController.Instance.isPause == false && isDie == false)
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
            x = -step; y = 0;
        }
        if (Input.GetKey(KeyCode.D) && x != -step && MainUIController.Instance.isPause == false && isDie == false)
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, -90);
            x = step; y = 0;
        }
    }

    void Move()
    {
        headPos = gameObject.transform.localPosition;                                               //保存蛇身移动前的位置
        gameObject.transform.localPosition = new Vector3(headPos.x + x, headPos.y + y, headPos.z);  //蛇头向希望位置移动
        if (bodyList.Count > 0)
        {   //双色蛇身不适用
            //bodyList.Last().localPosition = headPos;                                              //将蛇尾移动到蛇身移动前的位置
            //bodyList.Insert(0, bodyList.Last());                                                  //将蛇尾在List中的位置更新到最前
            //bodyList.RemoveAt(bodyList.Count - 1);                                                //移除List最末尾的蛇尾引用
            for(int i = bodyList.Count - 2; i >= 0; i--)                                            //从后往前开始移动蛇身
            {
                bodyList[i + 1].localPosition = bodyList[i].localPosition;                          //每一个蛇身都移动到前一个节点的位置
            }
            bodyList[0].localPosition = headPos;                                                    //第一个蛇身移动到蛇头移动前的位置
        }
    }

    void Grow()
    {
        AudioSource.PlayClipAtPoint(eatClip, Vector3.zero);
        int index = (bodyList.Count % 2 == 0) ? 0 : 1;
        GameObject body = Instantiate(bodyPrefab,new Vector3(2000,2000,0),Quaternion.identity);
        body.GetComponent<Image>().sprite = bodySprites[index];
        body.transform.SetParent(canvas, false);
        bodyList.Add(body.transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            Destroy(collision.gameObject);
            MainUIController.Instance.UpdateUI();
            Grow();
            FoodMaker.Instance.MakeFood((Random.Range(0, 100) < 20) ? true : false);
        }
        else if (collision.gameObject.CompareTag("Reward"))
        {
            Destroy(collision.gameObject);
            MainUIController.Instance.UpdateLength(Random.Range(5,20));
        }
        else if (collision.gameObject.CompareTag("Body"))
        {
            Die();
        }
        else
        {
            if (MainUIController.Instance.hasBorder)
            {
                Die();
            }
            else
            {
                switch (collision.gameObject.name)                                                       //自由模式
                {
                    case "Up":
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y + 20, transform.localPosition.z);
                        break;
                    case "Down":
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y - 20, transform.localPosition.z);
                        break;
                    case "Left":
                        transform.localPosition = new Vector3(-transform.localPosition.x + 135, transform.localPosition.y, transform.localPosition.z);
                        break;
                    case "Right":
                        transform.localPosition = new Vector3(-transform.localPosition.x + 160, transform.localPosition.y, transform.localPosition.z);
                        break;
                }
            }
        }
    }  
    void Die()
    {
        AudioSource.PlayClipAtPoint(dieClip, Vector3.zero);
        CancelInvoke();
        isDie = true;
        Instantiate(dieEffect);
        PlayerPrefs.SetInt("last_l", MainUIController.Instance.length);
        PlayerPrefs.SetInt("last_s", MainUIController.Instance.score);
        if (PlayerPrefs.GetInt("bast_s", 0) < MainUIController.Instance.score)
        {
            PlayerPrefs.SetInt("bast_l", MainUIController.Instance.length);
            PlayerPrefs.SetInt("bast_s", MainUIController.Instance.score);
        }
        StartCoroutine(Gameover(1.5f));
    }
    IEnumerator Gameover(float t)
    {
        yield return new WaitForSeconds(t);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
