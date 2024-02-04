using System;
using UnityEngine;

public class Circle:Shape
{
    public float radius;

    /// <summary>
    /// self是this所拥有的
    /// </summary>
    /// <param name="self"></param>
    /// <param name="other"></param>
    /// <param name="shape"></param>
    /// <returns></returns>
    //重写圆形与任意形状碰撞的方法，返回碰撞类信息
    public override Contact On_Collide_With(Rigidbody self, Rigidbody other, Shape shape)
    {
        //碰撞是相互的，所以返回任意形状（other）与圆形（self）的碰撞信息
        //让对方来撞
        return shape.On_Collide_With_Circle(other, self, this,true);
    }
    //重写圆形与圆形（如小球与小球之间）碰撞的方法，返回碰撞类的具体信息
    public override Contact On_Collide_With_Circle(Rigidbody self, Rigidbody other, Circle circle, bool isReversed)
    {
        //刚体位置与形状位置=>实际位置
        var self_position= self.position+local_position;
        var other_position= other.position+circle.local_position;
        //二者之间的位置差
        var delta_position=self_position-other_position;
        //二者之间位置差的平方
        var sqr_distance=delta_position.sqrMagnitude;
        //安全距离
        var safe_distance=radius+circle.radius;
        var sqr_safe_distance = safe_distance * safe_distance;
        //实际距离大于安全距离，说明没撞到
        if(sqr_distance>sqr_safe_distance)
        {
            return null;
        }
        //位置差的平方再开方，得到二者间实际的距离
        var distance=MathF.Sqrt(sqr_distance);
        //返回碰撞所产生的信息并存储在Contact中
        Vector2 normal;
        //float.Epsilon是float浮点数的最小可能，在这里防止distance太小而报错
        if (distance>float.Epsilon)
        {
           normal=delta_position/distance;
        }
        else
        {
            normal=new Vector2(1,0);
        }
        return new Contact(self,other,this,circle,
            radius,circle.radius,normal,safe_distance-distance,isReversed);
    }
    //重写圆形与矩形（如小球与砖块之间）碰撞的方法，返回碰撞类的具体信息
    public override Contact On_Collide_With_Rectangle(Rigidbody self, Rigidbody other, Rectangle rectangle, bool isReversed)
    {
       return rectangle.On_Collide_With_Circle(other, self, this, !isReversed);
    }


}