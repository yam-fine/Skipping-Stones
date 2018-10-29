using UnityEngine;

public class Cloud : Obstacle {

    [SerializeField]
    int force = 10;
    Quaternion angle = Quaternion.AngleAxis(45, Vector2.up);
    Vector2 direction;

    public override void Start()
    {
        base.Start();
        direction = angle * (new Vector2(force / 2, force / 2));
    }

    public override void Ability(Rigidbody2D rb)
    {
        base.Ability(rb);
        rb.AddForce(direction, ForceMode2D.Force);
    }
}
