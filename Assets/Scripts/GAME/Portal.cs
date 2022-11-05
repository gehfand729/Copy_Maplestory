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
	public Portal(int i, iPoint p)
    {
		index = i;
		pos = p;
    }
	iImage img;
	
	public void paint(float dt, iPoint off)
    {
		iPoint p = pos + off;
    }
#endif
}