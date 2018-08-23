using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBoard : MonoBehaviour
{

	public string password = "sanboysone";
	private string student_year;
	private string student_class;
	private string student_number;
	private string schoolname;

	private string url;
	private string code;

	private bool urlError = false;
	private bool timeOutError = false;

	private string status;
	
	
	// Use this for initialization
	void Start ()
	{
		student_year   = login.student_year;
		student_class  = login.student_class;
		student_number = login.student_number;

		url = SceneSelect.UrlString;
		schoolname = SceneSelect.schoolname;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void clickONE()
	{
		
	}
	
	public void clickTWO()
	{
		
	}
	
	public void clickTHREE()
	{
		
	}
	
	public void clickFOUR()
	{
		
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
