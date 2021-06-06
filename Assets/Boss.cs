using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject karakter;
    public bool startMove = false;
    public float Strength=5;
    [SerializeField] GameObject Fade;
    [SerializeField] GameObject YourScore;
    [SerializeField] GameObject BestScore;
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

        Invoke("FadeOutEbd", 3f);

    }
    private void FadeOutEbd()
    {
        YourScore.GetComponent<TextMeshProUGUI>().text = "Your Score: "+GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStats>().yourScore;

        BestScore.GetComponent<TextMeshProUGUI>().text = "Best Score: " + PlayerPrefs.GetInt("bestscore", 0);
        YourScore.SetActive(true);
        BestScore.SetActive(true);
        Invoke("ReturnToMenu", 6f);

    }
    private void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
