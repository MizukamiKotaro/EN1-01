using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject goalPrefab;
    public GameObject clearText;

    int[,] map;          //ゲームデザイン用の配列
    GameObject[,] field; //ゲーム管理用の配列

    //配置の文字列の出力メソッド
    //void PrintArray()
    //{
    //    //追加。文字列の宣言と初期化
    //    string debugText = "";

    //    for (int y = 0; y < map.GetLength(0); y++)
    //    {
    //        for (int x = 0; x < map.GetLength(1); x++)
    //        {
    //            //変更。文字列に結合していく
    //            debugText += map[y, x].ToString() + ",";
    //        }
    //        debugText += "\n";
    //    }

    //    //結合した文字列を出力
    //    Debug.Log(debugText);
    //}

    //playerIndexの取得メソッド
    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (field[y,x] == null) { continue; }
                if (field[y,x].tag == "Player") { return new Vector2Int(x, y); }
            }
        }
        return new Vector2Int(-1, -1);
    }

    bool IsCleared()
    {
        //Vector2Int型の可変長配列の作成
        List<Vector2Int> goals = new List<Vector2Int>();

        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (map[y, x] == 3)
                {
                    //格納場所のインデックス
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }

        //要素数はgoals.Countで取得
        for(int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if (f == null || f.tag != "Box") { return false; }

        }

        Debug.Log("Clear");
        return true;
    }

    ////移動の可不可を判断して移動処理をするメソッド
    bool MoveObject(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0))
        {
            //動けない条件を先に置き、リターンする。早期リターン
            return false;
        }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1))
        {
            //動けない条件を先に置き、リターンする。早期リターン
            return false;
        }

        //if (field[moveTo.y, moveTo.x] == null || field[moveFrom.y, moveFrom.x].tag != tag) { return false; }

        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            //どの方向に移動するかを算出
            Vector2Int velocity = moveTo - moveFrom;
            //プレイヤーの移動先から、さらに先へ箱を移動させる
            //箱の移動処理、MoveNumberメソッド内でMoveNumberメソッドを呼び、
            //処理が再起している。移動可不可をboolで記録
            bool success = MoveObject(tag, moveTo, moveTo + velocity);

            //もし箱が移動失敗したら、プレイヤーの移動も失敗
            if (!success) { return false; }
        }

        field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0);
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //配列の実態の作成と初期化
        map = new int[,] {
            {0,0,0,0,0},
            {0,3,1,3,0},
            {0,0,2,0,0},
            {0,2,3,2,0},
            {0,0,0,0,0}
        };
        field = new GameObject
        [
            map.GetLength(0),
            map.GetLength(1)
        ];

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 1)
                {
                    field[y,x] = Instantiate(playerPrefab, new Vector3(x, map.GetLength(0) - y, 0), Quaternion.identity);
                }
                if (map[y, x] == 2)
                {
                    field[y, x] = Instantiate(boxPrefab, new Vector3(x, map.GetLength(0) - y, 0), Quaternion.identity);
                }
                if (map[y, x] == 3)
                {
                    field[y, x] = Instantiate(goalPrefab, new Vector3(x, map.GetLength(0) - y, 0.01f), Quaternion.identity);
                }
            }
        }

        //PrintArray();
    }

    void DeleteObject()
    {
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                Destroy(field[y, x]);
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveObject(playerPrefab.tag, playerIndex, new Vector2Int(playerIndex.x + 1,playerIndex.y));
            //PrintArray();
            
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveObject(playerPrefab.tag, playerIndex, new Vector2Int(playerIndex.x - 1, playerIndex.y));
            //PrintArray();
            
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveObject(playerPrefab.tag, playerIndex, new Vector2Int(playerIndex.x, playerIndex.y + 1));
            //PrintArray();
            
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveObject(playerPrefab.tag, playerIndex, new Vector2Int(playerIndex.x, playerIndex.y - 1));
            //PrintArray();
            
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (clearText)
            {
                clearText.SetActive(false);
            }
            DeleteObject();
            Start();

        }

        if (IsCleared())
        {
            clearText.SetActive(true);
        }
    }
}
