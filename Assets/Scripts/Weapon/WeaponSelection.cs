using System;
using System.Collections.Generic;
using UnityEngine;
using UI;

namespace Weapon
{
    public class WeaponSelection : MonoBehaviour
    {
        List<WeaponShoot> weaponList;
        MagazinUI magazinUI;

        GameObject currentSelection;

        public Animator CurrentSelectionAnimator { get; private set; }

        void Awake()
        {
            magazinUI = FindObjectOfType<MagazinUI>();
            InitWeaponList();
            weaponList.ForEach(Debug.Log);
        }

        void Start()
        {
        }

        void InitWeaponList()
        {
            weaponList = new List<WeaponShoot>(GetComponentsInChildren<WeaponShoot>(true));
            for (var i = 0; i < weaponList.Count; i++)
            {
                var weapon = weaponList[i].gameObject;
                weapon.gameObject.SetActive(i == 0);
            }

            SelectWeapon(0);
        }

        void Update()
        {
            for (var i = 1; i < 5; i++)
            {
                if (Input.GetKeyDown(i.ToString()))
                {
                    SelectWeapon(i-1);
                }
            }
        }

        void SelectWeapon(int index)
        {
            if (index >= weaponList.Count) return;
            
            if (currentSelection != null) currentSelection.SetActive(false);
            
            currentSelection = weaponList[index].gameObject;
            currentSelection.SetActive(true);
            CurrentSelectionAnimator = currentSelection.GetComponentInChildren<Animator>();

            var weaponShoot = currentSelection.GetComponent<WeaponShoot>();
            magazinUI.UpdateUI(weaponShoot.MagazinAmmo, weaponShoot.MagazinSize);
        }
    }
}