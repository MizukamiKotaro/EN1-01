using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject goalPrefab;
    public GameObject clearText;

    int[,] map;          //�Q�[���f�U�C���p�̔z��
    GameObject[,] field; //�Q�[���Ǘ��p�̔z��

    //�z�u�̕�����̏o�̓��\�b�h
    //void PrintArray()
    //{
    //    //�ǉ��B������̐錾�Ə�����
    //    string debugText = "";

    //    for (int y = 0; y < map.GetLength(0); y++)
    //    {
    //        for (int x = 0; x < map.GetLength(1); x++)
    //        {
    //            //�ύX�B������Ɍ������Ă���
    //            debugText += map[y, x].ToString() + ",";
    //        }
    //        debugText += "\n";
    //    }

    //    //����������������o��
    //    Debug.Log(debugText);
    //}

    //playerIndex�̎擾���\�b�h
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
        //Vector2Int�^�̉ϒ��z��̍쐬
        List<Vector2Int> goals = new List<Vector2Int>();

        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (map[y, x] == 3)
                {
                    //�i�[�ꏊ�̃C���f�b�N�X
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }

        //�v�f����goals.Count�Ŏ擾
        for(int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if (f == null || f.tag != "Box") { return false; }

        }

        Debug.Log("Clear");
        return true;
    }

    ////�ړ��̉s�𔻒f���Ĉړ����������郁�\�b�h
    bool MoveObject(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0))
        {
            //�����Ȃ��������ɒu���A���^�[������B�������^�[��
            return false;
        }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1))
        {
            //�����Ȃ��������ɒu���A���^�[������B�������^�[��
            return false;
        }

        //if (field[moveTo.y, moveTo.x] == null || field[moveFrom.y, moveFrom.x].tag != tag) { return false; }

        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            //�ǂ̕����Ɉړ����邩���Z�o
            Vector2Int velocity = moveTo - moveFrom;
            //�v���C���[�̈ړ��悩��A����ɐ�֔����ړ�������
            //���̈ړ������AMoveNumber���\�b�h����MoveNumber���\�b�h���ĂсA
            //�������ċN���Ă���B�ړ��s��bool�ŋL�^
            bool success = MoveObject(tag, moveTo, moveTo + velocity);

            //���������ړ����s������A�v���C���[�̈ړ������s
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
        //�z��̎��Ԃ̍쐬�Ə�����
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
