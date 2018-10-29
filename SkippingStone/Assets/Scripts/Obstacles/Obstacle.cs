using UnityEngine;
using Lean.Pool;

[RequireComponent(typeof(AudioSource))]
public abstract class Obstacle: MonoBehaviour {

    [SerializeField]
    bool floatingObstacle = true;
    [SerializeField]
    float xMoveRange = 0, yMoveRange = 0, zMoveRange = 0, lerpAmount = 0;
    [SerializeField]
    AudioClip defaultClip, realisticClip;

    Vector3 upper;
    Vector3 lower;
    bool goingDown;
    AudioSource audioSource;
    GameManager gm;

    public virtual void Ability(Rigidbody2D rb)
    {
        audioSource.Play();
    }

    public virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gm = GameManager.Instance.GetComponent<GameManager>();
    }

    public virtual void Start()
    {
        ChangeAudioClip(gm.RealisticAudio);
    }

    public virtual void OnEnable()
    {
        if (floatingObstacle)
        {
            upper = transform.localPosition;
            lower = new Vector3(upper.x - xMoveRange, upper.y - yMoveRange, upper.z - zMoveRange);
            goingDown = 0.5 > Random.Range(0, 1);
        }
    }

    public virtual void Update()
    {
        if (floatingObstacle)
            Float();
    }

    void Float()
    {
        if (goingDown)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                lower,
                lerpAmount
                );
            if (transform.position == lower)
            {
                goingDown = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                upper,
                lerpAmount
                );
            if (transform.position == upper)
            {
                goingDown = true;
            }
        }
    }

    void ChangeAudioClip(bool realistic)
    {
        if (realistic)
            audioSource.clip = realisticClip;
        else
        {
            if (defaultClip != null)
                audioSource.clip = defaultClip;
            else
                audioSource.clip = null;
        }
    }

    private void OnBecameInvisible()
    {
        LeanPool.Despawn(gameObject);
    }
}
