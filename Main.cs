using UnityEngine;

public class Main:MonoBehaviour
{
    public static Main instance;
    public World world=new World();
    private void Awake()
    {
        instance = this;
        world.board = new Board();
    }
    private void Update()
    {
        world.Update(Time.deltaTime);
    }
}