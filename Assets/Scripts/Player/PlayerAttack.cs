using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [Header("Sonic Charge Settings")]
    public float minDashDistance = 3f;
    public float maxDashDistance = 12f;
    public float chargeTimeForMax = 2f; // Detik yang dibutuhkan untuk max charge
    public float minHoldTime = 0.5f;
    private float _finalDashDistance;
    private float _chargeStartTime;
    private bool _isCharging;
    private bool _isFullyDecayed;

    [Header("Charge Status")]
    [RangeWithFormat(0f, 1f, "F2")]
    [SerializeField]
    private float _currentChargeFactor = 0f;
    public float CurrentChargeFactor => _currentChargeFactor;

    private bool _isDecaying = false;
    private Coroutine _decayCoroutine;

    [Header("Sonic Charge Decay Settings")]
    [SerializeField] private float _fullChargeDelay = 0.2f;
    [SerializeField] private float _decayRate = 0.1f;
    [SerializeField] private float _decayInterval = 0.1f;

    [Header("References")]
    public Rigidbody rb;
    public Slider dashIndicator;
    private PlayerMovement _playerMovement;
    private PlayerGetMousePosition _mousePosRef;

    public void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _mousePosRef = GetComponent<PlayerGetMousePosition>();
    }

    private void Start()
    {
        dashIndicator.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_isCharging && !PauseManager.instance.isPaused && !OpenInventory.instance.isPaused)
        {
            if (!_isDecaying && !_isFullyDecayed)
            {
                float currentHoldDuration = Time.time - _chargeStartTime;
                _currentChargeFactor = Mathf.Clamp01(currentHoldDuration / chargeTimeForMax);
                dashIndicator.value = _currentChargeFactor;
            }

            if (_currentChargeFactor >= 0.99f && !_isDecaying && _decayCoroutine == null)
            {
                _isDecaying = true;
                _decayCoroutine = StartCoroutine(HandleFullChargeAndDecay());
            }
        }
    }

    private IEnumerator HandleFullChargeAndDecay()
    {
        Debug.Log("[Player Attack] Fully charged! Waiting before decay...");
        yield return new WaitForSeconds(_fullChargeDelay);

        while (_isCharging && _currentChargeFactor > 0)
        {
            _currentChargeFactor -= _decayRate;
            _currentChargeFactor = Mathf.Max(0, _currentChargeFactor);

            dashIndicator.value = _currentChargeFactor;

            if (_currentChargeFactor <= 0)
            {
                Debug.Log("[Player Attack] Fully Decayed");
                _isFullyDecayed = true;
                break;
            }
            yield return new WaitForSeconds(_decayInterval);
        }

        _isDecaying = false;
        _decayCoroutine = null;
    }


    private void StopDecay()
    {
        if (_decayCoroutine != null)
        {
            StopCoroutine(_decayCoroutine);
            _decayCoroutine = null;
        }
        _isDecaying = false;
        _isFullyDecayed = false;
    }

    
    public void SonicCharge(InputAction.CallbackContext context)
    {
        if (_playerMovement.isDashing == false && !PauseManager.instance.isPaused && !OpenInventory.instance.isPaused)
        {
            // STARTED: Mulai menghitung waktu
            if (context.started)
            {
                _isCharging = true;
                _chargeStartTime = Time.time;
                _currentChargeFactor = 0f;
                dashIndicator.gameObject.SetActive(true);
                dashIndicator.value = 0f;

                // Stop any ongoing decay
                StopDecay();

                Debug.Log("[Player Attack] Charging started...");
            }

            // CANCELED: Tombol dilepas, lakukan dash
            else if (context.canceled && _isCharging && context.duration >= minHoldTime)
            {
                _isCharging = false;

                if (_isFullyDecayed)
                {
                    _finalDashDistance = 0f;
                }
                else
                {
                    _finalDashDistance = Mathf.Lerp(minDashDistance, maxDashDistance, _currentChargeFactor);
                }

                Debug.Log($"[Player Attack] Final Dash distance: {_finalDashDistance}");

                StartCoroutine(PerformDash(_mousePosRef.screenSpace, _finalDashDistance));
                StopDecay();
                dashIndicator.gameObject.SetActive(false);

                _currentChargeFactor = 0f;
            }

            // Optional: Handle if canceled too early (below minHoldTime)
            else if (context.canceled && _isCharging && context.duration < minHoldTime)
            {
                StopDecay();
                _isCharging = false;
                dashIndicator.gameObject.SetActive(false);
                _currentChargeFactor = 0f;
                Debug.Log("[Player Attack] Charge canceled - hold too short");
            }
        }
    }

    /// <summary>
    /// dash depends on how long its hold
    /// </summary>
    /// <param name="camerapos"></param>
    /// <param name="dashForce"></param>
    /// <returns></returns>
    public IEnumerator PerformDash(Vector3 camerapos, float dashForce)
    {
        Ray ray = Camera.main.ScreenPointToRay(camerapos);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            Vector3 targetPoint = ray.GetPoint(rayDistance);

            Vector3 dashDirection = (targetPoint - transform.position).normalized;
            dashDirection.y = 0;

            rb.linearVelocity = dashDirection * dashForce;
            Debug.Log(dashForce);

            if (!_isFullyDecayed)
            {
                CameraShake.instance.ShakeCamera(intensity: 3f, time: 0.2f);
            }
        }

        yield return new WaitForSeconds(0.2f);
        rb.linearVelocity = Vector3.zero;
        Debug.Log("[Player Attack] Dash siap digunakan");
    }
}