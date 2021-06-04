using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    [Header("Bottom Left")]
    [SerializeField] GameObject CoinText;
    [SerializeField] GameObject MaxText;
    [SerializeField] GameObject BossDemandText;
    [SerializeField] GameObject GoldBar;
    [SerializeField] GameObject GoldBarBack;


    [Space(10)]
    [Header("Bottom Right")]
    [SerializeField] GameObject SickCount;
    [SerializeField] GameObject WorkerCount;
    [SerializeField] GameObject DeathCount;   
    [SerializeField] GameObject SickImage;
    [SerializeField] GameObject WorkerImage;
    [SerializeField] GameObject DeathImage;

    [Space(10)]
    [Header("Top Left")]
    [SerializeField] GameObject ArriveBarBack;
    [SerializeField] GameObject ArriveBar;
    public float Coin=0,BossArrive, Max, BossDemand;
    public int Worker, Sick, Death;
    float GoldBarBackWidth, ArriveBarBackHeight, BossStartTime;
    void Start()
    {
        GoldBarBackWidth = GoldBarBack.GetComponent<RectTransform>().sizeDelta.x;
        ArriveBarBackHeight = ArriveBarBack.GetComponent<RectTransform>().sizeDelta.y;

        NewStage(100, 80,10);
        print(GoldBarBackWidth);
    }

    // Update is called once per frame
    public void NewStage(int maxLimit,int demand,float bossArrive)
    {
        BossStartTime = bossArrive;
        BossArrive = bossArrive;
        Max = maxLimit;
        BossDemand = demand;
        MaxText.GetComponent<TextMeshProUGUI>().text = "Max: "+Max.ToString();
        BossDemandText.GetComponent<TextMeshProUGUI>().text = "Demand: "+BossDemand.ToString();
        BossDemandText.GetComponent<RectTransform>().anchoredPosition = new Vector3((float)(BossDemand / Max) * GoldBarBackWidth, BossDemandText.GetComponent<RectTransform>().anchoredPosition.y,0);
        print((float)(BossDemand / Max) * GoldBarBackWidth);
        print(BossDemandText.GetComponent<RectTransform>().localPosition);
        print(BossDemandText.GetComponent<RectTransform>().position);
        print(BossDemandText.GetComponent<RectTransform>().anchoredPosition);
        StartCoroutine(BossTick(bossArrive));
    }
    void Update()
    {
        ArriveBar.GetComponent<RectTransform>().sizeDelta = new Vector2( ArriveBar.GetComponent<RectTransform>().sizeDelta.x,(float)(1 - BossArrive / BossStartTime) * ArriveBarBackHeight);
        GoldBar.GetComponent<RectTransform>().sizeDelta = new Vector2((float)Coin/Max* GoldBarBackWidth, GoldBar.GetComponent<RectTransform>().sizeDelta.y);
        CoinText.GetComponent<TextMeshProUGUI>().text = Coin.ToString();
        SickCount.GetComponent<TextMeshProUGUI>().text=Sick.ToString();
        WorkerCount.GetComponent<TextMeshProUGUI>().text= Worker.ToString();
        DeathCount.GetComponent<TextMeshProUGUI>().text=Death.ToString();
        if (Worker == 0)
        {
            WorkerImage.SetActive(false);
        }else if (!WorkerImage.activeSelf)
        {
            WorkerImage.SetActive(true);
        }
        if (Sick == 0)
        {
            SickImage.SetActive(false);
        }
        else if (!SickImage.activeSelf)
        {
            SickImage.SetActive(true);
        }
        if (Death == 0)
        {
            DeathImage.SetActive(false);
        }
        else if (!DeathImage.activeSelf)
        {
            DeathImage.SetActive(true);
        }
    }
    // every 2 seconds perform the print()
    private IEnumerator BossTick(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            BossArrive -= 0.1f;
            print(BossArrive);

            if (BossArrive < 0)
            {
                print("OVER");

                yield return null;
            }
        }
    }
}
