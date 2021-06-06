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

    [Space(10)]
    [Header("PopUp")]
    [SerializeField] GameObject NewWorkerPopUp;
    [SerializeField] GameObject NewWorkerText;
    [SerializeField] GameObject NewWorkerYesButton;
    [SerializeField] GameObject NewWorkerNoButton;



    [Space(10)]
    [Header("Prefabs")]
    [SerializeField] GameObject WorkerPrefab;
    [SerializeField] GameObject CanvasWhole;
    [SerializeField] GameObject Boss;

    [Space(10)]
    [Header("Marketplace")]
    [SerializeField] GameObject MarketPlaceOpen;
    [SerializeField] GameObject MarketPanel;

    public float Coin=0,BossArrive, Max, BossDemand;
    public int Worker, Sick, Death;
    float GoldBarBackWidth, ArriveBarBackHeight, BossStartTime;
    int NestCount = 6;
    public bool[] Nests = new bool[6];
    bool GameActive = true,newWorkerWaiting=false;
    float newWorkerRate = 8;
    IEnumerator bossTick,newWorkerEnum;
    public bool MarketTrigger = false;
    public int yourScore = 0;
    void Start()
    {
        GoldBarBackWidth = GoldBarBack.GetComponent<RectTransform>().sizeDelta.x;
        ArriveBarBackHeight = ArriveBarBack.GetComponent<RectTransform>().sizeDelta.y;

        NewStage(100, 50,45,12);

    }

    // Update is called once per frame
    public void NewStage(int maxLimit,int demand,float bossArrive,int newWorker)
    {
        newWorkerRate = UnityEngine.Random.RandomRange(newWorker-2,newWorker+4);
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
        newWorkerEnum = NewWorkerEnum();
        StartCoroutine(newWorkerEnum);
        Nests[0] = true;
    }
    private void Pause()
    {
        Time.timeScale = 0;
    }    
    private void Resume()
    {
        Time.timeScale = 1;
    }
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
        CoinText.GetComponent<TextMeshProUGUI>().text = Coin.ToString("F1");
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
        if (MarketTrigger == true&&!MarketPanel.activeSelf)
        {
            MarketPlaceOpen.SetActive(true);
        }
        else
        {
            MarketPlaceOpen.SetActive(false);
        }
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
                if (Coin < BossDemand)
                {
                    //Yeteri kadar biriktiremedik
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<SoundControl>().PlaySound(2);

                    Boss.GetComponent<Boss>().startMove = true;
                    CanvasWhole.SetActive(false);
                    GameObject.FindGameObjectWithTag("Karakter").GetComponent<Karakter>().isActive = false;
                    StopAllCoroutines();
                }
                else
                {
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<SoundControl>().PlaySound(7);
                    BossArrive = 30;
                    Coin -= BossDemand;
                    NewStage((int)Max+25, (int)BossDemand+20, 30, (int)newWorkerRate+6);
                    yourScore++;
                }
            }
        }
    }    

    private IEnumerator NewWorkerEnum()
    {
        while (true)
        {
            yield return new WaitForSeconds(newWorkerRate);
            if (newWorkerWaiting == false&& GameActive==true)
            {
                NewWorker();
            }
        }
    }
    private void PutWorker(int nestIndex)
    {
        GameObject gmj = Instantiate(WorkerPrefab, new Vector3(11+nestIndex*3f, -0.68f, -22.79f), new Quaternion(0, 0, 0, 0));
        gmj.GetComponent<Worker>().MineIndex = nestIndex;
    }
    private void NewWorker()
    {
        int count = 0;
        foreach (GameObject element in GameObject.FindGameObjectsWithTag("Worker"))
        {
            if (element.gameObject.GetComponent<Worker>().Status.Equals("Working"))
            {
                count++;
            }
            else if (element.gameObject.GetComponent<Worker>().Status.Equals("Sick"))
            {
                count++;
            }
            else
            {
                count++;
            }
        }
        if (count >= 6)
        {
            return;
        }
        newWorkerWaiting = true;
        NewWorkerPopUp.SetActive(true);
        NewWorkerText.GetComponent<TextMeshProUGUI>().text = "New worker has arrived. Can he join?";
    }
    public void NewWorkerYes()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<SoundControl>().PlaySound(0);
        for (int i = 0; i < NestCount; i++)
        {
            if (Nests[i] == false)
            {
                Nests[i] = true;
                PutWorker(i);
                NewWorkerPopUp.SetActive(false);
                newWorkerWaiting = false;
                return;
            }
        }
    }   
    public void NewWorkerNo()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<SoundControl>().PlaySound(0);
        NewWorkerPopUp.SetActive(false);
        newWorkerWaiting = false;
        return;
    }
    public void MarketCloseFunc()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<SoundControl>().PlaySound(0);

        MarketPanel.SetActive(false);

        return;
    }
    public void MarketOpenFunc()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<SoundControl>().PlaySound(0);

        MarketPanel.SetActive(true);
        MarketPlaceOpen.SetActive(false);

        return;
    }
}
