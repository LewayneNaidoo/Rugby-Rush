using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieldGenerator : MonoBehaviour
{
    public GameObject fieldPrefab;
    public Vector3 lastPos;
    public float offset = 0.707f;

    public List<GameObject> fieldPieces;

    private int fieldCount = 0;

    public void StartBuilding()
    {
        InvokeRepeating("CreateNewFieldPart", 1f, 2.5f);
    }

    public void CreateNewFieldPart()
    {
        Vector3 spawnPos = Vector3.zero;

        float chance = UnityEngine.Random.Range(0, 100);

        spawnPos = new Vector3(lastPos.x, lastPos.y, lastPos.z + offset);

        GameObject g = Instantiate(fieldPrefab, spawnPos, Quaternion.Euler(0, 0, 0));
        lastPos = g.transform.position;
        g.GetComponentInChildren<NavMeshSurface>().BuildNavMesh();

        fieldCount++;
        if (fieldCount % 5 == 0)
        {
            //g.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    //private void Rebake()
    //{
    //    //GameObject[] fieldPieces = GameObject.FindGameObjectsWithTag("Field");
    //    foreach (GameObject f in fieldPieces)
    //    {
    //        f.GetComponentInChildren<NavMeshSurface>().BuildNavMesh();
    //    }
    //}
}
