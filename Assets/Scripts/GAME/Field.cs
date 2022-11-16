//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//using STD;
//using System.Runtime.InteropServices;

//public class Field
//{
//	Texture texBg;
//	float ratioW, ratioH;

//	public string strWorld, strMap;

//	public int tileX, tileY;
//	public int tileW, tileH;
//	public int[] tiles;
//	public iPoint off, offMin, offMax;

//	public Color[] colorTile;
//	Texture texField;
//	Texture texFieldHead;

//	public Portal pt;


//	public Field()
//	{
//		strWorld = "°ñ·½ »ç¿ø"; strMap = "1´Ü°è : °ñ·½ »ç¿ø1";
//		texBg = Resources.Load<Texture>("Background");
//		ratioW = 1.0f * MainCamera.devWidth / texBg.width;
//		ratioH = 1.0f * MainCamera.devHeight / texBg.height;

//		texField = Resources.Load<Texture>("Map/Tile/bsc0");
//		texFieldHead = Resources.Load<Texture>("Map/Tile/enH00");
//		tileX = 19;
//		tileY = 9;
//		tileW = texField.width;
//		tileH = texField.height;
//		Proc.me.p.position = new iPoint(1 * tileW, (tileY - 2) * tileH);
//		pt = new Portal(1, new iPoint(18 * tileW, (tileY - 1) * tileH));

//		Proc.me.addMonster(new iPoint(5 * tileW, (tileY - 3) * tileH));
//		Proc.me.addMonster(new iPoint(10 * tileW, (tileY - 3) * tileH));
//		Proc.me.addMonster(new iPoint(12 * tileW, (tileY - 3) * tileH));
//		Proc.me.addMonster(new iPoint(18 * tileW, (tileY - 3) * tileH));

//		tiles = new int[]
//		{
//			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
//			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
//			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
//			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
//			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
//			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
//			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
//			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
//			1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
//		};
//		off = new iPoint(0, 0);
//		offMin = new iPoint(MainCamera.devWidth - tileW * tileX, MainCamera.devHeight - tileH * tileY);
//		offMax = new iPoint(0, 0);

//		colorTile = new Color[(int)TileAttr.Max]
//		{
//			Color.clear, Color.red, Color.blue, Color.gray, Color.yellow,Color.clear, Color.green,
//		};
//	}

//	public void reset(int stage)
//	{
//		// Ä³¸¯ÅÍ À§Ä¡ ÃÊ±âÈ­, ¸Ê Á¤º¸ ÃÊ±âÈ­
//		Proc.me.removeMonster();
//		strWorld = "°ñ·½ »ç¿ø"; strMap = "2´Ü°è : °ñ·½ »ç¿ø2";
//		tileX = 24;
//		tileY = 13;
//		Proc.me.p.position = new iPoint(1 * tileW, 11 * tileH);
//		off = new iPoint(0, 0);
//		pt.Pos = new iPoint(20 * tileW, (tileY - 1) * tileH);
//		Proc.me.addMonster(new iPoint(8 * tileW, (tileY - 5) * tileH));
//		Proc.me.addMonster(new iPoint(13 * tileW, (tileY - 7) * tileH));
//		Proc.me.addMonster(new iPoint(14 * tileW, (tileY - 9) * tileH));
//		Proc.me.addMonster(new iPoint(18 * tileW, (tileY - 10) * tileH));
//		//Proc.me.f.tiles
//		tiles = new int[]
//		{
//			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
//			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
//			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
//			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
//			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
//			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
//			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
//			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0,
//			0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 5, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0,
//			0, 0, 0, 0, 0, 5, 0, 0, 0, 5, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0,
//			0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
//			0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
//			1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
//		};
//		offMin = new iPoint(MainCamera.devWidth - tileW * tileX, MainCamera.devHeight - tileH * tileY);
//		offMax = new iPoint(0, 0);


//		// ÆäÀÌµå ÀÎ ¾Æ¿ô

//	}

//	public void paint(float dt)
//	{
//		iGUI.instance.setRGBAWhite();
//		iGUI.instance.drawImage(texBg, 0, 0, ratioW, ratioH, iGUI.TOP | iGUI.LEFT);

//		iPoint p = new iPoint(MainCamera.devWidth / 2, MainCamera.devHeight / 2)
//				- Proc.me.p.position;
//		//iPoint lOff = new iPoint(0, 0);
//		off.x = Mathf.Lerp(off.x, p.x, dt * 2);
//		off.y = Mathf.Lerp(off.y, p.y, dt * 2);

//		//off = lOff;

//		if (off.x < offMin.x)
//			off.x = offMin.x;
//		else if (off.x > offMax.x)
//			off.x = offMax.x;
//		if (off.y < offMin.y)
//			off.y = offMin.y;
//		else if (off.y > offMax.y)
//			off.y = offMax.y;

//		int i, tileXY = tileX * tileY;
//		for (i = 0; i < tileXY; i++)
//		{
//			float x = off.x + tileW * (i % tileX);
//			float y = off.y + tileH * (i / tileX);
//			int t = tiles[i];
//			Color c = colorTile[t];
//			if (t == 1 || t == 2)
//			{
//				iGUI.instance.setRGBAWhite();
//				iGUI.instance.drawImage(texFieldHead, x, y + 18 - texFieldHead.height, iGUI.TOP | iGUI.LEFT);
//				iGUI.instance.drawImage(texField, x, y + 18, iGUI.TOP | iGUI.LEFT);
//			}
//			else if (t == 6)
//			{
//				iGUI.instance.setRGBA(c.r, c.g, c.b, c.a);
//				iGUI.instance.drawLine(x, y + tileH, x + tileW, y);
//			}
//			else
//			{
//				iGUI.instance.setRGBA(c.r, c.g, c.b, c.a);
//				iGUI.instance.fillRect(x, y, tileW, tileH);
//			}
//		}
//		iGUI.instance.setRGBAWhite();
//		pt.paint(dt, off);
//	}
//}
