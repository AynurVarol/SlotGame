using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotObjects : MonoBehaviour
{
    
    public int ID; // Objeye �zg� benzersiz kimlik
    public string Name; // Objenin ad� veya sembol� (opsiyonel)
   public float  Multiplier; // Objeye ait �arpan de�eri
    public int Order;
    



        // Kurucu metod, bir slot objesi olu�tururken gerekli bilgileri al�r
        public SlotObjects(int id, string name, float multiplier, int order)
        {
            ID = id;
            Name = name;
            Multiplier = multiplier;
             Order = order;
        
             



        }

    


  



}
