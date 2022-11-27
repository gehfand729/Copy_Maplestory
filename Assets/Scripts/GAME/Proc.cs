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
	//public Portal pt;

	public int countLoad = 0;

	public AttMgt am;

	public InfoLog infoLog;
	public iPoint positionInfoLog;

	public override void load()
	{
		me = this;
		loadMonster();

		p = new Player();
		f = new Field();
		//pt = new Portal(1, new iPoint(4 * f.tileW, 16 * f.tileH));
		//ui = new UI();
		 
		am = new AttMgt();
		createInven();

		createPopUI();
		popInfo.show(true);
		popMiniMap.show(true);

		// for testing

		infoLog = new InfoLog();
		positionInfoLog = new iPoint(10, 300);
	}


	public override void draw(float dt)
	{
		float _dt = dt;
		if (popMenu.bShow)
			dt = 0f;

		f.paint(dt);
		p.paint(dt, f.off);
		drawMonster(dt, f.off);
		//pt.paint(dt, f.off);
		//ui.paint(dt);
		drawItem(dt, f.off);

		infoLog.paint(dt, positionInfoLog);


		am.update(dt, f.off);
		drawPopUI(_dt);
	}


	public override void key(iKeystate stat, iPoint point)
	{
		mousePopInven(stat, point);
		mousePopMiniMap(stat, point);
		mouseInfoLog(stat, point);
		if (popMenu.bShow == true)
			mousePopMenu(stat, point);
		if (popEnd.bShow == true)
			mousePopEnd(stat, point);
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
			if ((key & (int)iKeyboard.Esc) == (int)(iKeyboard.Esc))
			{
				if (popMenu.bShow == false)
					popMenu.show(true);
				else if (popMenu.state == iPopupState.proc)
					popMenu.show(false);
			}
			//if ((key & (int)iKeyboard.Esc) == (int)(iKeyboard.Esc))
			//{
			//	addMonster(new iPoint(4 * f.tileW, 12 * f.tileH));
			//}
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
		createPopMenu();
		createPopEnd();
	}

	void drawPopUI(float dt)
	{
		drawPopInfo(dt);
		drawPopMiniMap(dt);
		drawPopInven(dt);
		drawPopSetting(dt);
		drawPopEnd(dt);
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
		setRGBAWhite();
		popInven.closePoint = new iPoint(x, y);
	}

	void drawPopInvenAfter(float dt, iPopup pop, iPoint zero)
	{
		iPoint off = zero + new iPoint(14, 36);
		setRGBAWhite();

		for (int i=0; i< invenXY; i++)
		{
			// draw
			if (inven[i] == null) continue;
			popInfo.show(true);
			drawImage(inven[i].getTex(), off.x + (12 + 30) * (i % 4), off.y + 20 + (12 + 30) * (i / 4), TOP | LEFT);
			setStringRGBA(0, 0, 0, 1);
			setStringSize(14);
			drawString(invenNum[i].ToString(), off.x + 20 + (12 + 30) * (i % 4), off.y + 20 + 20 + (12 + 30) * (i / 4), TOP | LEFT);
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

#if false
	void createPopInfo()
	{
		iPopup pop = new iPopup();
		iImage img = new iImage();

		lv = 0;

		maxHp = p.maxHp;
		minHp = 0;
		mp = maxMp = 100;
		minMp = 0;
		// hp, mp, exp, lv 다 분리.
		// 위치는 이미지의 pos이용 off는 openPoint
		// pop 위치는 그대로 이용.
		stInfo = new iStrTex(methodStInfo, 204, 70);
		img.add(stInfo.tex);
		img.position = new iPoint((MainCamera.devWidth - 204) * 0.5f, 0);
		pop.add(img);
		
		stInfo = new iStrTex(methodStInfo, 204, 70);
		img.add(stInfo.tex);
		img.position = new iPoint((MainCamera.devWidth - 204) * 0.5f, 0);
		pop.add(img);


		pop.style = iPopupStyle.alpha;
		pop.openPoint = new iPoint(0, MainCamera.devHeight - 80);
		pop.closePoint = pop.openPoint;

		popInfo = pop;
	}
	void methodStInfo(iStrTex st)
	{
	}
	void drawPopInfoBefore(float dt, iPopup pop, iPoint zero)
    {

    }
	void drawPopInfoAfter(float dt, iPopup pop, iPoint zero)
	{

	}
#else
	void createPopInfo()
	{
		iPopup pop = new iPopup();
		iImage img = new iImage();

		lv = 0;

		maxHp = p.maxHp;
		minHp = 0;
		mp = maxMp = 100;
		minMp = 0;
		// hp, mp, exp, lv 다 분리.
		// 위치는 이미지의 pos이용 off는 openPoint
		// pop 위치는 그대로 이용.
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
		drawImage(tex, (MainCamera.devWidth - 250) / 2, 29, TOP | LEFT);

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

		setStringRGBA(1, 1, 0, 1);
		drawString("Lv. " + lv, (MainCamera.devWidth - 250) / 2 + 20, 0, TOP | LEFT);
		setStringRGBA(0, 0, 0, 1);
		drawString($"{hp} / {maxHp} ", (MainCamera.devWidth - 250) / 2 + 120, 23, TOP | HCENTER);
		setStringRGBA(0, 0, 0, 1);
		drawString($"{mp} / {maxMp} ", (MainCamera.devWidth - 250) / 2 + 120, 40, TOP | HCENTER);
		//setStringRGBA(1, 1, 1, 1);
		//drawString(exp.ToString(), (MainCamera.devWidth - 250) / 2 + 120, 50, TOP | LEFT);
		setStringRGBA(1, 1, 1, 1);
		setStringSize(10);
		drawString($"{result}%", (MainCamera.devWidth - 250) / 2 + 120, 63, TOP | HCENTER);

		setRGBAWhite();

	}
#endif
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

		miniRatio = 0.1f;
		miniTileW = f.tileW * miniRatio;
		miniTileH = f.tileH * miniRatio;

		miniMapW = f.tileX * miniTileW;
		miniMapH = f.tileY * miniTileH;

		stMiniMap = new iStrTex(methodStMiniMap, miniMapW + 18, miniMapH + 60);

		img.add(stMiniMap.tex);
        imgMiniMap[0] = img;
		pop.add(img);

		pop.methodDrawAfter = drawPopMiniMapAfter;
		popMiniMap = pop;
	}
	void methodStMiniMap(iStrTex st)
	{
#if false
		Texture tex = Resources.Load<Texture>("miniMap");
		drawImage(tex, 0, 0, 1.2f, 1.2f, TOP | LEFT, 2, 0, REVERSE_NONE);

		string[] str = st.str.Split("\n");
		string worldName = str[0];
		string mapName = str[1];
		float posX = float.Parse(str[2]);
		float posY = float.Parse(str[3]);

		popMiniMap.closePoint = new iPoint(posX, posY);
#else
		
		string[] str = st.str.Split("\n");
		worldName = str[0];
		mapName = str[1];
		float posX = float.Parse(str[2]);
		float posY = float.Parse(str[3]);

		popMiniMap.closePoint = new iPoint(posX, posY);
		// 몬스터(0, 1, 2, 3, ) 별, 네모, 동그라미
		// 주인공 삼각형

		setRGBAWhite();

#endif
	}

	void drawPopMiniMapAfter(float dt, iPopup pop, iPoint zero)
	{
		iPoint pos = zero;
		miniTileW = f.tileW * miniRatio;
		miniTileH = f.tileH * miniRatio;

		miniMapW = f.tileX * miniTileW;
		miniMapH = f.tileY * miniTileH;

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
				setRGBA(0, 0, 0, 0);
			}
			fillRect(pos.x + x + 9, pos.y + y + 61, miniTileW, miniTileH);
		}

		Texture tex = Resources.Load<Texture>("MiniMap/nw");
		setRGBAWhite();
		drawImage(tex, pos.x + 0, pos.y + 0, TOP | LEFT);
		int nwth = tex.height;
		int tw = tex.width;
		tex = Resources.Load<Texture>("MiniMap/sw");
		int swth = tex.height;
		drawImage(tex, pos.x + 0, pos.y + nwth + miniMapH - swth + 3, TOP | LEFT);
		for (int j = nwth; j < nwth + miniMapH - swth + 3; j++)
		{
			tex = Resources.Load<Texture>("MiniMap/w");
			drawImage(tex, pos.x + 0, pos.y + j, TOP | LEFT);
			tex = Resources.Load<Texture>("MiniMap/e");
			drawImage(tex, pos.x + miniMapW + 18 - tex.width, pos.y + j, TOP | LEFT);
		}
		for (int i = tw; i < miniMapW + 18 - tw; i++)
		{
			tex = Resources.Load<Texture>("MiniMap/n");
			drawImage(tex, pos.x + i, pos.y + 0, TOP | LEFT);
			tex = Resources.Load<Texture>("MiniMap/s");
			drawImage(tex, pos.x + i, pos.y + 67 + miniMapH - 6, TOP | LEFT);
		}
		tex = Resources.Load<Texture>("MiniMap/ne");
		drawImage(tex, pos.x + miniMapW + 18 - tex.width, pos.y + 0, TOP | LEFT);
		tex = Resources.Load<Texture>("MiniMap/se");
		drawImage(tex, pos.x + miniMapW + 18 - tex.width, pos.y + 67 + miniMapH - 24, TOP | LEFT);

		

		// 몬스터(0, 1, 2, 3, ) 별, 네모, 동그라미
		// 주인공 삼각형

		setRGBAWhite();

		setStringRGBA(1, 1, 1, 1);
		setStringSize(15);
		drawString(worldName,	pos.x +	48, pos.y + 15, TOP | LEFT);
		drawString(mapName,		pos.x +	48, pos.y +	15 + 17, TOP | LEFT);
		setRGBA(1, 1, 0, 1);
		pPos = p.position * miniRatio;
		fillRect(pos.x + pPos.x + 9, pos.y + pPos.y + 61, 50 * miniRatio, 50 * miniRatio);
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
		stMiniMap.setString(f.strWorld + "\n" + f.strMap + "\n" + posMiniMap.x + "\n" + posMiniMap.y);

		popMiniMap.paint(dt);
		
	}

	// popMenu ====================================================
	public iPopup popMenu;
	iImage[] imgMenuBtn;
	void createPopMenu()
	{
		iPopup pop = new iPopup();

		int w = 400, h = 250;
		iStrTex st = new iStrTex(methodMenuBg, w, h);
		st.setString("null");
		iImage img = new iImage();
		img.add(st.tex);
		pop.add(img);

		imgMenuBtn = new iImage[2];
		string[] strMenuBtn = new string[]
		{
			"처음으로", "이어하기",
		};
		iStrTex stPop = new iStrTex();
		for(int i = 0; i< 2; i++)
        {
			img = new iImage();
			stPop = new iStrTex(methodStMenu, 300, 100);
			stPop.setString(strMenuBtn[i]);
			img.position = new iPoint((w - stPop.tex.tex.width) /2 ,
									15 + 120 * i);
			img.add(stPop.tex);
			pop.add(img);
			imgMenuBtn[i] = img;
		}
		pop.style = iPopupStyle.alpha;
		pop.openPoint = new iPoint(	(MainCamera.devWidth - w) / 2,
									(MainCamera.devHeight - h) / 2);
		pop.closePoint = pop.openPoint;

		popMenu = pop;
	}

	void methodMenuBg(iStrTex st)
	{
		Texture tex = Resources.Load<Texture>("bgMenu");
		setRGBA(1, 1, 1, 0.5f);
		drawImage(tex, 0, 0, TOP | LEFT);
	}

	void methodStMenu(iStrTex st)
	{
		setRGBA(0.5f, 0.5f, 0.5f, 1);
		fillRect(0, 0, 500, 80);
		setRGBA(0, 0, 0, 1);
		setLineWidth(3);
		drawRect(0, 0, 500, 80);

		setStringSize(30);
		setStringName("Maplestory Bold");
		setStringRGBA(0, 0, 0, 1);
		drawString(	st.str, st.wid/2, st.hei/2, VCENTER | HCENTER);
	}

	private void mousePopMenu(iKeystate stat, iPoint point)
	{
		iPopup pop = popMenu;
		iImage[] imgBtn = imgMenuBtn;

		int i, j = -1;

		switch (stat)
		{
			case iKeystate.Began:
				i = pop.selected;
				if (i == -1) break;
				imgBtn[i].select = true;
				break;
			case iKeystate.Moved:
				for (i = 0; i < imgBtn.Length; i++)
				{
					if (imgBtn[i].touchRect(pop.closePoint, new iSize(0, 0)).containPoint(point))
					{
						j = i;
						break;
					}
				}

				if (pop.selected != j)
				{
					pop.selected = j;
					if (pop.selected != -1)
						imgBtn[pop.selected].select = false;
					pop.selected = j;
				}

				break;
			case iKeystate.Ended:
				i = pop.selected;
				if (i == -1) break;
				imgBtn[i].select = false;
				if (i == 0)
					Main.me.reset("Intro");
				else if (i == 1)
					popMenu.show(false);
				break;
		}
	}
	void drawPopSetting(float dt)
	{
		popMenu.paint(dt);
	}

	// end ============
	public iPopup popEnd;
	iStrTex stEndBg;
	iImage[] imgEndBtn;

	void createPopEnd()
	{
		iPopup pop = new iPopup();

		int w = 400, h = 350;
		iStrTex st = new iStrTex(methodEndBg, w, h);
		st.setString("0");
		iImage img = new iImage();
		img.add(st.tex);
		pop.add(img);
		stEndBg = st;

		imgEndBtn = new iImage[1];
		string[] strEndBtn = new string[]
		{
			"처음으로",
		};
		iStrTex stPop = new iStrTex();
		for (int i = 0; i < 1; i++)
		{
			img = new iImage();
			stPop = new iStrTex(methodStEnd, 300, 100);
			stPop.setString(strEndBtn[i]);
			img.position = new iPoint((w - stPop.tex.tex.width) / 2,
									65 + 120 * i);
			img.add(stPop.tex);
			pop.add(img);
			imgEndBtn[i] = img;
		}
		pop.style = iPopupStyle.alpha;
		pop.openPoint = new iPoint((MainCamera.devWidth - w) / 2,
									(MainCamera.devHeight - h) / 2);
		pop.methodDrawBefore = drawPopEndBefore;
		pop.closePoint = pop.openPoint;

		popEnd = pop;
	}

	void methodEndBg(iStrTex st)
	{
		Texture tex = Resources.Load<Texture>("bgMenu");
		setRGBA(1, 1, 1, 0.5f);
		drawImage(tex, 0, 0, TOP | LEFT);
		setStringName("Maplestory Bold");
		setRGBA(0.3f, 1, 1, 1);
		setStringSize(50);

		int fail = int.Parse(st.str);
		string[] s = new string[] { "Fail", "Clear!!" };
		drawString(s[fail], st.wid / 2, 10, TOP | HCENTER);
	}

	void methodStEnd(iStrTex st)
	{
		

		setRGBA(0.5f, 0.5f, 0.5f, 1);
		fillRect(0, 0, 500, 80);
		setRGBA(0, 0, 0, 1);
		setLineWidth(3);
		drawRect(0, 0, 500, 80);

		setStringName("Maplestory Bold");

		setStringSize(30);
		setStringRGBA(0, 0, 0, 1);
		drawString(st.str, st.wid / 2, st.hei / 2, VCENTER | HCENTER);
	}

	private void mousePopEnd(iKeystate stat, iPoint point)
	{
		iPopup pop = popEnd;
		iImage[] imgBtn = imgEndBtn;

		int i, j = -1;

		switch (stat)
		{
			case iKeystate.Began:
				i = pop.selected;
				if (i == -1) break;
				imgBtn[i].select = true;
				break;
			case iKeystate.Moved:
				for (i = 0; i < imgBtn.Length; i++)
				{
					if (imgBtn[i].touchRect(pop.closePoint, new iSize(0, 0)).containPoint(point))
					{
						j = i;
						break;
					}
				}

				if (pop.selected != j)
				{
					pop.selected = j;
					if (pop.selected != -1)
						imgBtn[pop.selected].select = false;
					pop.selected = j;
				}

				break;
			case iKeystate.Ended:
				i = pop.selected;
				if (i == -1) break;
				imgBtn[i].select = false;
				if (i == 0)
					Main.me.reset("Intro");
				break;
		}
	}

	void drawPopEndBefore(float dt, iPopup pop, iPoint zero)
	{
		int n = (Proc.me.p.hp == 0 ? 0 : 1);
		stEndBg.setString("" + n);
	}

	void drawPopEnd(float dt)
	{
		popEnd.paint(dt);
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

	public void addMonster(iPoint p)
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

	public void removeMonster()
    {
		for(int i = 0; i< monsterNum; i++)
        {
			if(monster[i].alive)
				monster[i].alive = false;
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
					iStrTex animSt = new iStrTex(methodStBe, 64, 65);
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

		Texture tex = null;
		if (be == 0)
			tex = Resources.Load<Texture>("OrangeMush/Stand/mobStand" + frame);
		else if (be == 1)
			tex = Resources.Load<Texture>("OrangeMush/Move/mobMove" + frame);
		else if (be == 4)
			tex = Resources.Load<Texture>("OrangeMush/Hit/mobHit0");
		else if (be == 6)
			tex = Resources.Load<Texture>("OrangeMush/Die/mobDie" + frame);
		else
			tex = Resources.Load<Texture>("OrangeMush/Stand/mobStand0");
		setRGBAWhite();
		drawImage(tex, st.wid * 0.5f, st.hei, BOTTOM | HCENTER);
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
	bool drag;
	iPoint posLog = new iPoint();
	void mouseInfoLog(iKeystate stat, iPoint point)
	{
		InfoLog iLog = infoLog;
		int i, j = -1;

		switch (stat)
		{
			case iKeystate.Began:
				i = iLog.selected;
				if (i == -1) break;
				drag = true;
				posLog = point - positionInfoLog;
				break;
			case iKeystate.Moved:
				for (i = 0; i < 1; i++)
				{
					if (infoLog.rect.containPoint(point))
					{
						j = i;
						break;
					}
				}
				if (iLog.selected != j)
					iLog.selected = j;
				if (drag)
				{
					positionInfoLog = point - posLog;
				}
				break;
			case iKeystate.Ended:
				i = iLog.selected;
				drag = false;
				if (i == -1) break;
				break;
		}
	}
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

	public string strWorld, strMap;

    public int tileX, tileY;
    public int tileW, tileH;
    public int[] tiles;
    public iPoint off, offMin, offMax;

    public Color[] colorTile;
    Texture texField;
    Texture texFieldHead;

	public Portal pt;


	public Field()
    {
		strWorld = "버섯 숲"; strMap = "1단계 : 버섯 숲1";
		texBg = Resources.Load<Texture>("Background");
        ratioW = 1.0f * MainCamera.devWidth / texBg.width;
        ratioH = 1.0f * MainCamera.devHeight / texBg.height;

		texField = Resources.Load<Texture>("Map/Tile/bsc0");
		texFieldHead = Resources.Load<Texture>("Map/Tile/enH00");
		tileX = 19;
        tileY = 9;
        tileW = texField.width;
        tileH = texField.height;
		Proc.me.p.position = new iPoint(1 * tileW, (tileY - 2) * tileH);
		pt = new Portal(1, new iPoint(18 * tileW, (tileY - 1) * tileH));

		Proc.me.addMonster(new iPoint(5 * tileW, (tileY - 3) * tileH));
		Proc.me.addMonster(new iPoint(10 * tileW, (tileY - 3) * tileH));
		Proc.me.addMonster(new iPoint(12 * tileW, (tileY - 3) * tileH));
		Proc.me.addMonster(new iPoint(18 * tileW, (tileY - 3) * tileH));

		tiles = new int[]
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		};
		off = new iPoint(0, 0);
		offMax = new iPoint(0, 0);
		offMin = new iPoint(MainCamera.devWidth - tileW * tileX, MainCamera.devHeight - tileH * tileY);
#if true// 스크롤 되지 않는 맵에서는 발생!! 
		if (offMax.x < offMin.x)
			offMax.x = offMin.x;
		if (offMax.y < offMin.y)
			offMax.y = offMin.y;
#endif

		colorTile = new Color[(int)TileAttr.Max]
		{
			Color.clear, Color.red, Color.blue, Color.gray, Color.yellow,Color.clear, Color.green,
        };
    }

	public void reset(int stage)
	{
		// 캐릭터 위치 초기화, 맵 정보 초기화
		if (stage == 1)
		{
			Proc.me.removeMonster();
			strWorld = "버섯 숲"; strMap = "2단계 : 버섯 숲2";
			tileX = 24;
			tileY = 13;
			Proc.me.p.position = new iPoint(1 * tileW, 11 * tileH);
			off = new iPoint(0, 0);
			pt.Pos = new iPoint(5 * tileW, (tileY - 7) * tileH);
			pt.Index = 2;
			Proc.me.addMonster(new iPoint(8 * tileW, (tileY - 5) * tileH));
			Proc.me.addMonster(new iPoint(13 * tileW, (tileY - 7) * tileH));
			Proc.me.addMonster(new iPoint(14 * tileW, (tileY - 9) * tileH));
			Proc.me.addMonster(new iPoint(5 * tileW, (tileY - 12) * tileH));
			Proc.me.addMonster(new iPoint(18 * tileW, (tileY - 10) * tileH));
			//Proc.me.f.tiles
			tiles = new int[]
			{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 5, 0, 0, 0, 0, 5, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 2, 2, 2, 2, 5, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 5, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 2, 2, 2, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 5, 0, 0, 0, 5, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			};
			offMax = new iPoint(0, 0);
			offMin = new iPoint(MainCamera.devWidth - tileW * tileX, MainCamera.devHeight - tileH * tileY);
#if true// 스크롤 되지 않는 맵에서는 발생!! 
			if (offMax.x < offMin.x)
				offMax.x = offMin.x;
			if (offMax.y < offMin.y)
				offMax.y = offMin.y;
#endif
		}
		else if (stage == 2)
		{
			Proc.me.removeMonster();
			strWorld = "버섯 숲"; strMap = "3단계 : 버섯 숲3";
			tileX = 15;
			tileY = 25;
			Proc.me.p.position = new iPoint(1 * tileW, (tileY - 2) * tileH);
			off = new iPoint(0, 0);
			pt.Pos = new iPoint(8 * tileW, (tileY - 19) * tileH);
			pt.Index = 3;
			Proc.me.addMonster(new iPoint(8 * tileW, (tileY - 8) * tileH));
			Proc.me.addMonster(new iPoint(13 * tileW, (tileY - 18) * tileH));
			Proc.me.addMonster(new iPoint(12 * tileW, (tileY - 9) * tileH));
			Proc.me.addMonster(new iPoint(10 * tileW, (tileY - 10) * tileH));
			Proc.me.addMonster(new iPoint(5 * tileW, (tileY - 12) * tileH));
			Proc.me.addMonster(new iPoint(13 * tileW, (tileY - 9) * tileH));
			Proc.me.addMonster(new iPoint(7 * tileW, (tileY - 15) * tileH));
			Proc.me.addMonster(new iPoint(13 * tileW, (tileY - 20) * tileH));
			//Proc.me.f.tiles
			tiles = new int[]
			{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 5, 0, 0, 0, 5, 0, 0, 0, 0, 0,
			0, 0, 0, 5, 0, 5, 2, 2, 2, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 5, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 2, 5, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 2, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 2, 2, 2, 5, 0, 0, 5, 0, 2,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 5, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 2, 5, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 2, 5, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 2, 5, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 2, 5, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 2, 5, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 2, 5, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 2, 2, 2, 0, 0,
			0, 5, 0, 0, 0, 5, 0, 0, 5, 2, 0, 0, 0, 0, 0,
			0, 5, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			2, 2, 5, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			};
			offMax = new iPoint(0, 0);
			offMin = new iPoint(MainCamera.devWidth - tileW * tileX, MainCamera.devHeight - tileH * tileY);
#if true// 스크롤 되지 않는 맵에서는 발생!! 
			if (offMax.x < offMin.x)
				offMax.x = offMin.x;
			if (offMax.y < offMin.y)
				offMax.y = offMin.y;
#endif
		}
		else if (stage == 3)
		{
			Proc.me.removeMonster();
			if(Proc.me.popEnd.bShow == false)
				Proc.me.popEnd.show(true);
		}
		// 페이드 인 아웃

	}

    public void paint(float dt)
    {
		iGUI.instance.setRGBAWhite();
        iGUI.instance.drawImage(texBg, 0, 0, ratioW, ratioH, iGUI.TOP | iGUI.LEFT);

        iPoint p = new iPoint(MainCamera.devWidth / 2, MainCamera.devHeight / 2)
                - Proc.me.p.position;
        //iPoint lOff = new iPoint(0, 0);
		off.x = Mathf.Lerp(off.x, p.x, dt * 2);
		off.y = Mathf.Lerp(off.y, p.y, dt * 2);

		//off = lOff;

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
            if (t == 1 || t == 2)
            {
                iGUI.instance.setRGBAWhite();
				iGUI.instance.drawImage(texFieldHead, x, y + 18 - texFieldHead.height, iGUI.TOP | iGUI.LEFT);
				iGUI.instance.drawImage(texField, x, y + 18, iGUI.TOP | iGUI.LEFT);
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
        iGUI.instance.setRGBAWhite();
		pt.paint(dt, off);
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
					if ((int)Proc.me.p.be % 2 == 0)
						a.rt.origin.x -= Proc.me.p.rect.size.width;
					Monster m = Proc.me.monster[j];
					dst = m.rect;
					dst.origin += m.position;
					if( dst.containRect(a.rt) )
					{
						a.liveDt = a._liveDt;// kill rect
						m.hp -= a.ap;
						m.be = FObject.Behave.hitLeft + (int)m.be % 2 ;
                        m.imgCurr = m.imgs[(int)m.be];
                        m.imgCurr.startAnimation(m.cbAnim, m);
						Proc.me.infoLog.addDmg(a.ap);
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
					Proc.me.infoLog.addDmg(-a.ap);
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

public class InfoLog
{
	struct Info
	{
		public Info(string s, Color c)
		{
			this.s = s;
			this.c = c;
		}

		public string s;
		public Color c;
	}

	public int selected;
	Info[] info;
	int infoNum;
	float displayDt;

	public iRect rect;

	public InfoLog(int max = 5)
	{
		info = new Info[max];
		infoNum = 0;
		selected = -1;
	}

	public void paint(float dt, iPoint off)
	{
		rect = new iRect(off.x, off.y, 400, 100);
		if (displayDt == 0f)
			return;

		displayDt -= dt;
		if (displayDt < 0)
		{
			displayDt = 0;
			infoNum = 0;// 이전 데이터 삭제
		}

		float a = 1f;
		if (displayDt < 1)
			a = displayDt;

		iGUI.instance.setRGBA(0, 0, 0, 0.6f * a);
		iGUI.instance.fillRect(off.x, off.y, 400, 140);
		iGUI.instance.setRGBA(1, 1, 1, 1);

		iGUI.instance.setStringSize(20);
		for(int i=0; i<infoNum; i++)
		{
			ref Info d = ref info[i];
			iGUI.instance.setStringRGBA(d.c.r, d.c.g, d.c.b, d.c.a * a);
			iGUI.instance.drawString(d.s, off + new iPoint(5, 5+25*i));
		}
	}

	private void add(string s, Color c)
	{
		if( infoNum==info.Length )
		{
			infoNum--;
			for (int i = 0; i < infoNum; i++)
				info[i] = info[1 + i];
		}

		Info t = new Info(s, c);
		info[infoNum] = t;
		infoNum++;

		displayDt = 2f;
	}

	public void addDmg(int dmg)
	{
		if (dmg > 0)
		{
			add("적에게 대미지" + dmg + "을(를) 입혔습니다.", Color.white);

		}
		if (dmg < 0)
		{
			add("대미지 피해" +  (-dmg) + "을(를) 받았습니다.", Color.red);
		}
	}

	public void addItem(string name)
	{
		add("아이템 " + name + "을 획득했습니다.", Color.blue);
	}


}