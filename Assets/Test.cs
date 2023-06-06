using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class Test : MonoBehaviour
{
    [Serializable]
    public class tests
    {
        [SerializeField] public int a = 1;
        [SerializeField] public Vector3 v3 = Vector3.one;

        public override string ToString()
        {
            return a + " : " + v3;
        }
    }

    void Start()
    {
        tests s1 = new tests();
        tests s2;
        s2 = App.SaveService.LoadFromPlayerPrefs<tests>("test");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
