using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;

public class Main : iGUI
{
	public static GameObject createGameObject(string name)
	{
		GameObject go = new GameObject(name);
		go.AddComponent<Main>();

		return go;
	}

	void Start()
	{
		init();

		texFbo = new RenderTexture(MainCamera.devWidth,
									MainCamera.devHeight, 32,
									RenderTextureFormat.ARGB32);

		Camera.onPreCull = onPreCull;
		Camera.onPreRender = onPrev;
		Camera.onPostRender = onEnd;

		loadGame();
		MainCamera.methodMouse = new MethodMouse(keyGame);
	}
	//void Update() { }

	RenderTexture texFbo;
	RenderTexture texBack;
	Rect rtBack;

	public void onPrev(Camera c)
	{
		texBack = c.targetTexture;
		c.targetTexture = texFbo;

		rtBack = Camera.main.rect;
		Camera.main.rect = new Rect(0, 0, 1, 1);
	}
	// void OnRenderObject(){}
	public void onEnd(Camera c)
	{
		c.targetTexture = texBack;

		Camera.main.rect = rtBack;
	}

	void onPreCull(Camera c)
	{
		preCull();
	}

	void OnPreCull()
	{
		preCull();
	}

	void Update()
	{
		// keyboard
	}

	int prevFrameCount = -1;
	int prevFramecountDelta = -1;

	float degree = 0.0f;
	void OnGUI()
	{
#if false// 첫번째만 그리는 로직(한 프레임에 딱 1번 그림)
		// #issue 안그릴때는, 화면을 검은색으로 안 비우도록 하는 방법 모름!!
		Camera.main.clearFlags = CameraClearFlags.Nothing;
		if (prevFrameCount == Time.frameCount)
		{
			Debug.Log("그림 XXXXXXXXX");
			return;
		}
		Debug.Log("그림 OOOOOOOO");
		prevFrameCount = Time.frameCount;
#else// 첫번째만 안그리는 로직(한 프레임에 1번 이상 그림)
		if (prevFrameCount != Time.frameCount)
		{
			//Debug.Log("그림 XXXXXXXXX");
			prevFrameCount = Time.frameCount;
			return;
		}
		//Debug.Log("그림 OOOOOOOO");
#endif

		float delta = 0.0f;
		if (prevFramecountDelta != Time.frameCount)
		{
			prevFramecountDelta = Time.frameCount;
			delta = Time.deltaTime;
		}

#if true// rt : onPrev() ~ onEnd()
		GL.Clear(true, true, Color.clear);
		setProject();
		setRGBA(1, 1, 1, 1);
		drawImage(texFbo, 0, 0, TOP | LEFT);
		degree += 360 * Time.deltaTime;
#endif

		// 0. onPrev : c.targetTexture = rt;
		texBack = RenderTexture.active;
		RenderTexture.active = texFbo;
		//rtBack = Camera.main.rect;
		//Camera.main.rect = new Rect(0, 0, 1, 1);
		GUI.matrix = Matrix4x4.TRS(
			Vector3.zero, Quaternion.identity, new Vector3(1, 1, 1));

		// 1. OnRenderObject
		GL.Clear(true, true, Color.clear);
		drawGame(delta);

		// 2. onEnd : c.targetTexture = backupRt;
		RenderTexture.active = texBack;
		//Camera.main.rect = rtBack;
		setProject();

#if true// rt : 0 ~ 2 : drawGui
		setRGBA(1, 1, 1, 1);
		drawImage(texFbo, 0, 0, TOP | LEFT);
#endif
	}

	public static GObject curr, next;
	void loadGame()
    {
		setStringName("BM-JUA");
		next = null;
#if false
		curr = new Intro();
#else
		curr = new Proc();
#endif
	}

	
	//void freeGame()
    //{
	//
    //}

	void drawGame(float dt)
	{
		curr.draw(dt);

		if (next != null)
		{
			curr = next;
			next = null;
		}
	}

	void keyGame(iKeystate stat, iPoint point)
    {
		curr.key(stat, point);
    }

#if false
	iStrTex st2 = null;
	void methodSt2(iStrTex st)
	{
		RenderTexture bkT = RenderTexture.active;
		RenderTexture.active = (RenderTexture)st.tex.tex;
		//Rect bkR = Camera.main.rect;
		//Camera.main.rect = new Rect(0, 0, 1, 1);
		Matrix4x4 matrixBk = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(
			Vector3.zero, Quaternion.identity, new Vector3(1, 1, 1));

		GL.Clear(true, true, Color.clear);// add

		string sn = getStringName();
		float ss = getStringSize();
		Color sc = getStringRGBA();
		setStringName("BM-JUA");
		setStringSize(100);
		setStringRGBA(0, 0, 1, 1);

		iSize size = new iSize(st.wid, st.hei);

		setRGBA(0, 0, 0, 0.5f);
		fillRect(0, 0, size.width, size.height);

		setLineWidth(3);
		setRGBA(1, 1, 1, 1);
		drawRect(1, 1, size.width-2, size.height-2);

		drawString(st.str, size.width/2, size.height/2, VCENTER|HCENTER);

		setStringName(sn);
		setStringSize(ss);
		setStringRGBA(sc.r, sc.g, sc.b, sc.a);


		setRGBA(1, 1, 1, 1);
		Texture t = Resources.Load<Texture>("sns0");
		drawImage(t, 0, 0, TOP | LEFT);

		t = Resources.Load<Texture>("sns1");
		drawImage(t, size.width, 0, TOP | RIGHT);

		RenderTexture.active = bkT;
		//Camera.main.rect = bkR;
		GUI.matrix = matrixBk;
	}
#endif


}