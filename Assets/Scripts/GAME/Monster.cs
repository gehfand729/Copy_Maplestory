using UnityEditor;
using UnityEngine;

using STD;

public class Monster : FObject
{
	public delegate void MethodAI(float dt);

	MethodAI methodAI = null;

	public Monster()
	{
		ap = 10;
		alive = false;
		position = new iPoint(0, 0);
		rect = new iRect(0, 0, 60, 60);
		v = new iPoint(0, 0);
		moveSpeed = 150;

		hp = 100;

		methodAI = mobAI;
	}

	iImage[] imgs;
	iImage imgCurr;

	public void loadImage()
	{
		int beIndex, maxBe = (int)Behave.max;
		imgs = new iImage[maxBe];
		for(beIndex = 0; beIndex < maxBe; beIndex++)
		{
			iImage img;
			if(beIndex % 2 == 0)
			{
				img = new iImage();
				for(int frame = 0; frame < 3; frame++)
				{
					iStrTex st = new iStrTex(methodStBe, rect.size.width, rect.size.height);
					st.setString((beIndex / 2) + "\n" + frame);
					img.add(st.tex);
				}
			}
			else
			{
				img = imgs[beIndex - 1].clone();
				img.leftRight = true;
			}
			if (beIndex < (int)Behave.walkLeft)
			{
				img.repeatNum = 0;
				img.startAnimation();
			}
			else
			{
				img.repeatNum = 1;
			}

			imgs[beIndex] = img;
		}
		be = Behave.waitLeft;
		imgCurr = imgs[(int)be];
	}

	void methodStBe(iStrTex st)
	{
		string[] s = st.str.Split("\n");
		int be = int.Parse(s[0]);
		int frame = int.Parse(s[1]);

		if( be == 0)
		{
			iGUI.instance.setRGBA(0, 1, 0, 1);
			iGUI.instance.fillRect(0,0,rect.size.width, rect.size.height);
		}
		else if( be == 1)
		{
			iGUI.instance.setRGBA(1, 0, 1, 1);
			iGUI.instance.fillRect(0, 0, rect.size.width, rect.size.height);

			iGUI.instance.setStringSize(25);
			iGUI.instance.setStringRGBA(0, 0, 0, 1);
			iGUI.instance.drawString("w", new iPoint(rect.size.width / 2, rect.size.height / 2));
		}
		else if(be == 4)
		{
			iGUI.instance.setRGBA(1, 0, 0, 1);
			iGUI.instance.fillRect(0, 0, rect.size.width, rect.size.height);
		}
	}

	public void cbAnim(object obj)
	{
		Monster m = (Monster)obj;
		m.be = (Behave)((int)m.be % 2);
		m.imgCurr = m.imgs[(int)m.be];
	}

	public override void paint(float dt, iPoint off)
	{ 
		iPoint p = position + rect.origin + off;
		imgCurr.paint(dt,p);

		move(dt);

		if (methodAI != null)
			methodAI(dt);
	}

	float t = 0;
	public void mobAI(float dt)
	{
		imgCurr = imgs[(int)be];
		imgCurr.startAnimation(cbAnim, this);
		int a;
		t -= dt;
		if (t < 0)
		{
			a = Random.Range(0, 2);
			t = 2;
			switch (a)
			{
				case 0:
					be = (Behave)((int)Behave.waitLeft + (int)be % 2);
					break;
				case 1:
					be = (Behave)((int)Behave.walkLeft + (int)be % 2);
					break;
			}
		}


		if ((int)be / 2 == 0)
		{
			v.x = 0;
		}
		if ((int)be / 2 == 1)
		{
			if (lBlock)
				be = Behave.walkRight;
			if (rBlock)
				be = Behave.walkLeft;
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