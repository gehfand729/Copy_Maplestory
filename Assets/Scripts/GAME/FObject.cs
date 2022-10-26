using UnityEditor;
using UnityEngine;

using STD;

public class FObject
{
	public enum Behave
	{
		waitLeft = 0,
		waitRight,
		walkLeft,
		walkRight,

		att0Left,
		att0Right,
		att1Left,
		att1Right,

		hitLeft,
		hitRight,

		jumpLeft,
		jumpRight,

		dieLeft,
		dieRight,

		max
	}
	public Behave be;
	public bool alive;
	public iPoint position;
	public iRect rect;
	public iPoint v;
	public float moveSpeed;
	public float gravity;
	public bool jumping;
	public float jumpForce;

	public bool lBlock = false;
	public bool rBlock = false;

	public int hp, maxHp, ap;

	public FObject()
	{
		gravity = 2000;
	}

	public virtual void attack() { }

	public void move(float dt)
	{
		lBlock = false; rBlock = false;
		iPoint v = this.v * moveSpeed;
		jumpForce += gravity * dt;
		v.y += jumpForce;
		if (v.x < 0)
		{
			float xx = position.x + rect.origin.x;
			float yy = position.y + rect.origin.y;
			int x = (int)xx; x /= Proc.me.f.tileW;
			int y = (int)yy; y /= Proc.me.f.tileH;
			float minX = 0;
			bool check = false;

			float k = (yy + rect.size.height) / Proc.me.f.tileH;
			for (int i = x - 1; i > -1; i--)
			{
				for (int j = y; j < k; j++)
				{
					int n = Proc.me.f.tiles[Proc.me.f.tileX * j + i];
					if (n == 0) continue;
					if (n == 2 || n == 3) continue;

					minX = Proc.me.f.tileW * (i + 1);
					check = true;
					break;
				}
				if (check)
					break;
			}
			xx += v.x * dt;
			if (xx < minX)
			{
				xx = minX;
				lBlock = true;
			}
			position.x = xx - rect.origin.x;
		}
		else if (v.x > 0)
		{
			float xx = position.x + rect.origin.x + rect.size.width;
			float yy = position.y + rect.origin.y;
			int x = (int)xx; x /= Proc.me.f.tileW;
			int y = (int)yy; y /= Proc.me.f.tileH;
			float maxX = Proc.me.f.tileX * Proc.me.f.tileW - 1;
			bool check = false;

			float k = (yy + rect.size.height) / Proc.me.f.tileH;
			for (int i = x + 1; i < Proc.me.f.tileX; i++)
			{
				for (int j = y; j < k; j++)
				{
					int n = Proc.me.f.tiles[Proc.me.f.tileX * j + i];
					if (n == 0) continue;
					if (n == 2 || n == 4) continue;

					maxX = Proc.me.f.tileW * i - 1;
					check = true;
					break;
				}
				if (check)
					break;
			}
			xx += v.x * dt;
			if (xx > maxX)
			{
				xx = maxX;
				rBlock = true;
			}
			position.x = xx - rect.origin.x - rect.size.width;
		}
		if (v.y < 0)
		{
			float xx = position.x + rect.origin.x;
			float yy = position.y + rect.origin.y;
			int x = (int)xx; x /= Proc.me.f.tileW;
			int y = (int)yy; y /= Proc.me.f.tileH;
			float minY = 0;
			bool check = false;

			float k = (xx + rect.size.width) / Proc.me.f.tileW;
			for (int j = y - 1; j > -1; j--)
			{
				for (int i = x; i < k; i++)
				{
					int n = Proc.me.f.tiles[Proc.me.f.tileX * j + i];
					if (n == 0) continue;
					if (n == 1)
					{
						minY = Proc.me.f.tileH * (j + 1);
						check = true;
						break;
					}
				}
				if (check)
					break;
			}
			yy += v.y * dt;
			if (yy < minY)
			{
				yy = minY;
				jumpForce = 0;
			}
			position.y = yy - rect.origin.y;
		}
		else if (v.y > 0)
		{
			float xx = position.x + rect.origin.x;
			float yy = position.y + rect.origin.y + rect.size.height;
			int x = (int)xx; x /= Proc.me.f.tileW;
			int y = (int)yy; y /= Proc.me.f.tileH;
			float maxY = Proc.me.f.tileY * Proc.me.f.tileH - 1;
			bool check = false;

			float k = (xx + rect.size.width) / Proc.me.f.tileW;
			for (int j = y + 1; j < Proc.me.f.tileY; j++)
			{
				for (int i = x; i < k; i++)
				{
					int n = Proc.me.f.tiles[Proc.me.f.tileX * j + i];
					if (n == 0) continue;
					if (n == 3 || n == 4) continue;

					maxY = Proc.me.f.tileH * j - 1;
					check = true;
					break;
				}
				if (check)
					break;
			}
			yy += v.y * dt;
			if (yy > maxY)
			{
				jumping = false;
				jumpForce = 0;
				yy = maxY;
			}
			else
				jumping = true;
			position.y = yy - rect.origin.y - rect.size.height;
		}
	}
	public virtual void paint(float dt, iPoint off) { }
}