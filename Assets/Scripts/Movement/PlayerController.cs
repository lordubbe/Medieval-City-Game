using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerController : MonoBehaviour {

    CharacterMovement character;
    Camera characterCam;

    Vector2 moveInput;
    Vector3 moveDir;

    [SerializeField] MouseLook mouseLook;

	// Use this for initialization
	void Start () {
        character = GetComponent<CharacterMovement>();
        characterCam = GetComponentInChildren<Camera>();

        //Can't use RequireComponent, checking manually instead.
        if(characterCam == null)
        {
            AddCharacterCamera();
        }

        mouseLook.Init(transform, characterCam.transform);
	}
	
	// Update is called once per frame
	void Update () {
        mouseLook.UpdateLook(Time.deltaTime);

        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        moveDir = transform.forward * moveInput.y + transform.right * moveInput.x;
        moveDir.y = Input.GetButton("Jump") ? 1f : 0f;

        character.Move(moveDir, Time.deltaTime);
	}

    void AddCharacterCamera()
    {
        Debug.LogWarning("No camera found in children of " + gameObject.name + ", adding one.");
        GameObject child = new GameObject("PlayerCamera");
        child.transform.position = transform.position + Vector3.up * character.playerCollider.height / 2f - Vector3.up * 0.2f;
        child.transform.parent = transform;
        child.AddComponent<Camera>();
    }
}
