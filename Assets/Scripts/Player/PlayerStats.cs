using UnityEngine;

namespace GimGim.Player {
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "GimGim/Player/PlayerStats")]
    public class PlayerStats : ScriptableObject {
        [Header("Movement stats")]
        [SerializeField] public float moveSpeed = 5f;
    }
}