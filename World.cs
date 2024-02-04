using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
//容器
public class World
{
    public Board board;

    public List<Ball> balls=new List<Ball>();

    public List<Bricks> bricks=new List<Bricks>();

    public void Update(float delta_time)
    {
        foreach(Ball ball in balls )
        {
            ball.UpdatePosition(delta_time); 
        }
        var contacts = new List<Contact>();
                    
        for(int i = 0; i < balls.Count; ++i)
        {
            var ball = balls[i];
            //球与球（圆形）之间的相撞，并把碰撞信息存入列表   
            for(int j =i+1; j < balls.Count; ++j)
            {
                var other_ball = balls[j];

                var contact = ball.shape.On_Collide_With_Circle(
                    ball,
                    other_ball,
                    other_ball.shape,
                    false);
                if (contact != null)
                {
                    contacts.Add(contact);
                }
            }
            //球与板子（任意形状）之间的相撞，并把碰撞信息存入列表 
            foreach (var shape in board.shapes)
            {
                var contact = ball.shape.On_Collide_With(
                    ball,
                    board,
                    shape);
                if (contact != null)
                {
                    contacts.Add(contact);
                }
            }
            //球与方块（矩形）之间的相撞，并把碰撞信息存入列表 
            foreach (var brick in bricks)
            {
                var contact = ball.shape.On_Collide_With_Rectangle(
                    ball,
                    brick,
                    brick.shape,
                    false);
                if (contact != null)
                {
                    contacts.Add(contact);
                }
            }
        }
        //碰撞速度检测迭代
        int velocity_iterator_count = 8;
        for (int i = 0; i < velocity_iterator_count; ++i)
        {
            var flag = false;
            foreach(var contact in contacts)
            {
                if(contact.VelocitySolution())
                {
                    flag = true;
                }
            }
            if(!flag)
            {
                break;
            }
        }
        //碰撞位置检测迭代
        int position_iterator_count = 3;
        for (int i = 0;i < position_iterator_count; ++i)
        {
            foreach (var contact in contacts)
            {
                contact.PositionSolution();
               
            }
            
        }
        foreach(var ball in balls)
        {
            ball.SynchronizePosition();
        }
    }
}