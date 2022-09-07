using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;

public class Menu : GObject
{
    public override void load()
    {
    }

    public override void draw(float dt)
    {
        setRGBA(1, 1, 0, 1);
        fillRect(0, 0, MainCamera.devWidth, MainCamera.devHeight);
    }

    public override void key(iKeystate stat, iPoint point)
    {
        if (stat == iKeystate.Began)
        {
            Main.me.reset("Proc");
        }
    }
}