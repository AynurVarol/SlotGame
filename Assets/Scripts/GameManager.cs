using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject entryCanvas; // Giriþ canvasý
    public SlotMachine slotMachine;


    void Start()
    {
        // Baþlangýçta giriþ canvasýný aktif hale getir ve oyun canvasýný pasif hale getir
        entryCanvas.SetActive(true);
        
      
    }

    public void StartGame()
    {
        // Oyuna baþlamak için giriþ canvasýný pasif hale getir ve oyun canvasýný aktif hale getir
        entryCanvas.SetActive(false);
        // SlotMachine sýnýfýndaki CreateSlotMachineTable fonksiyonunu çaðýrmak için;
        slotMachine.CreateSlotMachineTable();


    }

    public void QuitGame()
    {// Oyundan çýkýþ yapmak için bir onay mesajý göster
    
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void RestartGame()
    {
        // Oyunu yeniden baþlat
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainGameScene()
    {
        // Ana oyun sahnesine geçiþ yap
        SceneManager.LoadScene("Sample Scene"); 
    }
}
