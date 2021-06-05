using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControl : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] sounds;    // Start is called before the first frame update
    [SerializeField] AudioClip[] musicAudio;    // Start is called before the first frame update
    bool volumeChange = false;
    float valueToChange = 0, startingValue = 0;
    float time = 0, endTime = 0;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }
    public void PlaySound(int index)
    {
        if (PlayerPrefs.GetInt("sound",1) == 1)
        {
            audioSource.PlayOneShot(sounds[index], 1f);
        }
    }
    public void ChangeMusic(int index)
    {
        if (gameObject.transform.GetChild(0).GetComponent<AudioSource>().clip != musicAudio[index])
        {
            gameObject.transform.GetChild(0).GetComponent<AudioSource>().clip = musicAudio[index];
            gameObject.transform.GetChild(0).GetComponent<AudioSource>().Play();
        }
    }
    public void VolumeSlowly(float value, float timeT)
    {
        if (PlayerPrefs.GetInt("music", 1) == 1)
        {
            if (valueToChange > 0.5f) { valueToChange = 0.5f; }
            time = 0;
            valueToChange = value;
            endTime = timeT;
            startingValue = gameObject.transform.GetChild(0).GetComponent<AudioSource>().volume;
            volumeChange = true;
        }
    }
    void Update()
    {
        if (volumeChange == true)
        {
            time += Time.deltaTime;
            gameObject.transform.GetChild(0).GetComponent<AudioSource>().volume = (time / endTime * (valueToChange - startingValue)) + startingValue;
            if (time >= endTime)
            {
                gameObject.transform.GetChild(0).GetComponent<AudioSource>().volume = valueToChange;
                volumeChange = false;
            }
        }
    }
}
