using UnityEngine;

public class JellyFish : Obstacle
{
    Quaternion angle = Quaternion.AngleAxis(45, Vector2.up);
    Vector2 direction;

    [SerializeField]
    int jumpForce = 30;

    public override void Start()
    {
        base.Start();
        direction = angle * (new Vector2(jumpForce / 2, jumpForce / 2));
    }

    public override void Ability(Rigidbody2D rb)
    {
        base.Ability(rb);
        rb.AddForce(direction, ForceMode2D.Impulse);
    }
}
