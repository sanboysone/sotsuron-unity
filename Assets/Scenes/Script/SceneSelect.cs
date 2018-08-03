using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelect : MonoBehaviour {

	public string[] textMessage; //テキストの加工前の一行を入れる変数
	public static string UrlString;
	
	// Use this for initialization
	void Start () {
		TextAsset textasset = new TextAsset(); //テキストファイルのデータを取得するインスタンスを作成
		textasset = Resources.Load("URL", typeof(TextAsset) )as TextAsset; //Resourcesフォルダから対象テキストを取得
		string TextLines = textasset.text; //テキスト全体をstring型で入れる変数を用意して入れる
		
		Debug.Log(TextLines);

		//Splitで一行づつを代入した1次配列を作成
		textMessage = TextLines.Split('¥'); 
		
		Debug.Log(textMessage[0]);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClick(){
		if (textMessage == null || textMessage[0].Equals("none"))
		{
			SceneManager.LoadScene ("school_select"); //学校のurlを入力するシーン
		}
		else
		{
			UrlString = textMessage[0];
			SceneManager.LoadScene ("main"); //学校のurlが入力済みだった場合はメイン画面に行く
		}
	}
}
