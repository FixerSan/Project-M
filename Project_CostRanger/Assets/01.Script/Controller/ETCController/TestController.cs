using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class TestController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            TestClass rangerDatas = new TestClass();
            Debug.Log(rangerDatas._array[0]);
            Debug.Log(rangerDatas._int);

            string textAsset = JsonUtility.ToJson(rangerDatas, true);
            Debug.Log(textAsset);

            File.WriteAllText(Application.dataPath+"TestClassData.txt", textAsset);

        }
    }
}

[System.Serializable]
public class TestClass
{
    public string[] _array = new string[10];
    public int _int;
    public TestClass() 
    {
        _array[0] = "sksk";
        _array[1] = "sksk";
        _array[2] = "sksk";
        _int = 1;
    }
}