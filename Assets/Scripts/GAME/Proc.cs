using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;
using System.Runtime.InteropServices;

public class Proc : GObject
{
    public static Field f;
    public Player p;
    //public UI ui;

    public override void load()
    {
        f = new Field();
        p = new Player();
        //ui = new UI();
        loadMonster();

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
    }


    public override void key(iKeystate stat, iPoint point)
    {
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
    iImage[] invenImg;
    iImage img;
    iPoint offInven;
    Texture texInven;

    void createPopInven()
    {
        // inventory
        iPopup pop = new iPopup();
        iImage img = new iImage();
        texInven = Resources.Load<Texture>("invenBg1");
        iTexture tex = new iTexture(texInven);
        img.add(tex);
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
    public static Monster[] monster;
    public static int monsterNum;

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


        fieldTex = Func.textureFromSprite(Resources.Load<Sprite>("map"));
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
            0, 0, 5, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0,
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
                - ((Proc)Main.curr).p.position;
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

public class FObject
{
    public bool alive;
    public iPoint position;
    public iRect rect;
    public iPoint v;
    public float moveSpeed;
    public float gravity;
    public bool jumping;
    public float jumpForce;

    public bool block;

    public int hp, maxHp, ap;
    
    public FObject()
    {
        gravity = 2000;
    }

    public virtual void attack() { }

    public void move(float dt) 
    {
        iPoint v = this.v * moveSpeed;
        jumpForce += gravity * dt;
        v.y += jumpForce;
        if (v.x < 0)
        {
            float xx = position.x + rect.origin.x;
            float yy = position.y + rect.origin.y;
            int x = (int)xx; x /= Proc.f.tileW;
            int y = (int)yy; y /= Proc.f.tileH;
            float minX = 0;
            bool check = false;

            float k = (yy + rect.size.height) / Proc.f.tileH;
            for (int i = x - 1; i > -1; i--)
            {
                for (int j = y; j < k; j++)
                {
                    int n = Proc.f.tiles[Proc.f.tileX * j + i];
                    if (n == 0) continue;
                    if (n == 2 || n == 3) continue;

                    minX = Proc.f.tileW * (i + 1);
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
                block = true;
            }
            else
                block = false;
            position.x = xx - rect.origin.x;
        }
        else if (v.x > 0)
        {
            float xx = position.x + rect.origin.x + rect.size.width;
            float yy = position.y + rect.origin.y;
            int x = (int)xx; x /= Proc.f.tileW;
            int y = (int)yy; y /= Proc.f.tileH;
            float maxX = Proc.f.tileX * Proc.f.tileW - 1;
            bool check = false;

			float k = (yy + rect.size.height) / Proc.f.tileH;
            for (int i = x + 1; i < Proc.f.tileX; i++)
            {
                for (int j = y; j < k; j++)
                {
                    int n = Proc.f.tiles[Proc.f.tileX * j + i];
                    if (n == 0) continue;
                    if (n == 2 || n == 4) continue;

					maxX = Proc.f.tileW * i - 1;
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
                block = true;
            }
            else
                block = false;
            position.x = xx - rect.origin.x - rect.size.width;
        }
        if (v.y < 0)
        {
            float xx = position.x + rect.origin.x;
            float yy = position.y + rect.origin.y;
            int x = (int)xx; x /= Proc.f.tileW;
            int y = (int)yy; y /= Proc.f.tileH;
            float minY = 0;
            bool check = false;

            float k = (xx + rect.size.width) / Proc.f.tileW;
            for (int j = y - 1; j > -1; j--)
            {
                for (int i = x; i < k; i++)
                {
                    int n = Proc.f.tiles[Proc.f.tileX * j + i];
                    if (n == 0) continue;
                    if (n == 1)
                    {
                        minY = Proc.f.tileH * (j + 1);
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
            int x = (int)xx; x /= Proc.f.tileW;
            int y = (int)yy; y /= Proc.f.tileH;
            float maxY = Proc.f.tileY * Proc.f.tileH - 1;
            bool check = false;

            float k = (xx + rect.size.width) / Proc.f.tileW;
            for (int j = y + 1; j < Proc.f.tileY; j++)
            {
                for (int i = x; i < k; i++)
                {
                    int n = Proc.f.tiles[Proc.f.tileX * j + i];
                    if (n == 0) continue;
                    if (n == 3 || n == 4) continue;

                    maxY = Proc.f.tileH * j - 1;
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

public class Player : FObject
{
	enum Behave
	{
		waitLeft = 0,
		waitRight,
        walkLeft,
        walkRight,

        att0Left,
        att0Right,
        att1Left,
        att1Right,
        att2Left,
        att2Right,

        jumpLeft,
		jumpRight,



		max
	}
	iImage[] imgs;
	iImage imgCurr;
	Behave be;

#if true
#if false
    void loadImage()
    {
        int i, j = (int)Behave.max;
        imgs = new iImage[j];
        for (i = 0; i < j; i++)
        {
            iImage img;
            if (i % 2 == 0)// left 동wkr
            {
                img = new iImage();
                for (int k = 0; k < 3; k++)
                {
                    iStrTex st = new iStrTex(methodStCreateImg, 50,50);
                    st.setString(i % 2 + "\n" + k );
                }
            }
        }
    }
#else
    void loadImage()
    {
        int i, j = (int)Behave.max;
        imgs = new iImage[j];
        for (i = 0; i < j; i++)
        {
            iImage img;
            if (i % 2 == 0)
            {
                img = new iImage();
                for (int k = 0; k < 2; k++)
                {
                    iStrTex st = new iStrTex(methodStCreateImg, 100, 200);
                    st.setString((i / 2) + "\n" + k);
                    img.add(st.tex);
                }
            }
            else
            {
                img = imgs[i - 1].clone();
                img.leftRight = true;
            }

            if (i < (int)Behave.walkLeft)
            {
                img.repeatNum = 0;
                img.startAnimation();
            }
            else
            {
                img.repeatNum = 1;
            }

            imgs[i] = img;
        }
        be = Behave.waitLeft;
        imgCurr = imgs[(int)be];

        // test
        //if (imgCurr.leftRight)
        //{
        //	be = Behave.waitRight;
        //}
        //be = Behave.attLeft;
        //imgs[(int)be].startAnimation(cbAnimation, this);
        //imgCurr = imgs[(int)be];
    
    }
#endif

    void methodStCreateImg(iStrTex st)
    {
        string[] str = st.str.Split("\n");
        int be = int.Parse(str[0]);
        int frame = int.Parse(str[1]);
        //iImage imgs;
        if(be == 0)
        {
            Sprite[] sprite = Resources.LoadAll<Sprite>("heroWait");
            Texture tex = Func.textureFromSprite(sprite[frame]);
            iGUI.instance.setRGBA(1, 1, 1, 1);
            iGUI.instance.drawImage(tex, 0, 0, iGUI.TOP | iGUI.LEFT);
        }
        else if( be == 1)
        {
            Sprite[] sprite = Resources.LoadAll<Sprite>("heroWalk0");
            Texture tex = Func.textureFromSprite(sprite[frame]);
            iGUI.instance.setRGBA(1, 1, 1, 1);
            iGUI.instance.drawImage(tex, 0, 0, iGUI.TOP | iGUI.LEFT);
        }
        

        

        iGUI.instance.setStringSize(25);
        iGUI.instance.drawString("" + (1 + frame), 25, 25, iGUI.VCENTER | iGUI.HCENTER);
        iGUI.instance.setRGBA(1, 1, 1, 1);
    }

    void cbAnim(object obj)
    {
        Player p = (Player)obj;
        p.be = (Behave)((int)p.be % 2);
        p.imgCurr = p.imgs[(int)p.be];
    }
    void cbAttAnim(object obj)
    {
        Player p = (Player)obj;
    }
#else
    void loadImage() 
    { 
        int i, j = (int)Behave.max;
		imgs = new iImage[j];
		for (i = 0; i < j; i++)
		{
			iImage img;
			if (i % 2 == 0)
			{
				img = new iImage();
				for (int k = 0; k < 3; k++)
				{
					iStrTex st = new iStrTex(methodStCreateImage, rect.size.width, rect.size.height);
					st.setString((i / 2) + "\n" + k);
					img.add(st.tex);
				}
			}
			else
			{
				img = imgs[i - 1].clone();
				img.leftRight = true;
			}

			if (i < (int)Behave.attLeft)
			{
				img.repeatNum = 0;
				img.startAnimation();
			}
			else
			{
				img.repeatNum = 1;
			}

			imgs[i] = img;
		}
		be = Behave.waitLeft;
		imgCurr = imgs[(int)be];

		// test
		//if (imgCurr.leftRight)
		//{
		//	be = Behave.waitRight;
		//}
		//be = Behave.attLeft;
		//imgs[(int)be].startAnimation(cbAnimation, this);
		//imgCurr = imgs[(int)be];
	}

	public void cbAnim(object obj)
	{
		Player p = (Player)obj;
		p.be = (Behave)((int)p.be % 2);
		p.imgCurr = p.imgs[(int)p.be];
	}


	void methodStCreateImage(iStrTex st)
	{
		string[] s = st.str.Split("\n");
		int be = int.Parse(s[0]);
		int frame = int.Parse(s[1]);

		if( be==0 )// wait
		{
			iGUI.instance.setRGBA(1, 1, 1, 1);
			iGUI.instance.fillRect(0, 0, rect.size.width, rect.size.height);
			iGUI.instance.setRGBA(1, 0, 0, 1);
			iGUI.instance.fillRect(5, 20, 10, 10);

			iGUI.instance.setStringRGBA(0, 0, 0, 1);
		}
		else if( be==1 )// att
		{
			iGUI.instance.setRGBA(0, 0, 1, 1);
			iGUI.instance.fillRect(0, 0, rect.size.width, rect.size.height);
			iGUI.instance.setRGBA(0, 0, 0, 1);
			iGUI.instance.fillRect(5, 20, 10, 10);

			iGUI.instance.setStringRGBA(1, 0, 0, 1);
		}

		iGUI.instance.setStringSize(25);
		iGUI.instance.drawString("" + (1 + frame), 25, 25, iGUI.VCENTER | iGUI.HCENTER);
		iGUI.instance.setRGBA(1, 1, 1, 1);
	}

#endif

	bool down;
    public Player()
    {
        maxHp = 100;
        hp = maxHp;
        position = new iPoint(0, 0);
        rect = new iRect(0, 0, 50, 50);
        v = new iPoint(0, 0);

        moveSpeed = 300;

        down = false;
        jumping = true;
        jumpForce = 0;
        preHeight = rect.size.height;
        downHeight = preHeight / 2;
        preY = rect.origin.y;


        MainCamera.methodKeyboard += keyboard;
		loadImage();
    }


    float t = 0;
    float cInterval = 2;
    public override void paint(float dt, iPoint off)
    {
		
		iPoint p = position + rect.origin + off;
		//iGUI.instance.fillRect(p.x, p.y, rect.size.width, rect.size.height);
		imgCurr.paint(dt, p);
        move(dt);

#if false
		if (v.x > 0)
			be = Behave.waitRight;
		else if (v.x < 0)
			be = Behave.waitLeft;
#else
		if( v.x != 0)
		{
			be = (Behave)((int)be / 2 * 2) + (v.x > 0 ? 1 : 0);
			imgCurr = imgs[(int)be];
		}

#endif
		Monster m = checkCollision(rect);
        t += dt;
        if(m != null)
        {
            if (t > cInterval)
            {
                if (hp > 0)
                    hp -= m.ap;
                t = 0;
            }
        }
    }



    private bool CheckKey(int key, iKeyboard ik)
    {
        int k = (int)ik;
        return (key & k) == k;
    }

    float preHeight;
    float downHeight;

    float preY;

    public void keyboard(iKeystate stat, int key)
    {
        //v = new iPoint(0, 0);
        if (stat == iKeystate.Moved)
        {
            if (CheckKey(key, iKeyboard.Left))
            {
                v.x = -1;
            }
            else if (CheckKey(key, iKeyboard.Right))
            {
                v.x = +1;
            }
			if (CheckKey(key, iKeyboard.Up))
				;
			else if (CheckKey(key, iKeyboard.Down))
			{
				if (!jumping)
				{
					down = true;
					rect.origin.y = downHeight;
					rect.size.height = downHeight;
				}
			}
            if (CheckKey(key, iKeyboard.alt))
            {
                if (!jumping && !down)
                {
                    jumping = true;
                    jumpForce = -700;
                }
            }
        }

        if (stat == iKeystate.Ended)
        {
            if (CheckKey(key, iKeyboard.Down))
            {
                down = false;
                rect.origin.y = preY;
                rect.size.height = preHeight;
            }
            v.x = 0;
            v.y = 0;

        }

        if (stat == iKeystate.Began)
        {
            if(CheckKey(key, iKeyboard.ctrl))
            {
				be = (Behave)( (int)Behave.walkLeft + (int)be %2 );
				imgCurr = imgs[(int)be];
				imgCurr.startAnimation(cbAnim, this);
			}
		}

        if (v.x != 0 || v.y != 0)
            v /= v.getLength();
    }
    public Monster checkCollision(iRect rt)
    {
        iRect src = rt;
        src.origin += position;

        iRect dst;
        for (int i = 0; i < Proc.monsterNum; i++)
        {
            Monster m = Proc.monster[i];
            dst = m.rect;
            dst.origin += m.position;

            if (dst.containRect(src))
            {
                return m;
            }
        }
        return null;
    }


}

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
public class Attack
{
	FObject att;
	iRect rt;
	iPoint pos;
	iPoint v;
	float lifeTime;
	int nHitChar;
	int nHIt;

	public Attack(FObject _att, iRect _rt)
	{
		att = _att;
		rt = _rt;
		pos = new iPoint(_att.rect.origin.x + _att.position.x + _att.rect.size.width, _att.rect.origin.y + _att.position.y);
	}

	void paint(float dt)
	{

	}
}