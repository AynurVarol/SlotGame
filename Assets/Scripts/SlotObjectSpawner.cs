using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SlotObjectSpawner : MonoBehaviour
{
    public GameObject[] slotObjectPrefabs; // Oluþturulacak obje prefablarý
    public Transform tableParent;

    void Start()
    {


        SpawnObjects(tableParent) ;
        
    }

   public void SpawnObjects(Transform tableParent)
    {
        // Her bir hücreye rastgele slot objesi yerleþtir
        foreach (Transform colmn in tableParent)
        {
            // Her bir hücreye rastgele slot objesi yerleþtir
            foreach (Transform cell in colmn)
            {
                int randomIndex = Random.Range(0, slotObjectPrefabs.Length);
                GameObject randomPrefab = slotObjectPrefabs[randomIndex];


                // Rastgele bir slot objesi oluþtur ve hücreye yerleþtir
                GameObject newSlotObject = Instantiate(randomPrefab, cell);
                newSlotObject.transform.localPosition = Vector3.zero;
            }
        }
    }
}



