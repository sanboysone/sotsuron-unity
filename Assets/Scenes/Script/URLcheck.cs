﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class URLcheck : MonoBehaviour
{

	// URLと学校名のstatic
	public static string UrlString;
	public static string schoolname;
	
	public InputField InputF;
	private string url;
	private string code;

	public  Text result_text;
	public string result_string;
	
	private string password = "sanboysone";     //phpのパスワード
	
	//エラーテキスト
	public Text ErrorText;
	private bool urlError;
	private bool timeOutError;
	
	
	// Use this for initialization
	void Start ()
	{
		schoolname = null;
		urlError = false;
		timeOutError = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClick()
	{
		
		if (InputF.text == "")
		{
			ErrorText.text = "URLが入力されていません";
			urlError = true;
		}
		else
		{
			url =  "http://" +  InputF.text;
			Debug.Log(url);
			
			code = url + "/unity/firstconnection.php";
			StartCoroutine("Access");   //Accessコルーチンの開始
		
		}
		
		if (urlError == false && timeOutError == false && result_text != null)
		{
			// url time out にあわせた遅延
			StartCoroutine(DelayMethod(4, () =>
			{
				Debug.Log(result_text.GetComponent<Text>().text);
				UrlString = url;
				schoolname = result_text.GetComponent<Text>().text;
				url = url + "¥";
				
				WriteText("Assets/Resources/URL.txt", url);
				SceneSelect.schoolname = schoolname;
				SceneSelect.UrlString = UrlString;
				SceneManager.LoadScene("main");
				//データ入れる
				//テキストのURLにこのURLを入れてpublic statis string schoolを格納するスクリプトを作ってそこに入れる。
			}));
		}
	}

	private IEnumerator Access() {
		Dictionary<string, string> dic = new Dictionary<string, string>();

		dic.Add("password", password);  //インプットフィールドからidの取得);
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
			ErrorText.text = "URLが間違っています";
			//そもそも接続ができていないとき

		} else if (www.isDone) {
			//送られてきたデータをテキストに反映
			result_text.GetComponent<Text>().text = www.text;
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
				ErrorText.text = "接続エラー";
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
	
	private void WriteText( string _filePath, string _contents ){
		StreamWriter sw;
		sw = new StreamWriter(_filePath, false);
		sw.WriteLine(_contents);
		sw.Close();
	}
	
	
	
}
