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

	public AttMgt am;

    public override void load()
    {
		me = this;

        f = new Field();
        p = new Player();
        //ui = new UI();
        loadMonster();

		am = new AttMgt();

        createPopUI();
        popInfo.show(true);
        popMiniMap.show(true);

        // for testing
        addMonster(0, new iPoint(4 * f.tileW, 10 * f.tileH));
    }


    public override void draw(float dt)
    {
        f.paint(dt);
        p.paint(dt, f.off);
        drawMonster(dt, f.off);
        drawPopUI(dt);
		//ui.paint(dt);

		am.update(dt, f.off);
    }


    public override void key(iKeystate stat, iPoint point)
    {
            mousePopInven(stat, point);
        // ui
        // f
        // p
    }

    public override void keyboard(iKeystate stat, int key)
    {
        // ui
        if (stat == iKeystate.Began)
        {
            if ((key & (int)iKeyboard.i) == (int)(iKeyboard.i))
            {
                if (popInven.bShow == false)
                    popInven.show(true);
                else if (popInven.state == iPopupState.proc)
                    popInven.show(false);
            }
            if ((key & (int)iKeyboard.esc) == (int)(iKeyboard.esc))
            {
                if (popSetting.bShow == false)
                    popSetting.show(true);
                else if (popSetting.state == iPopupState.proc)
                    popSetting.show(false);
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

    // Inventory ===============================================================
    iPopup popInven = null;
    iImage[] imgInven;
    iImage img;
    iPoint offInven;
    Texture texInven;
    iRect rectInven;

    void createPopInven()
    {
        // inventory
        iPopup pop = new iPopup();
        iImage img = new iImage();
        imgInven = new iImage[1];
        texInven = Resources.Load<Texture>("invenBg1");
        iTexture tex = new iTexture(texInven);
        rectInven = new iRect(0, 0, texInven.width, 30);
        img.add(tex);
        imgInven[0] = img;
        pop.add(img);

        pop.openPoint = new iPoint(500, 300);
        pop.closePoint = pop.openPoint;

        popInven = pop;
    }
    void drawPopInven(float dt)
    {
        popInven.paint(dt);
    }
    void keyboardPopInven(iKeystate stat, int key)
    {

    }

    void mousePopInven(iKeystate stat, iPoint point)
    {
    }

    void wheelPopInven(iPoint wheel)
    {

    }

    void moveInven(iPoint point)
    {
        offInven = point;
    }

    // Info =====================================================================
    iPopup popInfo = null;
    int lv, hp, maxHp, minHp, mp, maxMp, minMp;
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


        stInfo = new iStrTex(methodStInfo, 204, 70);
        img.add(stInfo.tex);
        pop.add(img);

        pop.style = iPopupStyle.alpha;
        pop.openPoint = new iPoint((MainCamera.devWidth - 250) / 2, MainCamera.devHeight - 75);
        pop.closePoint = pop.openPoint;

        popInfo = pop;
    }
    void methodStInfo(iStrTex st)
    {
        Texture tex = Resources.Load<Texture>("bgInfo");
        drawImage(tex, 0, 29, TOP | LEFT);

        string[] strs = st.str.Split("\n");
        int lv = int.Parse(strs[0]);
        float hp = int.Parse(strs[1]);
        float mp = int.Parse(strs[2]);
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
        tex = Resources.Load<Texture>("hp");
        drawImage(tex, 25, 28, rHp, 1, TOP | LEFT);

        tex = Resources.Load<Texture>("mp");
        drawImage(tex, 25, 43, rMp, 1, TOP | LEFT);

        tex = Resources.Load<Texture>("infoCover");
        drawImage(tex, 0, 0, TOP | LEFT);


        setStringRGBA(0, 0, 0, 1);
        //drawString("Lv." + lv, 2, 2, TOP | LEFT);
        //
        //drawString("Hp:" + hp + "/" + maxHp, 2, 26, TOP | LEFT);
        //
        //drawString("Mp:" + mp + "/" + maxMp, 2, 50, TOP | LEFT);
        setRGBA(1, 1, 1, 1);

    }
    void drawPopInfo(float dt)
    {
        hp = p.hp;
        stInfo.setString(lv + "\n" + hp + "\n" + mp);

        popInfo.paint(dt);
    }

    // miniMap =====================================================================
    iPopup popMiniMap = null;
    iStrTex stMiniMap;
    iPoint pp;

    string worldName, mapName;

    float miniMapW, miniMapH;
    float miniRatio;

    float miniTileW, miniTileH;

    void createPopMiniMap()
    {
        iPopup pop = new iPopup();
        iImage img = new iImage();

        worldName = "리프레";
        mapName = "용의 둥지";

        miniRatio = 0.1f;
        miniTileW = f.tileW * miniRatio;
        miniTileH = f.tileH * miniRatio;

        miniMapW = f.tileX * miniTileW;
        miniMapH = f.tileY * miniTileH;

        stMiniMap = new iStrTex(methodStMiniMap, miniMapW + 10, miniMapH + 60);
        stMiniMap.setString(worldName + "\n" + mapName);

        img.add(stMiniMap.tex);
        pop.add(img);

        popMiniMap = pop;
    }
    void methodStMiniMap(iStrTex st)
    {
        Texture tex = Resources.Load<Texture>("miniMap");
        drawImage(tex, 0, 0, 1.2f, 1.2f, TOP | LEFT, 2, 0, REVERSE_NONE);

        string[] str = st.str.Split("\n");
        string worldName = str[0];
        string mapName = str[1];

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

    iPoint pPos;
    void drawPopMiniMap(float dt)
    {
        popMiniMap.paint(dt);
        setRGBA(1, 1, 0, 1);
        pPos = p.position * miniRatio;
        fillRect(pPos.x + 5, pPos.y + 50, 50 * miniRatio, 50 * miniRatio);
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
        stSetting.setString(strBtn +"\n");
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
        drawImage(tex, stSetting.wid/2, stSetting.hei/2, VCENTER | HCENTER);
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

    void loadMonster()
    {
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

    void addMonster(int index, iPoint p)
    {
        for (int i = 0; i < 20; i++)
        {
            if (_monster[i].alive == false)
            {
                _monster[i].alive = true;
                _monster[i].position = p;
                // 체력 만땅

                monster[monsterNum] = _monster[i];
                monsterNum++;
                return;
            }
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
						Debug.Log("test");
						m.hp -= a.ap;
						break;
					}
				}
			}
			else if( a.att != Proc.me.p )
			{
				// player 공격
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
