using System;
using UnityEngine;
using UnityEngine.UI;
using Weapon;

namespace UI
{
    public class MagazinUI : MonoBehaviour
    {
        [SerializeField] Text ammoDisplay;
        
        public void UpdateUI(int magazinAmmo, int magazinSize)
        {
            ammoDisplay.text = magazinAmmo.ToString() + " / " + magazinSize.ToString();
        }
    }
}
