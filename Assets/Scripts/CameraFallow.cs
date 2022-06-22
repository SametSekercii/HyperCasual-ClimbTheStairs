using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFallow : MonoBehaviour
{
    Vector3 diffVector;
    public Transform ScoreTable;
    void Start()
    {
        
        diffVector = ScoreTable.position-transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = ScoreTable.position - diffVector;
    }
}
