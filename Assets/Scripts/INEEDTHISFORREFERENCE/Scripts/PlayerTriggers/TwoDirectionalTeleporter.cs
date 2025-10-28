using System.Collections;
using UnityEngine;

public class TwoDirectionalTeleporter : MonoBehaviour
{
    [SerializeField] private TeleporterTrigger entryPointA;
    [SerializeField] private TeleporterTrigger entryPointB;
    [SerializeField] private float teleportDelay = 0.5f;
    [SerializeField] private GameObject cover;

    private IEnumerator TeleportWithDelay(TeleporterTrigger destinationTrigger, Player triggeringPlayer)
    {
        if (cover != null)
            cover.SetActive(true);

        yield return new WaitForSeconds(teleportDelay);

        destinationTrigger.PlayerInTrigger = true;
        destinationTrigger.TriggeringPlayer = triggeringPlayer;
        Globals.PlayerOne.transform.position = destinationTrigger.transform.position;
    }

    public void TeleportFromEntry(TeleporterTrigger entryTrigger, Player triggeringPlayer)
    {
        TeleporterTrigger destinationTrigger = null;

        if (entryTrigger.Equals(entryPointA))
            destinationTrigger = entryPointB;
        else if (entryTrigger.Equals(entryPointB))
            destinationTrigger = entryPointA;

        StartCoroutine(TeleportWithDelay(destinationTrigger, triggeringPlayer));
    }
}