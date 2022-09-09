using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;

public class Proc : GObject
{
    public static Field f;
    Player p;

    public Proc()
    {
        f = new Field();
        p = new Player();
    }

    ~Proc()
    {

    }

    public override void draw(float dt)
    {
        f.paint(dt);
        p.paint(dt, f.off);
    }

    public override void key(iKeystate stat, iPoint point)
    {
// �����̵� playerPosition+ Rect > tilePosition
// �����̵� playerPosition < tilePosition + tileW
        // �̵�����
        //if(p.position.x + p.rect.size.width > )

        // ui
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
    public iPoint off;

    Color[] colorTile;

    public Field()
    {
        tileX = 10;
        tileY = 10;
        tileW = 70;
        tileH = 70;
        tiles = new int[100]
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 1, 1, 1, 1, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        };
        off = new iPoint(0, 0);

        colorTile = new Color[(int)TileAttr.Max]
        {
            Color.green, Color.red, Color.blue, Color.gray, Color.yellow
        };
    }

    public void paint(float dt)
    {
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

        MainCamera.methodKeyboard += keyboard;
    }

    public override void paint(float dt, iPoint off)
    {
        iGUI.instance.setRGBA(1, 1, 1, 1);
        iPoint p = position + rect.origin + off;
        iGUI.instance.fillRect(p.x, p.y, rect.size.width, rect.size.height);

        // draw
        if (v.x < 0)
        {
            int x = (int)(position.x + rect.origin.x);
            x /= Proc.f.tileW;
            int y = (int)(position.y + rect.origin.y);
            y /= Proc.f.tileH;
            float minX = 0;
            for(int i = x - 1; i > -1; i--)
            {
                bool check = false;
                for (int j = y; j < (position.y + rect.origin.y + rect.size.height)/ Proc.f.tileH; j++)
                {
                    if (Proc.f.tiles[Proc.f.tileX * j + i] == 1)
                    {
                        minX = Proc.f.tileW * (i + 1);
                        check = true;
                        break;
                    }
                }
                if (check)
                    break;  
            }
            position.x += v.x * moveSpeed * dt;
            if(position.x < minX)
            {
                position.x = minX;
            }
        }
        else if (v.x > 0)
        {
            int x = (int)(position.x + rect.origin.x + rect.size.width);
            x /= Proc.f.tileW;
            int y = (int)(position.y + rect.origin.y);
            y /= Proc.f.tileH;
            float maxX = Proc.f.tileX * Proc.f.tileW;
            for (int i = x + 1; i < Proc.f.tileX; i++)
            {
                bool check = false;
                for (int j = y; j < (position.y + rect.origin.y + rect.size.height) / Proc.f.tileH; j++)
                {
                    if (Proc.f.tiles[Proc.f.tileX * j + i] == 1)
                    {
                        maxX = Proc.f.tileW * i;
                        check = true;
                        break;
                    }
                }
                if (check)
                    break;
            }
            position.x += v.x * moveSpeed * dt;
            if(position.x + rect.origin.x + rect.size.width > maxX)
            {
                position.x = maxX - 1 - (rect.origin.x + rect.size.width);
            }
        }
        if (v.y < 0)
        {
            int x = (int)(position.x + rect.origin.x);
            x /= Proc.f.tileW;
            int y = (int)(position.y + rect.origin.y);
            y /= Proc.f.tileH;
            float minY = 0;
            for (int j = y - 1; j > -1; j--)
            {
                bool check = false;
                for (int i = x; i < (position.x + rect.origin.x + rect.size.width) / Proc.f.tileW; i++)
                {
                    if (Proc.f.tiles[Proc.f.tileX * j + i] == 1)
                    {
                        minY = Proc.f.tileH * (j + 1);
                        check = true;
                        break;
                    }
                }
                if (check)
                    break;
            }
            position.y += v.y * moveSpeed * dt;
            if (position.y < minY)
            {
                position.y = minY;
            }
        }
        else if (v.y > 0)
        {
            int x = (int)(position.x + rect.origin.x);
            x /= Proc.f.tileW;
            int y = (int)(position.y + rect.origin.y + rect.size.height);
            y /= Proc.f.tileH;
            float maxY = Proc.f.tileY * Proc.f.tileH;
            for (int j = y + 1; j < Proc.f.tileY; j++)
            {
                bool check = false;
                for (int i = x; i < (position.x + rect.origin.x + rect.size.width) / Proc.f.tileW; i++)
                {
                    if (Proc.f.tiles[Proc.f.tileX * j + i] == 1)
                    {
                        maxY = Proc.f.tileH * j;
                        check = true;
                        break;
                    }
                }
                if (check)
                    break;
            }
            position.y += v.y * moveSpeed * dt;
            if (position.y + rect.origin.y + rect.size.height > maxY)
            {
                position.y = maxY - 1 - (rect.origin.y + rect.size.height);
            }
        }
    }

    private bool CheckKey(int key, iKeyboard ik)
    {
        int k = (int)ik;
        return (key & k) == k;
    }

    public void keyboard(iKeystate stat, int key)
    {
        v = new iPoint(0, 0);

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