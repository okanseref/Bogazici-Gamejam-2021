using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    [SerializeField] GameObject Canvas, ActivePanel, PassivePanel, HealthBarBack, HealthBar,EfficiencyText,EfficiencyBack,ProfitText,MaintenanceText;
    public string Status;
    float HealthBarWidth;
    float Efficiency, EfficiencyLoss = -0.5f;
    float Health,HealthMax,HealthGain=0.3f ;
    float BaseYield, BaseMaintenance, Maintenance;
    public float SickChance = 0,SickChanceSpeed=0.4f,SickCheckSeconds=3,SickKirbac=3f,cureChance=0,cureChanceSpeed=2f;
    IEnumerator sickEnum,gameEnum,cureEnum;
    float GameSpeed = 1;

    void Start()
    {
        HealthBarWidth = HealthBarBack.GetComponent<RectTransform>().sizeDelta.x;
        HealthMax = 100;
        Efficiency = 50;
        EfficiencyText.GetComponent<TMPro.TextMeshProUGUI>().text = ((int)Efficiency).ToString();
        Health = HealthMax;
        BaseYield = 1;
        BaseMaintenance = -0.25f;
        Maintenance = BaseMaintenance;
        sickEnum = SickChanceEnum();
        gameEnum = GameEnum();
        cureEnum = CureEnum();
         
        Working(); //Corotinleri baþlatýyor

    }
    private void Kirbac()
    {
        int Val = Random.RandomRange(8, 22);
        int Val2 = Random.RandomRange(8, 22);
        SickChance += SickKirbac;
        Damage(Val);
        EfficiencyChange(Val2);

    }
    private void EfficiencyChange(float val)
    {
        Efficiency += val;
        Efficiency = Mathf.Clamp(Efficiency, 0, 100);
        EfficiencyText.GetComponent<TMPro.TextMeshProUGUI>().text = ((int)Efficiency).ToString();
        if (Efficiency > 75){
            EfficiencyText.GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
        }else if(Efficiency > 35) {
            EfficiencyText.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(1, 0.90f, 0.14f, 1);
        }
        else{
            EfficiencyText.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(1,0.14f,0.14f,1);
        }
    }
    private void Damage(float damage)
    {
        Health -= damage;
        if (Health < 0)
        {
            Health = 0;
            Dead();
        }
        HealthBar.GetComponent<RectTransform>().sizeDelta = new Vector2((float)Health / HealthMax * HealthBarWidth, HealthBar.GetComponent<RectTransform>().sizeDelta.y);

    }
    private void Heal(float heal)
    {
        Health += heal;
        if (Health > HealthMax)
        {
            Health = HealthMax;
        }
        HealthBar.GetComponent<RectTransform>().sizeDelta = new Vector2((float)Health / HealthMax * HealthBarWidth, HealthBar.GetComponent<RectTransform>().sizeDelta.y);
    }
    private void Working()
    {
        Status = "Working";
        Efficiency = Random.RandomRange(50,80);
        Maintenance = BaseMaintenance;
        StartCoroutine(sickEnum);
        StartCoroutine(gameEnum);
    }
    private void Sick()
    {
        Status = "Sick";
        Maintenance *= 3;
        Efficiency = 0;
        Health = 10;
        SickChance = 0;
        cureChance = 0;
        StartCoroutine(cureEnum);
    }
    private void Dead()
    {
        Status = "Dead";
        Maintenance = 0;
        Efficiency = 0;
        StopCoroutine(sickEnum);
        StopCoroutine(gameEnum);

        //Destroy(gameObject);
    }
    private void OnMouseDown()
    {
        Kirbac();
    }
    private void OnMouseEnter()
    {
        ActivePanel.SetActive(true);

    }
    private void OnMouseExit()
    {
        ActivePanel.SetActive(false);

    }
    private void FixedUpdate()
    {
        
        EfficiencyBack.transform.Rotate(0f, 0.0f,Efficiency/100*3, Space.Self);

        GameTick(false);
    }
    private void GameTick(bool gain) //UI güncelleme içinse false
    {
        float Profit = 0;
        if (Status.Equals("Working"))
        {
            if (gain == true)
            {
                SickChance += SickChanceSpeed;
            }
            Profit = (Efficiency / 100 * BaseYield);
        }
        else if (Status.Equals("Sick"))
        {
            if (gain == true)
            {
                cureChance += cureChanceSpeed;
            }
            Profit = 0;
        }
        if (gain == true)
        {
            EfficiencyChange(EfficiencyLoss);
            Heal(HealthGain);
            //print("Gain: " +( Profit + Maintenance));
            bool result= GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStats>().AddCoin(Profit+Maintenance);
            if (result == false && Status.Equals("Sick")) // Para bittiyse ölüyor
            {
                Dead();
            }
        }
        ProfitText.GetComponent<TMPro.TextMeshProUGUI>().text = Profit.ToString("F2");
        MaintenanceText.GetComponent<TMPro.TextMeshProUGUI>().text = Maintenance.ToString("F2");
    }
    IEnumerator GameEnum()
    {
        while (true)
        {
            yield return new WaitForSeconds(GameSpeed);
            print("Game tick");
            GameTick(true);
        }
    }
    IEnumerator SickChanceEnum()
    {
        while (true)
        {
            yield return new WaitForSeconds(SickCheckSeconds);
            print("Sick Checked");
            
            if (Random.Range(0,100)<SickChance&&SickChance<8) // Çok erken hasta olmasýn
            {
                Sick();
                print("Sicked");
                StopCoroutine(sickEnum);
            }
        }
    }
    IEnumerator CureEnum()
    {
        while (true)
        {
            yield return new WaitForSeconds(SickCheckSeconds);
            print("Sick Checked");

            if (Random.Range(0, 100) < cureChance)
            {
                Health = Random.RandomRange(30, 60);
                Working();
                print("Cured");
                StopCoroutine(cureEnum);
            }
        }
    }
}
