using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

#if UNITY_STANDALONE_WIN
 
using System;
using System.Runtime.InteropServices;
 
#endif

[System.Serializable]
public class configInfor
{
	public string version;
	public int cycle_time;
	public float cycle_time_min;
	public float cycle_time_max;
	public float scale_min;
	public float scale_max;
	public float fall_speed_min;
	public float fall_speed_max;
	public float rot_speed_min;
	public float rot_speed_max;
	public float change_time;
	public float change_time_min, change_time_max;
	public float draw_range_top, draw_range_bottom;
	public float draw_range_left, draw_range_right;
	public float emit_y, fall_range;
	public float rare_pop_rate, rare_rot_speed;
	public float term;
	public string number_color;
	public float coefficient;

	public static configInfor CreateFromJSON(string jsonString)
	{
		return JsonUtility.FromJson<configInfor>(jsonString);
	}

}

public class main : MonoBehaviour
{
	#if UNITY_STANDALONE_WIN
	const int SWP_HIDEWINDOW = 0x80; //hide window flag.
	const int SWP_SHOWWINDOW = 0x40; //show window flag.
	const int SWP_NOMOVE = 0x0002; //don't move the window flag.
	const int SWP_NOSIZE = 0x0001; //don't resize the window flag.
	const uint WS_SIZEBOX = 0x00040000;
	const int GWL_STYLE = -16;
	const int WS_BORDER = 0x00800000; //window with border
	const int WS_DLGFRAME = 0x00400000; //window with double border but no title
	const int WS_CAPTION = WS_BORDER | WS_DLGFRAME; //window with a title bar
	const int WS_SYSMENU = 0x00080000;      //window with no borders etc.
	const int WS_MAXIMIZEBOX = 0x00010000;
	const int WS_MINIMIZEBOX = 0x00020000;  //window with minimizebox
	[StructLayout(LayoutKind.Sequential)]
	public struct RECT
	{
		public int Left, Top, Right, Bottom;
	}	
	[DllImport("user32.dll", EntryPoint = "SetWindowPos")] private static extern bool SetWindowPos(System.IntPtr hwnd, System.IntPtr hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
	[DllImport("user32.dll", EntryPoint = "GetWindowRect")] private static extern bool GetWindowRect(System.IntPtr hwnd, out RECT lpRect );
	[DllImport("user32.dll", EntryPoint = "FindWindow")] public static extern IntPtr FindWindow(System.String className, System.String windowName);
	[DllImport("user32.dll")]
	static extern System.IntPtr SetWindowLong(
		System.IntPtr hWnd, // window handle
		int nIndex,
		uint dwNewLong
	);

    [DllImport("user32.dll", EntryPoint="MoveWindow")]  
        static extern int  MoveWindow (System.IntPtr hwnd, int x, int y,int nWidth,int nHeight,int bRepaint );

	[DllImport("user32.dll")]
	static extern System.IntPtr GetWindowLong(
		System.IntPtr hWnd,
		int nIndex
	);

	System.IntPtr hWnd;
	System.IntPtr HWND_TOP = new System.IntPtr(0);
	System.IntPtr HWND_TOPMOST = new System.IntPtr(-1);
	System.IntPtr HWND_NOTOPMOST = new System.IntPtr(-2);

	#endif

	public GameObject myPrefab;

	int cycle_time = 30;
	float cycle_time_min = 1.0f;
	float cycle_time_max = 3.0f;
	float cycle_time_rate;
	float draw_range_right = 300, draw_range_bottom = 600;

	float time;

	bool invalid = true;

	string configureFile = System.IO.Path.GetFullPath("config.txt");
	// string licenseFile = System.IO.Path.GetFullPath("lisense.key");

	// Start is called before the first frame update
	void Start()
	{
		// #if UNITY_STANDALONE_WIN

		// int style = GetWindowLong(hWnd, GWL_STYLE).ToInt32(); //gets current style
		// SetWindowLong(FindWindow(null, Application.productName), GWL_STYLE, (uint)(style & ~(WS_CAPTION | WS_SIZEBOX))); //removes caption and the sizebox from current style.
		// SetWindowPos(FindWindow(null, Application.productName), HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_SHOWWINDOW); //Make the window render above toolbar.
		// // SetWindowPos(FindWindow(null, Application.productName), -1, 0, 0, 0, 0, 5);

		// #endif

		string config = "";//= textFile.text;
		byte[] license; //licenseKey.bytes;
		int expireTime = 0, utcTime = 0 ;

		config = File.ReadAllText(configureFile, Encoding.UTF8);
		// license = File.ReadAllBytes(licenseFile);

		configInfor ci = configInfor.CreateFromJSON(config);
//		Debug.Log(ci.ToString());
		draw_range_right = ci.draw_range_right;
		draw_range_bottom = ci.draw_range_bottom;
		Debug.Log(draw_range_right);
		Debug.Log(draw_range_bottom);
		cycle_time = ci.cycle_time;
		cycle_time_min = ci.cycle_time_min;
		cycle_time_max = ci.cycle_time_max;

		cycle_time_rate = (float)cycle_time;
		time = cycle_time;

		Screen.SetResolution((int)draw_range_right, (int)draw_range_bottom, false);
		// Screen.SetResolution(300, 900, false);

		// for (int i = 0; i < license.Length; i++)
		// {
		// 	expireTime = expireTime * 256 + license[i];
		// }

		// utcTime = (int)((System.DateTime.UtcNow - new System.DateTime(1970, 1, 1)).TotalSeconds);

		// Debug.Log("expire Time: = " + expireTime.ToString());
		// Debug.Log("UTC Time: = " + utcTime.ToString());

		// if (utcTime > expireTime)
		// {
		// 	invalid = false;
		// }

	}

	// Update is called once per frame
	void Update()
	{
		#if UNITY_STANDALONE_WIN

		RECT WinR;
		GetWindowRect( FindWindow(null, Application.productName), out WinR );
		if(WinR.Left != 0 || WinR.Top != 0) {
			int style = GetWindowLong(hWnd, GWL_STYLE).ToInt32(); //gets current style

			SetWindowLong(FindWindow(null, Application.productName), GWL_STYLE, (uint)(style & ~(WS_CAPTION | WS_SIZEBOX))); //removes caption and the sizebox from current style.
			// MoveWindow(FindWindow(null, Application.productName),0,0,Screen.width,Screen.height,1); // move the Unity Projet windows >>> 2000,0 Secondary monitor ;)
			SetWindowPos(FindWindow(null, Application.productName), HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_SHOWWINDOW); //Make the window render above toolbar.
		}

		#endif
		// Resolution res = Screen.currentResolution;
		// if(res.width != draw_range_right || res.height != draw_range_bottom)
		// {
		// 	Screen.SetResolution((int)draw_range_right, (int)draw_range_bottom, false);
		// }
		if (invalid == true)
		{
			time--;
			if (time <= 0)
			{
				float spawn_cycle_time = yuragiRand(cycle_time_min, cycle_time_max) * cycle_time_rate;
				time = spawn_cycle_time;
				//time = 500;
				Instantiate(myPrefab, Vector2.zero, Quaternion.identity);
			}
		}
	}

	int xRandomInt(float nMax, float nMin)
	{
		// nMinからnMaxまでのランダムな整数を返す (1単位)
		float nRandomInt = UnityEngine.Random.Range(nMin, nMax);
		//	trace("random:"+nRandomInt);
		return (int)nRandomInt;
	}

	float xRandom(float nMax, float nMin)
	{
		// nMinからnMaxまでのランダムな値を返す(0.1単位) 
		var nRandom = UnityEngine.Random.Range(nMin, nMax);
		//	trace("random:"+nRandom);
		return nRandom;
	}

	float xRandomS(float nMax, float nMin)
	{
		// nMinからnMaxまでのランダムな値を返す(0.01単位) 
		var nRandom = UnityEngine.Random.Range(nMin, nMax);
		//	trace("randomS:"+nRandom);
		return nRandom;
	}

	// ゆらぎを生成する関数(Java)
	// min: config.txtで指定された下限
	// max: config.txtで指定された上限
	float yuragiRand(float min, float max)
	{
		float yuragi = 0.0f;
		float randDouble = UnityEngine.Random.Range(0f, 1f); // 0.0以上1.0以下の乱数を生成

		if (randDouble < 0.5)
		{
			yuragi = randDouble + 2.0f * randDouble * randDouble;
		}
		else
		{
			yuragi = randDouble - 2.0f * (1.0f - randDouble) * (1.0f - randDouble);
		}

		float range = max - min;
		return min + (UnityEngine.Random.Range(0f, 1f) * range) * yuragi;
	}


	bool rareRand(float rate)
	{
		float nMax = 100f;
		float nMin = 0f;
		bool ret = false;
		//float nRand = Math.floor(Math.random() * ((nMax - nMin) + 1)) + nMin;
		float nRand = UnityEngine.Random.Range(nMin, nMax);
		if ((rate - nRand) >= 0)
		{
			ret = true; // rare hit!!!
		}
		return ret;
	}
}
