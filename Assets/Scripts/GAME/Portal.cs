using UnityEditor;
using UnityEngine;

using STD;

// 맵 이동 시스템
// 포탈은 index를 가지고 있음.
// 윗키 입력시 인덱스를 가져와서 Proc.f.reset(index)를 함.


// 포탈은 타일의 위에 붙어있음.

public class Portal
{
#if false
	int index;

	public Portal(int i)
	{
		index = i;
		iImage img;
		img = new iImage();
		for(int j =0; j<8; j++)
        {
			Texture tex = Resources.Load<Texture>("Portal/" + j);
			img.add(new iTexture(tex));
        }
	}
#else
	int index;
	iPoint pos;
	iRect rect;
	public int Index { get { return index; } set { index = value; } }
	public iPoint Pos { get { return pos; } set { pos = value - new iPoint(0, rect.size.height); } }
	public iRect Rect { get { return rect; } set { rect = value; } }

	iImage img;

	public Portal(int i, iPoint p)
    {
		rect = new iRect(0,0, 50, 50);
		pos = new iPoint(p.x, p.y - rect.size.height);
		loadImage();

		index = i;
    }
	
	void loadImage()
    {
		img = new iImage();
		for(int frame = 0; frame < 8; frame++)
        {
			iStrTex st = new iStrTex(methodStPt, 104, 142);
			st.setString(frame + "\n");
			img.add(st.tex);
        }
		img.repeatNum = 0;
		img._frameDt = 0.1f;
		img.startAnimation();
    }
	void methodStPt(iStrTex st)
    {
		string[] s = st.str.Split("\n");
		int frame = int.Parse(s[0]);
		Texture tex;

		tex = Resources.Load<Texture>("Portal/" + frame);
		iGUI.instance.setRGBAWhite();
		iGUI.instance.drawImage(tex, st.wid * 0.5f, st.hei + 5, iGUI.BOTTOM | iGUI.HCENTER);
    }

	public void paint(float dt, iPoint off)
    {
		iPoint p = pos + off + new iPoint(	rect.origin.x + (rect.size.width - img.listTex[0].tex.width) * 0.5f,
											rect.origin.y + rect.size.height - img.listTex[0].tex.height);
		iGUI.instance.setRGBAWhite();
		img.paint(dt, p);
    }
#endif
}