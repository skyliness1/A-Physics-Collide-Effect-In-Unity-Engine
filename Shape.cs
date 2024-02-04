using System.Collections.Generic;
using UnityEngine;

//抽象Shape类。不可被实例化，子类需实现父类里封装的所有内容
public abstract class Shape:MonoBehaviour 
{
    public Vector2 local_position;

    //与任意形状碰撞的抽象方法，返回碰撞类信息
    public abstract Contact On_Collide_With(Rigidbody self, Rigidbody other, Shape shape);

    //与圆形碰撞的抽象方法，返回碰撞类信息
    public abstract Contact On_Collide_With_Circle(Rigidbody self, Rigidbody other, Circle circle,bool isReversed);

    //与矩形碰撞的抽象方法，返回碰撞类信息
    public abstract Contact On_Collide_With_Rectangle(Rigidbody self, Rigidbody other, Rectangle rectangle, bool isReversed);
}