using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class SlotMachine : MonoBehaviour
{
    public int rowCount; //sat�r say�s�
    public int columnCount; //sutun say�s�

    public GameObject slotCellPrefab; // slot h�crelerini i�eren prefab
    public Transform tableParent;// parentleri
    public GameObject[] slotObjectPrefabs;
    public SlotObjectSpawner slotObjectSpawner; //SlotObjectSpawner bile�enine referans i�in


    public float startingBet = 5f;
    public float currentBet;
    public float totalMoney = 50f;

    public Text totalMoneyText; // totalMoney metin alan� i�in referans
    public Text currentBetText; // currentBet metin alan� i�in referans
    public Canvas winCanvas;

    private List<SlotObjects> slotObjectList = new List<SlotObjects>();
    private List<GameObject> spawnedSlotObjects = new List<GameObject>();
    private GameObject lineRendererParent;
    private List<LineRenderer> lineRenderers = new List<LineRenderer>(); // Olu�turulan LineRenderer nesnelerini tutan liste

    public LineRenderer lineRenderer; // Line Renderer objesine referans
    public LineRenderer blueLineRenderer;   
    public LineRenderer redLineRenderer;   
    public LineRenderer greenLineRenderer; 
    public LineRenderer whiteLineRenderer;  


    void Start()
    {

        currentBet = startingBet; // ba�lang�� bahis miktar�
        UpdateUITexts(); // Ba�lang��ta metin alanlar�n� g�ncelle

        slotObjectList.Add(new SlotObjects(1, "Bar", 6f, 2));
        slotObjectList.Add(new SlotObjects(2, "Cherry", 7f,3));
        slotObjectList.Add(new SlotObjects(3, "Diamond", 10f, 10));
        slotObjectList.Add(new SlotObjects(4, "Lemon", 9f,9));
        slotObjectList.Add(new SlotObjects(5, "Watermelon", 8f, 8));


       

        //CreateSlotMachineTable();
        CreateLineRendererParent();

        // Line Renderer referanslar�n� kontrol et
        if (blueLineRenderer == null || redLineRenderer == null || greenLineRenderer == null || whiteLineRenderer == null)
        {
            Debug.LogError("Line Renderer references are missing!");
        }

      
        
    }

    public void CreateSlotMachineTable()
    {
        // Sat�r ve s�tun say�s�na g�re slot machine tablosu olu�tur
        for (int column = 0; column < columnCount; column++)
        {
            GameObject colmnobj = new GameObject();
            colmnobj.transform.parent = tableParent;
            for (int row = 0; row < rowCount; row++)
            {
                // Slot h�cresini olu�tur
                GameObject slotCell = Instantiate(slotCellPrefab, colmnobj.transform);

                // H�crenin pozisyonunu ayarla
                RectTransform rt = slotCell.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(column * rt.rect.width, -row * rt.rect.height);
            }
        }
    }

    public void SpawnSlotObjectsOnClick()
    {
        DestroyPreviousSlotObjects();
        slotObjectSpawner.SpawnObjects(tableParent);
        CheckAndDrawLinesForSameSlotObjects();
    }

    void DestroyPreviousSlotObjects()
    {
        GameObject[] existingSlotObjects = GameObject.FindGameObjectsWithTag("SlotObject");
        foreach (GameObject slotObject in existingSlotObjects)
        {
           DestroyImmediate(slotObject);
        }
    }

    void CreateLineRendererParent()
    {
        // Line Renderer'�n ebeveyni olu�tur
        lineRendererParent = new GameObject("LineRendererParent");
    }


    void CheckAndDrawLinesForSameSlotObjects()
    {
        blueLineRenderer.positionCount = 0;
        redLineRenderer.positionCount = 0;
        greenLineRenderer.positionCount = 0;
        whiteLineRenderer.positionCount = 0;

        var firstColumn = GetSlotObjectsInColumn(0);


        foreach (var row in firstColumn)
        {
            int nextIndex = 1;
            var sameObjects = new List<SlotObjects>();
            sameObjects.Add(row);

            for (int i = 0; i < GetSlotObjectsInColumn(nextIndex).Count; i++)
            {
                var currentColumn = GetSlotObjectsInColumn(nextIndex);
                var nextRow = currentColumn[i];

                if (row.ID == nextRow.ID)
                {
                    sameObjects.Add(nextRow);
                    nextIndex++;
                    if (nextIndex > 4)
                        break;

                    i = 0; // S�radaki d�ng� ad�m�nda 1 artt�r�lacak
                }
                
            }


            if (sameObjects.Count > 2)
            {
                DrawLineForSameSlotObjects(sameObjects);
                // Bahis miktar�n� kazan�lara ekleyin ve yeni kazan� miktar�n� hesaplay�n
                float winnings = CalculateWinnings(sameObjects[0].ID, sameObjects.Count);
                totalMoney += currentBet + winnings;

                // Kazan�lan paray� yazd�r
                Debug.Log("Kazand�n�z! Kazan�: " + (currentBet + winnings));
                Debug.Log("Kazand�n�z!");
                UpdateUITexts();

                winCanvas.gameObject.SetActive(true); // Win canvas�n� aktif hale getir

                
                Invoke("CloseWinCanvas", 0.5f); // 0.5 saniye sonra CloseWinCanvas fonksiyonunu �a��r



                return;
            }
        }

        //E�er hi�bir sat�rda ayn� slot objeleri bulunamazsa;
        Debug.Log("Kaybettiniz! Kalan para:" + totalMoney);

        totalMoney -= 10;// her kaybedildi�inde total paray� 10 azalt
        
        // Bahis miktar�n� kay�plardan ��kar�n
        totalMoney -= currentBet;
        UpdateUITexts();

        // Yeni bahis miktar�n� ayarlay�n (iste�e ba�l� olarak)
        // �rne�in, her kaybetti�inizde bahsi azaltabilirsiniz
        // currentBet -= 1f;
    }

    public void CloseWinCanvas()
    {
        winCanvas.gameObject.SetActive(false); // Win canvas�n� pasif hale getir
    }

    float CalculateWinnings(int id, int count)
    {
        float totalMultiplier = 0f;

        foreach (var slotObject in slotObjectList)
        {
            if (slotObject.ID == id)
            {
                // Slot objesinin �arpan�n� al ve toplam �arpandaki katk�s�n� hesapla
                totalMultiplier += slotObject.Multiplier;
            }
        }
        // Toplam kazan�, toplam �arpman�n bahisle �arp�lmas�yla hesaplan�r
        return totalMultiplier * currentBet;
    }


   /* float GetMultiplier(int id)
    {
        // ID'ye g�re �arpan� d�nd�r�n
        switch (id)
        {
            case 1:
                return 2f; // �rnek �arpan
            case 2:
                return 3f; // �rnek �arpan
            // Di�er case'ler buraya eklenir...
            default:
                return 1f; // Varsay�lan �arpan
        }
    }*/






    SlotObjects GetSlotObject(int row, int column)
    {
        Transform slotCellTransform = tableParent.GetChild(column).GetChild(row);
        return slotCellTransform.GetComponent<SlotObjects>();
    }







    List<SlotObjects> GetSlotObjectsInColumn(int columnIndex) //sutundaki slot objelerini toplamak i�in
    {
        List<SlotObjects> slotObjectsInColumn = new List<SlotObjects>();
        int childCount = tableParent.GetChild(columnIndex).childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = tableParent.GetChild(columnIndex);
            Transform cell = child.GetChild(i);
            SlotObjects slotObject = cell.GetComponentInChildren<SlotObjects>();
            if (slotObject != null)
            {
                slotObjectsInColumn.Add(slotObject);
            }
        }
      
        return slotObjectsInColumn;
    }

    void DrawLineForSameSlotObjects(List<SlotObjects> sameSlotObjects)
    {
        // Slot objelerinin ID'lerine g�re Line Renderer se�imi
        LineRenderer selectedLineRenderer = null;
        switch (sameSlotObjects[0].ID) // SameSlotObjects listesinin ilk eleman�n�n ID'sine bak�yoruz
        {
            case 1:
                selectedLineRenderer = blueLineRenderer;
                break;
            case 2:
                selectedLineRenderer = redLineRenderer;
                break;
            case 3:
                selectedLineRenderer = greenLineRenderer;
                break;
            case 4:
                selectedLineRenderer = whiteLineRenderer;
                break;
            default:
                Debug.LogWarning("Unhandled ID for Line Renderer!");
                break;
        }

        if (selectedLineRenderer != null)
        {
            Vector3[] linePositions = new Vector3[sameSlotObjects.Count];
            for (int i = 0; i < sameSlotObjects.Count; i++)
            {
                linePositions[i] = sameSlotObjects[i].transform.position;
            }

            selectedLineRenderer.positionCount = linePositions.Length;
            selectedLineRenderer.SetPositions(linePositions);
            selectedLineRenderer.enabled = true; // Line Renderer'� g�r�n�r hale getir

            // Kazan�lan slot objelerini debug olarak yazd�r
            string debugMessage = "Kazan�lan objeler: ";
            foreach (var slotObject in sameSlotObjects)
            {
                debugMessage += slotObject.Name + ", ";
            }
            Debug.Log(debugMessage.TrimEnd(',', ' ')); // Virg�l ve bo�luklar� temizle
        }
    }

    void UpdateUITexts()
    {
        // totalMoney ve currentBet metin alanlar�n� g�ncelle
        totalMoneyText.text = ": "    + totalMoney.ToString();
        currentBetText.text = "Current Bet: " + currentBet.ToString();
    }

    



}




