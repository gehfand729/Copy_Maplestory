using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;


public class Intro : GObject
{
    public override void load()
    {
        createPop();
        popGs.show(true);
    }
    public override void draw(float dt)
    {
        Texture tex = Resources.Load<Texture>("IntroScreen");
        float sW, sH;
        sW = 1.0f * MainCamera.devWidth / tex.width;
        sH = 1.0f * MainCamera.devHeight / tex.height;
        setRGBA(1, 1, 1, 1);
        drawImage(tex, 0, 0, sW, sH, TOP | LEFT);

        drawPop(dt);
    }

    public override void key(iKeystate stat, iPoint point)
    {
        //if(stat == iKeystate.Began)
        //{
        //    Main.me.reset("Proc");
        //}
    }

    // gameStart UI--------------------------------------------
    iPopup popGs;
    iImage popimg;
    iStrTex stTex;
    string start;
    private void createPop()
    {
        iPopup pop = new iPopup();
        iImage img = new iImage();

        start = "start";

        stTex = new iStrTex(methodStGameStart, 100, 80);
        stTex.setString(start);

        img.add(stTex.tex);
        pop.add(img);

        pop.openPoint = new iPoint(MainCamera.devWidth / 2, MainCamera.devHeight / 2);
        pop.closePoint = pop.openPoint;
        popGs = pop;
        popimg = img;

        MainCamera.methodMouse += mousePopBtn;
    }
    void methodStGameStart(iStrTex st)
    {
        
        Texture tex = Resources.Load<Texture>("startBtn");
        drawImage(tex, 0, 0, 1.2f, 1.2f, TOP | LEFT, 2, 0, REVERSE_NONE);

        string[] strs = st.str.Split("\n");
        string start = strs[0];

        
    }
    private void drawPop(float dt)
    {
        popGs.paint(dt);
    }
    private void mousePopBtn(iKeystate stat, iPoint point)
    {
        iPopup pop = popGs;
        iImage imgBtn = popimg;

        if (imgBtn.touchRect(pop.openPoint, new iSize(0, 0)).containPoint(point))
        {
            if (stat == iKeystate.Began)
            {
                imgBtn.scale = 0.9f;
            }
            else if (stat == iKeystate.Moved)
            {

            }
            else if (stat == iKeystate.Ended)
            {
                imgBtn.scale = 1.0f;
                Main.me.reset("Proc");
            }
        }
        else
        {
            if (stat == iKeystate.Ended)
            {
                imgBtn.scale = 1.0f;
            }
        }
    }
}
