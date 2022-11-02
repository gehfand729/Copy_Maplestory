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
		"장비" , "소비", "기타"
	};

	string[] strItemName = new string[]
	{
		"파워 엘릭서", "주황버섯의 갓"
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