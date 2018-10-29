using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableBoard : MonoBehaviour {

    GameManager gm;

	void Start () {
        gm = GameManager.Instance.GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (gm.gameStarted)
            gameObject.SetActive(false);
	}
}
