using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Weapon : NetworkBehaviour
{
   public enum WeaponType
   {
       AK47,
       M4A1,
       USP,
   }
   public int Damage { get; set; }
   public int Ammo { get; set; }
   public WeaponType weaponType;

   void Start()
   {
     InitWeapon();
   }

   public void InitWeapon()
   {
       switch (weaponType)
       {
           case WeaponType.AK47:
               Damage = 25;
               Ammo = 10;
               break;
           case WeaponType.M4A1:
               Damage = 20;
               Ammo = 15;
               break;
           case WeaponType.USP:
               Damage = 10;
               Ammo = 60;
               break;
           default:
               throw new ArgumentOutOfRangeException();
       }
    }

   private void OnTriggerEnter2D(Collider2D col)
   {
       if (col.tag=="Player")
       {
           col.GetComponent<PlayerController>().AcceptWeapon(this);
           Destroy(this.gameObject);
       }
   }

}
