using MoreMountains.TopDownEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private ParticleSystem onShootParticles;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource reloadAudioSource;
    [SerializeField] private RandomAudioClipPlayer shootRandomAudioClipPlayer;
    [SerializeField] private Animator animator;
    [SerializeField] private List<Transform> firepoints;
    [SerializeField] private GameObject shootablePrefab;
    [SerializeField] private GameObject weaponBody;
    [SerializeField] private WeaponLaserSight weaponLaser;
    [Space(25)][Header("Damage Collider Weapons")][Space(10)]
    [SerializeField] private GameObject damageCollider;
    [Space(25)][Header("Chargeable Weapons")][Space(10)]
    [SerializeField] private GameObject chargeObject;
    [SerializeField] private Animator chargeAnimator;
    [SerializeField] private GameObject maxChargedShootablePrefab;
    [Space(25)]
    [SerializeField] private List<GameObject> objectsToSwitchEnabledStateOnPickup;
    private bool canShoot = true;
    private bool isReloading = false;
    private InputAction fireAction;
    private InputAction reloadAction;
    private WeaponParent weaponParent;
    private bool isSpecialActive = false;
    
    // Automatic firing
    private const int BULLETS_TO_BE_ENQUEUED = 15;
    private const float SPECIAL_EFFECTS_PLAY_FREQUENCY_CAP = 0.05f;
    private const float BACKWARDS_CHECK_SPHERECAST_RADIUS = 0.25f;
    private const float DISTANCE_BEHIND_FIREPOINT_CHECKED = 1.25f;
    private const int PROJECTILE_HIT_LAYER_MASK = 208640;
    private bool isAutomaticFiring = false;
    private IEnumerator automaticSpecialEffects;
    private float lastShootTime;
    private Queue<BulletSpawnInfo> bulletSetsQueue = new();
    private BulletSpawnInfo latestDequeuedBulletSpawnInfo;
    private BulletSpawnInfo latestEnqueuedBulletSpawnInfo;
    private float bulletFlySpeed;
    private float specialBulletFlySpeed;
    
    // Chargeable weapons
    private bool isCharging = false;
    private bool isFirstCharged = false;
    private bool isSecondCharged = false;
    private float chargingTimeModifier = 1f;
    private Coroutine shootCoroutine;
    private Coroutine reloadCoroutine;

    // Properties
    public bool CanShoot { get => canShoot; set => canShoot = value; }
    public WeaponParent WeaponParent { get => weaponParent; set => weaponParent = value; }
    public bool IsSpecialActive { get => isSpecialActive; set => isSpecialActive = value; }
    public WeaponType WeaponType { get => weaponType; set => weaponType = value; }
    public WeaponData WeaponData => weaponData;
    public GameObject DamageCollider => damageCollider;
    public WeaponLaserSight WeaponLaser => weaponLaser;
    
    // Animator hashes
    private static readonly int ShootHash = Animator.StringToHash("Shoot");
    private static readonly int Charge1 = Animator.StringToHash("Charge_1");
    private static readonly int Charge2 = Animator.StringToHash("Charge_2");

    // Ammo queries
    public int CurrentAmmo => AmmoManager.Instance.GetMagazineAmmo(weaponType);
    public int MaxAmmo => AmmoManager.Instance.GetMaxMagazineAmmo(weaponType);
    public int ReserveAmmo => AmmoManager.Instance.GetReserveAmmo(weaponData.ammoType);

    private void Awake()
    {
        automaticSpecialEffects = PlayAutomaticSpecialEffects();
    }

    private void OnEnable()
    {
        EnergyManager.OnSpecialAbilityActivation += OnSpecialAbilityActivation;
        EnergyManager.OnSpecialAbilityDeactivation += OnSpecialAbilityDeactivation;
        AmmoManager.OnMagazineAmmoChanged += OnMagazineAmmoChanged;

        if (!weaponParent) return;

        fireAction = weaponParent.Player.IsPlayerOne ? InputManager.Instance.PlayerInputActions.Player1.Fire : InputManager.Instance.PlayerInputActions.Player2.Fire;
        reloadAction = weaponParent.Player.IsPlayerOne ? InputManager.Instance.PlayerInputActions.Player1.Reload : InputManager.Instance.PlayerInputActions.Player2.Reload;
        
        if (!weaponParent.Player.ActionsTempDisabled)
        {
            fireAction.Enable();
            reloadAction.Enable();
        }
        
        AdjustAmmoFillImage();
        weaponParent.FillImage.color = Color.white;
        canShoot = true;
        isReloading = false;

        if (AmmoManager.Instance.IsMagazineEmpty(weaponType)) 
            reloadCoroutine = StartCoroutine(Reload());
    }

    private void OnDisable()
    {
        EnergyManager.OnSpecialAbilityActivation -= OnSpecialAbilityActivation;
        EnergyManager.OnSpecialAbilityDeactivation -= OnSpecialAbilityDeactivation;
        AmmoManager.OnMagazineAmmoChanged -= OnMagazineAmmoChanged;
        
        fireAction.Disable();
        reloadAction.Disable();

        if (weaponData.IsChargeable)
        {
            chargeObject.SetActive(false);
            isCharging = false;
            isFirstCharged = false;
            isSecondCharged = false;
        }

        StopAllCoroutines();

        if (isReloading)
            weaponParent.StopReloadAnimation();
    }

    private void Start()
    {
        canShoot = true;
        weaponLaser.gameObject.SetActive(true);

        if (UpgradeManager.Instance)
            UpgradeManager.Instance.ReapplyAmmoBoosts(weaponParent.Player);

        AdjustAmmoFillImage();
        weaponParent.FillImage.color = Color.white;

        if (!shootablePrefab.TryGetComponent(out Bullet bullet))
            return;

        bulletFlySpeed = bullet.FlySpeed;
        specialBulletFlySpeed = bulletFlySpeed * 0.01f * EnergyManager.Instance.SpecialBulletSpeedPercentage;
    }
    
    private void OnMagazineAmmoChanged(WeaponType changedWeaponType, int currentAmmo, int maxAmmo)
    {
        if (changedWeaponType != weaponType)
            return;
        
        AdjustAmmoFillImage();
        
        if (currentAmmo == 0 && !isReloading)
            reloadCoroutine = StartCoroutine(Reload());
    }

    private void OnSpecialAbilityActivation(float specialAbilityDuration, int index)
    {
        if (isAutomaticFiring)
        {
            StopAutomaticFiring();

            if (shootCoroutine != null)
                StopCoroutine(shootCoroutine);

            canShoot = true;
            animator.SetBool(ShootHash, false);
        }

        if (!isReloading) return;

        StopCoroutine(reloadCoroutine);
        AmmoManager.Instance.InstantReload(weaponType);
        weaponParent.FillImage.fillAmount = 1f;
        weaponParent.FillImage.color = Color.white;
        canShoot = true;
        isReloading = false;
    }

    private void OnSpecialAbilityDeactivation(int emptiedSpecialSlotIndex = -1)
    {
        StopAutomaticFiring();
    }

    private void Update()
    {
        if (!weaponParent) return;

        if (weaponParent.Player.PlayerHealth.IsDead || PauseMenuController.IsPaused) return;

        if (!isReloading)
            ShootController();
        else if (isAutomaticFiring)
            StopAutomaticFiring();

        if (reloadAction.WasPressedThisFrame() && CurrentAmmo != MaxAmmo)
            reloadCoroutine = StartCoroutine(Reload());

        CheckMovementSlow();
    }

    private void ShootController()
    {
        int currentAmmo = CurrentAmmo;
        
        if (weaponData.IsChargeable)
            HandleChargeableShoot();
        else if (weaponData.UsesDamageCollider)
            HandleDamageColliderShoot();
        else if (weaponData.IsAutomatic)
        {
            if (fireAction.IsPressed() && canShoot && currentAmmo > 0)
                shootCoroutine = StartCoroutine(Shoot(true));
            else if (!fireAction.IsPressed() && isAutomaticFiring)
                StopAutomaticFiring();
        }
        else if (fireAction.WasPressedThisFrame() && canShoot && currentAmmo > 0)
            StartCoroutine(Shoot(false));
    }

    private void HandleChargeableShoot()
    {
        if (!isCharging && !isFirstCharged && fireAction.IsPressed())
            StartCoroutine(nameof(ChargeWeapon));
        else if (isCharging && fireAction.WasReleasedThisFrame())
            HandleChargeableEarlyRelease();
        else if (!isCharging && isSecondCharged && fireAction.WasReleasedThisFrame())
            HandleChargeableFullyChargedShot();
    }

    private void HandleChargeableEarlyRelease()
    {
        if (!isFirstCharged)
            StopFirstCharge(true);
        else if (isFirstCharged && !isSecondCharged)
        {
            StopSecondCharge(true);

            if (canShoot && CurrentAmmo > 0)
            {
                StartCoroutine(Shoot(false));
                isFirstCharged = false;
            }
        }
    }

    private void HandleChargeableFullyChargedShot()
    {
        StopFullyChargedEffects();

        if (canShoot && CurrentAmmo > 0)
        {
            StartCoroutine(Shoot(false));
            isFirstCharged = false;
            isSecondCharged = false;
        }
    }

    private IEnumerator ChargeWeapon()
    {
        StartFirstCharge();
        yield return new WaitForSecondsRealtime(isSpecialActive ? weaponData.SpecialFirstChargingTime * chargingTimeModifier : weaponData.FirstChargingTime * chargingTimeModifier);
        StopFirstCharge(false);
        StartSecondCharge();
        yield return new WaitForSecondsRealtime(isSpecialActive ? weaponData.SpecialSecondChargingTime * chargingTimeModifier : weaponData.SecondChargingTime * chargingTimeModifier);
        StopSecondCharge(false);
    }

    private void StartFirstCharge()
    {
        isCharging = true;
        canShoot = false;
        PlayFirstChargingEffects();
    }

    private void StopFirstCharge(bool isEarlyReleased)
    {
        canShoot = true;

        if (isEarlyReleased)
        {
            StopFirstChargingEffects(isEarlyReleased);
            isCharging = false;
            StopCoroutine(nameof(ChargeWeapon));
            return;
        }

        isFirstCharged = true;
    }

    private void StartSecondCharge()
    {
        PlaySecondChargingEffects();
    }

    private void StopSecondCharge(bool isEarlyReleased)
    {
        if (isEarlyReleased)
        {
            isFirstCharged = false;
            StopCoroutine(nameof(ChargeWeapon));
        }
        else
        {
            isSecondCharged = true;
            PlayFullyChargedEffects();
        }

        isCharging = false;
        StopSecondChargingEffects(isEarlyReleased);
    }

    private void PlayFirstChargingEffects()
    {
        chargeObject.SetActive(true);
    }

    private void StopFirstChargingEffects(bool isEarlyReleased)
    {
        if (isEarlyReleased) chargeObject.SetActive(false);
    }

    private void PlaySecondChargingEffects()
    {
        chargeAnimator.SetTrigger(Charge1);
    }

    private void StopSecondChargingEffects(bool isEarlyReleased)
    {
        if (isEarlyReleased)
        {
            chargeAnimator.ResetTrigger(Charge1);
            chargeObject.SetActive(false);
        }
    }

    private void PlayFullyChargedEffects()
    {
        chargeAnimator.ResetTrigger(Charge1);
        chargeAnimator.SetTrigger(Charge2);
    }

    private void StopFullyChargedEffects()
    {
        chargeAnimator.SetTrigger(Charge2);
        chargeObject.SetActive(false);
    }

    private void HandleDamageColliderShoot()
    {
        int currentAmmo = CurrentAmmo;
        
        if (!damageCollider.activeInHierarchy && fireAction.IsPressed() && canShoot && currentAmmo > 0)
        {
            damageCollider.SetActive(true);
            StartCoroutine(nameof(HandleDamageColliderWeaponEffectsAndAmmo));
        }
        else if (damageCollider.activeInHierarchy && (fireAction.WasReleasedThisFrame() || currentAmmo <= 0 || !canShoot))
        {
            damageCollider.SetActive(false);
            StopCoroutine(nameof(HandleDamageColliderWeaponEffectsAndAmmo));
            audioSource.Stop();

            if (currentAmmo == 0)
                reloadCoroutine = StartCoroutine(Reload());
        }
    }

    private IEnumerator Shoot(bool isAutomatic)
    {
        canShoot = false;
        animator.SetBool(ShootHash, true);

        if (weaponData.SlowMovementSpeedOnShoot)
        {
            lastShootTime = Time.time;

            if (!weaponParent.IsMovementSpeedSlowedByShooting)
                weaponParent.ApplyMovementSpeedSlow(weaponData.SlowingFactor);
        }

        if (isAutomatic)
            CreateShootableAutomatic(shootablePrefab);
        else if (weaponData.IsChargeable)
            CreateShootableNonAutomatic(isSecondCharged ? maxChargedShootablePrefab : shootablePrefab);
        else
            CreateShootableNonAutomatic(shootablePrefab);

        AdjustAmmoFillImage();

        if (CurrentAmmo == 0)
            reloadCoroutine = StartCoroutine(Reload());

        yield return new WaitForSeconds(GetTimeBetweenShots());

        canShoot = true;
        animator.SetBool(ShootHash, false);
    }

    private void CreateShootableAutomatic(GameObject shootableGameObject)
    {
        if (!isAutomaticFiring)
        {
            isAutomaticFiring = true;
            HandleFirstBulletSpecialCase();
        }

        if (isAutomaticFiring)
            HandleAutomaticFiring(shootableGameObject);
    }

    private void HandleFirstBulletSpecialCase()
    {
        EnqueueFirstBullet(new BulletSpawnInfo(Time.time));
        StartCoroutine(automaticSpecialEffects);
    }

    private void HandleAutomaticFiring(GameObject shootableGameObject)
    {
        for (int i = bulletSetsQueue.Count; i > 0; i--)
        {
            if (bulletSetsQueue.Peek().timeAtSpawn <= Time.time)
                InstantiateAutomaticShootable(shootableGameObject);
            else break;
        }

        if (bulletSetsQueue.Count > 0)
            EnqueueBullet(latestEnqueuedBulletSpawnInfo);
        else
            EnqueueBullet(latestDequeuedBulletSpawnInfo);
    }

    private GameObject InstantiateAutomaticShootable(GameObject shootableGameObject)
    {
        GameObject instantiatedShootable = null;
        latestDequeuedBulletSpawnInfo = bulletSetsQueue.Dequeue();
        float timeDiff = Time.time - latestDequeuedBulletSpawnInfo.timeAtSpawn;
        float flySpeed = (isSpecialActive ? specialBulletFlySpeed : bulletFlySpeed);

        for (int j = 0; j < firepoints.Count; j++)
        {
            instantiatedShootable = Instantiate(shootableGameObject, firepoints[j].position + firepoints[j].forward * flySpeed * timeDiff, firepoints[j].rotation);

            if (!isSpecialActive && CurrentAmmo > 0)
            {
                AmmoManager.Instance.TrySpendMagazineAmmo(weaponType, 1);
            }

            BackwardsCheckIfSpawnedAheadOfTarget(instantiatedShootable, j);
        }

        if (firepoints.Count == 3) ActivateFeedbacks(instantiatedShootable);

        return instantiatedShootable;
    }

    private void BackwardsCheckIfSpawnedAheadOfTarget(GameObject bulletToCheckGameObject, int firepointIndex)
    {
        Vector3 pointToCheckFrom = firepoints[firepointIndex].position - firepoints[firepointIndex].forward * DISTANCE_BEHIND_FIREPOINT_CHECKED;
        float castMaxDistance = (bulletToCheckGameObject.transform.position - pointToCheckFrom).magnitude;
        RaycastHit[] hits = Physics.SphereCastAll(pointToCheckFrom, BACKWARDS_CHECK_SPHERECAST_RADIUS, firepoints[firepointIndex].forward, castMaxDistance, PROJECTILE_HIT_LAYER_MASK, QueryTriggerInteraction.Collide);

        if (hits.Length == 0) return;

        HitDistanceFromPoint[] distances = new HitDistanceFromPoint[hits.Length];

        for (int i = 0; i < hits.Length; i++)
        {
            distances[i].index = i;
            distances[i].distance = hits[i].distance;
        }

        Array.Sort(distances);
        Bullet bulletToCheck = bulletToCheckGameObject.GetComponent<Bullet>();

        for (int i = 0; i < distances.Length; i++)
        {
            if (bulletToCheck.CheckTriggerHit(hits[distances[i].index].collider, out DamageableType typeHit))
            {
                if (typeHit == DamageableType.Enemy && bulletToCheck.ProjectilesGoThroughBooleans.shouldPassThroughEnemies)
                    continue;

                if (typeHit == DamageableType.Obstacle && bulletToCheck.ProjectilesGoThroughBooleans.shouldPassThroughDamageableObstacles)
                    continue;

                bulletToCheck.enabled = false;
                break;
            }
        }
    }

    private void EnqueueFirstBullet(BulletSpawnInfo bulletSpawnInfo)
    {
        latestEnqueuedBulletSpawnInfo = new BulletSpawnInfo(bulletSpawnInfo.timeAtSpawn);
        bulletSetsQueue.Enqueue(latestEnqueuedBulletSpawnInfo);
    }

    private void EnqueueBullet(BulletSpawnInfo bulletSpawnInfo)
    {
        for (int j = 0; j < BULLETS_TO_BE_ENQUEUED; j++)
        {
            latestEnqueuedBulletSpawnInfo = new BulletSpawnInfo(bulletSpawnInfo.timeAtSpawn + (j + 1) * GetTimeBetweenShots());
            bulletSetsQueue.Enqueue(latestEnqueuedBulletSpawnInfo);
        }
    }

    private void StopAutomaticFiring()
    {
        StopCoroutine(automaticSpecialEffects);
        isAutomaticFiring = false;
        bulletSetsQueue.Clear();
    }

    private void CreateShootableNonAutomatic(GameObject shootableGameObject)
    {
        for (int i = 0; i < firepoints.Count; i++)
        {
            GameObject instantiatedShootable = Instantiate(shootableGameObject, firepoints[i].position, firepoints[i].rotation);

            if (i == 2)
                ActivateFeedbacks(instantiatedShootable);

            PlaySpecialEffects();

            if (!WeaponData.IsChargeable && !IsSpecialActive || WeaponData.IsChargeable && i == 0)
            {
                AmmoManager.Instance.TrySpendMagazineAmmo(weaponType, 1);
            }                

            AdjustAmmoFillImage();

            if (!WeaponData.IsChargeable && CurrentAmmo == 0 || WeaponData.IsChargeable && !isSecondCharged)
                break;
        }
    }

    private IEnumerator HandleDamageColliderWeaponEffectsAndAmmo()
    {
        while (true)
        {
            if (CurrentAmmo > 0 && !isSpecialActive)
            {
                AmmoManager.Instance.TrySpendMagazineAmmo(weaponType, 1);
            }

            PlaySpecialEffects();
            float timeBetweenShots = GetTimeBetweenShots();

            yield return new WaitForSeconds(timeBetweenShots / Time.timeScale < SPECIAL_EFFECTS_PLAY_FREQUENCY_CAP ? SPECIAL_EFFECTS_PLAY_FREQUENCY_CAP : timeBetweenShots);
        }
    }

    private IEnumerator PlayAutomaticSpecialEffects()
    {
        while (true)
        {
            PlaySpecialEffects();
            float timeBetweenShots = GetTimeBetweenShots();

            yield return new WaitForSeconds(timeBetweenShots / Time.timeScale < SPECIAL_EFFECTS_PLAY_FREQUENCY_CAP ? SPECIAL_EFFECTS_PLAY_FREQUENCY_CAP : timeBetweenShots);
        }
    }

    private void PlaySpecialEffects()
    {
        PlaySound();
        onShootParticles.Play();
    }

    private void PlaySound()
    {
        audioSource.clip = shootRandomAudioClipPlayer.RandomAudioClip;
        audioSource.Play();
    }

    private float GetTimeBetweenShots()
    {
        float shootDelay = weaponData.TimeBetweenShots;

        if (isSpecialActive)
            shootDelay /= 0.01f * EnergyManager.Instance.SpecialBulletSpeedPercentage;

        return shootDelay;
    }

    private void ActivateFeedbacks(GameObject shootable)
    {
        Bullet bullet = shootable.GetComponent<Bullet>();

        if (bullet) bullet.BulletFeedbacks.SetActive(true);
    }

    private void CheckMovementSlow()
    {
        if (weaponParent.IsMovementSpeedSlowedByShooting && lastShootTime + weaponData.SlowDuration < Time.time)
            weaponParent.RemoveMovementSpeedSlow(weaponData.SlowingFactor);
    }

    private IEnumerator Reload()
    {
        if (weaponParent.ReloadSpikesActivated)
            weaponParent.Player.PlayerUpgradesController.ReloadSpikesController.UnleashSpikes();

        ReloadInfo reloadInfo = AmmoManager.Instance.CalculateReload(weaponType);

        if (!reloadInfo.isValid || reloadInfo.ammoToAdd == 0)
        {
            StopAutomaticFiring();
            yield break;
        }

        canShoot = false;
        isReloading = true;
        reloadAudioSource.Play();

        weaponParent.FillImage.color = Color.gray;
        float startFill = weaponParent.FillImage.fillAmount;
        float currentTime = 0f;

        float endFill = startFill + (float)reloadInfo.ammoToAdd / MaxAmmo;
        float reloadTime = (weaponData.IsChargeable && isSpecialActive)
            ? weaponData.SpecialReloadTime 
            : weaponData.MaxReloadTime * reloadInfo.ammoToAdd / MaxAmmo;
        
        weaponParent.StartReloadAnimation();

        while (currentTime < reloadTime)
        {
            if (Time.timeScale != 0f)
                currentTime += Time.unscaledDeltaTime;

            weaponParent.FillImage.fillAmount = startFill + (currentTime / reloadTime) * (endFill - startFill);

            yield return null;
        }

        weaponParent.StopReloadAnimation();

        // perform the actual reload in the AmmoManager
        AmmoManager.Instance.PerformReload(weaponType);
        
        weaponParent.FillImage.fillAmount = endFill;
        weaponParent.FillImage.color = Color.white;
        canShoot = true;
        isReloading = false;
    }

    public void AdjustAmmoFillImage()
    {
        weaponParent.FillImage.fillAmount = (float)CurrentAmmo / MaxAmmo;
    }

    public void IncreaseAmmo(int percentage)
    {
        if (weaponData.IsChargeable)
        {
            chargingTimeModifier -= percentage * 0.01f;

            if (chargingTimeModifier < weaponData.MinimumChargingTimeModifier)
                chargingTimeModifier = weaponData.MinimumChargingTimeModifier;

            return;
        }

        int increase = Mathf.RoundToInt(MaxAmmo * percentage * 0.01f);

        if (increase == 0) increase = 1;
        
        AmmoManager.Instance.IncreaseMaxMagazineCapacity(weaponType, increase);
        AmmoManager.Instance.SetMagazineAmmo(weaponType, MaxAmmo);
        
        weaponParent.FillImage.fillAmount = 1f;
        weaponParent.FillImage.color = Color.white;
    }

    public void HandleObjectsToSwitchEnabledStateOnPickup()
    {
        foreach (GameObject go in objectsToSwitchEnabledStateOnPickup)
        {
            go.SetActive(!go.activeInHierarchy);
        }
    }

    /// <summary>
    /// Initialize weapon when first equipped.
    /// </summary>
    public void InitializeWeapon()
    {
	    if (!weaponData.NewAmmoStruct.HasInfiniteAmmo)
	    {
		    AmmoManager.Instance.AddReserveAmmo(weaponData.ammoType, weaponData.NewAmmoStruct.DefaultExtraAmmo);
	    }
	    
        AmmoManager.Instance.InitializeMagazine(
            weaponType,
            weaponData.ammoType,
            weaponData.MaxAmmo,
            0,
            weaponData.NewAmmoStruct.HasInfiniteAmmo);

        AmmoManager.Instance.PerformReload(weaponType);
    }
    
    /// <summary>
    /// Restore weapon state from AmmoManager (weapon switching, respawn)
    /// AmmoManager already has the magazine state for this WeaponType.
    /// </summary>
    public void RestoreWeaponState()
    {
        if (AmmoManager.Instance.GetMagazineData(weaponType) == null)
        {
            AmmoManager.Instance.InitializeMagazine(
                weaponType,
                weaponData.ammoType,
                weaponData.MaxAmmo,
                weaponData.MaxAmmo,
                weaponData.NewAmmoStruct.HasInfiniteAmmo
            );
        }
    }

    /// <summary>
    /// Set weapon state for weapon pickup (includes extra ammo)
    /// </summary>
    /// <param name="magazineAmmo">Current ammo to set.</param>
    /// <param name="extraAmmo">Extra ammo to add to reserve.</param>
    public void SetWeaponStateFromPickup(int extraAmmo)
    {
	    // Add extra ammo to reserves first if not infinite
	    if (!weaponData.NewAmmoStruct.HasInfiniteAmmo && extraAmmo > 0)
	    {
		    AmmoManager.Instance.AddReserveAmmo(weaponData.ammoType, extraAmmo);
	    }
        
        AmmoManager.Instance.InitializeMagazine(
            weaponType,
            weaponData.ammoType,
            weaponData.MaxAmmo,
            0,
            weaponData.NewAmmoStruct.HasInfiniteAmmo);
        
        AmmoManager.Instance.PerformReload(weaponType);
    }
}