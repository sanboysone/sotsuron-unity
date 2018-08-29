using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBoard : MonoBehaviour
{
	//接続パスワード
	private string password = "sanboysone";
	
	//ユーザー情報
	private string student_year;
	private string student_class;
	private string student_number;
	private string student_id;
	private string student_name;
	
	//ボードテキスト
	public Text schoolName;
	public Text UserInformation;
	public Text ErrorText;
	
	//学校名
	private string schoolname;

	//接続情報
	private string url;
	private string code;
	private string code2;

	//エラー
	private bool urlError = false;
	private bool timeOutError = false;
	private bool noquestion = false;

	//php格納
	private string status;
	private string status2;
	
	//問題格納用配列
	public static question_data[] qd;

	//開く問題のIDを入れる
	public static int openID;
	//開く問題のタイプを入れる
	public static string openType;
	
	//区切り文字
	string[] splitter = {"¥@¥"};
	//問題の数
	private string[] arr;
	private string[] questionsplit;
	private int count;
	
	//ボタンの設定
	private int[] buttonNumber = new int[4];

	public Text one;
	public Text two;
	public Text three;
	public Text four;
	
	//現在のページ数
	private int pages = 1;
	
	//区切り変数
	//string[] arr =  status.Split(splitter, StringSplitOptions.None);

	
	// Use this for initialization
	void Start ()
	{
		//生徒情報
		student_year   = login.student_year;
		student_class  = login.student_class;
		student_number = login.student_number;
		student_id     = login.student_id;
		student_name   = login.student_name;

		//接続情報と学校名
		url = SceneSelect.UrlString;
		schoolname = SceneSelect.schoolname;

		code = url + "/unity/numquestion.php";

		StartCoroutine("Access");   //Accessコルーチンの開始
		StartCoroutine(DelayMethod(4, () =>
		{
			Debug.Log("status = " + status);
			arr =  status.Split(splitter, StringSplitOptions.None);
			count = arr.Length;

			if (count != 0)
			{
				code2 = url + "/unity/sendquestion.php";
				qd = new question_data[count];

				/*
				for (int i = 0; i < count; i++)
				{
					qd[i] = new question_data();
				}
				*/
				
				
			}
			else
			{
				noquestion = true;
			}
			//ここから問題の検索をする　/unity/sendquestion.php
			Debug.Log("qd data = " + qd.Length);
			Debug.Log("count =" + count);
			if (noquestion == false)
			{
				for (int i = 0; i < count; i++)
				{
					
					StartCoroutine(Access2(arr[i], i));   //Accessコルーチンの開始
					//このコルーチン処理に入る前にfor文が終わり、iが最大値+1になってるからout of rangeが出る
					//これをなんとかする
				}
				
				StartCoroutine(DelayMethod(6, () =>
				{
					//ボタンの番号を割り振る
					//数が４より少ない場合は-1とし、-1は「問題がありません」と表示し、ボタンの反応を無しにする
					if (count < 4)
					{
						int j = 0;
						for (int i = 0; i < count; i++, j++)
						{
							buttonNumber[i] = i;
						}

						while (j < 4)
						{
							buttonNumber[j] = -1;
							j++;
						}
					}
					else
					{
						for (int i = 0; i < buttonNumber.Length; i++)
						{
							buttonNumber[i] = i;
						}
					}
					
					one.text = buttonNumber[0] != -1 ? qd[0].question_title : "問題が登録されていません";
					two.text = buttonNumber[1] != -1 ? qd[1].question_title : "問題が登録されていません";
					three.text = buttonNumber[2] != -1 ? qd[2].question_title : "問題が登録されていません";
					four.text = buttonNumber[3] != -1 ? qd[3].question_title : "問題が登録されていません";
					
				}));
			}

			
			
			
		}));

		schoolName.GetComponent<Text>().text = schoolname;
		UserInformation.GetComponent<Text>().text =
			student_year + "年　" + student_class + "組　" + student_number + "番　" + student_name;
		
		


	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void clickONE()
	{
		//elseの中に処理を書く
		if (buttonNumber[0] == -1)
		{
			ErrorText.text = "押したボタンに問題がありません";
		}
		else
		{
			
		}
	}
	
	public void clickTWO()
	{
		//elseの中に処理を書く
		if (buttonNumber[1] == -1)
		{
			ErrorText.text = "押したボタンに問題がありません";
		}
		else
		{
			
		}
	}
	
	public void clickTHREE()
	{
		//elseの中に処理を書く
		if (buttonNumber[2] == -1)
		{
			ErrorText.text = "押したボタンに問題がありません";
		}
		else
		{
			
		}
	}
	
	public void clickFOUR()
	{
		//elseの中に処理を書く
		if (buttonNumber[3] == -1)
		{
			ErrorText.text = "押したボタンに問題がありません";
		}
		else
		{
			
		}
		
	}

	public void back()
	{
		//elseの中に処理を書く
		if (pages == 1)
		{
			ErrorText.text = "これ以上戻れません";
		}
		else
		{
			if (pages == 2)
			{
				for (int i = 0; i < buttonNumber.Length; i++)
				{
					buttonNumber[i] = i;
				}
				
				one.text = buttonNumber[0] != -1 ? qd[buttonNumber[0]].question_title : "問題が登録されていません";
				two.text = buttonNumber[1] != -1 ? qd[buttonNumber[1]].question_title : "問題が登録されていません";
				three.text = buttonNumber[2] != -1 ? qd[buttonNumber[2]].question_title : "問題が登録されていません";
				four.text = buttonNumber[3] != -1 ? qd[buttonNumber[3]].question_title : "問題が登録されていません";
				
				pages--;
			}
			else
			{
				int num =3;
				for (int i = ((pages -1) *4) -1; i >= (pages -2)*4; i--, num--)
				{
					buttonNumber[num] = i;
				}
				
				one.text = buttonNumber[0] != -1 ? qd[buttonNumber[0]].question_title : "問題が登録されていません";
				two.text = buttonNumber[1] != -1 ? qd[buttonNumber[1]].question_title : "問題が登録されていません";
				three.text = buttonNumber[2] != -1 ? qd[buttonNumber[2]].question_title : "問題が登録されていません";
				four.text = buttonNumber[3] != -1 ? qd[buttonNumber[3]].question_title : "問題が登録されていません";
				
				pages--;
			}
			ErrorText.text = "";
		}
	}
	
	public void next()
	{
		//elseの中に処理を書く
		if (count < pages * 4)
		{
			ErrorText.text = "これ以上進めません";
		}
		else
		{
			int j = pages *4;
			int num = 0;
			for (int i = pages *4; i < count; i++, j++, num++)
			{
				buttonNumber[num] = i;
			}

			while (j < (pages +1 )*4)
			{
				buttonNumber[num] = -1;
				j++;
				num++;
			}
			
			one.text = buttonNumber[0] != -1 ? qd[buttonNumber[0]].question_title : "問題が登録されていません";
			two.text = buttonNumber[1] != -1 ? qd[buttonNumber[1]].question_title : "問題が登録されていません";
			three.text = buttonNumber[2] != -1 ? qd[buttonNumber[2]].question_title : "問題が登録されていません";
			four.text = buttonNumber[3] != -1 ? qd[buttonNumber[3]].question_title : "問題が登録されていません";
			
			pages++;
		}
	}
	
	private IEnumerator Access() {
		Dictionary<string, string> dic = new Dictionary<string, string>();

		dic.Add("password"    , password);  //インプットフィールドからidの取得);
		dic.Add("year"        , student_year);
		dic.Add("class"       , student_class);
		dic.Add("number"      , student_number);
		dic.Add("id"          , student_id);
		
		
		//複数phpに送信したいデータがある場合は今回の場合dic.Add("hoge", value)のように足していけばよい

		StartCoroutine(Post(code, dic));  // POST

		yield return 0;
	} 
	
	private IEnumerator Access2(string q_id, int point) {
		Dictionary<string, string> dic = new Dictionary<string, string>();

		dic.Add("password"          , password);  //インプットフィールドからidの取得);
		dic.Add("question_id"       , q_id);
		dic.Add("seito_id"          , student_id);
		
		
		//複数phpに送信したいデータがある場合は今回の場合dic.Add("hoge", value)のように足していけばよい

		StartCoroutine(Post2(code2, dic, point));  // POST

		yield return 0;
	}

	private IEnumerator Post(string url, Dictionary<string, string> post) {
		WWWForm form = new WWWForm();
		foreach (KeyValuePair<string, string> post_arg in post) {
			form.AddField(post_arg.Key, post_arg.Value);
		}
		WWW www = new WWW(url, form);

		yield return StartCoroutine(CheckTimeOut(www, 3f)); //TimeOutSecond = 3s;

		if (www.error != null) {
			Debug.Log("HttpPost NG: " + www.error);
			urlError = true;
			//そもそも接続ができていないとき

		} else if (www.isDone) {
			//送られてきたデータをテキストに反映
			status = www.text;
		}
	}

	private IEnumerator Post2(string url, Dictionary<string, string> post, int point) {
		WWWForm form = new WWWForm();
		foreach (KeyValuePair<string, string> post_arg in post) {
			form.AddField(post_arg.Key, post_arg.Value);
		}
		WWW www = new WWW(url, form);

		yield return StartCoroutine(CheckTimeOut(www, 3f)); //TimeOutSecond = 3s;

		if (www.error != null) {
			Debug.Log("HttpPost NG: " + www.error);
			urlError = true;
			//そもそも接続ができていないとき

		} else if (www.isDone) {
			//送られてきたデータをテキストに反映
			status2 = www.text;
			Debug.Log(status2);
			questionsplit =  status2.Split(splitter, StringSplitOptions.None);
						
			qd[point] = new question_data();
			qd[point].question_id    = Int32.Parse(questionsplit[0]);
			qd[point].question_title = questionsplit[1];
			qd[point].question_type  = questionsplit[2];
			qd[point].main           = questionsplit[3];
			qd[point].tate           = Double.Parse(questionsplit[4]);
			qd[point].yoko           = Double.Parse(questionsplit[5]);
			qd[point].takasa         = Double.Parse(questionsplit[6]);
			qd[point].zukei_type     = questionsplit[7];
			qd[point].answer         = questionsplit[8];
			qd[point].kaisetsu       = questionsplit[9];
			qd[point].allready       = questionsplit[10];
			qd[point].kaitou         = questionsplit[11];
			qd[point].saiten         = questionsplit[12];
			
			Debug.Log(message: "q_id =" + qd[point].question_id);
//			Debug.Log(message: "q_title =" + qd[point].question_title);
//			Debug.Log(message: "q_type =" + qd[point].question_type);
//			Debug.Log(message: "q_main =" + qd[point].main);
//			Debug.Log(message: "q_tate =" + qd[point].tate);
//			Debug.Log(message: "q_yoko =" + qd[point].yoko);
//			Debug.Log(message: "q_takasa =" + qd[point].takasa);
//			Debug.Log(message: "q_zukei_type =" + qd[point].zukei_type);
//			Debug.Log(message: "q_answer =" + qd[point].answer);
//			Debug.Log(message: "q_kaisetsu =" + qd[point].kaisetsu);
//			Debug.Log(message: "q_allready =" + qd[point].allready);
//			Debug.Log(message: "q_kaitou =" + qd[point].kaitou);
//			Debug.Log(message: "q_saiten =" + qd[point].saiten);
		}
	}
	
	private IEnumerator CheckTimeOut(WWW www, float timeout) {
		float requestTime = Time.time;

		while (!www.isDone) {
			if (Time.time - requestTime < timeout)
				yield return null;
			else {
				Debug.Log("TimeOut");  //タイムアウト
				//タイムアウト処理
				timeOutError = true;
				break;
			}
		}
		yield return null;
	}
	
	/// <summary>
	/// 渡された処理を指定時間後に実行する
	/// </summary>
	/// <param name="delayFrameCount"></param>
	/// <param name="action">実行したい処理</param>
	/// <returns></returns>
	private IEnumerator DelayMethod(int delayFrameCount, Action action)
	{
		for (var i = 0; i < delayFrameCount; i++)
		{
			yield return null;
		}
		action();
	}
}
