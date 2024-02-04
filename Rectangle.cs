using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Rectangle : Shape
{
    public Vector2 half_size;

    //重写矩形与任意形状碰撞的方法，返回碰撞类信息
    public override Contact On_Collide_With(Rigidbody self, Rigidbody other, Shape shape)
    {
        //碰撞是相互的，所以返回任意形状（other）与矩形（self）的碰撞信息
        //让对方来撞
        return shape.On_Collide_With_Rectangle(other, self, this, true);
    }
    //重写矩形与圆形（如小球与砖块之间）碰撞的方法，返回碰撞类信息
    public override Contact On_Collide_With_Circle(Rigidbody self, Rigidbody other, Circle circle, bool isReversed)
    {
        //碰撞是相互的，所以返回圆形（other）与矩形（self）的碰撞信息
        //让对方来撞
        var self_position = self.position + local_position;
        var other_position = other.position + circle.local_position;

        var min_corner = self_position - half_size;
        var max_corner = self_position + half_size;
        Vector2 Collide_point;
        #region 矩形解释
        //                                     3.2
        //                  1.2   -----------------------------  2.2  (max_corner)
        //                  /    |            (dy2)            |    \
        //                 /     |             3.3             |     \
        //       1     1.3       | (dx1)  self_position  (dx2) |      2.3     2    half_size=>(half_x,half_y)
        //                 \     |             3.3             |     /
        //                  \    |            (dy1)            |    /
        //    (min_corner)  1.1   -----------------------------  2.1
        //                                     3.1
        #endregion
        //Condition 1
        if (other.position.x < min_corner.x)
        {
            float safe_y;       
            //Condition 1.1
            if (other.position.y < min_corner.y)
            {
                 safe_y=min_corner.y;
            }
            //Condition 1.2
            else if (other.position.y > max_corner.y)
            {
                 safe_y=max_corner.y;
            }
            //Comdition 1.3
            else
            {
                safe_y=other_position.y;
            }
            Collide_point = new Vector2(min_corner.x,safe_y);
        }
        //Condition 2
        else if(other_position.x>max_corner.x)
        {
            float safe_y;
            //Condition 2.1
            if (other.position.y < min_corner.y)
            {
                safe_y = min_corner.y;
            }
            //Condition 2.2
            else if (other.position.y > max_corner.y)
            {
                safe_y = max_corner.y;
            }
            //Comdition 2.3
            else
            {
                safe_y = other_position.y;
            }
            Collide_point = new Vector2(max_corner.x, safe_y);
        }
        //Condition 3
        else
        {
            //Condition 3.1
            if (other.position.y < min_corner.y)
            {
                Collide_point = new Vector2(other_position.x, min_corner.y);
                
            }
            //Condition 3.2
            else if (other.position.y > max_corner.y)
            {
                Collide_point = new Vector2(other_position.x, max_corner.y);
            }
            //Comdition 3.3
            else
            {
                var dx1=other_position.x-min_corner.x;
                var dx2=max_corner.x-other_position.x;
                var dy1=other_position.y-min_corner.y;
                var dy2=max_corner.y-other_position.y;
                //满足条件？取，不满足：取
                Collide_point=new Vector2(
                    dx1<=dx2?min_corner.x:max_corner.x,
                    dy1<=dy2?min_corner.y:max_corner.y);
            }
        }
        //点与物体之间的连线
        var delta_position = Collide_point - other_position;
        //连线模的平方
        var sqr_distance = delta_position.sqrMagnitude;
        //安全距离
        var safe_distance = circle.radius;
        var sqr_safe_distance = safe_distance * safe_distance;
        //同平方后，比较距离
        if (sqr_distance > sqr_safe_distance)
        {
            return null;
        }
        else
        {
            var distance = Mathf.Sqrt(sqr_distance);
            Vector2 normal;
            if (distance > float.Epsilon)
            {
                normal = delta_position / distance;
            }
            else
            {
                normal = new Vector2(1, 0);
            }
            return new Contact(self, other, this, circle,
                Vector2.Distance(self_position,Collide_point),circle.radius,
                normal, safe_distance - distance, isReversed);
        }
            
    }
    //重写矩形与矩形（不存在这种情况，返回空）碰撞的方法，返回空的碰撞类信息
    public override Contact On_Collide_With_Rectangle(Rigidbody self, Rigidbody other, Rectangle rectangle, bool isReversed)
    {
        return null;
    }
}