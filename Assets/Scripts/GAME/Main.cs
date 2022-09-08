using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;

public class Main
{
	public static Main me;

	GObject curr, next;

	public Main()
	{
		me = this;

		curr = createGameObject("Proc");
		next = null;
	}

	public void reset(string className)
    {
		next = createGameObject(className);

		// fade out-in animation

		GameObject.Destroy(curr.gameObject);
		curr = next;
    }

	GObject createGameObject(string className)
    {
		GameObject go = new GameObject(className);
		System.Type cls = System.Type.GetType(className);
		return (GObject)go.AddComponent(cls);
	}
}