using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

//[System.Serializable]
//public class configInfor
//{
//    public string version;
//    public int cycle_time;
//    public float cycle_time_min;
//    public float cycle_time_max;
//    public float scale_min;
//    public float scale_max;
//    public float fall_speed_min;
//    public float fall_speed_max;
//    public float rot_speed_min;
//    public float rot_speed_max;
//    public float change_time;
//    public float change_time_min, change_time_max;
//    public float draw_range_top, draw_range_bottom;
//    public float draw_range_left, draw_range_right;
//    public float emit_y, fall_range;
//    public float rare_pop_rate, rare_rot_speed;
//    public float term;

//    public static configInfor CreateFromJSON(string jsonString)
//    {
//        return JsonUtility.FromJson<configInfor>(jsonString);
//    }

//}

public class SevenSeg : MonoBehaviour
{
	public GameObject textObj;
	public TextAsset textFile;
	int num = 0;
	int time = 30;
	int cycle_time = 30;

	float original_size = 787200.0f;

	float rotate_speed = 5.0f;
	float fall_speed;
	float fall_speed_min = 4.0f, fall_speed_max = 32.0f;
	float move_speed;

	int move_dir = -1;
	int num_dir = -1;       // -1:count down / 1:count up
	int rotate_dir = 1;

	float scale = 1;
	float scale_min = 1.0f;
	float scale_max = 3.0f;

	float change_time = 24;
	float change_time_min = 0.2f;
	float change_time_max = 27.0f;

	float draw_range_top = 100;
	float draw_range_bottom = 600;
	float draw_range_left = 0;
	float draw_range_right = 1080;
	float rot_speed_min = 3;
	float rot_speed_max = 60;
	float coefficient = 1.0f;
	float emit_y = 0;

	float rot = 90;

	float fall_range = 30;

	float fx, fy;

	string color_str;

	Color font_color = Color.green;

	string configureFile = System.IO.Path.GetFullPath("config.txt");
	// Start is called before the first frame update
	void Start()
	{
		string config = File.ReadAllText(configureFile, Encoding.UTF8);
		configInfor ci = configInfor.CreateFromJSON(config);

		scale_min = ci.scale_min; scale_max = ci.scale_max;
		cycle_time = ci.cycle_time;
		draw_range_top = ci.draw_range_top;
		draw_range_bottom = ci.draw_range_bottom;
		draw_range_left = ci.draw_range_left;
		draw_range_right = ci.draw_range_right;
		fall_range = ci.fall_range;
		fall_speed_min = ci.fall_speed_min;
		fall_speed_max = ci.fall_speed_max;
		change_time_min = ci.change_time_min;
		change_time_max = ci.change_time_max;
		color_str = ci.number_color;
		rot_speed_min = ci.rot_speed_min * 20;
		rot_speed_max = ci.rot_speed_max * 20;
		emit_y = ci.emit_y;
		// coefficient = ci.coefficient;

		string[] color_arr =  color_str.Split(char.Parse(","));

		if(color_arr.Length == 3) {
			font_color = new Color(System.Single.Parse(color_arr[0])/255.0f,System.Single.Parse(color_arr[1])/255.0f,System.Single.Parse(color_arr[2])/255.0f);
		}

		cycle_time = (int)(Random.Range(change_time_min, change_time_max) * 24.0f);
		// ColorUtility.TryParseHtmlString(color_str, out font_color);

		// float new_size = draw_range_bottom * draw_range_right;
		// float magnification = Mathf.Sqrt(new_size / original_size);
		// float scale_min_to_use = scale_min * magnification;
		// float scale_max_to_use = scale_max * magnification;

		scale = Random.Range(scale_min, scale_max);
		// int font_size = (int)(500.0f * scale);
		// textObj.GetComponent<UnityEngine.UI.Text>().fontSize = font_size;
		textObj.GetComponent<UnityEngine.UI.Text>().rectTransform.localScale = new Vector3(scale,scale,1f);
		textObj.GetComponent<UnityEngine.UI.Text>().color = font_color;
		fx = Random.Range(1, 9) * draw_range_right / 9;
		fy = draw_range_bottom-emit_y;
		rot = 90 - (fall_range / 2) + (float)yuragiRand(0, fall_range);
		// textObj.transform.position = new Vector2(0, fy);
		// textObj.transform.position = new Vector2(Random.Range(0, draw_range_right), fy);
		textObj.transform.position = new Vector2(fx, fy);
		textObj.transform.Rotate(0, 0, rot * Mathf.Deg2Rad, Space.Self);
		// fall_speed = 500;
		num = xRandomInt(1.0f, 9.0f);
		textObj.GetComponent<UnityEngine.UI.Text>().text = num.ToString();
		// Debug.Log(num);
		fall_speed = yuragiRand(fall_speed_min, fall_speed_max) * 25;
		int _num_dir = Random.Range(0, 2);
		rotate_dir = xRandomInt(0.0f, 9.0f);
		// int flg = (int)_num_dir % 2;
		if (_num_dir == 1)
		{
			move_dir = -1;
			// num_dir = -1;
		}
		else
		{
			move_dir = 1;
			// num_dir = 1;
		}
		// move_dir = 1;
		rotate_speed = Random.Range(rot_speed_min, rot_speed_max);

		int flg = (int)rotate_dir % 2;
		if (flg == 1)
		{
			rotate_dir = -1;
		}
		else
		{
			rotate_dir = 1;
		}
		move_speed = yuragiRand(1.0f, 8.0f);
	}

	// Update is called once per frame
	void Update()
	{

		fx = textObj.transform.position.x;
		fy = textObj.transform.position.y;

		float deltaRot = (float)rotate_dir * rotate_speed;

		//rot += deltaRot;

		if (rot > 90.0f)
		{
			fx += (Time.deltaTime * fall_speed) * Mathf.Cos(rot * Mathf.Deg2Rad) * (float)(-move_dir);
		}
		else
		{
			fx += (Time.deltaTime * fall_speed) * Mathf.Cos(rot * Mathf.Deg2Rad) * (float)move_dir;
		}
		fy -= (Time.deltaTime * fall_speed) * Mathf.Sin(rot * Mathf.Deg2Rad);

		if (fx > draw_range_right)
		{
			move_dir = -1;
		}
		if (fx < draw_range_left)
		{
			move_dir = 1;
		}

		if (fy < 0)
		{
			Destroy(gameObject);
		}

		RectTransform rt = (RectTransform)textObj.transform;

		float check_left = fx - (rt.rect.width * scale / 2);
		float check_right = fx + (rt.rect.width * scale / 2);

		if (check_left <= draw_range_left)
		{
			move_dir = 1;
		}
		if (check_right >= draw_range_right)
		{
			move_dir = -1;
		}

		textObj.transform.position = new Vector2(fx, fy);
		textObj.transform.Rotate(0, 0, deltaRot * Mathf.Deg2Rad, Space.Self);

		time--;
		//textObj.transform.position = new Vector2(Random.Range(0, 1000), Random.Range(0, 500));
		//textObj.GetComponent<UnityEngine.UI.Text>().text = yy.ToString();
		if (time < 0)
		{
			num += (num_dir == 1 ? 1 : 9);
			num %= 10;
			// if (num > 9) num = 1;
			// if (num < 1) num = 9;
			time = cycle_time;
			textObj.GetComponent<UnityEngine.UI.Text>().text = num.ToString();
			//Destroy(gameObject);
		}
	}
	int xRandomInt(float nMax, float nMin)
	{
		// nMinからnMaxまでのランダムな整数を返す (1単位)
		float nRandomInt = Random.Range(nMin, nMax);
		//	trace("random:"+nRandomInt);
		return (int)nRandomInt;
	}

	float xRandom(float nMax, float nMin)
	{
		// nMinからnMaxまでのランダムな値を返す(0.1単位) 
		var nRandom = Random.Range(nMin, nMax);
		//	trace("random:"+nRandom);
		return nRandom;
	}

	float xRandomS(float nMax, float nMin)
	{
		// nMinからnMaxまでのランダムな値を返す(0.01単位) 
		var nRandom = Random.Range(nMin, nMax);
		//	trace("randomS:"+nRandom);
		return nRandom;
	}

	// ゆらぎを生成する関数(Java)
	// min: config.txtで指定された下限
	// max: config.txtで指定された上限
	float yuragiRand(float min, float max)
	{
		float yuragi = 0.0f;
		float randDouble = Random.Range(0f, 1f); // 0.0以上1.0以下の乱数を生成

		if (randDouble < 0.5)
		{
			yuragi = randDouble + 2.0f * randDouble * randDouble;
		}
		else
		{
			yuragi = randDouble - 2.0f * (1.0f - randDouble) * (1.0f - randDouble);
		}

		float range = max - min;
		return min + (Random.Range(0f, 1f) * range) * yuragi;
	}


	bool rareRand(float rate)
	{
		float nMax = 100f;
		float nMin = 0f;
		bool ret = false;
		//float nRand = Math.floor(Math.random() * ((nMax - nMin) + 1)) + nMin;
		float nRand = Random.Range(nMin, nMax);
		if ((rate - nRand) >= 0)
		{
			ret = true; // rare hit!!!
		}
		return ret;
	}

}
