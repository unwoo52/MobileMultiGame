using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class MyClass
{
    public string name;
    public int age;
}

[Serializable]
public class MyData
{
    public MyClass[] myClassArray;
}

public class Example : MonoBehaviour
{
    List<MyClass> myClassList = new();

    MyData _mydata = new();

    public MyClass createSubObject(string name, int age)
    {
        MyClass myClass = new MyClass();
        myClass.name = name;
        myClass.age = age;
        return myClass;
    }
    private void Start()
    {
        // ������ ����Ʈ �ʱ�ȭ
        myClassList.Add(createSubObject("test", 10));
        myClassList.Add(createSubObject("test2", 12));
        myClassList.Add(createSubObject("test2", 12));
        _mydata.myClassArray = myClassList.ToArray();

        // Json���� ����ȭ
        string json = JsonUtility.ToJson(_mydata);

        // ��� ���
        Debug.Log(json);
    }
}