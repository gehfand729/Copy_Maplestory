using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;

public class Proc : GObject
{
    public static Field f;
    public Player p;

    public override void load()
    {
        f = new Field();
        p = new Player();

        createPopUI();
        popInfo.show(true);
        popMiniMap.show(true);
    }


    public override void draw(float dt)
    {
        f.paint(dt);
        p.paint(dt, f.off);
        drawPopUI(dt);
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
            if((key & (int)iKeyboard.a) == (int)(iKeyboard.a))
            {
                hp -= 10;
                mp -= 10;
            }
        }
        // f
        // p


    }


    iPopup popUI;
    void createPopUI()
    {
        createPopInven();
        createPopInfo();
        createPopMiniMap();
    }
    void drawPopUI(float dt)
    {
        drawPopInven(dt);
        drawPopInfo(dt);
        drawPopMiniMap(dt);
    }
    
    // Inventory ===============================================================
    iPopup popInven = null;
    iImage[] invenImg;
    iPoint offInven;
    Texture texInven;

    void createPopInven()
    {
        // inventory
        iPopup pop = new iPopup();
        iImage img = new iImage();
        texInven = Resources.Load<Texture>("inventory");
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
    int lv, hp, maxHp, minHp, mp, maxMp, minMp, _hp;
    iStrTex stInfo;
    void createPopInfo()
    {
        iPopup pop = new iPopup();
        iImage img = new iImage();
                                                
        lv = 0;
        hp = maxHp = 100;
        minHp = _hp = 0;
        mp = maxMp = 100;
        minMp = 0;


        stInfo = new iStrTex(methodStInfo, 250, 100);
        img.add(stInfo.tex);
        pop.add(img);

        pop.style = iPopupStyle.alpha;
        pop.openPoint = new iPoint((MainCamera.devWidth - 250) / 2, MainCamera.devHeight - 110);
        pop.closePoint = new iPoint((MainCamera.devWidth - 250) / 2, MainCamera.devHeight - 110);

        popInfo = pop;
    }
    void methodStInfo(iStrTex st)
    {
        Texture tex = Resources.Load<Texture>("bgInfo");
        drawImage(tex, 0, 0, 1.2f, 1.2f, TOP|LEFT, 2,0,REVERSE_NONE);

        string[] strs = st.str.Split("\n");
        int lv = int.Parse(strs[0]);
        float hp = int.Parse(strs[1]);
        float mp = int.Parse(strs[2]);

        if(hp < minHp)
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

#if true
        float rHp = hp / maxHp;
        float rMp = mp / maxMp;
        setRGBA(1, 0, 0, 1);
        fillRect(42, 26, 200 * rHp, 20);


        setRGBA(0, 0, 1, 1);
        fillRect(42, 50, 200 * rMp, 20);
        
#else
#endif

        setStringRGBA(0, 0, 0, 1);
        drawString("Lv." + lv, 2, 2, TOP | LEFT);

        drawString("Hp:" + hp + "/" + maxHp, 2, 26, TOP | LEFT);

        drawString("Mp:" + mp + "/" + maxMp, 2, 50, TOP | LEFT);

    }
    void drawPopInfo(float dt)
    {
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

        // 미니맵은 필드의 전체를 작게 그려놓은 것
        // 필드의 정보를 가져와야함.
        // 미니맵의 크기는 필드의 크기에 비례함.
        // 필드의 크기는 타일수 * 타일 길이
        // 플레이어 좌표의 구성
        //   position + off
        // 플레이어 rect 크기


#if true
        for(int i = 0; i < f.tileX * f.tileY; i++)
        {
            float x = miniTileW * (i % f.tileX);
            float y = miniTileH * (i / f.tileX);
            int t = f.tiles[i];
            Color c = f.colorTile[t];
            if(t != 0)
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


#else
#endif

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

}


enum TileAttr
{
    none = 0,
    cant,
    canUp,
    canLeft,
    canRight,

    Max
}

public static Texture CSTT(Sprite sprite)
{
    if(sprite.rect.width != sprite.texture.width)
    {
        int x = Mathf.FloorToInt(sprite.textureRect.x);
        int y = Mathf.FloorToInt(sprite.textureRext.y);
        int width = Mathf.FloorToInt(sprite.textureRect.width);
        int height = Mathf.FloorToInt(sprite.textureRect.height);
    }
}
// https://202psj.tistory.com/1358

public class Field
{
    public int tileX, tileY;
    public int tileW, tileH;
    public int[] tiles;
    public iPoint off, offMin, offMax;

    public Color[] colorTile;
    Texture fieldTex;

    public Field()
    {
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
            0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        };
        off = new iPoint(0, 0);
        offMin = new iPoint(MainCamera.devWidth- tileW*tileX, MainCamera.devHeight-tileH*tileY);
        offMax = new iPoint(0, 0);

        colorTile = new Color[(int)TileAttr.Max]
        {
            Color.green, Color.red, Color.blue, Color.gray, Color.yellow
        };
    }

    public void paint(float dt)
    {

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
            if(t == 1)
            {
                iGUI.instance.drawImage(fieldTex, x, y, iGUI.TOP | iGUI.LEFT);
            }
            Color c = colorTile[t];
            iGUI.instance.setRGBA(c.r, c.g, c.b, c.a);
            iGUI.instance.fillRect(x, y, tileW, tileH);
        }
        iGUI.instance.setRGBA(1, 1, 1, 1);
    }
}

public class FObject
{
    public iPoint position;
    public iRect rect;
    public iPoint v;
    public float moveSpeed;
    public float gravity;
    public bool jumping;
    public float jumpForce;
    
    public virtual void paint(float dt, iPoint off) { }
}

public class Player : FObject
{
    public Player()
    {
        position = new iPoint(0, 0);
        rect = new iRect(0, 0, 50, 50);
        v = new iPoint(0, 0);
        moveSpeed = 300;
        gravity = 2000;
        jumping = true;
        jumpForce = 0;
        preHeight = rect.size.height;
        downHeight = preHeight / 2;
        preY = rect.origin.y;


        MainCamera.methodKeyboard += keyboard;
    }

    public override void paint(float dt, iPoint off)
    {
        iGUI.instance.setRGBA(1, 1, 1, 1);
        iPoint p = position + rect.origin + off;
        iGUI.instance.fillRect(p.x, p.y, rect.size.width, rect.size.height);

#if true
        iPoint v = this.v * moveSpeed;
        jumpForce += gravity * dt;
        v.y += jumpForce;
        if(v.x < 0)
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
                    if ( n == 2 || n == 3) continue;
                 
                    minX = Proc.f.tileW * (i + 1);
                    check = true;
                    break;
                }
                if (check)
                    break;
            }
            xx += v.x * dt;
            if (xx < minX)
                xx = minX;
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
                xx = maxX;
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
            position.y = yy - rect.origin.y - rect.size.height;
        }
        
#endif
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
                v.x = -1;
            else if (CheckKey(key, iKeyboard.Right))
                v.x = +1;

            if (CheckKey(key, iKeyboard.Up))
                v.y = -1;
            else if (CheckKey(key, iKeyboard.Down))
            {
                rect.origin.y = downHeight;   
                rect.size.height = downHeight;
            }
        }

        if (stat == iKeystate.Ended)
        {
            if (CheckKey(key, iKeyboard.Down))
            {
                rect.origin.y = preY;
                rect.size.height = preHeight;
            }
            v.x = 0;
            v.y = 0;

        }

        if (stat == iKeystate.Began)
        {
            if (CheckKey(key, iKeyboard.alt))
            {
                jumping = true;
                jumpForce = -700;
            }
        }

        if (v.x != 0 || v.y != 0)
            v /= v.getLength();
    }
}

public class Monster : FObject
{
    public delegate void MethodAI(float dt);

    MethodAI methodAI = null;

    public Monster()
    {
        position = new iPoint(0, 0);
        rect = new iRect(0, 0, 60, 60);
        v = new iPoint(0, 0);

    }

    public override void paint(float dt, iPoint off)
    {
        // draw

        if( methodAI!=null )
            methodAI(dt);
    }
}