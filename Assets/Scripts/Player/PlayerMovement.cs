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
    private bool canDash = true;

    [Header("References")]
    public PauseManager pauseManager;
    private PlayerGetMousePosition _mousePosRef;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _mousePosRef = GetComponent<PlayerGetMousePosition>();
        
        _rb.isKinematic = false;
    }

    public void StartDash(InputAction.CallbackContext context)
    {
        
        if (context.performed && canDash && !pauseManager.isPaused)
        {
            StartCoroutine(PerformDash());
        }

        if (context.started)
        {
            Debug.Log("Input dash terdeteksi oleh sistem.");
        }

        if (context.performed)
        {
            if (!canDash)
            {
                Debug.Log("Dash gagal: Masih dalam cooldown.");
                return;
            }

            if (pauseManager.isPaused)
            {
                Debug.Log("Dash gagal: Game sedang di-pause.");
                return;
            }

            Debug.Log("Dash berhasil di-trigger!");
            StartCoroutine(PerformDash());
        }
    }

    private IEnumerator PerformDash()
    {
        canDash = false;

        
        Ray ray = Camera.main.ScreenPointToRay(_mousePosRef.screenSpace);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero); 
        
        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            Vector3 targetPoint = ray.GetPoint(rayDistance);
            
            
            Vector3 dashDirection = (targetPoint - transform.position).normalized;
            dashDirection.y = 0; 

            
            _rb.linearVelocity = dashDirection * dashForce;
        }

       
        yield return new WaitForSeconds(dashDuration);
        _rb.linearVelocity = Vector3.zero;

        
        yield return new WaitForSeconds(dashCooldown - dashDuration);
        canDash = true;
        Debug.Log("Dash siap digunakan");
    }

    //void Update()
    //{
    //    if (Mouse.current == null) { Debug.LogError("No mouse device!"); return; }
    //
    //    if (Mouse.current.leftButton.wasPressedThisFrame)
    //        Debug.Log("RAW left click detected");
    //
    //    Debug.Log("RAW mouse pos: " + Mouse.current.position.ReadValue());
    //}
}