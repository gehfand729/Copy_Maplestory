using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;


public class Intro : GObject
{
    public override void load()
    {
        createPopGs();
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
        mousePopBtn(stat, point);
        //if(stat == iKeystate.Began)
        //{
        //    Main.me.reset("Proc");
        //}
    }

    // gameStart UI--------------------------------------------
    iPopup popGs;
    iImage[] imgGsBtn;
#if false
    private void createPopGs()
    {
        iPopup pop = new iPopup();

        imgGsBtn = new iImage[1];

        iImage img = new iImage();
        iStrTex st = new iStrTex(methodStGameStart, 100, 80);
        st.setString("start");
        img.add(st.tex);
        pop.add(img);
        img._selectDt = 0.1f;
        img.selectScale = -0.5f;
        imgGsBtn[0] = img;

        pop.style = iPopupStyle.alpha;
        pop.openPoint = new iPoint(MainCamera.devWidth / 2, MainCamera.devHeight / 2);
        pop.closePoint = pop.openPoint;
        popGs = pop;
    }
    void methodStGameStart(iStrTex st)
    {
        Texture tex = Resources.Load<Texture>("startBtn");
        drawImage(tex, 0, 0, 1.2f, 1.2f, TOP | LEFT, 2, 0, REVERSE_NONE);

        setStringSize(3);
        setStringRGBA(1, 0, 0, 1);
        drawString(st.str, 100 / 2, 80 / 2, VCENTER | HCENTER);
    }
    private void mousePopBtn(iKeystate stat, iPoint point)
    {
        iPopup pop = popGs;
        iImage[] imgBtn = imgGsBtn;

        int i, j = -1;

        switch( stat )
        {
            case iKeystate.Began:
                break;

            case iKeystate.Moved:
                for(i=0; i< imgBtn.Length; i++)
                {
                    if( imgBtn[i].touchRect(pop.closePoint, new iSize(0, 0)).containPoint(point) )
                    {
                        j = i;
                        break;
                    }
                }

                if( pop.selected != j )
                {
                    Debug.Log("À½");
                    if (pop.selected != -1)
                        imgBtn[pop.selected].select = false;

                    if (j != -1)
                        imgBtn[j].select = true;

                    pop.selected = j;
                }

                break;

            case iKeystate.Ended:
                break;
        }
    
#else
    private void createPopGs()
    {
        iPopup pop = new iPopup();
        
        imgGsBtn = new iImage[2];
        
        iImage img = new iImage();

        string[] strGsBtn = new string[]
        {
            "GameStart", "Info"
        };
        iStrTex stPop = new iStrTex();
        for (int i = 0; i < 2; i++)
        {
            img = new iImage();
            stPop = new iStrTex(methodPopGs, 300, 100);
            stPop.setString(strGsBtn[i]);
            img.position = new iPoint(0, (stPop.tex.tex.height + 20) * i);
            img.add(stPop.tex);
            pop.add(img);
            imgGsBtn[i] = img;
        }

        pop.style = iPopupStyle.alpha;
        pop.openPoint = new iPoint((MainCamera.devWidth - stPop.wid) / 2 , (MainCamera.devHeight - stPop.hei) / 2);
        pop.closePoint = pop.openPoint;


        popGs = pop;

    }

    private void methodPopGs(iStrTex st)
    {
        Texture tex = Resources.Load<Texture>("startBtn");
        setRGBA(1, 1, 1, 1);
        drawImage(tex, 0, 0, 1.2f, 1.2f, VCENTER | HCENTER);

        setRGBA(0, 0, 0, 1);
        setLineWidth(5);
        drawRect(0, 0, st.wid+6, st.hei+6);

        setStringSize(30);
        setStringRGBA(1, 0, 0, 1);
        drawString(st.str, st.wid / 2, st.hei / 2, VCENTER | HCENTER);
    }
#endif
    private void mousePopBtn(iKeystate stat, iPoint point)
    {
        iPopup pop = popGs;
        iImage[] imgBtn = imgGsBtn;

        int i, j = -1;

        switch (stat)
        {
            case iKeystate.Began:
                i = pop.selected;
                if (i == -1) break;
                imgBtn[i].select = true;
                break;
            case iKeystate.Moved:
                for(i = 0; i < imgBtn.Length; i++)
                {
                    if(imgBtn[i].touchRect(pop.closePoint, new iSize(0, 0)).containPoint(point))
                    {
                        j = i;
                        break;
                    }
                }

                if( pop.selected !=j )
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
                Main.me.reset("Proc");
                break;
        }
    }
    
    private void drawPop(float dt)
    {
        popGs.paint(dt);
    }
}
