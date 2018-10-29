using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Cinemachine;

public class UI : MonoBehaviour {

    Rigidbody2D rb;
    Image darkMask;
    Image myButtonImage;
    [SerializeField]
    Button button;
    [SerializeField]
    TextMeshProUGUI scoreNum;
    [SerializeField]
    float coolDownDuration;
    [SerializeField]
    GameObject endScreen;
    [SerializeField]
    TextMeshProUGUI endScoreNum;
    CinemachineVirtualCamera cam;

    GameManager gm;
    float nextReadyTime;
    float coolDownTimeLeft;
    CameraConfiner camConfiner;

    private void Start()
    {
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        gm = GameManager.Instance.GetComponent<GameManager>();
        cam = FindObjectOfType<CinemachineVirtualCamera>();
        camConfiner = FindObjectOfType<CameraConfiner>();

        foreach (Image img in button.GetComponentsInChildren<Image>())
        {
            if (img.name == "DarkMask")
                darkMask = img;
            else if (img.name == "DownButton")
                myButtonImage = img;
        }

        AbilityReady();
        gm.EndGame += (() => endScreen.SetActive(true));
        gm.EndGame += (() => endScoreNum.text = (Mathf.Floor(Stone.Instance.transform.position.x)).ToString());
        gm.EndGame += (() => button.interactable = false);
    }

    private void Update()
    {
        scoreNum.text = (Mathf.Floor(Stone.Instance.transform.position.x)).ToString();
    }

    public void DownButton()
    {
        float velocityY = rb.velocity.y;
        float posY = rb.transform.position.y;

        if (Mathf.Abs(velocityY) > posY)
            velocityY = Mathf.Clamp(Mathf.Abs(velocityY), 4, 60);
        else
            velocityY = Mathf.Clamp(posY, 4, 60);

        rb.velocity = new Vector2(rb.velocity.x / 2, -velocityY);
        ButtonTriggered();
    }

    private void AbilityReady()
    {
        button.interactable = true;
        darkMask.enabled = false;
    }

    void ButtonTriggered()
    {
        nextReadyTime = coolDownDuration + Time.time;
        coolDownTimeLeft = coolDownDuration;
        darkMask.enabled = true;
        StartCoroutine(CoolDown());
    }

    IEnumerator CoolDown()
    {
        bool coolDownComplete = false;
        button.interactable = false;

        while (!coolDownComplete)
        {
            coolDownTimeLeft -= Time.deltaTime;
            darkMask.fillAmount = (coolDownTimeLeft / coolDownDuration);
            
            coolDownComplete = (Time.time > nextReadyTime);
            yield return new WaitForEndOfFrame();
        }

        AbilityReady();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Scene");
    }

    public void ChooseStone(GameObject stone)
    {
        Stone.Instance.SetActive(false);
        stone.SetActive(true);        
        Stone.Instance = null;
        cam.Follow = stone.transform;
        camConfiner.Target = stone;
        gm.Player = stone.transform;
    }
}
