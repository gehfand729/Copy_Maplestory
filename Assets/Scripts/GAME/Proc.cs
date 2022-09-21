using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;

public class Proc : GObject
{
    public static Field f;
    public Player p;

    public Proc()
    {
        f = new Field();
        p = new Player();
        createPopUI();
        popUI.show(true);
    }

    ~Proc()
    {
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
        if (keyPopUI(stat, point))
            return;

        // f
        // p

        
    }

    iPopup popUI;
    void createPopUI()
    {
        iPopup pop = new iPopup();

        iImage img = new iImage();
        for(int i=0; i<4; i++)
        {
            iTexture tex = new iTexture(Resources.Load<Texture>("0_0_" + i));
            img.add(tex);
        }
        img.repeatNum = 0;
        img._frameDt = 0.2f;
        img.startAnimation();
        pop.add(img);
        
        pop.style = iPopupStyle.zoomRotate;
        pop.openPoint = new iPoint(0, 0);
        pop.closePoint = new iPoint(300, 300);
        popUI = pop;
    }

    void freePopUI()
    {

    }

    void drawPopUI(float dt)
    {
        popUI.paint(dt);
    }
    bool keyPopUI(iKeystate stat, iPoint point)
    {
        return false;
    }
}

public class UI
{
    iStrTex tex;
    public void paint(float dt)
    {
        iStrTex.runSt();
        iGUI.instance.setRGBA(1, 1, 1, 1);

        iGUI.instance.setStringName("BM-JUA");
        iGUI.instance.setStringSize(50);
        iGUI.instance.setStringRGBA(1, 1, 0, 1);
        iGUI.instance.drawString("Hello",MainCamera.devWidth / 2, MainCamera.devHeight / 2, iGUI.VCENTER | iGUI.HCENTER);

        if (tex == null)
            tex = new iStrTex(methodSt, 100, 120);
        tex.drawString("Hello", MainCamera.devWidth / 2, MainCamera.devHeight / 2, iGUI.VCENTER | iGUI.HCENTER);
        

    }
    void methodSt(iStrTex st)
    {
        Texture t = Resources.Load<Texture>("Map");
        iGUI.instance.drawImage(t, 0, 0, iGUI.TOP | iGUI.LEFT);
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

public class Field
{
    public int tileX, tileY;
    public int tileW, tileH;
    public int[] tiles;
    public iPoint off, offMin, offMax;

    Color[] colorTile;

    public Field()
    {
        tileX = 30;
        tileY = 19;
        tileW = 70;
        tileH = 70;
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
                v.y = +1;
        }

        if (stat == iKeystate.Ended)
        {
            v.x = 0;
            v.y = 0;
        }

        if (stat == iKeystate.Began)
        {
            if (CheckKey(key, iKeyboard.Space))
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

    public override void paint(float dt, iPoint off)
    {
        // draw

        if( methodAI!=null )
            methodAI(dt);
    }
}