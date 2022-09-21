using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;
public class GObject:iGUI
{
    void Start()
    {
        init();

        texFbo = new RenderTexture(MainCamera.devWidth,
                                    MainCamera.devHeight, 32,
                                    RenderTextureFormat.ARGB32);

        Camera.onPreCull = onPreCull;
        Camera.onPreRender = onPrev;
        Camera.onPostRender = onEnd;

        load();
        MainCamera.methodMouse = new MethodMouse(key);

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

    int prevFrameCount = -1;
    int prevFramecountDelta = -1;

    float degree = 0.0f;
    void OnGUI()
    {
#if false// ù��°�� �׸��� ����(�� �����ӿ� �� 1�� �׸�)
		// #issue �ȱ׸�����, ȭ���� ���������� �� ��쵵�� �ϴ� ��� ��!!
		Camera.main.clearFlags = CameraClearFlags.Nothing;
		if (prevFrameCount == Time.frameCount)
		{
			Debug.Log("�׸� XXXXXXXXX");
			return;
		}
		Debug.Log("�׸� OOOOOOOO");
		prevFrameCount = Time.frameCount;
#else// ù��°�� �ȱ׸��� ����(�� �����ӿ� 1�� �̻� �׸�)
        if (prevFrameCount != Time.frameCount)
        {
            //Debug.Log("�׸� XXXXXXXXX");
            prevFrameCount = Time.frameCount;
            return;
        }
        //Debug.Log("�׸� OOOOOOOO");
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
        iStrTex.runSt();
        draw(delta);

        // 2. onEnd : c.targetTexture = backupRt;
        RenderTexture.active = texBack;
        //Camera.main.rect = rtBack;
        setProject();

#if true// rt : 0 ~ 2 : drawGui
        setRGBA(1, 1, 1, 1);
        drawImage(texFbo, 0, 0, TOP | LEFT);
#endif
    }

    public virtual void keyboard(iKeystate stat, int key) { }

    public virtual void load() { }
    public virtual void draw(float dt) { }
    public virtual void key(iKeystate stat, iPoint point) { }
}

