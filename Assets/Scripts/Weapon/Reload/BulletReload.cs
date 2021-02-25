using System;
using System.Collections;
using UI;
using UnityEngine;

namespace Weapon.Reload
{
    public class BulletReload : MonoBehaviour, IReload
    {
        bool isReloadWaitRunning = false;
        
        public float reloadSpeedPerBullet = 0.5f;
        bool reload = false; WeaponShoot weaponShoot;

        void Awake()
        {
            weaponShoot = GetComponent<WeaponShoot>();
        }

        void OnEnable()
        {
            weaponShoot.OnShot += StopReload;
        }

        void OnDisable()
        {
            weaponShoot.OnShot -= StopReload;
        }

        public void Reload(WeaponShoot context)
        {
            reload = !reload;
            if (!isReloadWaitRunning) StartCoroutine(ReloadWait(context));
        }

        IEnumerator ReloadWait(WeaponShoot context)
        {
            isReloadWaitRunning = true;
            while (context.MagazinAmmo < context.MagazinSize && reload)
            {
                context.MagazinAmmo++;
                context.MagazinUI.UpdateUI(context.MagazinAmmo, context.MagazinSize);
                yield return new WaitForSeconds(reloadSpeedPerBullet);    
            }
            isReloadWaitRunning = false;
        }

        void StopReload()
        {
            reload = false;
        }
    }
}