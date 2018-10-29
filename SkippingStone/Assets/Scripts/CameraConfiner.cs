using UnityEngine;

public class CameraConfiner : MonoBehaviour {

    GameObject target;
    public GameObject Target { get { return target; } set { target = value; } }
    float myY;

	void Start () {
        target = Stone.Instance;
        myY = transform.position.y;
	}
	
	void Update () {
        transform.position = new Vector2(target.transform.position.x, myY);
	}
}
