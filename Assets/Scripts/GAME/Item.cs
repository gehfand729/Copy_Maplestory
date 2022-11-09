using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;

#if true
public class Item
{
	// 아이템이 가지고 있어야 하는것.
	// 이름, 종류, 설명, 이미지, 아이템 위치, 아이템 rect, ...
	int index;
	int kind;
	Texture tex;
	public iPoint position;
	public iRect rect;

	public Texture getTex() { return tex; }
	public void setTex(Texture t) { tex = t; }

	public int getIndex() { return index; }

	public Item(int i)
	{
		index = i;
		position = new iPoint(0,0);
		rect = new iRect(0, 0, 10, 10);
	}

	public void paint(float dt, iPoint off)
	{
		rect.size = new iSize(tex.width, tex.height);
		iPoint p = position + rect.origin + off;
		//iGUI.instance.setRGBA(1, 1, 1, 1);
		//iGUI.instance.drawImage(tex, p.x, p.y, iGUI.TOP | iGUI.LEFT);
		iGUI.instance.setRGBA(1, 1, 1, 1);
		iGUI.instance.drawImage(tex, p.x + rect.size.width / 2, p.y , iGUI.TOP|iGUI.LEFT);
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