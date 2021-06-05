using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject karakter;
    public bool startMove = false;
    public float Strength=5;
    [SerializeField] GameObject Fade;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (startMove)
        {
            transform.position = new Vector3(transform.position.x + 0.2f, transform.position.y, transform.position.z);
        }
        if (startMove ==true&& Mathf.Abs(karakter.transform.position.x - transform.position.x) < 2.5f)
        {
            print("Triggered");
            GetComponent<Animator>().SetTrigger("Kick");
            Invoke("Kick", 1.3f);
            startMove = false;
        }
    }
    private void Kick()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<SoundControl>().PlaySound(6);

        karakter.GetComponent<Rigidbody>().AddForce(new Vector3(Strength, Strength, 0));
        FindObjectOfType<CinemachineVirtualCamera>().Follow = null;
        //karakter.GetComponent<Rigidbody>().AddRelativeTorque(Vector3.up * 1000);
        karakter.GetComponent<Rigidbody>().freezeRotation = false;
        Fade.GetComponent<Animator>().SetTrigger("Fade");

        //Invoke("FadeOut", 1f);

    }
    private void FadeOut()
    {
    }
}
