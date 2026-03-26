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

    [Header("References")]
    public Rigidbody rb; // Pastikan assign via Inspector
    private PlayerMovement _playerMovement;
    private PlayerGetMousePosition _mousePosRef;

    public void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _mousePosRef = GetComponent<PlayerGetMousePosition>();
    }

    public void SonicCharge(InputAction.CallbackContext context)
    {
        if (_playerMovement.isDashing == false)
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

                StartCoroutine(_playerMovement.PerformDash(_mousePosRef.screenSpace));
            }
        }
        
    }
}