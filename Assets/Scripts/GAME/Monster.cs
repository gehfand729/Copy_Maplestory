using UnityEditor;
using UnityEngine;

using STD;
using static UnityEditor.Progress;

public class Monster : FObject
{
	public delegate void MethodAI(float dt);

	MethodAI methodAI = null;
	
	int exp;
	public int Exp { get { return exp; } set { exp = value; } }
	public Monster()
	{
		ap = 10;
		exp = 10;
		alive = false;
		position = new iPoint(0, 0);
		rect = new iRect(0, 0, 60, 60);
		v = new iPoint(0, 0);
		moveSpeed = 150;

		hp = 2;

		imgs = new iImage[Proc.me.imgsMonster.Length];
		for(int i=0; i<Proc.me.imgsMonster.Length; i++)
		{
			if(Proc.me.imgsMonster[i] != null)
				imgs[i] = Proc.me.imgsMonster[i].clone();
		}
		methodAI = mobAI;
	}

	public iImage[] imgs;
	public iImage imgCurr;

	public void cbAnim(object obj)
	{
		Monster m = (Monster)obj;
		if((int)m.be/2 == 6)
		{
			m.alive = false;
			return;
		}
		m.be = (Behave)((int)m.be % 2);
		m.imgCurr = m.imgs[(int)m.be];
	}

	public override void paint(float dt, iPoint off)
	{ 
		if (methodAI != null)
			methodAI(dt);
		iPoint p = position + rect.origin + off + new iPoint(	rect.origin.x + (rect.size.width - imgs[0].listTex[0].tex.width) * 0.5f,
																rect.origin.y + rect.size.height - imgs[0].listTex[0].tex.height);
		imgCurr.paint(dt,p);

		move(dt);
	}

	float t = 0;

	public void mobAI(float dt)
	{
		int a;
		t -= dt;
		if (be != (Behave)((int)Behave.dieLeft + (int)be % 2))
		{
			if (t < 0)
			{
				a = Random.Range(0, 2);
				t = Random.Range(0.5f, 3);
				switch (a)
				{
					case 0:
						be = (Behave)((int)Behave.waitLeft + (int)be % 2);
						break;
					case 1:
						be = (Behave)((int)Behave.walkLeft + (int)be % 2);
						break;
				}
				imgCurr = imgs[(int)be];
				
				imgCurr.startAnimation();
			}
			if (hp < 1)
			{
				be = (Behave)((int)Behave.dieLeft + (int)be % 2);
				imgCurr = imgs[(int)be];
				imgCurr._frameDt = 0.2f;
				imgCurr.startAnimation(cbAnim, this);
				// 수정사항. (드랍되는 아이템이 매번 다르기에 이렇게 하면 안됨.)
				Item i = new Item(1);
				i.setTex(Resources.Load<Texture>("Items/mushroomHead"));
				Proc.me.dropItem(i, new iPoint(position.x + rect.size.width/2,
					position.y + rect.size.height - i.rect.size.height));

				Proc.me.p.addExp(exp);
			}
		}

		if ((int)be / 2 == 0)
		{
			v.x = 0;
		}
		if ((int)be / 2 == 1)
		{
			if (lBlock)
			{
				be = Behave.walkRight;
				imgCurr = imgs[(int)be];
                imgCurr._frameDt = 0.2f;
                imgCurr.startAnimation();
			}
			if (rBlock)
			{
				be = Behave.walkLeft;
				imgCurr = imgs[(int)be];
                imgCurr._frameDt = 0.2f;
                imgCurr.startAnimation();
			}
			if ((int)be % 2 == 0)
				v.x = -1;
			else
				v.x = +1;
		}
		if((int)be / 2 == 4)
		{
			v.x = 0;
		}
	}

}