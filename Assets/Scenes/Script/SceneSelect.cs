using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSelect : MonoBehaviour {

	public string[] textMessage; //テキストの加工前の一行を入れる変数
	public static string UrlString;
	public static string schoolname;
	public  Text result_text;
	private string result;
	private string code;
	private bool urlError     = false;
	private bool timeOutError = false;
	private string password   = "sanboysone";
	
	// Use this for initialization
	void Start ()
	{

		result = null;
		schoolname = null;
		
		TextAsset textasset = new TextAsset(); //テキストファイルのデータを取得するインスタンスを作成
		textasset = Resources.Load("URL", typeof(TextAsset) )as TextAsset; //Resourcesフォルダから対象テキストを取得
		string TextLines = textasset.text; //テキスト全体をstring型で入れる変数を用意して入れる
		
		Debug.Log(TextLines);

		//Splitで一行づつを代入した1次配列を作成
		textMessage = TextLines.Split('¥'); 
		
		Debug.Log(textMessage[0]);
		
		if (textMessage[0] == "" || textMessage[0].Equals("none"))
		{
			Debug.Log("URL未入力");
		}
		else
		{
			UrlString = textMessage[0];
			code = UrlString + "/unity/firstconnection.php";
			StartCoroutine("Access");

			if ( result_text.GetComponent<Text>().text == null ||  urlError == true || timeOutError == true)
			{
				 Debug.Log("エラー");	
			}
			else
			{
				// url time out にあわせた遅延
				StartCoroutine(DelayMethod(4, () =>
				{
					Debug.Log("学校名：　" + result);
					schoolname = result;	
				}));
				
			}
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClick(){
		if (textMessage[0] == "" || textMessage[0].Equals("none"))
		{
			SceneManager.LoadScene ("school_select"); //学校のurlを入力するシーン
		}
		else
		{
			/*
			UrlString = textMessage[0];
			code = UrlString + "/unity/firstconnection.php";
			StartCoroutine("Access");
			*/
			
			if ( result_text.GetComponent<Text>().text == null ||  urlError == true || timeOutError == true)
			{
				SceneManager.LoadScene ("school_select"); 	
			}
			else
			{
				//Debug.Log(result);
				schoolname = result;
				SceneManager.LoadScene ("main"); //学校のurlが入力済みだった場合はメイン画面に行く
			}
			
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
			//そもそも接続ができていないとき

		} else if (www.isDone) {
			//送られてきたデータをテキストに反映
			//Debug.Log("www.log = " + www.text);
			result_text.GetComponent<Text>().text = www.text;
			result = www.text;
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
