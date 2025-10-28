using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float range = 20f;
    [SerializeField] private float flySpeed = 100f;
    [SerializeField] private float minDistance = 0.1f;
    [SerializeField] private float destroyDelay = 1f;
    [SerializeField] private GameObject bulletBody;
    [SerializeField] private GameObject bulletImpact;
    [SerializeField] private float damage = 35f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject bulletFeedbacks;
    [SerializeField] private bool destroyBulletImpacts = true;
    [SerializeField] private ProjectileGoThroughBooleans projectilesGoThroughBooleans;
    [SerializeField] private bool shouldDamagePlayer = false;
    [SerializeField] private bool isSnowball = false;
    [SerializeField] private bool isRollingBomb;
    [SerializeField] private bool isGrenadeLauncherBullet;
    [Header("Push Enemies On Hit")][Space(5)]
    [SerializeField] private bool pushEnemiesOnHit;
    [SerializeField] private float pushDistance;
    [SerializeField] private float pushTime = 0.25f;
    [Header("Ricochet")][Space(5)]
    [SerializeField] private bool doesRicochet;
    [SerializeField] private int maxNumberOfRicochets = 2;
    private Vector3 startPosition;
    private const float PROJECTILE_LAYER = 18;
    private bool isMoving = true;
    private float criticalHitChance;
    private float criticalHitDamage;
    private bool isCriticalBullet;
    private float rollingBombDamageMultiplier;
    private int numberOfRicochets = 0;
    private HashSet<GameObject> alreadyHitObjects = new();
    private const int OBSTACLES_LAYER_MASK = 256;
    private Vector3 reflectionNormal;
    private List<GameObject> myBulletImpacts = new();

    public GameObject BulletFeedbacks => bulletFeedbacks;
    public float FlySpeed => flySpeed;
    public float Damage { get => damage; set => damage = value; }
    public float Range { get => range; set => range = value; }
    public ProjectileGoThroughBooleans ProjectilesGoThroughBooleans { get => projectilesGoThroughBooleans; set => projectilesGoThroughBooleans = value; }
    public float RollingBombDamageMultiplier { get => rollingBombDamageMultiplier; set => rollingBombDamageMultiplier = value; }

    private void OnDisable()
    {
        isMoving = false;

        if (gameObject.activeInHierarchy)
            StartCoroutine(CheckIfBulletStuck());
    }

    private IEnumerator CheckIfBulletStuck()
    {
        yield return null;

        if (bulletBody == null) yield break;
                
        StartCoroutine(CreateImpactAndDestroyBullet());
    }

    private void Start()
    {
        startPosition = transform.position;
        isMoving = true;

        if (isSnowball) return;

        damage *= GameProgressionManager.Instance.LevelingAndUpgrading.DamageIncreaseMultiplier;
        criticalHitChance = GameProgressionManager.Instance.LevelingAndUpgrading.CriticalChance;
        criticalHitDamage = damage * 0.01f * GameProgressionManager.Instance.LevelingAndUpgrading.skillsCommonData.criticalHitDamagePercentage;
        isCriticalBullet = Random.Range(0f, 1f) < criticalHitChance;
        damage = isCriticalBullet ? criticalHitDamage : damage;

        if (!Globals.IsSpecialActive) return;

        range *= 0.01f * EnergyManager.Instance.SpecialBulletRangePercentage;
        damage *= 0.01f * EnergyManager.Instance.SpecialBulletDamagePercentage;
        flySpeed *= 0.01f * EnergyManager.Instance.SpecialBulletSpeedPercentage;
    }

    private void Update()
    {
        Fly();
        CheckRange();
    }

    private void Fly()
    {
        if (!isMoving) return;

        for (int i = 0; i < flySpeed / minDistance; i++)
        {
            transform.position += minDistance * Time.deltaTime * transform.forward;
        }
    }

    private void CheckRange()
    {
        if (Utilities.GetSquaredDistance2D(startPosition.x, startPosition.z, transform.position.x, transform.position.z) >= Utilities.GetSquaredFloat(range))
        {
            if (isGrenadeLauncherBullet)
                AutoDetonateGrenadeLauncherBullet();
            else
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isMoving)
            CheckTriggerHit(other, out _);
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleGrenadeLauncherBulletAndTargetDummyCollision(collision);
        HandleGrenadeLauncherBulletAndSnowballCollision(collision);
    }

    private void HandleGrenadeLauncherBulletAndTargetDummyCollision(Collision collision)
    {
        if (!collision.collider.CompareTag("TargetDummy")) return;

        GetComponent<Collider>().enabled = false;
        PlaySound();
        StartCoroutine(CreateImpactAndDestroyBullet());
        collision.collider.GetComponent<IDamageable>().TakeDamage(damage, true, isCriticalBullet);
    }

    private void HandleGrenadeLauncherBulletAndSnowballCollision(Collision collision)
    {
        if (isSnowball && (collision.collider.CompareTag("Snowball") || collision.collider.gameObject.layer == PROJECTILE_LAYER))
        {
            GetComponent<Collider>().enabled = false;
            PlaySound();
            StartCoroutine(CreateImpactAndDestroyBullet());
        }
    }

    private IEnumerator CreateImpactAndDestroyBullet()
    {
        isMoving = false;
        GameObject myBulletImpact = Instantiate(bulletImpact, transform.position, Quaternion.identity);
        myBulletImpacts.Add(myBulletImpact);

        if (isRollingBomb)
        {
            int percentageIncrease = (int)(100f * rollingBombDamageMultiplier) - 100;
            myBulletImpact.GetComponentInChildren<DamageCollider>().IncreaseDamagesByPercentage(percentageIncrease);
        }

        Destroy(bulletBody);

        yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject);

        if (!destroyBulletImpacts) yield break;

        for (int i = 0; i < myBulletImpacts.Count; i++)
        {
            Destroy(myBulletImpacts[i]);
        }
    }

    private void HandleBulletImpactsOrRicochets(Collider other, DamageableType type)
    {       
        if (!doesRicochet || numberOfRicochets >= maxNumberOfRicochets)
        {
            StartCoroutine(CreateImpactAndDestroyBullet());

            return;
        }

        if (type == DamageableType.Enemy || type == DamageableType.Player)
            reflectionNormal = transform.position - other.transform.position;
        else
        {
            if (Physics.Raycast(transform.position - transform.forward, transform.forward, out RaycastHit hit, Mathf.Infinity, OBSTACLES_LAYER_MASK))
                reflectionNormal = hit.normal;
        }

        numberOfRicochets++;
        Vector3 deflectionDirection = Vector3.Reflect(transform.forward, reflectionNormal);
        deflectionDirection.y = 0f;
        transform.forward = deflectionDirection.normalized;
    }

    public bool CheckTriggerHit(Collider other, out DamageableType typeHit)
    {
        if (alreadyHitObjects.Contains(other.gameObject))
        {
            typeHit = DamageableType.EmptyType;
            return false;
        }

        alreadyHitObjects.Add(other.gameObject);

        if (other.gameObject.layer == PROJECTILE_LAYER)
        {
            if (other.CompareTag("Snowball"))
            {
                typeHit = DamageableType.Enemy;
                PlaySound();
                StartCoroutine(CreateImpactAndDestroyBullet());

                return true;
            }

            if (isSnowball)
            {
                typeHit = DamageableType.EmptyType;
                PlaySound();
                StartCoroutine(CreateImpactAndDestroyBullet());
                GetComponent<Collider>().enabled = false;

                return false;
            }

            typeHit = DamageableType.EmptyType;

            return false;
        }

        if (other.CompareTag("DashCollider"))
        {
            typeHit = DamageableType.Player;

            return false;
        }

        if (other.TryGetComponent(out IDamageable damageable))
        {
            DamageableType damageableType = damageable.DamageableType;
            typeHit = damageableType;

            if (!shouldDamagePlayer && damageableType == DamageableType.Player)
                return false;

            damageable.TakeDamage(damage, true, isCriticalBullet);

            if (shouldDamagePlayer && damageableType == DamageableType.Player)
            {
                HandleBulletImpactsOrRicochets(other, damageableType);

                return true;
            }

            if (damageableType == DamageableType.Enemy || damageableType == DamageableType.TargetDummy)
            {
                if (pushEnemiesOnHit && damageableType != DamageableType.TargetDummy)
                    ((EnemyHealth)damageable).PushOnBulletHit(transform.forward, pushDistance, pushTime);

                if (isSnowball || isGrenadeLauncherBullet || !Globals.IsSpecialActive && !projectilesGoThroughBooleans.shouldPassThroughEnemies)
                    HandleBulletImpactsOrRicochets(other, damageableType);

                return true;
            }

            if (damageableType == DamageableType.Nest)
            {
                HandleBulletImpactsOrRicochets(other, damageableType);

                return true;
            }

            if (damageableType == DamageableType.Obstacle || damageableType == DamageableType.Explodable || damageableType == DamageableType.AnimationTrigger)
            {
                if (isSnowball || !Globals.IsSpecialActive && !projectilesGoThroughBooleans.shouldPassThroughDamageableObstacles)
                    HandleBulletImpactsOrRicochets(other, damageableType);

                return true;
            }

            return false;
        }

        if (other.TryGetComponent(out IShakeable shakeable))
            shakeable.Shake();

        PlaySound();
        
        HandleBulletImpactsOrRicochets(other, DamageableType.EmptyType);
        typeHit = DamageableType.EmptyType;

        return false;
    }

    private void PlaySound()
    {
        audioSource.Play();
    }

    public void AutoDetonateGrenadeLauncherBullet()
    {
        if (!isMoving) return;

        StartCoroutine(CreateImpactAndDestroyBullet());
    }
}