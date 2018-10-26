using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Rittai : MonoBehaviour {

	
	//問題文等表示情報と入力
	public Text title;
	public Text main;
	public InputField IF;
	public Text ErrorText;
	
	//生成する図形オブジェクト
	public GameObject obj;

	public Mesh cube;
	public Mesh cone;
	public Mesh pyramid;
	public Mesh cylinder;
	public Mesh triangularprism;
	public Mesh triangularpyramid;
	
	//問題のデータ
	private question_data qd;
	
	//図形sizeデータ
	private double tate;
	private double yoko;
	private double takasa;
	private const double scale = 12000;
	
	//php送信情報
	private string password ="sanboysone";
	private string seito_id;
	private string url;
	private string code;
	private string saiten;

	//受信ステータス
	private string status;
	private bool urlError;
	private bool timeOutError;
	
	//移動変数
	private float x, y;
	private float speed = 300;
	
	// Use this for initialization
	void Start () {
		seito_id = login.student_id;
		qd = MenuBoard.qd[MenuBoard.openID];
		title.text = qd.question_title;
		main.text  = qd.main;
		url = SceneSelect.UrlString;
		code = url + "/unity/registerAnswer.php";
		
		/*
		 * Gameobject test = (GameObject)Instantiate(入れるオブジェクト,obj.transform);
		 */
		if (qd.tate == qd.yoko && qd.yoko == qd.tate)
		{
			tate   = scale;
			yoko   = tate;
			takasa = yoko;
		}
		else if (qd.tate == qd.yoko && qd.yoko > qd.takasa)
		{
			tate = scale;
			yoko = tate;			
			takasa = scale * (qd.takasa / qd.yoko );
		}
		else if (qd.tate == qd.yoko && qd.yoko < qd.takasa)
		{
			takasa = scale;
			yoko = takasa * (qd.yoko / qd.takasa);
			tate = yoko;
		}
		else if (qd.tate > qd.yoko && qd.yoko == qd.takasa)
		{
			tate = scale;
			yoko = tate * (qd.yoko / qd.tate);
			takasa = yoko;
		}
		else if (qd.tate < qd.yoko && qd.yoko == qd.takasa)
		{
			takasa = scale;
			yoko = takasa;
			tate = takasa * (qd.tate / qd.yoko);
		}
		else if (qd.tate > qd.yoko && qd.yoko > qd.takasa)
		{
			tate = scale;
			yoko = tate * (qd.yoko / qd.tate);
			takasa = yoko * (qd.takasa / qd.yoko);
		}
		else if (qd.tate > qd.yoko && qd.yoko < qd.takasa)
		{
			if (qd.tate < qd.takasa)
			{
				takasa = scale;
				tate = takasa * (qd.tate / qd.tate);
				yoko = tate * (qd.yoko / qd.tate);
			}
			else
			{
				tate = scale;
				takasa = tate * (qd.takasa / qd.tate);
				yoko = tate * (qd.yoko / qd.takasa);
			}
		}
		else if (qd.tate < qd.yoko && qd.yoko < qd.takasa)
		{
			takasa = scale;
			yoko = takasa * (qd.yoko / qd.takasa);
			tate = yoko * (qd.tate / qd.yoko);
		}
		else if (qd.tate < qd.yoko && qd.yoko > qd.takasa)
		{
			yoko = scale;
			
			if (qd.tate < qd.takasa)
			{
				takasa = yoko * (qd.takasa / qd.yoko);
				tate = takasa * (qd.tate / qd.takasa);
			}
			else
			{
				tate = yoko * (qd.tate / qd.yoko);
				takasa = tate * (qd.takasa / qd.tate);
			}
		}
		
		if (qd.zukei_type == "cube") //立方体
		{
			obj.GetComponent<MeshFilter>().mesh = cube;
			obj.GetComponent<Transform>().localScale = new Vector3((float)yoko,(float)takasa,(float)tate);

		}
		else if (qd.zukei_type == "triangularprism") //三角柱
		{
			obj.GetComponent<MeshFilter>().mesh = triangularprism;
			obj.GetComponent<Transform>().localScale = new Vector3((float)yoko,(float)takasa,(float)tate);
		}
		else if (qd.zukei_type == "squarepyramid") //四角錐
		{
			obj.GetComponent<MeshFilter>().mesh = pyramid;
			obj.GetComponent<Transform>().localScale = new Vector3((float)yoko,(float)takasa,(float)tate);
		}
		else if (qd.zukei_type == "trianglepyramid") //三角錐
		{
			obj.GetComponent<MeshFilter>().mesh = triangularpyramid;
			obj.GetComponent<Transform>().localScale = new Vector3((float)yoko,(float)takasa,(float)tate);
		}
		else if (qd.zukei_type == "cylinder") //円柱
		{
			obj.GetComponent<MeshFilter>().mesh = cylinder;
			obj.GetComponent<Transform>().localScale = new Vector3((float)yoko,(float)takasa,(float)tate);
		}
		else if (qd.zukei_type == "cone") //円錐
		{
			obj.GetComponent<MeshFilter>().mesh = cone;
			obj.GetComponent<Transform>().localScale = new Vector3((float)yoko,(float)takasa,(float)tate);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0))
		{
			x += Input.GetAxis("Mouse X") * speed * -0.02f;
			y -= Input.GetAxis("Mouse Y") * speed * -0.02f;
		}
		Quaternion rotation = Quaternion.Euler(y,x,0);
		obj.transform.rotation = rotation;
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
				SceneManager.LoadScene("Answer");
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
