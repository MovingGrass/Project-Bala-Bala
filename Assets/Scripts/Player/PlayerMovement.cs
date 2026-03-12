using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Related")]
    public bool canDash;
    public float dashCooldown;
    public float speed;

    [Header("Current Screen")]
    public Vector2 screenSize;
    public Vector2 middle;

    [Header("Reference related")]
    public PauseManager pauseManager;
    private PlayerGetMousePosition _playerGetMousePos;

    private void Start()
    {
        canDash = true;
        _playerGetMousePos = GetComponent<PlayerGetMousePosition>();
    }

    public void StartDash(InputAction.CallbackContext context)
    {
        if (context.interaction is TapInteraction && canDash && !pauseManager.isPaused)
        {
            if (context.performed)
            {
                StartCoroutine(StartDashing(dashCooldown));
                Debug.Log("Start Dashing");
            }
        }
        else
        {
            Debug.Log("Cannot Dash");
        }
    }

    IEnumerator StartDashing(float delay)
    {
        canDash = false;

        CheckDirection(_playerGetMousePos.screenSpace);

        yield return new WaitForSeconds(delay);

        canDash = true;
        Debug.Log("Can Dash now");
    }


    /// <summary>
    /// curr hitung buat 8 arah dash, may change
    /// </summary>
    /// <param name="dir"></param>
    private void CheckDirection(Vector2 dir)
    {
        screenSize = new Vector2(Screen.width, Screen.height);
        middle = new Vector2(Screen.width/2, Screen.height/2);

        Vector3 diff = new Vector3(middle.x - dir.x, this.transform.localPosition.y , middle.y - dir.y).normalized;
        transform.position += diff * speed * Time.deltaTime;

        //compare the size
        Debug.Log(diff);
    }
}
