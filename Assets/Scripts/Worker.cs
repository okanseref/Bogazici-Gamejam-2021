using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    [SerializeField] GameObject Canvas, ActivePanel, PassivePanel, HealthBarBack, HealthBar,EfficiencyText,EfficiencyBack,ProfitText,MaintenanceText,Corpse;
    public string Status;
    float HealthBarWidth;
    float Efficiency, EfficiencyLoss = -0.5f;
    float Health,HealthMax,HealthGain=0.3f ;
    float BaseYield, BaseMaintenance, Maintenance;
    public float SickChance = 0,SickChanceSpeed=0.4f,SickCheckSeconds=3,SickKirbac=3f,cureChance=0,cureChanceSpeed=2f;
    IEnumerator sickEnum,gameEnum,cureEnum;
    public GameObject particle = null;
    float GameSpeed = 1;
    Animator anim;
    public int MineIndex = 0;
    void Start()
    {
        HealthBarWidth = HealthBarBack.GetComponent<RectTransform>().sizeDelta.x;
        anim = GetComponent<Animator>();
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
        GameObject.FindGameObjectWithTag("GameController").GetComponent<SoundControl>().PlaySound(3);

        int Val = Random.RandomRange(8, 22);
        int Val2 = Random.RandomRange(8, 22);
        SickChance += SickKirbac;
        Damage(Val);
        if (Status == "Working")
        {
            EfficiencyChange(Val2);
            anim.SetTrigger("GotHit");
        }
        else
        {
            Dead();
        }

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
        anim.SetInteger("State", 1);
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
        anim.SetBool("SickHealed", true);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<SoundControl>().PlaySound(4);
    }
    private void Dead()
    {
        StopCoroutine(sickEnum);
        StopCoroutine(gameEnum);
        Status = "Dead";
        Maintenance = 0;
        Efficiency = 0;
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStats>().Nests[MineIndex] = false;
        anim.SetBool("SickHealed", true);
        Invoke("DestroyGmj",2);
    }
    private void DestroyGmj()
    {
        Destroy(Instantiate(Corpse, gameObject.transform.position, Quaternion.identity),3f);
        Destroy(gameObject);
    }
    private void OnMouseDown() //Kýrbaçlama
    {
        if(Status != "New")
        {
            KirbacKontrol();
        }
    }
    private void KirbacKontrol()
    {
        if (Mathf.Abs(GameObject.FindGameObjectWithTag("Karakter").transform.position.x - transform.position.x) < 1)
        {
            GameObject.FindGameObjectWithTag("Karakter").GetComponent<Karakter>().Kirbac();
            Kirbac();
        }
    }
    private void OnMouseEnter()
    {
        if (Status != "New")
        {
            ActivePanel.SetActive(true);
        }
    }
    private void OnMouseExit()
    {
        if (Status != "New")
        {
            ActivePanel.SetActive(false);
        }
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            KirbacKontrol();
        }
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
    public void ParticleInstance()
    {
        Vector3 pos = transform.position;
        pos.y += 1.25f;
        pos.z += 2;
        Destroy(Instantiate(particle, pos, Quaternion.identity),1f);
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
            
            if (Random.Range(0,100)<SickChance&&SickChance>8) // Çok erken hasta olmasýn
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
                anim.SetBool("SickHealed", false);

                StopCoroutine(cureEnum);
            }
        }
    }
}
