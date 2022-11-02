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
	iImage image;
	public iPoint position;
	public iRect rect;

	public Item(int i, int k, )
	{
		position = new iPoint(0,0);
		rect = new iRect(0, 0, 10, 10);
	}

	public void paint(float dt, iPoint off)
	{
		iPoint p = position + rect.origin + off;
		iGUI.instance.setRGBA(1, 0, 0, 1);
		iGUI.instance.fillRect(p.x, p.y, rect.size.width, rect.size.height);
	}
}
#else
public class ItemInfo
{
	string[] strItemKind = new string[]
	{
		"���" , "�Һ�", "��Ÿ"
	};

	string[] strItemName = new string[]
	{
		"�Ŀ� ������", "��Ȳ������ ��"
	};

	public ItemInfo(int k, int i, int p)
    {
		kind = k;
		index = i;
		price = p;
    }

	int kind;
	int index;
	int grade;
	int price;


}
#endif