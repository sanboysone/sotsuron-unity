using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class login : MonoBehaviour
{

	public Text       titleText;  //タイトル（学校名のテキスト）
	public InputField yearText;   //学年のインプット
	public InputField classText;  //クラスのインプット
	public InputField number;     //出席番号のインプット
	public InputField pass;       //ログインパスワード
	public Text       errorText;  //エラー表示テキスト
	
	
	private string schoolname;
	private string URL;
	private string code;           // phpのパスとファイル名を入れる場所


	public static string student_year;    //学年
	public static string student_class;   //クラス
	public static string student_number;  //出席番号
	public static string student_name;    //名前

	private string status;
	
	private bool timeOutError;
	private bool urlError;

	private string password = "sanboysone";
	// Use this for initialization
	void Start ()
	{
		timeOutError = false;
		urlError = false;

		status = null;
		
		schoolname = SceneSelect.schoolname;
		Debug.Log("schoolname = " + schoolname);

		titleText.GetComponent<Text>().text = schoolname;
		Debug.Log("login text :" + titleText.GetComponent<Text>().text);

		URL = SceneSelect.UrlString;

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void onclick()
	{
		if (string.IsNullOrEmpty(yearText.text) || string.IsNullOrEmpty(classText.text) || string.IsNullOrEmpty(number.text))
		{
			errorText.GetComponent<Text>().text = "未入力があります";
		}
		else
		{
			code = URL + "/unity/userinformation.php"; //phpのファイル指定

			student_year   = yearText.text;
			student_class  = classText.text;
			student_number = number.text;

			StartCoroutine("Access");   //Accessコルーチンの開始
			StartCoroutine(DelayMethod(4, () =>
			{
				if (status.Equals("success")) //phpから返ってくる結果に応じた処理をする
				{
					//メインメニューに進む
					SceneManager.LoadScene("Menu");
				}
				else if (status.Equals("nopass"))
				{
					//createpassword
					SceneManager.LoadScene("createpass");
				}
				else if (status.Equals("notaccess"))
				{
					errorText.GetComponent<Text>().text = "パスワードが間違っています";
				}
				else if (status.Equals("notinput"))
				{
					errorText.GetComponent<Text>().text = "パスワードが未入力です";
				}
				else if (status.Equals("nouser"))
				{
					errorText.GetComponent<Text>().text = "ユーザーが登録されていません";
				}
				else
				{
					errorText.GetComponent<Text>().text = "エラー：管理者に連絡してください";
				}
			}));
		}
		
	}
	
	private IEnumerator Access() {
		Dictionary<string, string> dic = new Dictionary<string, string>();

		dic.Add("password"   , password);  //インプットフィールドからidの取得);
		dic.Add("year"       , student_year);
		dic.Add("class"      , student_class);
		dic.Add("number"     , student_number);
		dic.Add("login_pass" , pass.text);
		
		
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
