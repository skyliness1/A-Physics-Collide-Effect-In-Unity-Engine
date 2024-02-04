using UnityEngine;

public class BrickView:MonoBehaviour
{
    public Vector2 size;
    private void Awake()
    {
        var brick = new Bricks();
        brick.shape = new Rectangle
        {
            half_size = size*0.5f,
        };
        brick.view = this.transform;
        brick.position = this.transform.position;

        Main.instance.world.bricks.Add(brick);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, size);
    }
}