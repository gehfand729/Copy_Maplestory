using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;

#if true
public class Item
{
	enum Behave
    {
		fall = 0,
		land,
		pickedUp,

		max
    }
	// 아이템이 가지고 있어야 하는것.
	// 이름, 종류, 설명, 이미지, 아이템 위치, 아이템 rect, ...
	int index;
	int kind;
	public string name;
	Texture tex;
	Behave be;
	public iPoint position;
	public iRect rect;

	public Texture getTex() { return tex; }
	public void setTex(Texture t) { tex = t; }

	public int getIndex() { return index; }

	public Item(int i)
	{
		index = i;
		kind = 0;
		name = "버섯";
		position = new iPoint(0,0);
		rect = new iRect(0, 0, 10, 10);
		be = Behave.fall;
		loadImage();
	}

	iImage[] imgs;
	iImage imgCurr;
	iStrTex st;

	void loadImage()
	{
		int i, j = (int)Behave.max;
		imgs = new iImage[j];
		for (i = 0; i < j; i++)
		{
			iImage img = new iImage();
			st = new iStrTex(methodStCreateImage, 35, 35);
			st.setString((i / 2) + "\n");
			img.add(st.tex);
			imgs[i] = img;
		}
	}

	void methodStCreateImage(iStrTex st)
	{
		string[] s = st.str.Split("\n");
		int be = int.Parse(s[0]);
		//float dt = float.Parse(s[1]);
		if (be == 0)// fall
		{
			iGUI.instance.setRGBAWhite();
			iGUI.instance.drawImage(tex, 0, 0, iGUI.TOP | iGUI.LEFT);
		}
		else if (be == 1) // land
		{
			iGUI.instance.setRGBAWhite();
			iGUI.instance.drawImage(tex, 0, 0, iGUI.TOP | iGUI.LEFT);
		}
		else if(be ==2) // pickedUp
        {
			iGUI.instance.setRGBAWhite();
			iGUI.instance.drawImage(tex, 0, 0, iGUI.TOP | iGUI.LEFT);
		}

	}

	public void paint(float dt, iPoint off)
	{
		rect.size = new iSize(tex.width, tex.height);
		iPoint p = position + rect.origin + off;
		imgCurr = imgs[(int)be];
		imgCurr.paint(dt, p);
	}
}
#else
public struct ItemInfo
{
	public ItemInfo(int k, int i, int p)
    {
		kind = k;
		index = i;
		price = p;
    }

	int kind;
	int index;
	int price;
}
#endif