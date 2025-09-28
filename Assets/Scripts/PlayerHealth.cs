using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerHealth : MonoBehaviour
{
    [Header("Kill Conditions")]
    [SerializeField] private string obstacleTag = "Obstacle";
    [SerializeField] private bool killOnTrigger = false; // enable only if you add trigger obstacles

    [Header("Bounds Kill")]
    [SerializeField] private float killYUpper = 0.2f;
    [SerializeField] private float killYLower = -0.2f;

    private bool isDead = false;
    private CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
            Debug.LogError("PlayerHealth requires a CharacterController on the same GameObject.");
    }

    void Update()
    {
        if (!isDead && (transform.position.y > killYUpper || transform.position.y < killYLower))
        {
            Die("Bounds");
        }
    }

    // Fires on NON-TRIGGER colliders when moving via CharacterController.Move/SimpleMove
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Debug visuals
        Debug.DrawRay(hit.point, hit.normal, Color.blue, 2f);
        string parentName = hit.transform.parent ? hit.transform.parent.name : "No Parent";
        Debug.LogWarning($"IMPACT DETECTED! Hit Object: '{hit.gameObject.name}' | With Tag: '{hit.gameObject.tag}' | Parent: '{parentName}'");

        if (!isDead && hit.collider != null && hit.collider.isTrigger == false && hit.gameObject.CompareTag(obstacleTag))
        {
            Die($"Collision:{hit.gameObject.name}");
        }
    }

    // Optional: enable if you decide to use trigger obstacles.
    void OnTriggerEnter(Collider other)
    {
        if (!killOnTrigger || isDead) return;

        // Note: CharacterController does NOT register trigger events for other triggers by itself.
        // If you want this path, add a separate trigger collider to the player (and usually a kinematic Rigidbody on the obstacle or the player).
        if (other != null && other.gameObject.CompareTag(obstacleTag))
        {
            Debug.Log($"Trigger with '{other.gameObject.name}' (tag {other.gameObject.tag})");
            Die($"Trigger:{other.gameObject.name}");
        }
    }

    private void Die(string reason)
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"Game Over! Reason = {reason}");

        // It’s safest to switch scenes BEFORE pausing time.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // If your GameOver scene relies on normal time, reset there (e.g., in a bootstrap script),
        // or just avoid pausing here:
        // Time.timeScale = 0f;

        SceneManager.LoadScene("GameOver");
    }
}