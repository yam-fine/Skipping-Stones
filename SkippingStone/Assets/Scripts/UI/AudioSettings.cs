using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour {

    [SerializeField]
    Slider slider;
    [SerializeField]
    Toggle toggle;

    [SerializeField]
    AudioSource bgMusicSource;
    [SerializeField]
    AudioClip realisticBGMusic;
    [SerializeField]
    AudioClip defaultBGMusic;    

    GameManager gm;

    private void Awake()
    {
        gm = GameManager.Instance.GetComponent<GameManager>();
    }

    public void Realistic()
    {
        if (toggle.isOn)
        {
            bgMusicSource.clip = realisticBGMusic;
            gm.RealisticAudio = true;
        }
        else
        {
            bgMusicSource.clip = defaultBGMusic;
            gm.RealisticAudio = false;
        }

        bgMusicSource.Play();
    }

    public void Volume()
    {
        bgMusicSource.volume = slider.value;
    }
}
