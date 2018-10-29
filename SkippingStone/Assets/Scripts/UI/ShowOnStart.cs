using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowOnStart : MonoBehaviour {

    GameManager gm;
    WaitForSeconds wait = new WaitForSeconds(0.25f);
    Image[] images;
    TextMeshProUGUI text;

	// Use this for initialization
	void Start () {
        gm = GameManager.Instance.GetComponent<GameManager>();
        images = GetComponentsInChildren<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();

        gm.StartGame += (() => ImgState(false));

        ImgState(false);

        StartCoroutine(helpUser());
	}

    IEnumerator helpUser()
    {
        int count = 0;

        while (count < 8)
        {
            if (gm.gameStarted)
                StopCoroutine(helpUser());
            count++;
            yield return wait;
        }

        if (!gm.gameStarted)
            ImgState(true);
    }

    void ImgState(bool lol)
    {
        foreach (Image img in images)
            img.enabled = lol;
        text.enabled = lol;
    }
}
