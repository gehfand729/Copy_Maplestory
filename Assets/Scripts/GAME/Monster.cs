using UnityEditor;
using UnityEngine;

using STD;

enum MobState
{
	Idle = 0,
	Move,
	Attack,
	Damaged,
	Die,
};

public class Monster : FObject
{
	public delegate void MethodAI(float dt);

	MethodAI methodAI = null;
	MobState ms;

	public Monster()
	{
		ap = 10;
		alive = false;
		position = new iPoint(0, 0);
		rect = new iRect(0, 0, 60, 60);
		v = new iPoint(1, 0);
		methodAI = mobAI;
		moveSpeed = 150;

	}

	public override void paint(float dt, iPoint off)
	{
		// draw
		iPoint p = position + rect.origin + off;
		iGUI.instance.setRGBA(0, 0, 0, 1);
		iGUI.instance.fillRect(p.x, p.y, rect.size.width, rect.size.height);
		move(dt);

		if (methodAI != null)
			methodAI(dt);
	}

	float t = 0;
	public void mobAI(float dt)
	{
		int a;
		t -= dt;
		if (t < 0)
		{
			a = Random.Range(0, 2);
			t = 2;
			switch (a)
			{
				case 0:
					ms = MobState.Idle;
					break;
				case 1:
					ms = MobState.Move;
					v.x = 1;
					break;
			}

		}
		if ((int)ms == 0)
			v.x = 0;
		if ((int)ms == 1)
		{
			if (block)
				v.x *= -1;
			else
				v.x *= +1;
		}
	}
}