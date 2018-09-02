using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Sonota : MonoBehaviour
{
	private question_data qd;

	public Text title;
	public Text main;
	public InputField IF;
	public Text ErrorText;
	
	
	private string password ="sanboysone";
	private string seito_id;
	private string url;
	private string code;
	private string saiten;

	private string status;
	private bool urlError;
	private bool timeOutError;
	
	// Use this for initialization
	void Start ()
	{
		seito_id = login.student_id;
		qd = MenuBoard.qd[MenuBoard.openID];
		title.text = qd.question_title;
		main.text  = qd.main;
		url = SceneSelect.UrlString;
		code = url + "/unity/registerAnswer.php";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void onclick()
	{
		if (string.IsNullOrEmpty(IF.text))
		{
			ErrorText.text = "解答が入力されていません";
		}
		else
		{
			//ここで採点をしてphpに送る
			if (IF.text.Equals(qd.answer))
			{
				saiten = "yes";
			}
			else
			{
				saiten = "no";
			}

			//Debug.Log("text :" + IF.text + "¥¥");
			//Debug.Log("answer :" + qd.answer + "¥¥");
			//Debug.Log("saiten :" + saiten);
			StartCoroutine("Access");

		}
	}

	public void backClick()
	{
		SceneManager.LoadScene("Menu");
	}
	
	private IEnumerator Access() {
		Dictionary<string, string> dic = new Dictionary<string, string>();

		dic.Add("password"          , password);  //インプットフィールドからidの取得);
		dic.Add("question_id"       , qd.question_id.ToString());
		dic.Add("seito_id"          , seito_id);
		dic.Add("answer"            , IF.text);
		dic.Add("saiten"            , saiten);
		
		
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
			if (status.Equals("success"))
			{
				ErrorText.text = "解答完了　メニューに戻ります";
				MenuBoard.qd[MenuBoard.openID].allready = "yes";
				MenuBoard.qd[MenuBoard.openID].kaitou = IF.text;
				MenuBoard.qd[MenuBoard.openID].saiten = saiten;
				SceneManager.LoadScene("Menu");
			}
			else
			{
				ErrorText.text = "エラー";
			}
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
}
