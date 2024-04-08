using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotObjects : MonoBehaviour
{
    
    public int ID; // objenin kimliði
    public string Name; // Objenin adý 
   public float  Multiplier; // Objeye ait çarpan deðeri
    public int Order;
    



        // Kurucu metod, bir slot objesi oluþtururken gerekli bilgileri alýr
        public SlotObjects(int id, string name, float multiplier, int order)
        {
            ID = id;
            Name = name;
            Multiplier = multiplier;
             Order = order;
        
             



        }

    


  



}
