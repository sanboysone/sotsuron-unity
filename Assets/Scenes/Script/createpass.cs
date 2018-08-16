using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class createpass : MonoBehaviour
{

	private string code;
	private string student_year;
	private string student_class;
	private string student_number;
	private string student_name;

	public Text       title;
	public Text       user;
	public InputField pass1;
	public InputField pass2;
	public Text       errorText;
	private string    password = "sanboysone";

	private string status; //php結果一時保存場所
	private bool   urlError = false;
	private bool   timeOutError = false;
	
	// Use this for initialization
	void Start ()
	{
		title.text = SceneSelect.schoolname;
		code = SceneSelect.UrlString + "/unity/username.php";
		student_year = login.student_year;
		student_class = login.student_class;
		student_number = login.student_number;
		user.text = student_year + "年　" + student_class + "組　" + student_number + "番　";
		StartCoroutine("Access");   //Accessコルーチンの開始
		StartCoroutine(DelayMethod(4, () =>
		{
			student_name = status;
			login.student_name = student_name;
		}));		
		
		user.text = student_year + "年　" + student_class + "組　" + student_number + "番　" + student_name;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void onclick()
	{
		if (pass1.text.Equals(pass2.text))
		{
			code = SceneSelect.UrlString + "/unity/registerpass.php";
		
			StartCoroutine("Access2");   //Accessコルーチンの開始
			StartCoroutine(DelayMethod(4, () =>
			{
				if (status == "success")
				{
					SceneManager.LoadScene("main");
				}
				else
				{
					errorText.GetComponent<Text>().text = "エラー：管理者に連絡してください";
				}
			}));	
		}
		else
		{
			errorText.GetComponent<Text>().text = "パスワードが再入力したパスワードと一致しません";
		}
		
	}
	
	private IEnumerator Access() {
		Dictionary<string, string> dic = new Dictionary<string, string>();

		dic.Add("password"   , password);  //インプットフィールドからidの取得);
		dic.Add("year"       , student_year);
		dic.Add("class"      , student_class);
		dic.Add("number"     , student_number);	
		//複数phpに送信したいデータがある場合は今回の場合dic.Add("hoge", value)のように足していけばよい

		StartCoroutine(Post(code, dic));  // POST

		yield return 0;
	}
	
	private IEnumerator Access2() {
		Dictionary<string, string> dic = new Dictionary<string, string>();

		dic.Add("password"     , password);  //インプットフィールドからidの取得);
		dic.Add("year"         , student_year);
		dic.Add("class"        , student_class);
		dic.Add("number"       , student_number);
		dic.Add("submit_pass"  , pass1.text);
		dic.Add("submit_pass2" , pass2.text);
		
		
		//複数phpに送信したいデータがある場合は今回の場合dic.Add("hoge", value)のように足していけばよい

		StartCoroutine(Post(code, dic));  // POST

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
