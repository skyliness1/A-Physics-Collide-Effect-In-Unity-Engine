using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Rectangle : Shape
{
    public Vector2 half_size;

    //��д������������״��ײ�ķ�����������ײ����Ϣ
    public override Contact On_Collide_With(Rigidbody self, Rigidbody other, Shape shape)
    {
        //��ײ���໥�ģ����Է���������״��other������Σ�self������ײ��Ϣ
        //�öԷ���ײ
        return shape.On_Collide_With_Rectangle(other, self, this, true);
    }
    //��д������Բ�Σ���С����ש��֮�䣩��ײ�ķ�����������ײ����Ϣ
    public override Contact On_Collide_With_Circle(Rigidbody self, Rigidbody other, Circle circle, bool isReversed)
    {
        //��ײ���໥�ģ����Է���Բ�Σ�other������Σ�self������ײ��Ϣ
        //�öԷ���ײ
        var self_position = self.position + local_position;
        var other_position = other.position + circle.local_position;

        var min_corner = self_position - half_size;
        var max_corner = self_position + half_size;
        Vector2 Collide_point;
        #region ���ν���
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
                //����������ȡ�������㣺ȡ
                Collide_point=new Vector2(
                    dx1<=dx2?min_corner.x:max_corner.x,
                    dy1<=dy2?min_corner.y:max_corner.y);
            }
        }
        //��������֮�������
        var delta_position = Collide_point - other_position;
        //����ģ��ƽ��
        var sqr_distance = delta_position.sqrMagnitude;
        //��ȫ����
        var safe_distance = circle.radius;
        var sqr_safe_distance = safe_distance * safe_distance;
        //ͬƽ���󣬱ȽϾ���
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
    //��д��������Σ�������������������ؿգ���ײ�ķ��������ؿյ���ײ����Ϣ
    public override Contact On_Collide_With_Rectangle(Rigidbody self, Rigidbody other, Rectangle rectangle, bool isReversed)
    {
        return null;
    }
}