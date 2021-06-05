using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Karakter : MonoBehaviour
{
    // Start is called before the first frame update
    Animator anim;
    bool Walking = false;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Walking = false;
        if (Input.GetKey(KeyCode.D)&&transform.position.x<35) //Sað limit
        {
            transform.position = new Vector3(transform.position.x+0.05f, transform.position.y, transform.position.z);
            gameObject.transform.eulerAngles = new Vector3(0, 90, 0);
            Walking = true;
        }
        if (Input.GetKey(KeyCode.A) && transform.position.x > 4) //Sol limit
        {
            transform.position = new Vector3(transform.position.x-0.05f, transform.position.y, transform.position.z);
            gameObject.transform.eulerAngles = new Vector3(0, -90, 0);

            Walking = true;
        }
        anim.SetBool("isWalking", Walking);
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
}
