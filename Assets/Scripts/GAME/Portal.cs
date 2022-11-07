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
	public iPoint Pos { get { return pos; } set { pos = value; } }
	public iRect Rect { get { return rect; } set { rect = value; } }

	iImage img;

	public Portal(int i, iPoint p)
    {
		pos = p;
		rect = new iRect(0,0,104,142);

		loadImage();
		index = i;
    }
	
	void loadImage()
    {
		img = new iImage();
		for(int frame = 0; frame < 8; frame++)
        {
			iStrTex st = new iStrTex(methodStPt, rect.size.width, rect.size.height);
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
		iGUI.instance.setRGBA(1, 1, 1, 1);
		iGUI.instance.drawImage(tex, 1.0f * (rect.size.width -tex.width)/2, rect.size.height - tex.height, iGUI.TOP | iGUI.LEFT);
    }

	public void paint(float dt, iPoint off)
    {
		iPoint p = pos +  off;
		img.paint(dt, p);
    }
#endif
}