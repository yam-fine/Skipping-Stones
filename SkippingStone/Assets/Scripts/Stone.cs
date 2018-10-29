using UnityEngine;

public class Stone : MonoBehaviour {

    Rigidbody2D rb;
    ParticleSystem splashParticles;
    AudioSource audioSource;

    static GameObject instance;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        splashParticles = GetComponentInChildren<ParticleSystem>();
        splashParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Obstacle")
            other.GetComponent<Obstacle>().Ability(rb);
        else if (other.tag == "Water")
            EmitSplash();
    }

    public static GameObject Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindGameObjectWithTag("Player");
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    void EmitSplash()
    {
        float speed;

        speed = (rb.velocity.sqrMagnitude / 100);
        speed = Mathf.Clamp(speed, 0, 20);

        if (speed > 0.7)
        {
            ParticleSystem.MainModule main = splashParticles.main;
            main.startSpeed = speed;
            splashParticles.Play();
            audioSource.Play();
        }        
    }
}
