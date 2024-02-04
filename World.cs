using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
//����
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
            //������Բ�Σ�֮�����ײ��������ײ��Ϣ�����б�   
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
            //������ӣ�������״��֮�����ײ��������ײ��Ϣ�����б� 
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
            //���뷽�飨���Σ�֮�����ײ��������ײ��Ϣ�����б� 
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
        //��ײ�ٶȼ�����
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
        //��ײλ�ü�����
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