using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [Header("Sonic Charge Settings")]
    public float minDashDistance = 3f;
    public float maxDashDistance = 12f;
    public float chargeTimeForMax = 2f; // Detik yang dibutuhkan untuk max charge
    public float minHoldTime = 0.5f;
    private float _chargeStartTime;
    private bool _isCharging;

    public PlayerMovement playerMovement;
    
    [Header("References")]
    public Rigidbody rb; // Pastikan assign via Inspector

    public void SonicCharge(InputAction.CallbackContext context)
    {
        if (playerMovement.isDashing == false)
        {
            // STARTED: Mulai menghitung waktu
            if (context.started)
            {
                _isCharging = true;
                _chargeStartTime = Time.time;
                Debug.Log("Charging started...");
            }
            // CANCELED: Tombol dilepas, lakukan dash
            else if (context.canceled && _isCharging && context.duration >= minHoldTime)
            {
                _isCharging = false;
                float holdDuration = Time.time - _chargeStartTime;

                // Hitung jarak (Clamp agar tidak kurang dari min atau lebih dari max)
                float chargeFactor = Mathf.Clamp01(holdDuration / chargeTimeForMax);
                float finalDashDistance = Mathf.Lerp(minDashDistance, maxDashDistance, chargeFactor);

                PerformSonicDash(finalDashDistance);
            }
        }
        
    }

    private void PerformSonicDash(float distance)
    {
        // Ambil posisi mouse saat dash dilepas
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            Vector3 targetPoint = ray.GetPoint(rayDistance);
            Vector3 dashDirection = (targetPoint - transform.position).normalized;
            dashDirection.y = 0;

            // Terapkan dash
            // Menggunakan MovePosition atau velocity tergantung style game anda
            rb.AddForce(dashDirection * (distance * 5f), ForceMode.Impulse);
            
            Debug.Log($"Sonic Slash! Jarak: {distance:F2}");
        }
    }
}