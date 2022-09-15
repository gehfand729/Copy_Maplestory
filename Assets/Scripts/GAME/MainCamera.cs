using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;

public class MainCamera : MonoBehaviour
{
	// 개발 해상도(1080p, 720p)
	// (ipad) 1024 x 768
	// (iphone) 2.1:1
	// (android) 12:9, 2:1
	// (pc) 16:9
	public static int devWidth = 1280, devHeight = 720;

	// 16:9 개발
	// 액자처리

	// 최적화 16:9 대응 4:3, 아이폰 2.1:1
	// 4:3 ~ 2.1:1

	public static MethodMouse methodMouse = null;
	bool drag;
	Vector3 prevV;

	public static MethodWheel methodWheel = null;

	public static MethodKeyboard methodKeyboard = null;

	void Start()
    {
		drag = false;

		initGameHierachy();

		Fbo.createGameObject("Fbo");
	}

	void Update()
	{
		iGUI.setResolution(devWidth, devHeight);

#if false
		int btn = 0;// 0:left, 1:right, 2:wheel, 3foward, 4back
		if (Input.GetMouseButtonDown(btn))
		{
			iPoint p = mousePosition();
            //Debug.LogFormat($"Began p({p.x},{p.y})");
            drag = true;
			prevV = Input.mousePosition;// 누르자 말자 Moved 안들어오게 방지

			if (methodMouse != null)
				methodMouse(iKeystate.Began, p);
		}
		else if (Input.GetMouseButtonUp(btn))
		{
			iPoint p = mousePosition();
			//Debug.LogFormat($"Ended p({p.x},{p.y})");
			drag = false;

			if (methodMouse != null)
				methodMouse(iKeystate.Ended, p);
		}

		//if (drag)
		{
			Vector3 v = Input.mousePosition;
			if (prevV == v)
				return;
			prevV = v;

			iPoint p = mousePosition();
			//Debug.LogFormat($"Moved p({p.x},{p.y})");

			if (methodMouse != null)
				methodMouse(iKeystate.Moved, p);
		}

		if( Input.mouseScrollDelta!=Vector2.zero )
		{
			if (methodWheel != null)
			{
				methodWheel(new iPoint(	Input.mouseScrollDelta.x,
										Input.mouseScrollDelta.y));
			}
		}
#endif
        // keyboard

#if false
		for(int i = 0; i < kc.Length; i++)
        {
			if (Input.GetKeyDown(kc[i]))
			{
				methodKeyboard(iKeystate.Began, (iKeyboard)(iKeyboard.Left+i));
				keyboard |= (1 + i);
			}
			else if (Input.GetKeyUp(kc[i]))
			{
				methodKeyboard(iKeystate.Ended, (iKeyboard)(iKeyboard.Left + i));
				keyboard &= ~(1 + i);
			}
			else if ( (keyboard & (1 + i)) == (1 + i))
			{
				methodKeyboard(iKeystate.Moved, (iKeyboard)(iKeyboard.Left + i));
			}
        }
#else
        int keyDown = 0;
        int keyUp = 0;

		for (int i = 0; i < kc.Length; i++)
		{
			if (Input.GetKeyDown(kc[i]))
			{
				int n = (int)Mathf.Pow(2, i);
				keyboard |= n;
				keyDown |= n;
			}
			else if (Input.GetKeyUp(kc[i]))
			{
				int n = (int)Mathf.Pow(2, i);
				keyboard &= ~n;
				keyUp |= n;
			}
		}
		if (keyDown != 0)
		{
			methodKeyboard(iKeystate.Began, keyDown);
		}
		if(keyboard != 0)
		{
            methodKeyboard(iKeystate.Moved, keyboard);
        }
		if(keyUp != 0)
		{
            methodKeyboard(iKeystate.Ended, keyUp);
        }
        
#endif

		drawGameHierachy();
	}

	int keyboard = 0;
	KeyCode[] kc = new KeyCode[] {
			KeyCode.LeftArrow, KeyCode.RightArrow,
			KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.Space,
			KeyCode.I,
		};

	public static Vector3 iPointToVector3(iPoint p)
	{
		return new Vector3(p.x - devWidth / 2, devHeight / 2 - p.y, 0f);
	}

	public static iPoint vector3ToiPoint(Vector3 v)
	{
		return new iPoint(v.x + devWidth / 2, devHeight / 2 - v.y);
	}

	public static iPoint mousePosition()
	{
		int sw = Screen.width, sh = Screen.height;
		float vx = Camera.main.rect.x * sw;
		float vy = Camera.main.rect.y * sh;
		float vw = Camera.main.rect.width * sw;
		float vh = Camera.main.rect.height * sh;

		Vector3 v = Input.mousePosition;
		iPoint p = new iPoint(	(v.x - vx) / vw * devWidth,
								(1f - (v.y - vy) / vh) * devHeight);
		//Debug.LogFormat($"screen({sw},{sh}) : input({v.x},{v.y}) => use({p.x},{p.y})");
		return p;
	}

	// ===========================================================
	// Game
	// ===========================================================
	public static MainCamera mc;

	void initGameHierachy()
	{
		mc = this;
	}
	void drawGameHierachy()
	{
	}
}

