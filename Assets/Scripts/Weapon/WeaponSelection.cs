using System;
using System.Collections.Generic;
using UnityEngine;
using UI;

namespace Weapon
{
    public class WeaponSelection : MonoBehaviour
    {
        List<Transform> weaponList;
        GameObject currentSelection;
        MagazinUI magazinUI;

        void Awake()
        {
            magazinUI = FindObjectOfType<MagazinUI>();
        }

        void Start()
        {
            weaponList = new List<Transform>(GetComponentsInChildren<Transform>());
            
            for (var i = 1; i < transform.childCount + 1; i++)
            {
                var weapon = weaponList[i];
                if (i > 1) weapon.gameObject.SetActive(false);
            }
            currentSelection = weaponList[1].gameObject;
        }
        
        void Update()
        {
            for (var i = 1; i < 5; i++)
            {
                if (Input.GetKeyDown(i.ToString()))
                {
                    SelectWeapon(i);
                }
            }
        }

        void SelectWeapon(int index)
        {
            if (index > transform.childCount) return;
            if (currentSelection != null) currentSelection.SetActive(false);
            currentSelection = weaponList[index].gameObject;
            currentSelection.SetActive(true);

            var weaponShoot = currentSelection.GetComponent<WeaponShoot>();
            magazinUI.UpdateUI(weaponShoot.MagazinAmmo, weaponShoot.MagazinSize);
        }
    }
}