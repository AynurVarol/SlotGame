using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SlotObjectSpawner : MonoBehaviour
{
    public GameObject[] slotObjectPrefabs; // Olu�turulacak obje prefablar�
    public Transform tableParent;

    void Start()
    {


        SpawnObjects(tableParent) ;
        
    }

   public void SpawnObjects(Transform tableParent)
    {
        // Her bir h�creye rastgele slot objesi yerle�tir
        foreach (Transform colmn in tableParent)
        {
            // Her bir h�creye rastgele slot objesi yerle�tir
            foreach (Transform cell in colmn)
            {
                int randomIndex = Random.Range(0, slotObjectPrefabs.Length);
                GameObject randomPrefab = slotObjectPrefabs[randomIndex];


                // Rastgele bir slot objesi olu�tur ve h�creye yerle�tir
                GameObject newSlotObject = Instantiate(randomPrefab, cell);
                newSlotObject.transform.localPosition = Vector3.zero;
            }
        }
    }
}



