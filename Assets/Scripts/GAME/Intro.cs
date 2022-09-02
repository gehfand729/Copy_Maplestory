using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;

public class GObject
{
    public GObject()
    {

    }

    ~GObject()
    {

    }

    public virtual void draw(float dt) { }
    public virtual void key(iKeystate stat, iPoint point) { }
}

public class Intro : GObject
{
    public Intro()
    {

    }

    ~Intro()
    {

    }

    public override void draw(float dt)
    {

    }

    public override void key(iKeystate stat, iPoint point)
    {

    }
}