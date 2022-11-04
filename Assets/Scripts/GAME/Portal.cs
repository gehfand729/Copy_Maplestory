using UnityEditor;
using UnityEngine;

using STD;

// 맵 이동 시스템
// 포탈은 index를 가지고 있음.
// 윗키 입력시 인덱스를 가져와서 Proc.f.reset(index)를 함.


// 포탈은 타일의 위에 붙어있음.

public class Portal
{
	iPoint pos;
	iRect rect;
	int index;
	
	public Portal(int i, iPoint p)
	{
		index = i;

		pos = p;
		rect = new iRect();
	}

}