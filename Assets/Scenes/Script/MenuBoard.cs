using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuBoard : MonoBehaviour
{
	//接続パスワード
	private string password = "sanboysone";
	
	//フラグ
	private bool connectFlag =false;
	
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
	private string[][] questionsplit;
	private int count;
	
	
	//解答済みかどうか
	private bool[] alreadyAnswer = new bool[4];
	
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

		schoolName.GetComponent<Text>().text = schoolname;
		UserInformation.GetComponent<Text>().text =
		student_year + "年　" + student_class + "組　" + student_number + "番　" + student_name;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (connectFlag == false && qd != null)
		{
			if (qd[count - 1] != null)
			{
				if (qd[count - 1].saiten != null)
				{
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
					
					alreadyAnswer[0] = qd[0].allready == "yes" ? true : false;
					alreadyAnswer[1] = qd[1].allready == "yes" ? true : false;
					alreadyAnswer[2] = qd[2].allready == "yes" ? true : false;
					alreadyAnswer[3] = qd[3].allready == "yes" ? true : false;
				}

				connectFlag = true;
			}	
		}	
	}

	public void clickONE()
	{
		//elseの中に処理を書く
		if (buttonNumber[0] == -1)
		{
			ErrorText.text = "押したボタンに問題がありません";
		}
		else if (alreadyAnswer[0] == true)
		{
			ErrorText.text = "すでに解答済みです";
		}
		else
		{
			openID   = buttonNumber[0];
			openType = qd[buttonNumber[0]].question_type;
			if (openType.Equals("heimen"))
			{
				SceneManager.LoadScene("Heimen");
			}
			else if (openType.Equals("rittai"))
			{
				SceneManager.LoadScene("Rittai");
			}
			else
			{
				SceneManager.LoadScene("sonota");
			}
		}
	}
	
	public void clickTWO()
	{
		//elseの中に処理を書く
		if (buttonNumber[1] == -1)
		{
			ErrorText.text = "押したボタンに問題がありません";
		}
		else if (alreadyAnswer[1] == true)
		{
			ErrorText.text = "すでに解答済みです";
		}
		else
		{
			openID   = buttonNumber[1];
			openType = qd[buttonNumber[1]].question_type;
			if (openType.Equals("heimen"))
			{
				SceneManager.LoadScene("Heimen");
			}
			else if (openType.Equals("rittai"))
			{
				SceneManager.LoadScene("Rittai");
			}
			else
			{
				SceneManager.LoadScene("sonota");
			}
		}
	}
	
	public void clickTHREE()
	{
		//elseの中に処理を書く
		if (buttonNumber[2] == -1)
		{
			ErrorText.text = "押したボタンに問題がありません";
		}
		else if (alreadyAnswer[2] == true)
		{
			ErrorText.text = "すでに解答済みです";
		}
		else
		{
			openID   = buttonNumber[2];
			openType = qd[buttonNumber[2]].question_type;
			if (openType.Equals("heimen"))
			{
				SceneManager.LoadScene("Heimen");
			}
			else if (openType.Equals("rittai"))
			{
				SceneManager.LoadScene("Rittai");
			}
			else
			{
				SceneManager.LoadScene("Sonota");
			}
		}
	}
	
	public void clickFOUR()
	{
		//elseの中に処理を書く
		if (buttonNumber[3] == -1)
		{
			ErrorText.text = "押したボタンに問題がありません";
		}
		else if (alreadyAnswer[3] == true)
		{
			ErrorText.text = "すでに解答済みです";
		}
		else
		{
			openID   = buttonNumber[3];
			openType = qd[buttonNumber[3]].question_type;	
			if (openType.Equals("heimen"))
			{
				SceneManager.LoadScene("Heimen");
			}
			else if (openType.Equals("rittai"))
			{
				SceneManager.LoadScene("Rittai");
			}
			else
			{
				SceneManager.LoadScene("Sonota");
			}
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
					alreadyAnswer[i] = qd[i].allready == "yes" ? true : false;
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
					alreadyAnswer[num] = qd[buttonNumber[i]].allready == "yes" ? true : false;
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
				alreadyAnswer[num] = qd[i].allready == "yes" ? true : false;
			}

			while (j < (pages +1 )*4)
			{
				buttonNumber[num] = -1;
				alreadyAnswer[num] = false;
				j++;
				num++;
			}
			
			one.text = buttonNumber[0]   != -1   ? qd[buttonNumber[0]].question_title : "問題が登録されていません";
			two.text = buttonNumber[1]   != -1   ? qd[buttonNumber[1]].question_title : "問題が登録されていません";
			three.text = buttonNumber[2] != -1   ? qd[buttonNumber[2]].question_title : "問題が登録されていません";
			four.text = buttonNumber[3]  != -1   ? qd[buttonNumber[3]].question_title : "問題が登録されていません";
			
			pages++;
		}
	}

	public void logout()
	{
		SceneManager.LoadScene("main");
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

		Debug.Log(password + " " + q_id + "  " + student_id);
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
			Debug.Log("status = " + status);
			arr =  status.Split(splitter, StringSplitOptions.None);
			count = arr.Length;
			questionsplit = new string[count][];

			if (count != 0)
			{
				code2 = SceneSelect.UrlString + "/unity/sendquestion.php";
				qd = new question_data[count];				
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
					Debug.Log(code2);
					StartCoroutine(Access2(arr[i], i)); //Accessコルーチンの開始
					//このコルーチン処理に入る前にfor文が終わり、iが最大値+1になってるからout of rangeが出る
					//これをなんとかする
				}
				
			}
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
			questionsplit[point] =  status2.Split(splitter, StringSplitOptions.None);
						
			qd[point] = new question_data();
			qd[point].question_id    = Int32.Parse(questionsplit[point][0]);
			qd[point].question_title = questionsplit[point][1];
			qd[point].question_type  = questionsplit[point][2];
			qd[point].main           = questionsplit[point][3];
			qd[point].tate           = Double.Parse(questionsplit[point][4]);
			qd[point].yoko           = Double.Parse(questionsplit[point][5]);
			qd[point].takasa         = Double.Parse(questionsplit[point][6]);
			qd[point].zukei_type     = questionsplit[point][7];
			qd[point].answer         = questionsplit[point][8];
			qd[point].kaisetsu       = questionsplit[point][9];
			qd[point].allready       = questionsplit[point][10];
			qd[point].kaitou         = questionsplit[point][11];
			qd[point].saiten         = questionsplit[point][12];
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
