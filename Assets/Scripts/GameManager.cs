using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject entryCanvas; // Giri� canvas�
    public SlotMachine slotMachine;


    void Start()
    {
        // Ba�lang��ta giri� canvas�n� aktif hale getir ve oyun canvas�n� pasif hale getir
        entryCanvas.SetActive(true);
        
      
    }

    public void StartGame()
    {
        // Oyuna ba�lamak i�in giri� canvas�n� pasif hale getir ve oyun canvas�n� aktif hale getir
        entryCanvas.SetActive(false);
        // SlotMachine s�n�f�ndaki CreateSlotMachineTable fonksiyonunu �a��rmak i�in;
        slotMachine.CreateSlotMachineTable();


    }

    public void QuitGame()
    {// Oyundan ��k�� yapmak i�in bir onay mesaj� g�ster
    
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void RestartGame()
    {
        // Oyunu yeniden ba�lat
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainGameScene()
    {
        // Ana oyun sahnesine ge�i� yap
        SceneManager.LoadScene("Sample Scene"); 
    }
}
