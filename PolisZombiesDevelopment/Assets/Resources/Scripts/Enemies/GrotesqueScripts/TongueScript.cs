using System.Collections;
using UnityEngine;

public class TongueScript : MonoBehaviour
{
    public float maxStretchLength = 5f; // Maximum length the tongue can stretch
    public float attackDuration = 1f; // Duration of the attack (stretch out and back)
    public float pullSpeed;
    public float pullDuration;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private Vector3 originalScale;
    private float timer;
    private bool isAttacking;
    public bool damaged = false;
    private const float minColliderHeight = 0.1f; // Minimum collider height threshold
    private Transform zombieTransform;

    void Start()
    {
        pullSpeed = 7f;
        pullDuration = 0.5f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        originalScale = transform.localScale;
        zombieTransform = transform.parent;
        ResetTongue();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" && !damaged)
        {
            damaged = true;
            PlayerScript playerScript = other.gameObject.GetComponent<PlayerScript>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(1);
                playerScript.TemporarilyRemoveMovement(pullDuration);
                StartCoroutine(PullPlayerTowardsZombie(other.transform));
            }
            else
            {
                Debug.LogError("PlayerScript not found on the Player GameObject.");
            }
        }
    }

    void Update()
    {
        if (isAttacking)
        {
            timer += Time.deltaTime;
            float progress = timer / attackDuration;

            if (progress <= 0.5f)
            {
                // Stretching out
                float stretchProgress = progress / 0.5f;
                StretchTongue(stretchProgress);
            }
            else if (progress <= 1f)
            {
                // Retracting
                float retractProgress = (progress - 0.5f) / 0.5f;
                StretchTongue(1f - retractProgress);
            }
            else
            {
                // End of attack
                isAttacking = false;
                ResetTongue();
                damaged = false; // Reset damage flag for the next attack cycle
            }
        }
    }

    public void StartAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            timer = 0f;
            damaged = false; // Reset damage flag at the start of a new attack
        }
    }

    private void StretchTongue(float progress)
    {
        float newLength = Mathf.Lerp(0, maxStretchLength, progress);
        transform.localScale = new Vector3(originalScale.x, newLength, originalScale.z);

        // Adjust the position so the bottom of the tongue stays at the zombie's mouth
        float tongueBottomY = newLength * 2; // Adjust this based on your specific sprite pivot
        transform.localPosition = new Vector3(transform.localPosition.x, tongueBottomY, transform.localPosition.z);

        // Ensure collider height is above minimum threshold
        float colliderHeight = Mathf.Max(minColliderHeight, newLength);
        boxCollider.size = new Vector2(boxCollider.size.x, colliderHeight);
        boxCollider.offset = new Vector2(0, colliderHeight / 2);

        // Log size and offset for debugging
        Debug.Log($"StretchTongue - Progress: {progress}, New Length: {newLength}, Collider Size: {boxCollider.size}, Offset: {boxCollider.offset}");

        // Validate collider to prevent warning
        ValidateCollider();
    }

    private void ResetTongue()
    {
        transform.localScale = new Vector3(originalScale.x, minColliderHeight, originalScale.z); // Avoid zero scale
        boxCollider.size = new Vector2(boxCollider.size.x, minColliderHeight);
        boxCollider.offset = new Vector2(0, minColliderHeight / 2);

        // Reset position
        transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);

        // Validate collider to prevent warning
        ValidateCollider();
    }

    private void ValidateCollider()
    {
        // Ensure collider size is valid
        if (boxCollider.size.y < minColliderHeight)
        {
            boxCollider.size = new Vector2(boxCollider.size.x, minColliderHeight);
            boxCollider.offset = new Vector2(0, minColliderHeight / 2);
        }
        
        // Log size and offset for debugging
        Debug.Log($"ValidateCollider - Collider Size: {boxCollider.size}, Offset: {boxCollider.offset}");
    }

IEnumerator PullPlayerTowardsZombie(Transform player)
{
    Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();
    if (playerRigidbody != null)
    {
        // Calculate the direction towards the zombie
        Vector2 direction = (transform.parent.position - player.position).normalized;

        // Apply an impulse force to the player
        playerRigidbody.AddForce(direction * pullSpeed, ForceMode2D.Impulse);

        // Debug log to confirm the impulse was applied
        Debug.Log($"Impulse applied to pull player towards zombie with force: {direction * pullSpeed}");

        // Wait for 0.5 seconds before allowing any other actions
        yield return new WaitForSeconds(pullDuration);

        // Optional: Stop the player's movement after the duration (if needed)
        playerRigidbody.velocity = Vector2.zero;

        // Debug log to confirm the player's movement was stopped
        Debug.Log("Player movement stopped after pull duration.");
    }
    else
    {
        Debug.LogError("Rigidbody2D not found on the Player GameObject.");
    }
}
}