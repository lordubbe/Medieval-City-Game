using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MouseLook {

    public float sensitivity = 0.5f;
    public bool lockVerticalLook = true;
    public float minVerticalLock = -90f;
    public float maxVerticalLock = 95f;
    public bool lockCursor = true;

    Transform character;
    Transform camera;

    float xRot;
    float yRot;

    /// <summary>
    /// Initializes the MouseLook class.
    /// </summary>
    /// <param name="character">The gameobject used for horizontal rotation.</param>
    /// <param name="camera">The gameobject used for vertical rotation.</param>
	public void Init(Transform character, Transform camera)
    {
        this.character = character;
        this.camera = camera;

        xRot = character.localRotation.eulerAngles.y;
        yRot = camera.localRotation.eulerAngles.x;
    }
	
	public void UpdateLook(float deltaTime)
    {
        if(character == null || camera == null)
        {
            Debug.LogError("No character or camera found! Please make sure to initialize both using the Init() function.");
        }

        xRot += Input.GetAxis("Mouse X") * sensitivity;
        yRot += -Input.GetAxis("Mouse Y") * sensitivity;

        character.localRotation = Quaternion.Euler(0f, xRot, 0f);
        camera.localRotation = Quaternion.Euler(yRot, 0f, 0f);

        if (lockVerticalLook)
        {
            camera.localRotation = ClampVerticalRotation(camera);
        }

        if (lockCursor)
        {
            LockCursor();
        }
    }

    Quaternion ClampVerticalRotation(Transform clampedTransform)
    {
        Vector3 euler = clampedTransform.localRotation.eulerAngles;

        yRot = Mathf.Clamp(yRot, minVerticalLock, maxVerticalLock);
        euler.x = yRot;

        return Quaternion.Euler(euler);
    }

    void LockCursor()
    {
        //TODO: Cursorlocking

    }
}
