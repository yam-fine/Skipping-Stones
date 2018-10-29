using System.Collections;
using UnityEngine;
using Cinemachine;
using Lean.Pool;

public delegate void GameOver();
public delegate void GameOn();

public class GameManager : MonoBehaviour {

    WaitForSeconds timeUntilDeath = new WaitForSeconds(2f);
    Transform player;
    Rigidbody2D rb;
    public Transform Player { get { return player; } set { player = value; rb = player.GetComponent<Rigidbody2D>(); } }
    bool isDead = false;
    float oldTransformY;
    float oldTransformX;
    public GameOn StartGame;
    public GameOver EndGame;
    static GameObject gameManager;
    [HideInInspector]
    public bool gameStarted = false;
    CinemachineVirtualCamera cam;
    bool realisticAudio = false;
    public bool RealisticAudio { get { return realisticAudio; } set { realisticAudio = value; } }

    [SerializeField]
    ScrollingBackground[] scrollingBackgrounds;
    [SerializeField]
    LeanGameObjectPool[] pools;

    // Use this for initialization
    void Start ()
    {
        player = Stone.Instance.transform;
        oldTransformY = player.position.y;
        oldTransformX = player.position.x;
        rb = player.GetComponent<Rigidbody2D>();
        cam = FindObjectOfType<CinemachineVirtualCamera>();
        StartGame += (() => gameStarted = true);
        //StartGame += (() => EnableLean());
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (gameStarted)
        {  
            EndGame += (() => player.GetComponent<PolygonCollider2D>().enabled = false);
            EndGame += (() => player.GetComponent<SpriteRenderer>().sortingOrder = 20);
            EndGame += (() => cam.Follow = null);

            if (player.position.y == oldTransformY &&
                player.position.x == oldTransformX)
                StartCoroutine(CheckDead());
            else
            {
                oldTransformY = player.position.y;
                oldTransformX = player.position.x;
            }
        }
        CamDist();
    }

        IEnumerator CheckDead()
    {
        float oldTransformY = player.position.y;
        float oldTransformX = player.position.x;

        yield return timeUntilDeath;
        if (gameStarted)
            if (player.position.y == oldTransformY &&
                player.position.x == oldTransformX)
                if (EndGame != null)
                    EndGame();
    }

    public static GameObject Instance
    {
        get
        {
            if (gameManager == null)
            {
                gameManager = GameObject.FindGameObjectWithTag("GameController");
            }
            return gameManager;
        }
    }

    void CamDist()
    {
        float camDist;
        int min = 6;
        int max = 15;
        
        camDist = Mathf.Clamp((player.position.y / 2), min, max);

        scrollingBackgrounds[0].ViewZone = camDist;
        scrollingBackgrounds[1].ViewZone = camDist;
        cam.m_Lens.OrthographicSize = camDist;
    }

    //public void RespawnLean()
    //{
    //    foreach (LeanGameObjectPool lean in pools)
    //    {
    //        lean.DespawnAll();
    //        lean.Spawn()
    //    }
    //}
}
