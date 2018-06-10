﻿using System.Collections.Generic;
using UnityEngine;

public class CustomList : ScriptableObject {
 

    //This is our list we want to use to represent our class as an array.
    public List<MyClass> MyList = new List<MyClass> (1);


    void AddNew () {
        //Add a new index position to the end of our list
        MyList.Add (new MyClass ());
    }

    void Remove (int index) {
        //Remove an index position from our list at a point in our list array
        MyList.RemoveAt (index);
    }
}

//This is our custom class with our variables
[System.Serializable]
public class MyClass {
    public GameObject AnGO;
    public int AnInt;
    public float AnFloat;
    public Vector3 AnVector3;
    public int[] AnIntArray = new int[0];
    public ItemRequirements ACustomClassArray;
}