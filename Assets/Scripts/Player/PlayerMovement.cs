using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashForce = 15f;
    public float dashDuration = 0.2f; // Berapa lama dash berlangsung
    public float dashCooldown = 1f;
    [SerializeField] bool canDash = true;

    [Header("References")]
    private PlayerGetMousePosition _mousePosRef;
    private Rigidbody _rb;

    [HideInInspector] public bool isDashing = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _mousePosRef = GetComponent<PlayerGetMousePosition>();
        
        _rb.isKinematic = false;
    }

    public void StartDash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash && !PauseManager.instance.isPaused && !OpenInventory.instance.isPaused)
        {
            StartCoroutine(PerformDash(_mousePosRef.screenSpace));
        }
    }

    public IEnumerator PerformDash(Vector3 camerapos)
    {
        isDashing = true;
        canDash = false;

        Ray ray = Camera.main.ScreenPointToRay(camerapos);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            Vector3 targetPoint = ray.GetPoint(rayDistance);

            Vector3 dashDirection = (targetPoint - transform.position).normalized;
            dashDirection.y = 0;

            _rb.linearVelocity = dashDirection * dashForce;

            //camera shake demo (nyoba aja)
            CameraShake.instance.ShakeCamera(intensity: 3f, time: 0.2f);
        }

        yield return new WaitForSeconds(dashDuration);
        _rb.linearVelocity = Vector3.zero;
        isDashing = false;
        
        yield return new WaitForSeconds(dashCooldown - dashDuration);
        canDash = true;
        Debug.Log("Dash siap digunakan");
    }
}