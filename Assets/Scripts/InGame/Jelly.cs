using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Type
{
    Yellow, Green, Red, Blue, Purple, Gray, Bomb, LineBomb
}


public class Jelly : MonoBehaviour
{
    public Type type; //{ get; private set; }
    public Vector2Int CurPos { get; private set; }
    public void SetType(Type _Type)
    {
        type = _Type;
    }

    public void SetPos(int _x, int _y)
    {
        CurPos = new Vector2Int(_x, _y);
    }

    public void SetPos(Vector2Int _vec)
    {
        CurPos = _vec;
    }

    public void MoveDown(int _length, float _speed = 10f)
    {
        StartCoroutine(MoveDownRoutine(_length, _speed));
    }

    IEnumerator MoveDownRoutine(int _length, float _speed = 10f)
    {
        float length = 0;
        Vector3 targetPos = transform.localPosition + Vector3.down * _length;
        while (length < _length)
        {
            transform.position += Vector3.down * Time.deltaTime * _speed;
            length += Time.deltaTime * _speed;
            yield return null;
        }

        transform.localPosition = targetPos;
        GameManager.Instance.MoveDownJellyCount--;
    }
}
