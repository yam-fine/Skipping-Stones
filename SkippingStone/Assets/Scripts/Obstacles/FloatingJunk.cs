using UnityEngine;

public class FloatingJunk : Obstacle {

    [SerializeField]
    int divideBy = 10;

    public override void Ability(Rigidbody2D rb)
    {
        base.Ability(rb);
        rb.velocity = rb.velocity / divideBy;
    }
}
