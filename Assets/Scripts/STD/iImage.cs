using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;

namespace STD
{
	public class iImage
	{
		public List<iTexture> listTex;

		public iTexture tex;
		public iPoint position;
		public iPoint posDrawCenter;

		public bool animation;
		public int repeatIdx, repeatNum; // 0 : loop, 1 ~ : count
		public int frame;
		public float frameDt, _frameDt;
		public float scale;

		public bool select;
		public float selectDt, _selectDt, selectScale;

		public bool leftRight;

		public iImage()
		{
			listTex = new List<iTexture>();

			//tex;
			position = new iPoint(0, 0);

			animation = false;
			repeatIdx = 0;
			repeatNum = 0;
			frame = 0;
			_frameDt = 1f;// 1 / 60
			frameDt = 0.0f;
			scale = 1.0f;

			select = false;
			_selectDt = 0.2f;
			selectDt = 0.0f;
			selectScale = -0.2f;

			leftRight = false;

			methodAnimation = null;
			obj = null;
		}
		public iImage(iPoint p)
        {
			listTex = new List<iTexture>();

			//tex;
			position = new iPoint(0, 0);
			posDrawCenter = p;

			animation = false;
			repeatIdx = 0;
			repeatNum = 0;
			frame = 0;
			_frameDt = 1f;// 1 / 60
			frameDt = 0.0f;
			scale = 1.0f;

			select = false;
			_selectDt = 0.2f;
			selectDt = 0.0f;
			selectScale = -0.2f;

			leftRight = false;

			methodAnimation = null;
			obj = null;
		}

		public iImage clone()
		{
			iImage img = new iImage();
			for(int i=0; i < listTex.Count; i++)
				img.add(listTex[i]);

			img.tex = tex;
			img.position = position;
			img.posDrawCenter = posDrawCenter;

			img.leftRight = leftRight;
			img.animation = animation;
			img.repeatIdx = repeatIdx;
			img.repeatNum = repeatNum;
			img.frame = frame;
			img.frameDt = frameDt;
			img._frameDt = _frameDt;
			img.scale = scale;

			return img;
		}

		public void add(iTexture t)
		{
			listTex.Add(t);
			t.retainCount++;
		}

		public void set(int index)
		{
			frame = index;
		}

		public void paint(float dt)
		{
			paint(dt, new iPoint(0, 0) + new iPoint());
		}
		public void paint(float dt, iPoint off)
		{
			if (animation)
			{
				frameDt += dt;
				if (frameDt >= _frameDt)
				{
					frameDt -= _frameDt;
					frame++;

					if (frame == listTex.Count)
					{
						frame = 0;
						repeatIdx++;

						if (repeatNum == 0)
							;// loop
						else// if (repeatNum != 0)
						{
							if (repeatIdx == repeatNum)
							{
								animation = false;
								if (methodAnimation != null)
									methodAnimation(obj);
							}
						}
					}
				}
			}

			tex = listTex[frame];
			Texture t = tex.tex;
			//iGUI.instance.drawImage(t, position + off, iGUI.TOP | iGUI.LEFT);
			off += position;

			float s = 1.0f;
			if( select )
            {
				selectDt += dt;
				if( selectDt > _selectDt)
					selectDt = _selectDt;
            }
            else
            {
				selectDt -= dt;
				if (selectDt < 0)
					selectDt = 0;
			}
			s = 1 + selectScale * selectDt / _selectDt;
			float ss = scale * s;

			if ( ss!=1.0f )
			{
				off.x += (1 - ss) * t.width / 2;
				off.y += (1 - ss) * t.height / 2;
			}
			iGUI.instance.drawImage(t, off.x, off.y, ss, ss,
				iGUI.TOP | iGUI.LEFT, 2, 0, leftRight ? iGUI.REVERSE_WIDTH : iGUI.REVERSE_NONE);
		}

		public delegate void MethodAnimation(object obj);
		MethodAnimation methodAnimation;
		object obj;

		public void startAnimation(MethodAnimation m = null, object o = null)
		{
			animation = true;
			repeatIdx = 0;
			frame = 0;
			frameDt = 0.0f;

			methodAnimation = m;
			obj = o;
		}

		public iRect touchRect()
		{
			return touchRect(new iPoint(0, 0), new iSize(0, 0));
		}

		public iRect touchRect(iPoint off, iSize s)
		{
			return new iRect(	position.x + off.x - s.width / 2,
								position.y + off.y - s.height / 2,
								tex.tex.width + s.width,
								tex.tex.height + s.height);
		}
		public iRect topTouchRect(iPoint off, iSize s)
		{
			return new iRect(	position.x + off.x,
								position.y + off.y,
								s.width,
								s.height);
		}
		public iRect topTouchRect(iPoint off)
		{
			return topTouchRect(off, new iSize(tex.tex.width, 22));
		}

		public iPoint center()
		{
			return new iPoint(	position.x + tex.tex.width/2,
								position.y + tex.tex.height/2);
		}
		public iPoint center(iPoint off)
		{
			return new iPoint(	off.x + position.x + tex.tex.width / 2,
								off.x + position.y + tex.tex.height / 2);
		}
	}
}

