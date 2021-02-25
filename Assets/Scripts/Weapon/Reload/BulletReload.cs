using System.Collections;
using UnityEngine;

namespace Weapon.Reload
{
    public class BulletReload : Reloadable
    {
        public float reloadSpeedPerBullet = 0.5f;

        protected override IEnumerator ReloadWait(WeaponShoot context)
        {
            while (context.MagazinAmmo < context.MagazinSize && base.isReloading)
            {
                context.MagazinAmmo++;
                context.MagazinUI.UpdateUI(context.MagazinAmmo, context.MagazinSize);
                yield return new WaitForSeconds(reloadSpeedPerBullet);    
            }
        }
    }
}