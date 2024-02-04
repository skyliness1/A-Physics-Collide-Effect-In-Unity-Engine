using UnityEngine;

public class BallView:MonoBehaviour
{
    public float radius = 0.5f;
    public Vector2 velocity = Vector2.zero;
    private void Awake()
    {
        var ball=new Ball();
        ball.shape = new Circle
        {
            radius = radius,
        };
        ball.view = this.transform;
        ball.position = this.transform.position;
        ball.velocity= Random.insideUnitCircle;

        Main.instance.world.balls.Add(ball);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}