using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    int[] map;

    //�z�u�̕�����̏o�̓��\�b�h
    void PrintArray()
    {
        //�ǉ��B������̐錾�Ə�����
        string debugText = "";

        for (int i = 0; i < map.GetLength(0); i++)
        {
            //�ύX�B������Ɍ������Ă���
            debugText += map[i].ToString() + ",";
        }

        //����������������o��
        Debug.Log(debugText);
    }

    //playerIndex�̎擾���\�b�h
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

    //�ړ��̉s�𔻒f���Ĉړ����������郁�\�b�h
    bool MoveNumber(int number,int moveFrom,int moveTo)
    {
        if (moveTo < 0 || moveTo >= map.Length)
        {
            //�����Ȃ��������ɒu���A���^�[������B�������^�[��
            return false;
        }

        //�ړ����2(��)��������
        if (map[moveTo] == 2)
        {
            //�ǂ̕����Ɉړ����邩���Z�o
            int velocity = moveTo - moveFrom;
            //�v���C���[�̈ړ��悩��A����ɐ��2(��)���ړ�������
            //���̈ړ������AMoveNumber���\�b�h����MoveNumber���\�b�h���ĂсA
            //�������ċN���Ă���B�ړ��s��bool�ŋL�^
            bool success = MoveNumber(2, moveTo, moveTo + velocity);
            //���������ړ����s������A�v���C���[�̈ړ������s
            if (!success) { return false; }
        }

        map[moveTo] = number;
        map[moveFrom] = 0;
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //�z��̎��Ԃ̍쐬�Ə�����
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
