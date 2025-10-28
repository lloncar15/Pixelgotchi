using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class PickupBase : MonoBehaviour, IAutoCollectable, ICollectableResource
{
	[Header("Pickup Components")]
	[SerializeField] protected Collider pickupTrigger;
	[SerializeField] protected GameObject model;
	[SerializeField] protected GameObject onPickupEffectObject;
	[SerializeField] protected AudioSource audioSource;
	[SerializeField] protected float destroyDelay = 2f;
	[SerializeField] protected bool showGizmos = false;
    
	[Header("Auto Collection")]
	[SerializeField] protected float autoCollectRadius;
	[SerializeField] protected SphereCollider autoCollectTrigger;
	[SerializeField] protected AutoCollectableCommonData autoCollectableCommonData;
	
	protected bool isBeingAutoCollected;
	protected bool isPickedUp;

	protected const float INNER_TRIGGER_RADIUS = 1.35f;

	protected virtual void Start()
	{
		SetAutoCollectRadius();
	}

	protected void OnTriggerEnter(Collider other)
	{
		if (isPickedUp || Globals.PlayerOne.PlayerHealth.IsDead)
			return;

		OnPickup();
	}
	
	protected virtual void OnPickup()
	{
		isPickedUp = true;
		pickupTrigger.enabled = false;
		model.SetActive(false);

		audioSource?.Play();

		if (onPickupEffectObject is not null)
		{
			// potencijalno 'Object Pooling' ?
			Instantiate(onPickupEffectObject, transform.position, Quaternion.identity);
		}

		Invoke(nameof(DelayedDestroy), destroyDelay);
		
		CollectResource();
	}

	public abstract void CollectResource();

	protected virtual void DelayedDestroy()
	{
		Destroy(gameObject);
	}
	
	public virtual void AutoCollect(Player player)
	{
		if (isBeingAutoCollected || isPickedUp)
			return;

		autoCollectTrigger.gameObject.SetActive(false);
		StartCoroutine(MoveTowardsPlayer(player.transform));
	}
	
	public virtual IEnumerator MoveTowardsPlayer(Transform player)
	{
		isBeingAutoCollected = true;
		float startTime = Time.time;

		while (!isPickedUp)
		{
			float speed = autoCollectableCommonData.timeSpeedMovementCurve.Evaluate(Time.time - startTime);

			if (player is null)
			{
				isPickedUp = false;
				isBeingAutoCollected = false;
				yield break;
			}

			transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
			yield return null;
		}
	}
	
	public virtual void SetAutoCollectRadius()
	{
		autoCollectRadius = GameProgressionManager.Instance.LevelingAndUpgrading.AutoCollectRadius;
		autoCollectTrigger.radius = autoCollectRadius / Utilities.XZMinScale(transform.lossyScale);
	}
	
	protected virtual void OnDrawGizmos()
	{
		if (!showGizmos || isBeingAutoCollected) return;

		Gizmos.color = new Color(0f, 0f, 1f, 0.3f);
		Gizmos.DrawSphere(transform.position, autoCollectRadius);

		Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
		Gizmos.DrawSphere(transform.position, INNER_TRIGGER_RADIUS);
	}
}

public interface ICollectableResource
{
	public void CollectResource();
}