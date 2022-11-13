using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;


public class Intro : GObject
{
    public override void load()
    {
        createPopGs();
        createPopInfo();
        popGs.show(true);
    }
    public override void draw(float dt)
    {
        Texture tex = Resources.Load<Texture>("IntroScreen");
        setRGBAWhite();
        drawImage(tex, MainCamera.devWidth / 2, MainCamera.devHeight/2, VCENTER | HCENTER);

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
                    Debug.Log("음");
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
        
        
        iImage img = new iImage();

        string[] strGsBtn = new string[]
        {
            "GameStart", "About the Game", "Info"
        };
        int btnLength = strGsBtn.Length;
        imgGsBtn = new iImage[btnLength];
        iStrTex stPop = new iStrTex();
        for (int i = 0; i < btnLength; i++)
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
        setRGBAWhite();
        drawImage(tex, 0, 0, 1.2f, 1.2f, VCENTER | HCENTER);

        setRGBA(0, 0, 0, 1);
        setLineWidth(5);
        drawRect(0, 0, st.wid+6, st.hei+6);

        setStringSize(30);
        setStringName("Maplestory Bold");
        setStringRGBA(0, 0, 0, 1);
        drawString(st.str, st.wid / 2, st.hei / 2, VCENTER | HCENTER);
    }
#endif
    // PopInfo ========================================================
    // 들어갈 정보(인적사항, 가능한 기술, 경력, 깃허브(QR))
    iPopup popInfo = null;
    iImage[] imgInfo;

    private void createPopInfo()
    {
        iPopup pop = new iPopup();
        iImage img = new iImage();

        imgInfo = new iImage[1];

        iStrTex st = new iStrTex(methodPopInfo, 800, 500);
        st.setString("0");
        img.add(st.tex);
        pop.add(img);
        pop.style = iPopupStyle.alpha;
        pop.openPoint = new iPoint((MainCamera.devWidth -st.wid) /2, (MainCamera.devHeight - st.hei) / 2);
        pop.closePoint = pop.openPoint;


        popInfo = pop;

    }

    private void methodPopInfo(iStrTex st)
    {
        setRGBAWhite();
        fillRect(0, 0, st.wid, st.hei);

        setStringSize(40);
        setStringName("Maplestory Light");
        setStringRGBA(0, 0, 0, 1);
        drawString("이름 : 이승찬", 10, 10);
        drawString("이메일 : gehfand729@gmail.com", 10, 60);

        setRGBAWhite();
        Texture tex = Resources.Load<Texture>("githubQR");
        float ratio = 0.5f;
        drawImage(tex, st.wid - tex.width * ratio - 10, 10, ratio, ratio, TOP | LEFT);
        drawString("Github", st.wid - tex.width * ratio - 10, 10 + tex.height * ratio + 10);

    }
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
                if (i == 0)
                    Main.me.reset("Proc");
                else if (i == 2)
                    popInfo.show(true);
                break;
        }
    }


    private void drawPop(float dt)
    {
        popGs.paint(dt);
        popInfo.paint(dt);
    }
}
