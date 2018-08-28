using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//問題格納用クラス
public class question_data //: MonoBehaviour
{
    public int    question_id;
    public string question_title;
    public string question_type;

    public string main;        //本文
    public double tate;        //平面の底辺
    public double yoko;        //平面の高さ
    public double takasa;
    public string zukei_type;  //rittai_type  or heimen_type
    public string answer;      //登録された解答
    public string kaisetsu;    //登録された解説

    public string allready;    //すでに解答済みかどうか
    public string kaitou;      //答えていなければnull
    public string saiten;      //正解：yes 不正解：no 未解答：null

    public question_data(int id, string title, string type, double tate, double yoko, double takasa, string zukei_type, string answer, string kaisetsu, string allready, string kaitou, string saiten)
    {
        this.question_id    = id;
        this.question_title = title;
        this.question_type  = type;
        this.tate           = tate;
        this.yoko           = yoko;
        this.takasa         = takasa;
        this.zukei_type     = zukei_type;
        this.answer         = answer;
        this.kaisetsu       = kaisetsu;
        this.allready       = allready;
        this.kaitou         = kaitou;
        this.saiten         = saiten;
    }

    public question_data(string status)
    {
        this.question_id = 1;
    }

    public question_data()
    {
        
    }
}
