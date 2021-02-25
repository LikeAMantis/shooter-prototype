using System.Collections;
using UnityEngine;

namespace Weapon.Reload
{
    public class MagazinReload : Reloadable
    {
        public float reloadSpeed = 2f;

        protected override IEnumerator ReloadWait(WeaponShoot context)
        {
            yield return new WaitForSeconds(reloadSpeed);
            if (!isReloading) yield break;
            context.MagazinAmmo = context.MagazinSize;
            context.MagazinUI.UpdateUI(context.MagazinSize, context.MagazinAmmo);
        }
    }
}