using UnityEditor;
using UnityEngine;

using STD;
using static UnityEditor.Progress;

public class Monster : FObject
{
	public delegate void MethodAI(float dt);

	MethodAI methodAI = null;

	public Monster()
	{
		ap = 10;
		alive = false;
		position = new iPoint(0, 0);
		rect = new iRect(0, 0, 63, 60);
		v = new iPoint(0, 0);
		moveSpeed = 150;

		hp = 2;
#if false
		imgs = Proc.me.imgs;
#else
		imgs = new iImage[Proc.me.imgs.Length];
		for(int i=0; i<Proc.me.imgs.Length; i++)
		{
			if(Proc.me.imgs[i] != null)
				imgs[i] = Proc.me.imgs[i].clone();
		}
#endif
		methodAI = mobAI;
	}

	public iImage[] imgs;
	public iImage imgCurr;
#if false
	public void loadImage()
	{
		int beIndex, maxBe = (int)Behave.max;
		imgs = new iImage[maxBe];
		int maxFrame = 2;
		for (beIndex = 0; beIndex < maxBe; beIndex++)
		{
			iImage img;
			if (beIndex % 2 == 0)
			{
				int be = beIndex / 2;
				if (be == 0)
					maxFrame = 2;
				else if (be == 1)
					maxFrame = 3;
				else if (be == 4)
					maxFrame = 1;
				else if (be == 6)
					maxFrame = 3;

				img = new iImage();
				for (int frame = 0; frame < maxFrame; frame++)
				{
					iStrTex animSt = new iStrTex(methodStBe, new Monster().rect.size.width, new Monster().rect.size.height);
					animSt.setString((beIndex / 2) + "\n" + frame);
					img.add(animSt.tex);
				}
			}
			else
			{
				img = imgs[beIndex - 1].clone();
				img.leftRight = true;
			}
			if (beIndex < (int)Behave.att0Left)
			{
				img.repeatNum = 0;
				img._frameDt = 0.5f;
				img.startAnimation();
			}
			else
			{
				img.repeatNum = 1;
			}

			imgs[beIndex] = img;
		}

	}

	void methodStBe(iStrTex st)
	{
		string[] s = st.str.Split("\n");
		int be = int.Parse(s[0]);
		int frame = int.Parse(s[1]);

		Texture tex;
		if (be == 0)
		{
			tex = Resources.Load<Texture>("OrangeMush/Stand/mobStand" + frame);
			iGUI.instance.setRGBA(1, 1, 1, 1);
			iGUI.instance.drawImage(tex, 0, new Monster().rect.size.width - tex.height, iGUI.TOP | iGUI.LEFT);
		}
		else if (be == 1)
		{
			tex = Resources.Load<Texture>("OrangeMush/Move/mobMove" + frame);
			iGUI.instance.setRGBA(1, 1, 1, 1);
			iGUI.instance.drawImage(tex, 0, new Monster().rect.size.width - tex.height, iGUI.TOP | iGUI.LEFT);
		}
		else if (be == 4)
		{
			tex = Resources.Load<Texture>("OrangeMush/Hit/mobHit0");
			iGUI.instance.setRGBA(1, 1, 1, 1);
			iGUI.instance.drawImage(tex, 0, new Monster().rect.size.width - tex.height, iGUI.TOP | iGUI.LEFT);
		}
		else if (be == 6)
		{
			tex = Resources.Load<Texture>("OrangeMush/Die/mobDie" + frame);
			iGUI.instance.setRGBA(1, 1, 1, 1);
			iGUI.instance.drawImage(tex, 0, new Monster().rect.size.width - tex.height, iGUI.TOP | iGUI.LEFT);
		}
		iGUI.instance.setStringSize(25);
		iGUI.instance.drawString("" + (1 + frame), 25, 25, iGUI.VCENTER | iGUI.HCENTER);
	}
#endif
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
		iPoint p = position + rect.origin + off;
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
				imgCurr = imgs[(int)be];
				
				imgCurr.startAnimation();
			}
			if (hp < 1)
			{
				be = (Behave)((int)Behave.dieLeft + (int)be % 2);
				imgCurr = imgs[(int)be];
				imgCurr._frameDt = 0.2f;
				imgCurr.startAnimation(cbAnim, this);
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

public class MushRoom : Monster
{
	public void loadImage()
	{

	}
}