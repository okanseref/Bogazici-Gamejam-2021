using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject camera, canvas,startButton, soundButton,bestScore,creditsPanel,creditsOpen,creditsClose,dialog,fade;
    [SerializeField] Sprite soundOn, soundOff;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] sounds;
    int soundBool = 1;
    void Start()
    {
        soundBool= PlayerPrefs.GetInt("sound", 1);
        bestScore.GetComponent<TMPro.TextMeshProUGUI>().text = "Best Score: "+PlayerPrefs.GetInt("bestscore", 0).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewGame()
    {
        canvas.SetActive(false);
        PlaySound(0);
        camera.GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 100;
        Invoke("Scene1", 2.3f);
    }
    private void Scene1()
    {
        dialog.SetActive(true);
        PlaySound(1);
        Invoke("Scene2", 4f);

    }
    private void Scene2()
    {
        fade.GetComponent<Animator>().SetTrigger("Fade");
        Invoke("Scene3", 3.5f);

    }
    private void Scene3()
    {
        SceneManager.LoadScene(1);
    }
    public void soundToggle()
    {
        PlaySound(0);
        if (soundBool == 1)
        {
            soundButton.transform.GetChild(0).GetComponent<Image>().sprite = soundOff;
            PlayerPrefs.SetInt("sound", 0);
            soundBool = PlayerPrefs.GetInt("sound", 1);
        }
        else
        {
            soundButton.transform.GetChild(0).GetComponent<Image>().sprite = soundOn;
            PlayerPrefs.SetInt("sound", 1);
            soundBool = PlayerPrefs.GetInt("sound", 1);
        }
    }
    public void CreditsToggle(bool b)
    {
        PlaySound(0);
        creditsPanel.SetActive(b);
    }
    public void ExitGame()
    {
        PlaySound(0);
        Application.Quit();
    }
    public void PlaySound(int index)
    {
        if (PlayerPrefs.GetInt("sound", 1) == 1)
        {
            audioSource.PlayOneShot(sounds[index], 1f);
        }
    }
}
