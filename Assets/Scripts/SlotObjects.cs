using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotObjects : MonoBehaviour
{
    
    public int ID; // Objeye özgü benzersiz kimlik
    public string Name; // Objenin adý veya sembolü (opsiyonel)
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

    


    /* SlotObjects sýnýfýnda Equals ve GetHashCode metodlarýný override ediyoruz
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        SlotObjects other = (SlotObjects)obj;
        return ID == other.ID && Name == other.Name;
    }

    public override int GetHashCode()
    {
        return (ID << 16) ^ Name.GetHashCode();
    }*/



}
