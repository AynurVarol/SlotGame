using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class SlotMachine : MonoBehaviour
{
    public int rowCount; //satýr sayýsý
    public int columnCount; //sutun sayýsý

    public GameObject slotCellPrefab; // slot hücrelerini içeren prefab
    public Transform tableParent;// parentleri
    public GameObject[] slotObjectPrefabs;
    public SlotObjectSpawner slotObjectSpawner; //SlotObjectSpawner bileþenine referans için


    public float startingBet = 5f;
    public float currentBet;
    public float totalMoney = 50f;

    public Text totalMoneyText; // totalMoney metin alaný için referans
    public Text currentBetText; // currentBet metin alaný için referans
    public Canvas winCanvas;

    private List<SlotObjects> slotObjectList = new List<SlotObjects>();
    private List<GameObject> spawnedSlotObjects = new List<GameObject>();
    private GameObject lineRendererParent;
    private List<LineRenderer> lineRenderers = new List<LineRenderer>(); // Oluþturulan LineRenderer nesnelerini tutan liste

    public LineRenderer lineRenderer; // Line Renderer objesine referans
    public LineRenderer blueLineRenderer;   
    public LineRenderer redLineRenderer;   
    public LineRenderer greenLineRenderer; 
    public LineRenderer whiteLineRenderer;  


    void Start()
    {

        currentBet = startingBet; // baþlangýç bahis miktarý
        UpdateUITexts(); // Baþlangýçta metin alanlarýný güncelle

        slotObjectList.Add(new SlotObjects(1, "Bar", 6f, 2));
        slotObjectList.Add(new SlotObjects(2, "Cherry", 7f,3));
        slotObjectList.Add(new SlotObjects(3, "Diamond", 10f, 10));
        slotObjectList.Add(new SlotObjects(4, "Lemon", 9f,9));
        slotObjectList.Add(new SlotObjects(5, "Watermelon", 8f, 8));


       

        //CreateSlotMachineTable();
        CreateLineRendererParent();

        // Line Renderer referanslarýný kontrol et
        if (blueLineRenderer == null || redLineRenderer == null || greenLineRenderer == null || whiteLineRenderer == null)
        {
            Debug.LogError("Line Renderer references are missing!");
        }

      
        
    }

    public void CreateSlotMachineTable()
    {
        // Satýr ve sütun sayýsýna göre slot machine tablosu oluþtur
        for (int column = 0; column < columnCount; column++)
        {
            GameObject colmnobj = new GameObject();
            colmnobj.transform.parent = tableParent;
            for (int row = 0; row < rowCount; row++)
            {
                // Slot hücresini oluþtur
                GameObject slotCell = Instantiate(slotCellPrefab, colmnobj.transform);

                // Hücrenin pozisyonunu ayarla
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
        // Line Renderer'ýn ebeveyni oluþtur
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

                    i = 0; // Sýradaki döngü adýmýnda 1 arttýrýlacak
                }
                
            }


            if (sameObjects.Count > 2)
            {
                DrawLineForSameSlotObjects(sameObjects);
                // Bahis miktarýný kazançlara ekleyin ve yeni kazanç miktarýný hesaplayýn
                float winnings = CalculateWinnings(sameObjects[0].ID, sameObjects.Count);
                totalMoney += currentBet + winnings;

                // Kazanýlan parayý yazdýr
                Debug.Log("Kazandýnýz! Kazanç: " + (currentBet + winnings));
                Debug.Log("Kazandýnýz!");
                UpdateUITexts();

                winCanvas.gameObject.SetActive(true); // Win canvasýný aktif hale getir

                
                Invoke("CloseWinCanvas", 0.5f); // 0.5 saniye sonra CloseWinCanvas fonksiyonunu çaðýr



                return;
            }
        }

        //Eðer hiçbir satýrda ayný slot objeleri bulunamazsa;
        Debug.Log("Kaybettiniz! Kalan para:" + totalMoney);

        totalMoney -= 10;// her kaybedildiðinde total parayý 10 azalt
        
        // Bahis miktarýný kayýplardan çýkarýn
        totalMoney -= currentBet;
        UpdateUITexts();

        // Yeni bahis miktarýný ayarlayýn (isteðe baðlý olarak)
        // Örneðin, her kaybettiðinizde bahsi azaltabilirsiniz
        // currentBet -= 1f;
    }

    public void CloseWinCanvas()
    {
        winCanvas.gameObject.SetActive(false); // Win canvasýný pasif hale getir
    }

    float CalculateWinnings(int id, int count)
    {
        float totalMultiplier = 0f;

        foreach (var slotObject in slotObjectList)
        {
            if (slotObject.ID == id)
            {
                // Slot objesinin çarpanýný al ve toplam çarpandaki katkýsýný hesapla
                totalMultiplier += slotObject.Multiplier;
            }
        }
        // Toplam kazanç, toplam çarpmanýn bahisle çarpýlmasýyla hesaplanýr
        return totalMultiplier * currentBet;
    }


   /* float GetMultiplier(int id)
    {
        // ID'ye göre çarpaný döndürün
        switch (id)
        {
            case 1:
                return 2f; // Örnek çarpan
            case 2:
                return 3f; // Örnek çarpan
            // Diðer case'ler buraya eklenir...
            default:
                return 1f; // Varsayýlan çarpan
        }
    }*/






    SlotObjects GetSlotObject(int row, int column)
    {
        Transform slotCellTransform = tableParent.GetChild(column).GetChild(row);
        return slotCellTransform.GetComponent<SlotObjects>();
    }







    List<SlotObjects> GetSlotObjectsInColumn(int columnIndex) //sutundaki slot objelerini toplamak için
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
        // Slot objelerinin ID'lerine göre Line Renderer seçimi
        LineRenderer selectedLineRenderer = null;
        switch (sameSlotObjects[0].ID) // SameSlotObjects listesinin ilk elemanýnýn ID'sine bakýyoruz
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
            selectedLineRenderer.enabled = true; // Line Renderer'ý görünür hale getir

            // Kazanýlan slot objelerini debug olarak yazdýr
            string debugMessage = "Kazanýlan objeler: ";
            foreach (var slotObject in sameSlotObjects)
            {
                debugMessage += slotObject.Name + ", ";
            }
            Debug.Log(debugMessage.TrimEnd(',', ' ')); // Virgül ve boþluklarý temizle
        }
    }

    void UpdateUITexts()
    {
        // totalMoney ve currentBet metin alanlarýný güncelle
        totalMoneyText.text = ": "    + totalMoney.ToString();
        currentBetText.text = "Current Bet: " + currentBet.ToString();
    }

    



}




