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
        // 데이터 리스트 초기화
        myClassList.Add(createSubObject("test", 10));
        myClassList.Add(createSubObject("test2", 12));
        myClassList.Add(createSubObject("test2", 12));
        _mydata.myClassArray = myClassList.ToArray();

        // Json으로 직렬화
        string json = JsonUtility.ToJson(_mydata);

        // 결과 출력
        Debug.Log(json);
    }
}