using UnityEngine;

public class ProjectileDragging : MonoBehaviour {

    [SerializeField]
    float maxStretch;
    [SerializeField]
    LineRenderer lineRenderer;

    SpringJoint2D spring;
    bool clickedOn = false;
    Rigidbody2D myRigidbody;
    Ray rayToMouse, startToProjectileRay;
    float maxStretchSqr, circleRadius;
    Transform startingPoint;
    Vector2 prevVelocity;
    GameManager gm;

    private void Awake()
    {
        spring = GetComponent<SpringJoint2D>();
        startingPoint = spring.connectedBody.transform;
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Start () {
        LineRendererSetup();
        rayToMouse = new Ray(startingPoint.position, Vector3.zero);
        startToProjectileRay = new Ray(lineRenderer.transform.position, Vector3.zero);
        maxStretchSqr = maxStretch * maxStretch;
        gm = GameManager.Instance.GetComponent<GameManager>();
        //CircleCollider2D radius = GetComponent<Collider2D>() as CircleCollider2D;
        //circleRadius = radius.radius;
	}
	
	void Update () {
        if (clickedOn)
            Dragging();

        // not launched yet
        if (spring != null)
        {
            if (!myRigidbody.isKinematic && 
                prevVelocity.sqrMagnitude > myRigidbody.velocity.sqrMagnitude)
            {
                Destroy(spring);
                myRigidbody.velocity = prevVelocity;
            }

            if (!clickedOn)
                prevVelocity = myRigidbody.velocity;
        }
        else
        {
            lineRenderer.enabled = false;
        }
	}

    void LineRendererSetup() {
        lineRenderer.SetPosition(0, lineRenderer.transform.position);
    }

    private void OnMouseDown()
    {
        spring.enabled = false;
        clickedOn = true;
    }

    private void OnMouseUp()
    {
        spring.enabled = true;
        myRigidbody.isKinematic = false;
        clickedOn = false;
        if (gm.StartGame != null)
            gm.StartGame();
    }

    void Dragging ()
    {
        Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 startToMouse = mouseWorldPoint - startingPoint.position;
        
        if (startToMouse.sqrMagnitude > maxStretchSqr)
        {
            rayToMouse.direction = startToMouse;
            mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
        }

        mouseWorldPoint.z = 0f;
        transform.position = mouseWorldPoint;
    }

    void LineRendererUpdate()
    {
        Vector2 startToProjectile = transform.position - lineRenderer.transform.position;
        startToProjectileRay.direction = startToProjectile;
        Vector3 holdPoint = startToProjectileRay.GetPoint(startToProjectile.magnitude /*+ circleRadius*/);
        lineRenderer.SetPosition(1, holdPoint);
    }
}
