using System.Collections;
using UnityEngine;

namespace Weapon.Reload
{
    public abstract class Reloadable : MonoBehaviour
    {
        WeaponShoot weaponShoot;
        protected bool isReloading = false;

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
            isReloading = !isReloading;
            StartCoroutine(ReloadWait(context));
        }
        
        protected abstract IEnumerator ReloadWait(WeaponShoot context);

        void StopReload()
        {
            isReloading = false;
        }
    }
}