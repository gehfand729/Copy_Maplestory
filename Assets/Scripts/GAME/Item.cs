using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;

#if true
public class Item
{
	// �������� ������ �־�� �ϴ°�.
	// �̸�, ����, ����, �̹���, ������ ��ġ, ������ rect, ...
	int index;
	int kind;
	Texture tex;
	public iPoint position;
	public iRect rect;
	int num;

	public int getNum()
	{
		return num;
	}
	public void setNum(int n)
	{
		num = n;
	}

	public Item()
	{
		position = new iPoint(0,0);
		rect = new iRect(0, 0, 10, 10);
		tex = Resources.Load<Texture>("")
		num = 0;
	}

	public void paint(float dt, iPoint off)
	{
		iPoint p = position + rect.origin + off;
		iGUI.instance.setRGBA(1, 0, 0, 1);
		iGUI.instance.fillRect(p.x, p.y, rect.size.width, rect.size.height);
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