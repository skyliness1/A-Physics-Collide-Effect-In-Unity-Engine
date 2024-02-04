using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleTest : MonoBehaviour
{
    public Vector2 half_size=Vector2.one;


    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position,half_size*2);
    }
}
