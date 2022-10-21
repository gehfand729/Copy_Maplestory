using UnityEditor;
using UnityEngine;

using STD;

public class Player : FObject
{
	iImage[] imgs;
	iImage imgCurr;

	void loadImage()
	{
		int i, j = (int)Behave.max;
		imgs = new iImage[j];
		for (i = 0; i < j; i++)
		{
			iImage img;
			if (i % 2 == 0)
			{
				img = new iImage();
				for (int k = 0; k < 3; k++)
				{
					iStrTex st = new iStrTex(methodStCreateImage, rect.size.width, rect.size.height);
					st.setString((i / 2) + "\n" + k);
					img.add(st.tex);
				}
			}
			else
			{
				img = imgs[i - 1].clone();
				img.leftRight = true;
			}

			if (i < (int)Behave.walkLeft)
			{
				img.repeatNum = 0;
				img.startAnimation();
			}
			else
			{
				img.repeatNum = 1;
			}

			imgs[i] = img;
		}
		be = Behave.waitLeft;
		imgCurr = imgs[(int)be];
	}

	public void cbAnim(object obj)
	{
		Player p = (Player)obj;
		p.be = (Behave)((int)p.be % 2);
		p.imgCurr = p.imgs[(int)p.be];
	}

	void cbAttAnim(object obj)
	{
		Player p = (Player)obj;
		if( (int)p.be / 2 == 2 )// att0
		{
			Proc.me.am.add(p);
		}
		p.be = (Behave)(((int)p.be + 2));
		p.imgCurr = p.imgs[(int)p.be];
		imgCurr._frameDt = 0.1f;
		if (p.be < Behave.hitLeft)
			p.imgCurr.startAnimation(cbAttAnim, p);
		else
			cbAnim(p);
	}
#if false
    void methodStCreateImg(iStrTex st)
    {
        string[] str = st.str.Split("\n");
        int be = int.Parse(str[0]);
        int frame = int.Parse(str[1]);
        //iImage imgs;
        if(be == 0)
        {
            Sprite[] sprite = Resources.LoadAll<Sprite>("heroWait");
            Texture tex = Func.textureFromSprite(sprite[frame]);
            iGUI.instance.setRGBA(1, 1, 1, 1);
            iGUI.instance.drawImage(tex, 0, 0, iGUI.TOP | iGUI.LEFT);
        }
        else if( be == 1)
        {
            Sprite[] sprite = Resources.LoadAll<Sprite>("heroWalk0");
            Texture tex = Func.textureFromSprite(sprite[frame]);
            iGUI.instance.setRGBA(1, 1, 1, 1);
            iGUI.instance.drawImage(tex, 0, 0, iGUI.TOP | iGUI.LEFT);
        }
        else if (be == 3)
        {
            iGUI.instance.setRGBA(0, 0, 1, 1);
            iGUI.instance.fillRect(0, 0, 50, 50);
        }

        iGUI.instance.setStringSize(25);
        iGUI.instance.drawString("" + (1 + frame), 25, 25, iGUI.VCENTER | iGUI.HCENTER);
        iGUI.instance.setRGBA(1, 1, 1, 1);
    }
#else
	void methodStCreateImage(iStrTex st)
	{
		string[] s = st.str.Split("\n");
		int be = int.Parse(s[0]);
		int frame = int.Parse(s[1]);

		if (be == 0)// wait
		{
			iGUI.instance.setRGBA(1, 1, 1, 1);
			iGUI.instance.fillRect(0, 0, rect.size.width, rect.size.height);
			iGUI.instance.setRGBA(1, 0, 0, 1);
			iGUI.instance.fillRect(5, 20, 10, 10);

			iGUI.instance.setStringRGBA(0, 0, 0, 1);
		}
		//else if(be==1)
		else if (be == 2)// att0
		{
			iGUI.instance.setRGBA(0, 1, 1, 1);
			iGUI.instance.fillRect(0, 0, rect.size.width, rect.size.height);
			iGUI.instance.setRGBA(0, 0, 0, 1);
			iGUI.instance.fillRect(5, 20, 10, 10);

			iGUI.instance.setStringRGBA(1, 0, 0, 1);
		}else if (be == 3)// att1
		{
			iGUI.instance.setRGBA(0, 0, 1, 1);
			iGUI.instance.fillRect(0, 0, rect.size.width, rect.size.height);
			iGUI.instance.setRGBA(0, 0, 0, 1);
			iGUI.instance.fillRect(5, 20, 10, 10);

			iGUI.instance.setStringRGBA(1, 0, 0, 1);
		}

		iGUI.instance.setStringSize(25);
		iGUI.instance.drawString("" + (1 + frame), 25, 25, iGUI.VCENTER | iGUI.HCENTER);
		iGUI.instance.setRGBA(1, 1, 1, 1);
	}
	//else if (be == 4)// att2
#endif

	bool down;
	public Player()
	{
		maxHp = 100;
		hp = maxHp;
		position = new iPoint(0, 0);
		rect = new iRect(0, 0, 50, 50);
		v = new iPoint(0, 0);

		moveSpeed = 300;

		down = false;
		jumping = true;
		jumpForce = 0;
		preHeight = rect.size.height;
		downHeight = preHeight / 2;
		preY = rect.origin.y;



		MainCamera.methodKeyboard += keyboard;
		loadImage();
	}


	float t = 0;
	float cInterval = 2;
	public override void paint(float dt, iPoint off)
	{
		iPoint p = position + rect.origin + off;
		//iGUI.instance.fillRect(p.x, p.y, rect.size.width, rect.size.height);
		imgCurr.paint(dt, p);
		move(dt);

		if (v.x != 0)
		{
			be = (Behave)((int)be / 2 * 2) + (v.x > 0 ? 1 : 0);
			imgCurr = imgs[(int)be];
		}
		
		Monster m = checkCollision(rect);
		t += dt;
		if (m != null)
		{
			if (t > cInterval)
			{
				if (hp > 0)
					hp -= m.ap;
				t = 0;
			}
		}
	}

	private bool CheckKey(int key, iKeyboard ik)
	{
		int k = (int)ik;
		return (key & k) == k;
	}

	float preHeight;
	float downHeight;

	float preY;

	public void keyboard(iKeystate stat, int key)
	{
		//v = new iPoint(0, 0);
		if (stat == iKeystate.Moved)
		{
			if (CheckKey(key, iKeyboard.Left))
			{
				v.x = -1;
			}
			else if (CheckKey(key, iKeyboard.Right))
			{
				v.x = +1;
			}
			if (CheckKey(key, iKeyboard.Up))
				;
			else if (CheckKey(key, iKeyboard.Down))
			{
				if (!jumping)
				{
					down = true;
					rect.origin.y = downHeight;
					rect.size.height = downHeight;
				}
			}
			if (CheckKey(key, iKeyboard.alt))
			{
				if (!jumping && !down)
				{
					jumping = true;
					jumpForce = -700;
				}
			}
		}

		if (stat == iKeystate.Ended)
		{
			if (CheckKey(key, iKeyboard.Down))
			{
				down = false;
				rect.origin.y = preY;
				rect.size.height = preHeight;
			}
			v.x = 0;
			v.y = 0;

		}

		if (stat == iKeystate.Began)
		{
			if (CheckKey(key, iKeyboard.ctrl))
			{
				be = (Behave)((int)Behave.att0Left + (int)be % 2);
				imgCurr = imgs[(int)be];
				imgCurr._frameDt = 0.1f;
				imgCurr.startAnimation(cbAttAnim, this);
			}
		}

		if (v.x != 0 || v.y != 0)
			v /= v.getLength();
	}
	public Monster checkCollision(iRect rt)
	{
		iRect src = rt;
		src.origin += position;

		iRect dst;
		for (int i = 0; i < Proc.me.monsterNum; i++)
		{
			Monster m = Proc.me.monster[i];
			dst = m.rect;
			dst.origin += m.position;

			if (dst.containRect(src))
			{
				return m;
			}
		}
		return null;
	}
}