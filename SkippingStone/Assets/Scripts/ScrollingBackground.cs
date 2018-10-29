using UnityEngine;

public class ScrollingBackground : MonoBehaviour {

    [SerializeField]
    float backgroundSize;
    [SerializeField]
    float paralaxSpeed;

    //Transform cameraTransform;
    Transform[] layers;
    float viewZone = 10, lastCameraX;
    public float ViewZone { get { return viewZone; } set { viewZone = value; } }
    int leftIndex, rightIndex;
    Transform mainCamera;
    bool isOcean;
    GameManager gm;
    bool gameStarted = false;

    void Start() {
        //gm = GameManager.Instance.GetComponent<GameManager>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        lastCameraX = mainCamera.position.x;
        isOcean = gameObject.name == "Ocean";
        layers = new Transform[transform.childCount];
        leftIndex = 0;

        for (int i = 0; i < transform.childCount; i++)
            layers[i] = transform.GetChild(i);
        rightIndex = layers.Length - 1;
	}
	
	void Update () {
        float deltaX = mainCamera.position.x - lastCameraX;
        transform.position += Vector3.right * (deltaX * paralaxSpeed);
        lastCameraX = mainCamera.position.x;

        if (mainCamera.position.x < (layers[leftIndex].transform.position.x + viewZone))
            ScrollLeft();
        else if (mainCamera.position.x > (layers[rightIndex].transform.position.x - viewZone))
            ScrollRight();
    }

    void ScrollLeft()
    {
        layers[rightIndex].position = Vector3.right * (layers[leftIndex].position.x - backgroundSize);
        leftIndex = rightIndex;
        rightIndex--;
        if (leftIndex == 2)
            gameStarted = true;
        if (rightIndex < 0)
            rightIndex = layers.Length - 1;        
        if (gameStarted && isOcean)
            layers[leftIndex].GetComponentInChildren<Water>().ChangeSpawnPoints();
    }

    void ScrollRight()
    {
        layers[leftIndex].position = Vector3.right * (layers[rightIndex].position.x + backgroundSize);
        rightIndex = leftIndex;
        leftIndex++;
        if (rightIndex == 0)
            gameStarted = true;
        if (leftIndex == layers.Length)
            leftIndex = 0;
        if (gameStarted && isOcean)
            layers[rightIndex].GetComponentInChildren<Water>().ChangeSpawnPoints();
    }
}
