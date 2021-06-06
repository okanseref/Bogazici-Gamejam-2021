using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Karakter : MonoBehaviour
{
    // Start is called before the first frame update
    Animator anim;
    public bool Walking = false,whipButton=false,isActive=true;
    [SerializeField ]GameObject whipButtonObject;
    void Start()
    {
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isActive==false)
        {
            anim.SetBool("isWalking", false);

            gameObject.transform.eulerAngles = new Vector3(0, -90, 0);
            return;
        }
        Walking = false;
        if (Input.GetKey(KeyCode.D)&&transform.position.x<35) //Sað limit
        {
            transform.position = new Vector3(transform.position.x+0.06f, transform.position.y, transform.position.z);
            gameObject.transform.eulerAngles = new Vector3(0, 90, 0);
            Walking = true;
        }
        if (Input.GetKey(KeyCode.A) && transform.position.x > 4) //Sol limit
        {
            transform.position = new Vector3(transform.position.x-0.06f, transform.position.y, transform.position.z);
            gameObject.transform.eulerAngles = new Vector3(0, -90, 0);

            Walking = true;
        }
        anim.SetBool("isWalking", Walking);

        whipButton = false;
        foreach (GameObject gm in GameObject.FindGameObjectsWithTag("Worker"))
        {
            if (Mathf.Abs(gm.transform.position.x - transform.position.x) < 1)
            {
                whipButton = true;
            }
        }

        whipButtonObject.SetActive(whipButton);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Marketplace")
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStats>().MarketTrigger=true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Marketplace")
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStats>().MarketTrigger = false;
        }
    }
    public void Kirbac()
    {
        GameObject.FindGameObjectWithTag("Karakter").GetComponent<Animator>().SetTrigger("Kirbac");
        gameObject.transform.eulerAngles = new Vector3(0, 0, 0);

    }
    public void KirbacRotateEnd()
    {
        GameObject.FindGameObjectWithTag("Karakter").GetComponent<Animator>().SetTrigger("Kirbac");

    }
}
