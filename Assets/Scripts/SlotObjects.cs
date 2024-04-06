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

    


    /* SlotObjects s�n�f�nda Equals ve GetHashCode metodlar�n� override ediyoruz
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
