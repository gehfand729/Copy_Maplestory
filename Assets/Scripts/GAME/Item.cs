using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;

public class Item
{
	// �������� ������ �־�� �ϴ°�.
	// �̸�, ����, ����, �̹���, ������ ��ġ, ������ rect, ...
	string name;
	int kind;
	iImage image;
	public iPoint position;
	public iRect rect;

	public Item()
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