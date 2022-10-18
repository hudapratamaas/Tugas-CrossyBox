using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    //static akan membuat variable ini shared pada semua tree
    public static List<Vector3> AllPosition = new List<Vector3>();

    public void OnEnable()
    {
        AllPosition.Add(this.transform.position);
        Debug.Log(AllPosition.Count);  
    }
    public void OnDisable()
    {
        AllPosition.Remove(this.transform.position);
    }
}
