using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class login : MonoBehaviour
{

	public Text titleText;
	private string schoolname;
	// Use this for initialization
	void Start () 
	{
		
		schoolname = SceneSelect.schoolname;
		Debug.Log("schoolname = " + schoolname);

		titleText.GetComponent<Text>().text = schoolname;
		Debug.Log(titleText.GetComponent<Text>().text);

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
