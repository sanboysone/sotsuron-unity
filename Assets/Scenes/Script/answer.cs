using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class answer : MonoBehaviour {
	
	//問題のデータ
	private question_data qd;
	
	//public status
	public Text title;
	public Text main;
	public Text answer2;
	public Text kaitou;
	public Text kaisetsu;
	public Text saiten;

	// Use this for initialization
	void Start () {
		
		qd = MenuBoard.qd[MenuBoard.openID];
		title.text = qd.question_title;
		main.text  = qd.main;
		answer2.text = qd.answer;
		kaitou.text = qd.kaitou;
		kaisetsu.text = qd.kaisetsu;
		if (qd.saiten == "yes")
		{
			saiten.text = "正解";
		}
		else
		{
			saiten.text = "不正解";
		}

	}


	public void onclick()
	{
		SceneManager.LoadScene("Menu");
	}
}
