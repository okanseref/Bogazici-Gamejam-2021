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
    IEnumerator bossTick;
    void Start()
    {
        GoldBarBackWidth = GoldBarBack.GetComponent<RectTransform>().sizeDelta.x;
        ArriveBarBackHeight = ArriveBarBack.GetComponent<RectTransform>().sizeDelta.y;

        NewStage(100, 80,2);
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
        bossTick = BossTick();
        StartCoroutine(bossTick);
    }

    Ray ray;
    RaycastHit hit;
    void Update()
    {
        Worker = 0;
        Sick = 0;
        Death = 0;
        foreach (GameObject element in GameObject.FindGameObjectsWithTag("Worker"))
        {
            if (element.gameObject.GetComponent<Worker>().Status.Equals("Working")){
                Worker++;
            }else if (element.gameObject.GetComponent<Worker>().Status.Equals("Sick")){
                Sick++;
            }else{
                Death++;
            }
        }
        ArriveBar.GetComponent<RectTransform>().sizeDelta = new Vector2( ArriveBar.GetComponent<RectTransform>().sizeDelta.x,(float)(1 - BossArrive / BossStartTime) * ArriveBarBackHeight);
        GoldBar.GetComponent<RectTransform>().sizeDelta = new Vector2((float)Coin/Max* GoldBarBackWidth, GoldBar.GetComponent<RectTransform>().sizeDelta.y);
        CoinText.GetComponent<TextMeshProUGUI>().text = Coin.ToString("F2");
        SickCount.GetComponent<TextMeshProUGUI>().text="x"+Sick.ToString();
        WorkerCount.GetComponent<TextMeshProUGUI>().text= "x"+Worker.ToString();
        DeathCount.GetComponent<TextMeshProUGUI>().text="x"+Death.ToString();
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

        /*ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) //Mouse obje üzerine gelinec
        {
            if (hit.collider.gameObject.tag == "Worker") {
                hit.collider.gameObject.GetComponent<Worker>().ToggleActivePanel(true);
            }
        }*/
    }
    public bool AddCoin(float val)
    {
        Coin += val;
        if (Coin > Max)
        {
            Coin = Max;
        }
        if (Coin < 0)
        {
            Coin = 0;
            return false;
        }
        return true;
    }
    private IEnumerator BossTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            BossArrive -= 0.1f;
            print(BossArrive);

            if (BossArrive < 0)
            {
                print("OVER");
                StopCoroutine(bossTick);
            }
        }
    }
}
