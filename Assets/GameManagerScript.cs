using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    int[] map;

    //配置の文字列の出力メソッド
    void PrintArray()
    {
        //追加。文字列の宣言と初期化
        string debugText = "";

        for (int i = 0; i < map.GetLength(0); i++)
        {
            //変更。文字列に結合していく
            debugText += map[i].ToString() + ",";
        }

        //結合した文字列を出力
        Debug.Log(debugText);
    }

    //playerIndexの取得メソッド
    int GetPlayerIndex()
    {
        for (int i = 0; i < map.Length; i++)
        {
            if (map[i] == 1)
            {
                return i;
            }
        }

        return -1;
    }

    //移動の可不可を判断して移動処理をするメソッド
    bool MoveNumber(int number,int moveFrom,int moveTo)
    {
        if (moveTo < 0 || moveTo >= map.Length)
        {
            //動けない条件を先に置き、リターンする。早期リターン
            return false;
        }

        //移動先に2(箱)が居たら
        if (map[moveTo] == 2)
        {
            //どの方向に移動するかを算出
            int velocity = moveTo - moveFrom;
            //プレイヤーの移動先から、さらに先へ2(箱)を移動させる
            //箱の移動処理、MoveNumberメソッド内でMoveNumberメソッドを呼び、
            //処理が再起している。移動可不可をboolで記録
            bool success = MoveNumber(2, moveTo, moveTo + velocity);
            //もし箱が移動失敗したら、プレイヤーの移動も失敗
            if (!success) { return false; }
        }

        map[moveTo] = number;
        map[moveFrom] = 0;
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //配列の実態の作成と初期化
        map = new int[] { 0, 0, 0, 1, 0, 2, 0, 0, 0 };

        PrintArray();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            int playerIndex = GetPlayerIndex();

            MoveNumber(1, playerIndex, playerIndex + 1);
            PrintArray();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int playerIndex = GetPlayerIndex();

            MoveNumber(1, playerIndex, playerIndex - 1);
            PrintArray();
        }
    }
}
