using UnityEngine;

public class WeaponShooting : MonoBehaviour
{
    [Header("Weapon Settings")]
    public float weaponRange = 100f;  // How far the weapon can shoot
    public int damage = 10;           // Damage dealt by the weapon

    [Header("Effects")]
    public ParticleSystem muzzleFlash;    // Primary muzzle flash effect
    public ParticleSystem secondaryEffect; // Secondary effect (e.g., smoke or sparks)
    public GameObject hitEffect;           // Hit effect prefab

    [Header("Audio")]
    public AudioSource audioSource;       // Audio source for all weapon sounds
    public AudioClip gunshotSound;        // Gunshot sound clip
    public AudioClip aimSound;            // Sound played when entering aiming mode

    [Header("Aiming Settings")]
    public Camera playerCamera;          // Camera used for aiming
    public float zoomFOV = 40f;          // Field of View when aiming
    public float normalFOV = 60f;        // Field of View for normal view
    public float zoomSpeed = 10f;        // Speed of FOV transition
    private bool isAiming = false;       // Is the player currently aiming?

    [Header("Animation")]
    public Animator weaponAnimator;      // Animator for weapon animations
    public Animator playerAnimator;      // Animator for player animations

    void Update()
    {
        HandleAiming();
        HandleShooting();
    }

    void HandleAiming()
    {
        if (Input.GetButtonDown("Fire2")) // Right mouse button for aiming
        {
            isAiming = true;
            PlaySound(aimSound);

            // Trigger aiming animation for weapon
            if (weaponAnimator != null)
            {
                weaponAnimator.SetBool("IsAiming", true); // Trigger aiming animation
            }

            // Trigger aiming animation for player
            if (playerAnimator != null)
            {
                playerAnimator.SetBool("IsAiming", true);
            }
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            isAiming = false;

            // Reset aiming animation for weapon
            if (playerAnimator != null)
            {
                weaponAnimator.SetBool("IsAiming", false); // Trigger return animation
            }

            // Reset aiming animation for player
            if (playerAnimator != null)
            {
                playerAnimator.SetBool("IsAiming", false);
            }
        }

        // Smooth transition for zooming in/out
        float targetFOV = isAiming ? zoomFOV : normalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);
    }

    void HandleShooting()
    {
        if (Input.GetButtonDown("Fire1")) // Left mouse button for shooting
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Play primary muzzle flash effect
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        // Play secondary effect
        if (secondaryEffect != null)
        {
            secondaryEffect.Play();
        }

        // Play gunshot sound
        PlaySound(gunshotSound);

        // Raycast from the camera to detect what is hit
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, weaponRange))
        {
            // Log the object hit (for debugging)
            Debug.Log("Hit: " + hit.collider.name);

            // Apply damage to the object if it has a health script
            EnemyHealth targetHealth = hit.collider.GetComponent<EnemyHealth>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage);
            }

            // Show hit effect if a prefab is assigned
            if (hitEffect != null)
            {
                Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
