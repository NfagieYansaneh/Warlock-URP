using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayCameraController : MonoBehaviour
{
    public CameraSettings settings;
    private Vector3 destLerp;
    public PlayerController player;
    [Tooltip("Obtains keyboard input")]
    public KeyboardController keyboard;

    void Update()
    {
        // Could be optimized
        // Uses ternary conditional operator
        // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-operator

        destLerp = new Vector3(
            settings.lerpSideAmplitude * (- keyboard.Keys.keyA + keyboard.Keys.keyD),           // X
            (player.isGrounded)? 0 : settings.lerpUpAmplitude * player.velocity.y,              // Y
            settings.lerpFwdAmplitude * (keyboard.Keys.keyW - keyboard.Keys.keyS));             // Z

        destLerp = Vector3.ClampMagnitude(destLerp, settings.lerpMax);
        transform.localPosition = Vector3.Lerp(transform.localPosition, destLerp, settings.lerpSpeed);
    }
}
