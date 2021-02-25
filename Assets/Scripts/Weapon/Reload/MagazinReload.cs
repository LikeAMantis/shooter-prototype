using System.Collections;
using UnityEngine;

namespace Weapon.Reload
{
    public class MagazinReload : MonoBehaviour, IReload
    {
        public float reloadSpeed = 1f;
        public void Reload(WeaponShoot context)
        {
            StartCoroutine(ReloadWait(context));
        }

        IEnumerator ReloadWait(WeaponShoot context)
        {
            yield return new WaitForSeconds(reloadSpeed);
            context.MagazinAmmo = context.MagazinSize;
            context.MagazinUI.UpdateUI(context.MagazinSize, context.MagazinAmmo);
        }
    }
}