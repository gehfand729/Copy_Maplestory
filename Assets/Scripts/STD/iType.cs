using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STD
{
	public enum iKeystate
	{
		Began = 0,	// pressed
		Moved,		// moved
		Ended,		// released
		Double,
	};

	public delegate void MethodMouse(iKeystate stat, iPoint point);
	public delegate void MethodWheel(iPoint wheel);

	public enum iKeyboard
	{
		Left = 1,// a, A, 4, <-
		Right = 2,
		Up = 4,
		Down = 8,
		alt = 16,
		i = 32,
		a = 64,
		esc = 128,
		ctrl = 256,
	};

	public delegate void MethodKeyboard(iKeystate stat, int key);

}

