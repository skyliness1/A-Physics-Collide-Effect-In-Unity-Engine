using UnityEngine;

public class Ball:Rigidbody
{
    public Circle shape;
    public Transform view;
    public override bool isStatic =>false;
    public void UpdatePosition(float delta_time)
    {
        position += velocity * delta_time;
    }
    public void SynchronizePosition()
    {
        view.position = position;
    }
}