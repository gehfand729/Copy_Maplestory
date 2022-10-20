using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;
using System.Runtime.InteropServices;

public class Proc2 : GObject
{
    public override void load()
    {

    }
}

class field
{
    enum tileAttr
    {
        air = 0,
        normal,
        canUp,
        cantLeft,
        cantRight,

        Max
    }

    int tileX, tileY;
    float tileW, tileH;
    int[] tiles;
    Color[] tileColors;
    iPoint off;
    field()
    {
        tileX = 10;
        tileY = 10;
        tileW = 70;
        tileH = 70;
        tiles = new int[]
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        };
        tileColors = new Color[(int)tileAttr.Max]
        {
            Color.clear, Color.green, Color.white, Color.blue,Color.green
        };
        off = new iPoint(0, 0);
    }
    public void paint(float dt)
    {
        for(int i = 0; i< tiles.Length; i++)
        {
            
        }
    }
}

class FObject
{

}