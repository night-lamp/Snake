using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodMaker : MonoBehaviour
{
    private static FoodMaker _instance;

    public static FoodMaker Instance
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

    public int xlimit = 22;
    public int ylimit = 11;
    public int xoffset = 7;
    public GameObject foodPrefab;
    public Sprite[] foodSprites;
    private Transform FoodHolder;
    public GameObject rewardPrefab;

    public void MakeFood(bool isReward)
    {
        int index = Random.Range(0, foodSprites.Length);
        GameObject food = Instantiate(foodPrefab);
        food.GetComponent<Image>().sprite = foodSprites[index];
        food.transform.SetParent(FoodHolder, false);
        int x = Random.Range(-xlimit + xoffset, xlimit);
        int y = Random.Range(-ylimit, ylimit);
        food.transform.localPosition = new Vector3(x * 21, y * 20, 0);
        if (isReward)
        {
            GameObject reward = Instantiate(rewardPrefab);
            reward.transform.SetParent(FoodHolder, false);
            reward.transform.SetParent(FoodHolder, false);
            reward.transform.SetParent(FoodHolder, false);
            reward.transform.SetParent(FoodHolder, false);
            x = Random.Range(-xlimit + xoffset, xlimit);
            y = Random.Range(-ylimit, ylimit);
            reward.transform.localPosition = new Vector3(x * 21, y * 20, 0);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        FoodHolder = GameObject.Find("FoodHolder").transform;
        //InvokeRepeating("MakeFood(false)", 0.2f, 0.25f);
        MakeFood(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
