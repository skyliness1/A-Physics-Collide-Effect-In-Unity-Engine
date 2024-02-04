using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTest : MonoBehaviour
{
    public float radius = 1f;
    public RectangleTest rect;

    public Vector2 normal;
    public float depth;
    void Update()
    {
        var self = new Circle()
        {
            radius = radius
        };
        var rb = new Rigidbody()
        {
            position=transform.position,
        };
        var other = new Rectangle()
        {
            half_size = rect.half_size,
        };
        var orb = new Rigidbody()
        {
            position=rect.transform.position,
        };

        var result = self.On_Collide_With(rb, orb, other);
        if(result!=null)
        {
            normal = result.bodyA==rb?result.normal:-result.normal;
            depth = result.depth;
        }
        else
        {
            normal = Vector2.zero;
            depth = 0f;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
