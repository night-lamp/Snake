using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour
{
    private static MainUIController _instance;

    public static MainUIController Instance
    {
        get
        {
            return _instance;
        }
    }
    void Awake()
    {
        _instance = this;
    }


    public int score = 0;
    public int length = 0;
    public Text msgText;
    public Text scoreText;
    public Text lengthText;
    public Image bgImage;
    private Color tempColor;
    public bool isPause = false;
    public Image pauseImage;
    public Sprite[] pauseSprites;
    public bool hasBorder = true;

    public void UpdateUI(int s = 5, int l = 1)
    {
        score += s;
        length += l;
        scoreText.text = "得分：\n" + score;
        lengthText.text = "长度：\n" + length;
    }

    public void UpdateLength(int s = 5, int l = 1)
    {
        score += s;
        scoreText.text = "得分：\n" + score;
    }


    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("border",1) == 0)
        {
            hasBorder = false;
            foreach(Transform t in bgImage.gameObject.transform)
            {
                t.gameObject.GetComponent<Image>().enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (score / 10)
        {
            case 3:
                ColorUtility.TryParseHtmlString("#CCEEFFFF",out tempColor);
                bgImage.color = tempColor;
                msgText.text = "阶段" + 2;
                break;
            case 5:
                ColorUtility.TryParseHtmlString("#CCFFBDFF", out tempColor);
                bgImage.color = tempColor;
                msgText.text = "阶段" + 3;
                break;
            case 7:
                ColorUtility.TryParseHtmlString("#EBFFCCFF", out tempColor);
                bgImage.color = tempColor;
                msgText.text = "阶段" + 4;
                break;
            case 9:
                ColorUtility.TryParseHtmlString("#FFF3CCFF", out tempColor);
                bgImage.color = tempColor;
                msgText.text = "阶段" + 5;
                break;
            case 11:
                ColorUtility.TryParseHtmlString("#FFDACCFF", out tempColor);
                bgImage.color = tempColor;
                msgText.text = "无尽阶段";
                break;
        }
    }
    public void Pause()
    {
        isPause = !isPause;
        if (isPause)
        {
            Time.timeScale = 0;
            pauseImage.sprite = pauseSprites[1];
        }
        else
        {
            Time.timeScale = 1;
            pauseImage.sprite = pauseSprites[0];
        }
    }
    public void Home()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
