using System;
using System.Collections;
using UnityEngine;
using UI;
using Random = UnityEngine.Random;
using Weapon.Reload;

namespace Weapon
{
    public enum WeaponMode { Single, Automatic, Burst, }

    [RequireComponent(typeof(Reloadable))]
    public class WeaponShoot : MonoBehaviour
    {
        #region Fields / Propertys
        
            public AudioClip weaponSound;
            public Camera playerCamera;
            public GameObject bulletHole;
            public static float test = 2f; 

            [Header("Weapon Settings")]
            public float shootDistance = 100f;

            public float shootForce = 100f;
            public float sprayAngle = 5f;

            [Header("Magazin")] 
            [SerializeField] int magazinSize = 10;

            [Header("Recoil")]
            public float recoil = 3f;

            public float recoilResetSpeed = 5f;

            [Header("Weapon Mode")]
            public WeaponMode weaponMode = WeaponMode.Single;

            [SerializeField] float fireRate = 10f;
            public bool burstToggleActive = false;
            [SerializeField] float burstRate = 5f;
            public int shotsPerBurst = 3;

            public MagazinUI MagazinUI { get; private set; }
            public int MagazinAmmo { get; set; }
            public int MagazinSize => magazinSize;

            float FireRateInterval => 60 / fireRate;
            float BurstRateInterval => 60 / burstRate;

            AudioSource audioSource;
            bool allowedToShoot = true;
            float currentRecoil = 0f;
            bool isRecoilResetRoutineRunning;
            Transform cameraTransform;
            Reloadable reload;
            Animator animator;
            ParticleSystem particle;

            public event Action ShotEvent;

        #endregion

        #region Events

            void Awake()
            {
                playerCamera = playerCamera ? playerCamera : Camera.main;
                if (playerCamera != null) audioSource = playerCamera.GetComponent<AudioSource>();
                cameraTransform = transform;
                MagazinAmmo = MagazinSize;
                MagazinUI = FindObjectOfType<MagazinUI>();
                reload = GetComponent<Reloadable>();
                animator = GetComponentInChildren<Animator>();
                particle = GetComponentInChildren<ParticleSystem>();
            }

            void Start()
            {
                MagazinUI.UpdateUI(MagazinAmmo, MagazinSize);
            }

            void Update()
            {
                UseWeaponMode(weaponMode);

                if (Input.GetButtonDown("Fire2") && burstToggleActive)
                {
                    BurstToggle();
                }

                if (Input.GetKeyDown(KeyCode.R))
                {
                    reload.Reload(this);
                }
            }

        #endregion

        #region Methods

            void UseWeaponMode(WeaponMode mode)
            {
                switch(mode) 
                {
                    case WeaponMode.Single :
                        SingleShot();
                        break;
                    case WeaponMode.Automatic:
                        AutomaticShot();
                        break;
                    case WeaponMode.Burst:
                        BurstShot();
                        break;
                }
            }

            void SingleShot() 
            {
                if (Input.GetButtonDown("Fire1") && allowedToShoot) 
                {
                    Shot();
                    StartCoroutine(FireRateWait(FireRateInterval));
                }
            }

            void AutomaticShot()
            {
                if (Input.GetButton("Fire1") && allowedToShoot)
                {
                    Shot();
                    StartCoroutine(FireRateWait(FireRateInterval));
                }
            }

            void BurstShot()
            {
                if (Input.GetButtonDown("Fire1") && allowedToShoot)
                {
                    Shot();
                    StartCoroutine(BurstWait(BurstRateInterval));
                    StartCoroutine(FireRateWait(FireRateInterval));
                }
            }

            void Shot()
            {
                if (!UpdateMagazin()) return;

                ShotEvent?.Invoke();
                particle.Play();
                animator.Play("Shoot");
                
                
                var ray = new Ray {origin = cameraTransform.position, direction = AddSpray(cameraTransform)};

                Physics.Raycast(ray, out var hit, shootDistance);
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.blue, 2);
                Debug.DrawRay(ray.origin, cameraTransform.forward * hit.distance, Color.green, 2);
                

                BulletImpact(hit);

                audioSource.PlayOneShot(weaponSound);

                var playerCameraTransform = playerCamera.transform;
                Recoil(playerCameraTransform);
                RecoilReset(playerCameraTransform);
            }

            void BulletImpact(RaycastHit hit)
            {
                var hitCollider = hit.collider;
                if (hitCollider != null)
                {
                    var point = hit.point;

                    var forward = transform.forward;
                    CreateBulletHole(point, forward, hit.transform);

                    var forceFallOff = Mathf.Pow(1 - 1 / shootDistance, hit.distance);
                    var attachedRigidbody = hitCollider.attachedRigidbody;
                    if (attachedRigidbody != null && attachedRigidbody.tag.Equals("Interactable")) attachedRigidbody.AddForceAtPosition(forward * (shootForce * forceFallOff), point);

                    // Debug.Log("<color=blue>Distance: </color>" + hit.distance);
                    // Debug.Log("<color=yellow>Impact: </color>" + forceFallOff);
                }
            }

            Vector3 AddSpray(Transform t)
            {
                var spray = Quaternion.Euler(0,0, Random.Range(0f, 360f)) * (Quaternion.Euler(Random.Range(0, sprayAngle), 0, 0) * Vector3.forward);
                return t.TransformDirection(spray);
            }

            private bool UpdateMagazin()
            {
                if (MagazinAmmo < 1) return false;
                MagazinAmmo -= 1;
                MagazinUI.UpdateUI(MagazinAmmo, MagazinSize);
                return true;
            }

            void Recoil(Transform t) {
                t.eulerAngles += new Vector3(-recoil, 0, 0);

                if (currentRecoil > recoil) {
                    currentRecoil += .5f;
                } else {
                    currentRecoil = recoil;
                }
            }

            void RecoilReset(Transform t) {
                if (!isRecoilResetRoutineRunning) {
                    StartCoroutine(RecoilResetEnumerator(t));
                }
            }

            IEnumerator RecoilResetEnumerator(Transform t) {
                isRecoilResetRoutineRunning = true;
                var waitForEndOfFrame = new WaitForEndOfFrame();

                while (true) {
                    if (currentRecoil < 0) {
                        isRecoilResetRoutineRunning = false;
                        yield break;
                    }

                    var recoilDelta = recoil * Time.deltaTime * (1 / recoilResetSpeed);
                    t.eulerAngles += new Vector3(recoilDelta, 0, 0);
                    currentRecoil -= recoilDelta;
                    yield return waitForEndOfFrame;
                }
            }

            void CreateBulletHole(Vector3 position, Vector3 direction, Transform parent) {
                var instance = GameObject.Instantiate(bulletHole, position, Quaternion.FromToRotation(Vector3.forward, direction));

                var transformInstance = instance.transform;

                transformInstance.parent = parent;
                transformInstance.eulerAngles += new Vector3(0, 0, Random.Range(0, 360));
            }

            IEnumerator FireRateWait(float seconds) {
                allowedToShoot = false;
                yield return new WaitForSeconds(seconds);
                allowedToShoot = true;
            }

            IEnumerator BurstWait(float rate) {
                for (var i = 0; i < shotsPerBurst; i++) {
                    Shot();
                    yield return new WaitForSeconds(rate);
                }
            }

            void BurstToggle()
            {
                switch (weaponMode)
                {
                    case WeaponMode.Single:
                        weaponMode = WeaponMode.Burst;
                        break;
                    case WeaponMode.Burst:
                        weaponMode = WeaponMode.Single;
                        break;
                }
            }

        #endregion
    }
}