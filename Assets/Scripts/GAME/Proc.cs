using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;
using System.Runtime.InteropServices;

public class Proc : GObject
{
	public static Proc me;

	public Field f;
	public Player p;
	//public UI ui;

	public int countLoad = 0;

	public AttMgt am;

	public override void load()
	{
		me = this;
		loadMonster();

		f = new Field();
		p = new Player();
		//ui = new UI();

		am = new AttMgt();
		createInven();

		createPopUI();
		popInfo.show(true);
		popMiniMap.show(true);

		// for testing
	}


	public override void draw(float dt)
	{
		f.paint(dt);
		p.paint(dt, f.off);
		drawMonster(dt, f.off);
		drawPopUI(dt);
		//ui.paint(dt);

		drawItem(dt, f.off);

		am.update(dt, f.off);
	}


	public override void key(iKeystate stat, iPoint point)
	{
		mousePopInven(stat, point);
		mousePopMiniMap(stat, point);
		// ui
		// f
		// p
	}

	public override void keyboard(iKeystate stat, int key)
	{
		// ui
		if (stat == iKeystate.Began)
		{
			if ((key & (int)iKeyboard.I) == (int)(iKeyboard.I))
			{
				if (popInven.bShow == false)
					popInven.show(true);
				else if (popInven.state == iPopupState.proc)
					popInven.show(false);
			}
			//if ((key & (int)iKeyboard.esc) == (int)(iKeyboard.esc))
			//{
			//	if (popSetting.bShow == false)
			//		popSetting.show(true);
			//	else if (popSetting.state == iPopupState.proc)
			//		popSetting.show(false);
			//}
			if ((key & (int)iKeyboard.Esc) == (int)(iKeyboard.Esc))
			{
				addMonster(new iPoint(4 * f.tileW, 10 * f.tileH));
			}
		}
		// f
		// p

	}

	iPopup popUI;
	void createPopUI()
	{
		createPopInfo();
		createPopMiniMap();
		createPopInven();
		createPopSetting();
	}

	void drawPopUI(float dt)
	{
		drawPopInfo(dt);
		drawPopMiniMap(dt);
		drawPopInven(dt);
		drawPopSetting(dt);
	}

    // Inventory =====================================================================
	// 인벤토리의 창의 격자는 그대로 있고 스크롤시 아이템의 이미지가 로드되냐 안되냐로 나뉨.
	// 인벤토리는 기본적으로 배열의 구조를 가지고 있음 ex) 4*n
	// 배열의 크기는 4*n임.

    iPopup popInven = null;
	iImage[] imgInven;
	iPoint posInven;

	iStrTex stInven;

	void createPopInven()
	{
		iPopup pop = new iPopup();
		iImage img = new iImage();

		imgInven = new iImage[1];

		stInven = new iStrTex(methodStInven, 197, 380);
		img.add(stInven.tex);
		imgInven[0] = img;
		
		pop.add(img);

		pop.style = iPopupStyle.alpha;

		pop.methodDrawAfter = drawPopInvenAfter;
		popInven = pop;
	}
	void methodStInven(iStrTex st)
	{
		Texture tex = Resources.Load<Texture>("invenBg1");
		drawImage(tex, 0, 0, TOP | LEFT);
        tex = Resources.Load<Texture>("invenBg2");
		drawImage(tex, 6, 23, TOP | LEFT);
        tex = Resources.Load<Texture>("invenBg3");
        drawImage(tex, 7, 46, TOP | LEFT);

        string[] strs = st.str.Split("\n");
		float x = float.Parse(strs[0]);
		float y = float.Parse(strs[1]);
		setRGBA(1, 1, 1, 1);
		popInven.closePoint = new iPoint(x, y);
	}

	void drawPopInvenAfter(float dt, iPopup pop, iPoint zero)
	{
		iPoint off = zero + new iPoint(14, 36);
		setRGBA(1, 1, 1, 1);

		for (int i=0; i< invenXY; i++)
		{
			// draw
			if (inven[i] == null) continue;
			setRGBA(1, 1, 1, 1);
			drawImage(inven[i].getTex(), off.x + (12 + 30) * (i % 4), off.y + 20 + (12 + 30) * (i / 4), TOP | LEFT);
			setStringRGBA(0, 0, 0, 1);
			setStringSize(20);
			drawString(invenNum[i].ToString(), off.x + 30 + (12 + 30) * (i % 4), off.y + 20 + 30 + (12 + 30) * (i / 4), TOP | LEFT);
		}
	}

	void drawPopInven(float dt)
	{
		stInven.setString(posInven.x + "\n" + posInven.y);
		popInven.paint(dt);
	}

	bool dragInven = false;
	iPoint mouseInven = new iPoint();
	void mousePopInven(iKeystate stat, iPoint point)
	{
		iPopup pop = popInven;
		iImage[] img = imgInven;
		if (pop.state == iPopupState.proc)
		{
			int i, j = -1;
			
			switch (stat)
			{
				case iKeystate.Began:
					i = pop.selected;
					if (i == -1) break;
					dragInven = true;
					mouseInven = point - posInven;
					break;
				case iKeystate.Moved:
					for (i = 0; i < img.Length; i++)
					{
						if (img[i].topTouchRect(popInven.closePoint).containPoint(point))
						{
							j = i;
							break;
						}
					}
					if(pop.selected != j)
						pop.selected = j;
					if (dragInven)
					{
						posInven = point - mouseInven;
					}
					break;
				case iKeystate.Ended:
					i = pop.selected;
					dragInven = false;
					if(i == -1) break;
					break;
			}
		}
	}

	// Info =====================================================================
	iPopup popInfo = null;
	int lv, hp, maxHp, minHp, mp, maxMp, minMp, exp, maxExp;
	iStrTex stInfo;
	void createPopInfo()
	{
		iPopup pop = new iPopup();
		iImage img = new iImage();

		lv = 0;

		maxHp = p.maxHp;
		minHp = 0;
		mp = maxMp = 100;
		minMp = 0;


		stInfo = new iStrTex(methodStInfo, 1280, 100);
		img.add(stInfo.tex);
		pop.add(img);

		pop.style = iPopupStyle.alpha;
		pop.openPoint = new iPoint(0, MainCamera.devHeight - 80);
		pop.closePoint = pop.openPoint;

		popInfo = pop;
	}
	void methodStInfo(iStrTex st)
	{
		Texture tex = Resources.Load<Texture>("bgInfo");
		drawImage(tex, (MainCamera.devWidth - 250)/ 2, 29, TOP | LEFT);

		string[] strs = st.str.Split("\n");
		int lv = int.Parse(strs[0]);
		float hp = float.Parse(strs[1]);
		float mp = float.Parse(strs[2]);
		int exp = int.Parse(strs[3]);
		int maxExp = int.Parse(strs[4]);

		float expPer = 0.0f;
		expPer = 1.0f * exp / maxExp;
		string result = string.Format("{0:0.#0}", expPer * 100);
		// for testing

		

		//hp = p.hp;
		if (hp < minHp)
		{
			hp = minHp;
		}
		if (hp > maxHp)
		{
			hp = maxHp;
		}
		if (mp < minMp)
		{
			mp = minMp;
		}
		if (mp > maxMp)
		{
			mp = maxMp;
		}

		float rHp = hp / maxHp;
		float rMp = mp / maxMp;

		// checkList - ui 위치 조정
        tex = Resources.Load<Texture>("exp/back");
        drawImage(tex, 0, 70, TOP | LEFT);
		
        tex = Resources.Load<Texture>("exp/gauge");
        drawImage(tex, 16, 71.5f, expPer, 1, TOP | LEFT);
		
        tex = Resources.Load<Texture>("exp/cover");
        drawImage(tex, 135, 71.5f, TOP | LEFT);
		

        tex = Resources.Load<Texture>("hp");
		drawImage(tex, (MainCamera.devWidth - 250) / 2 + 25, 28, rHp, 1, TOP | LEFT);

		tex = Resources.Load<Texture>("mp");
		drawImage(tex, (MainCamera.devWidth - 250) / 2 + 25, 43, rMp, 1, TOP | LEFT);

		tex = Resources.Load<Texture>("infoCover");
		drawImage(tex, (MainCamera.devWidth - 250) / 2, 0, TOP | LEFT);

		setStringRGBA(1, 1, 1, 1);
		drawString(lv.ToString(), (MainCamera.devWidth - 250) / 2, 0, TOP | LEFT);
		drawString(exp.ToString(), (MainCamera.devWidth - 250) / 2 + 30, 0, TOP | LEFT);
		drawString(result, (MainCamera.devWidth - 250) / 2 + 60, 0, TOP | LEFT);

		setRGBA(1, 1, 1, 1);

	}
	void drawPopInfo(float dt)
	{
		hp = p.hp;
		exp = p.getExp();
		lv = p.getlv();
		maxExp = p.getMaxExp();
		stInfo.setString(lv + "\n" + hp + "\n" + mp + "\n" + exp + "\n" + maxExp);

		popInfo.paint(dt);
	}

	// miniMap =====================================================================
	iPopup popMiniMap = null;
	iImage[] imgMiniMap;
	iStrTex stMiniMap;
	iPoint posMiniMap;

	string worldName, mapName;

	float miniMapW, miniMapH;
	float miniRatio;

	float miniTileW, miniTileH;

	void createPopMiniMap()
	{
		iPopup pop = new iPopup();
		iImage img = new iImage();

		imgMiniMap = new iImage[1];

		worldName = "리프레";
		mapName = "용의 둥지";

		miniRatio = 0.1f;
		miniTileW = f.tileW * miniRatio;
		miniTileH = f.tileH * miniRatio;

		miniMapW = f.tileX * miniTileW;
		miniMapH = f.tileY * miniTileH;

		stMiniMap = new iStrTex(methodStMiniMap, miniMapW + 10, miniMapH + 60);

		img.add(stMiniMap.tex);
        imgMiniMap[0] = img;
		pop.add(img);

		pop.methodDrawAfter = drawPopMiniMapAfter;
		popMiniMap = pop;
	}
	void methodStMiniMap(iStrTex st)
	{
		Texture tex = Resources.Load<Texture>("miniMap");
		drawImage(tex, 0, 0, 1.2f, 1.2f, TOP | LEFT, 2, 0, REVERSE_NONE);

		string[] str = st.str.Split("\n");
		string worldName = str[0];
		string mapName = str[1];
		float posX = float.Parse(str[2]);
		float posY = float.Parse(str[3]);

		popMiniMap.closePoint = new iPoint(posX, posY);

		for (int i = 0; i < f.tileX * f.tileY; i++)
		{
			float x = miniTileW * (i % f.tileX);
			float y = miniTileH * (i / f.tileX);
			int t = f.tiles[i];
			Color c = f.colorTile[t];
			if (t != 0)
			{
				setRGBA(c.r, c.g, c.b, c.a);
			}
			else
			{
				setRGBA(0, 0, 0, 1);
			}
			fillRect(x + 5, y + 50, miniTileW, miniTileH);
		}
		setRGBA(0, 0, 0, 1);

		// 몬스터(0, 1, 2, 3, ) 별, 네모, 동그라미
		// 주인공 삼각형

		setRGBA(1, 1, 1, 1);

		drawRect(5, 50, miniMapW + 2, miniMapH + 2);
		setRGBA(1, 1, 1, 1);

		setStringRGBA(0, 0, 0, 1);
		setStringSize(20);
		drawString(worldName, 1, 1, TOP | LEFT);
		drawString(mapName, 1, 24, TOP | LEFT);
	}

	void drawPopMiniMapAfter(float dt, iPopup pop, iPoint zero)
	{
		setRGBA(1, 1, 0, 1);
		pPos = p.position * miniRatio;
		fillRect(zero.x + pPos.x + 5, zero.y + pPos.y + 50, 50 * miniRatio, 50 * miniRatio);
	}

	bool dragMiniMap = false;
	iPoint mouseMiniMap = new iPoint();
	void mousePopMiniMap(iKeystate stat, iPoint point)
	{
		iPopup pop = popMiniMap;
		iImage[] img = imgMiniMap;
		if (pop.state == iPopupState.proc)
		{
			int i, j = -1;

			switch (stat)
			{
				case iKeystate.Began:
					i = pop.selected;
					if (i == -1) break;
					dragMiniMap = true;
					mouseMiniMap = point - posMiniMap;
					break;
				case iKeystate.Moved:
					for (i = 0; i < img.Length; i++)
					{
						if (img[i].topTouchRect(pop.closePoint).containPoint(point))
						{
							j = i;
							break;
						}
					}
					if (pop.selected != j)
						pop.selected = j;
					if (dragMiniMap)
					{
						posMiniMap = point - mouseMiniMap;
					}
					break;
				case iKeystate.Ended:
					i = pop.selected;
					dragMiniMap = false;
					if (i == -1) break;
					break;
			}
		}
	}

	iPoint pPos;
	void drawPopMiniMap(float dt)
	{
		stMiniMap.setString(worldName + "\n" + mapName + "\n" + posMiniMap.x + "\n" + posMiniMap.y);

		popMiniMap.paint(dt);
		
	}

	// popSetting ====================================================
	iPopup popSetting;
	iStrTex stSetting;
	iImage imgSettingBtn;
	void createPopSetting()
	{
		iPopup pop = new iPopup();
		iTexture tex = new iTexture(Resources.Load<Texture>("SettingBg"));
		iImage img = new iImage();

		img.add(tex);
		pop.add(img);

		string strBtn = "처음으로";
		img = new iImage();
		stSetting = new iStrTex(methodStSetting, 500, 300);
		stSetting.setString(strBtn + "\n");
		img.add(stSetting.tex);
		pop.add(img);

		pop.openPoint = new iPoint(200, 150);
		pop.closePoint = pop.openPoint;
		popSetting = pop;
		imgSettingBtn = img;
	}
	void methodStSetting(iStrTex st)
	{
		Texture tex = Resources.Load<Texture>("SettingBg");
		setRGBA(0, 0, 0, 1);
		setLineWidth(4);
		setRGBA(1, 1, 1, 1);
		drawImage(tex, stSetting.wid / 2, stSetting.hei / 2, VCENTER | HCENTER);
		string[] str = st.str.Split("\n");
		string test = str[0];

	}
	void drawPopSetting(float dt)
	{
		popSetting.paint(dt);
	}

	// mob ========================================================
	Monster[] _monster;
	public Monster[] monster;
	public int monsterNum;
	public iImage[] imgsMonster;

	void loadMonster()
	{
		loadMonsterImage();
		_monster = new Monster[20];
		for (int i = 0; i < 20; i++)
			_monster[i] = new Monster();

		monster = new Monster[20];
		monsterNum = 0;
	}

	void drawMonster(float dt, iPoint off)
	{
		for (int i = 0; i < monsterNum; i++)
		{
			monster[i].paint(dt, off);
			if (monster[i].alive == false)
			{
				monsterNum--;
				monster[i] = monster[monsterNum];
				i--;
			}
		}
	}

	void addMonster(iPoint p)
	{
		for (int i = 0; i < 20; i++)
		{
			if (_monster[i].alive == false)
			{
				_monster[i].alive = true;
				_monster[i].position = p;
				_monster[i].be = FObject.Behave.waitLeft;
				_monster[i].imgCurr = imgsMonster[(int)_monster[i].be];
				_monster[i].hp = 2;
				// 체력 만땅

				monster[monsterNum] = _monster[i];
				monsterNum++;
				return;
			}
		}
	}

#if true
	public void loadMonsterImage()
	{
		int beIndex, maxBe = (int)FObject.Behave.max;
		imgsMonster = new iImage[maxBe];
		int maxFrame = 2;
		for (beIndex = 0; beIndex < maxBe; beIndex++)
		{
			iImage img;
			if (beIndex % 2 == 0)
			{
				int be = beIndex / 2;
				if (be == 0)
					maxFrame = 2;
				else if (be == 1)
					maxFrame = 3;
				else if (be == 4)
					maxFrame = 1;
				else if (be == 6)
					maxFrame = 3;

				img = new iImage();
				for (int frame = 0; frame < maxFrame; frame++)
				{
					iStrTex animSt = new iStrTex(methodStBe, new Monster().rect.size.width, new Monster().rect.size.height);
					animSt.setString((beIndex / 2) + "\n" + frame);
					img.add(animSt.tex);
				}
			}
			else
			{
				img = imgsMonster[beIndex - 1].clone();
				img.leftRight = true;
			}
			if (beIndex < (int)FObject.Behave.att0Left)
			{
				img.repeatNum = 0;
				img._frameDt = 0.5f;
				img.startAnimation();
			}
			else
			{
				img.repeatNum = 1;
			}
			imgsMonster[beIndex] = img;
		}
	}

	void methodStBe(iStrTex st)
	{
		string[] s = st.str.Split("\n");
		int be = int.Parse(s[0]);
		int frame = int.Parse(s[1]);

		Texture tex;
		if (be == 0)
		{
			tex = Resources.Load<Texture>("OrangeMush/Stand/mobStand" + frame);
			setRGBA(1, 1, 1, 1);
			drawImage(tex, 0, new Monster().rect.size.width - tex.height, TOP | LEFT);
		}
		else if (be == 1)
		{
			tex = Resources.Load<Texture>("OrangeMush/Move/mobMove" + frame);
			setRGBA(1, 1, 1, 1);
			drawImage(tex, 0, new Monster().rect.size.width - tex.height, TOP | LEFT);
		}
		else if (be == 4)
		{
			tex = Resources.Load<Texture>("OrangeMush/Hit/mobHit0");
			setRGBA(1, 1, 1, 1);
			drawImage(tex, 0, new Monster().rect.size.width - tex.height, TOP | LEFT);
		}
		else if (be == 6)
		{
			tex = Resources.Load<Texture>("OrangeMush/Die/mobDie" + frame);
			setRGBA(1, 1, 1, 1);
			drawImage(tex, 0, new Monster().rect.size.width - tex.height, TOP | LEFT);
		}
		setStringSize(25);
		drawString("" + (1 + frame), 25, 25, VCENTER | HCENTER);
	}
#endif

	// item ================================

	
#if true
	public Item[] items = new Item[50];
	public int itemNum = 0;

	public void dropItem(Item i, iPoint p)
	{
		items[itemNum] = i;
		items[itemNum].position = p;
		itemNum++;
	}

	public void drawItem(float dt, iPoint off)
	{
		for(int i = 0; i<itemNum; i++)
		{
			items[i].paint(dt, off);
		}
	}

	public void removeItem(Item item)
    {
		for(int i = 0; i<itemNum; i++)
        {
            if (items[i] == item)
            {
				for (int j = 0; j < inven.Length; j++)
				{
					if (inven[j] == null)
					{
						inven[j] = item;
						invenNum[j]++;
						break;
					}
					else if (inven[j].getIndex() == item.getIndex())
					{
						invenNum[j]++;
						break;
					}
				}
				itemNum--;
				items[i] = items[itemNum];
				i--;
            }
        }
    }
	// invenSys===================
	private int invenX, invenY;
	private int invenXY;
	public Item[] inven;
	int[] invenNum;
	public void createInven()
	{
		invenX = 4; invenY = 10;
		invenXY = invenX * invenY;
		inven = new Item[invenXY];
		invenNum = new int[invenXY];
	}
#endif
}



public class Func
{
    public static Texture2D textureFromSprite(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }

}

enum TileAttr
{
    none = 0,
    cant,
    canUp,
    canLeft,
    canRight,
    mobCant,
	leftSlope,

    Max
}


public class Field
{
    Texture texBg;
    float ratioW, ratioH;

    public int tileX, tileY;
    public int tileW, tileH;
    public int[] tiles;
    public iPoint off, offMin, offMax;

    public Color[] colorTile;
    Texture fieldTex;

    public Field()
    {
        texBg = Resources.Load<Texture>("Background");
        ratioW = 1.0f * MainCamera.devWidth / texBg.width;
        ratioH = 1.0f * MainCamera.devHeight / texBg.height;

		Sprite[] spriteMap = Resources.LoadAll<Sprite>("map");

		fieldTex = Func.textureFromSprite(spriteMap[0]);
        tileX = 30;
        tileY = 19;
        tileW = fieldTex.width;
        tileH = fieldTex.height;

        tiles = new int[]
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0,
            0, 0, 5, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 5, 5, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 2, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 6, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        };
        off = new iPoint(0, 0);
        offMin = new iPoint(MainCamera.devWidth- tileW*tileX, MainCamera.devHeight-tileH*tileY);
        offMax = new iPoint(0, 0);

		colorTile = new Color[(int)TileAttr.Max]
		{
			Color.clear, Color.red, Color.blue, Color.gray, Color.yellow,Color.clear, Color.green,
        };
    }

	public void reset(int stage)
	{
		// 캐릭터 위치 초기화, 맵 정보 초기화
		// 페이드 인 아웃

	}

    public void paint(float dt)
    {
		iGUI.instance.setRGBA(1, 1, 1, 1);
        iGUI.instance.drawImage(texBg, 0, 0, ratioW, ratioH, iGUI.TOP | iGUI.LEFT);

        iPoint p = new iPoint(MainCamera.devWidth / 2, MainCamera.devHeight / 2)
                - Proc.me.p.position;
        iPoint lOff = new iPoint(0, 0);
        lOff.x = Mathf.Lerp(off.x, p.x, dt * 2);
        lOff.y = Mathf.Lerp(off.y, p.y, dt * 2);

        off = lOff;

        if (off.x < offMin.x)
            off.x = offMin.x;
        else if (off.x > offMax.x)
            off.x = offMax.x;
        if (off.y < offMin.y)
            off.y = offMin.y;
        else if (off.y > offMax.y)
            off.y = offMax.y;

        int i, tileXY = tileX * tileY;
        for(i = 0; i<tileXY; i++)
        {
            float x = off.x + tileW * (i % tileX);
            float y = off.y + tileH * (i / tileX);
            int t = tiles[i];
            Color c = colorTile[t];
            if (t == 1)
            {
                iGUI.instance.setRGBA(1, 1, 1, 1);
                iGUI.instance.drawImage(fieldTex, x, y, iGUI.TOP | iGUI.LEFT);
            }
			else if (t == 6)
			{
				iGUI.instance.setRGBA(c.r, c.g, c.b, c.a);
				iGUI.instance.drawLine(x, y + tileH, x + tileW, y);
			}
            else
            {
                iGUI.instance.setRGBA(c.r, c.g, c.b, c.a);
                iGUI.instance.fillRect(x, y, tileW, tileH);
            }
        }
        iGUI.instance.setRGBA(1, 1, 1, 1);
    }
}

class AttInfo
{
	public AttInfo()
	{
		liveDt = _liveDt = 0.000001f;
	}

	public float liveDt, _liveDt;

	public FObject att;
	public iRect rt;
	public int ap;
}

public class AttMgt
{
	AttInfo[] _ai;
	AttInfo[] ai;
	int aiNum;

	public AttMgt()
	{
		_ai = new AttInfo[100];
		for(int i=0; i<100; i++)
			_ai[i] = new AttInfo();
		ai = new AttInfo[100];
		aiNum = 0;
	}

	public void update(float dt, iPoint off)
	{
		iGUI.instance.setRGBA(1, 0, 1, 1);
		for(int i=0; i<aiNum; i++)
		{
			AttInfo a = ai[i];

			if( a.att == Proc.me.p )
			{
				// enemy 공격
				iRect dst;
				for(int j=0; j<Proc.me.monsterNum; j++)
				{
					iRect rt = a.rt;
					rt.origin += off;
					iGUI.instance.fillRect(rt);
					Monster m = Proc.me.monster[j];
					dst = m.rect;
					dst.origin += m.position;
					if( dst.containRect(a.rt) )
					{
						a.liveDt = a._liveDt;// kill rect
						m.hp -= a.ap;
						m.be = FObject.Behave.hitLeft;
                        m.imgCurr = m.imgs[(int)m.be];
                        m.imgCurr.startAnimation(m.cbAnim, m);
						break;
					}
				}
			}
			else if( a.att != Proc.me.p )
			{
				// player 공격
				iRect dst;
				iRect rt = a.rt;
				rt.origin += off;
				iGUI.instance.fillRect(rt);
				Player p = Proc.me.p;
				dst = p.rect;
				dst.origin += p.position;
				if (dst.containRect(a.rt))
				{
					a.liveDt = a._liveDt;// kill rect
					p.hp -= a.ap;
					break;
				}
			}

			a.liveDt += dt;
			if( a.liveDt > a._liveDt )
			{
				aiNum--;
				ai[i] = ai[aiNum];
				i--;
			}
		}
	}

	public void add(FObject att)
	{
		for(int i=0; i< 100; i++)
		{
			AttInfo a = _ai[i];
			if (a.liveDt < a._liveDt)
				continue;
			a.liveDt = 0;
			a.att = att;
			a.rt = new iRect(att.position.x, att.position.y, 100, 100);
			a.ap = 1;

			ai[aiNum] = a;
			aiNum++;
			return;
		}
	}
}
