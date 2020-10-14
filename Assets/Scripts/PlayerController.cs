using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(PlayerSpellController))]
[RequireComponent(typeof(PlayerGunController), typeof(InventoryObject))]
public class PlayerController : MonoBehaviour
{
    /* I could probably make the angle measuring a lil more optimized */
    /* And create a debug menu or something in the game */

    // VERY IMPORTANT TO DO LIST
    // Use Vector3.Angle instead of whatever functions I was using before


    // **** Public variables ****
    [Header("Player settings")]
    [Tooltip("Contains variables that defines: movement speed, jump height, maximum slope angles, etc.")]
    public PlayerSettings settings;
    [Space(10)]

    [Header("Ground checking & Wall checking")]
    [Tooltip("Cahced transform for checkSphere & Physics.Raycast when it comes to checking if our subject is grounded")]
    public Transform checkTransform;
    [Tooltip("Cahced transform for the lowestPoint for our subject. Is ultilised when colliding w/ Walls (determines wether or not we are colliding w/ a wall)")]
    public Transform lowestPoint;
    [Tooltip("Ground layer that is used to determine whether we have fallen unto an object that can be deemed as 'ground', thus, resulting in our object becoming grounded")]
    public LayerMask groundLayer;
    [Tooltip("Interact layer is used when determining wether we are trying to interact with an interactable object")]
    public LayerMask interactLayer;
    [Space(10)]

    [Header("External Interactions")]
    [Tooltip("Allows communication to an InventoryObject")]
    public InventoryObject inventory;
    [Tooltip("Obtains keyboard input")]
    public KeyboardController keyboard;
    [Tooltip("Allows communication with main camera")]
    public Camera mainCamera;

    // **** Private variables ****
    [HideInInspector]
    public bool isGrounded = true;
    // 'isGroundWalkable' is for stating whether the ground below is not too steep and wether we can walk on it
    private bool isGroundWalkable = true;
    // 'groundSlopeCheckHit' is for purely retaining the RaycastHit data from checkGroundSlope() which is used ApplyVectorOnGround() (Used to save computation)
    private RaycastHit groundSlopeCheckHit;
    private bool isCroutched = false;

    // IMPORTANT NOTE, 'isJmping' is only set true in the frame the subject commences a jump or airborne jump. 'isJmping' is not true for the duration of the jump
    private bool isJmping = false;
    // 'jmpSeries' contains the number of jumps we have done in an entire airborne sequence. Is ultilistied for multi jumping
    private int jmpSeries = 0;

    // 'kinematicVelocity' is the 'snappy' movement the character experiences & 'ghostVelocity' 
    // is the lingering slide or vertical velocity that changes due to gravity experienced when jumping
    [HideInInspector]
    public Vector3 kinematicVelocity = Vector3.zero;
    [HideInInspector]
    public Vector3 ghostVelocity = Vector3.zero;
    [HideInInspector]
    public Vector3 velocity = Vector3.zero;

    // 'boosterTimestamp' and 'dodgeTimestamp' are utilised for cooldowns. For how long the cooldowns last are defined in 'PlayerSettings'
    private float boosterTimestamp = 0f;
    private float dodgeTimestamp = 0f;

    // for interacting with kiosks or buttons
    private float interactTimestamp = 0f;
    private float interactDelay = 0f;
    private bool tryingToInteract = false; // this is for trying to activate an interactable
    private bool interacting = false; // this is for when interacting with an activated interactable
    private RaycastHit cachedInteractableHit;

    enum interactableObjTypes { nothing = -1, kiosk, elevator, gun };
    private interactableObjTypes interactingObjType = 0; // for classifying with type of object we are interacting with

    public Image circularProgressImage; // this is to visual indicate that we are interacting with something

    // A list of all contact points due to colliding collision boxes w/ our character. Every FixedUpdate(), 'contactPoints' is cleared
    private List<ContactPoint> contactPoints = new List<ContactPoint>();
    // Maximum slope/step angle that our character can walk up on
    private float slopeMaxAngle = 0f;
    private float stepMaxAngle = 0f;

    // **** Private interactions **** 
    private PlayerGunController gunController = null;
    private Rigidbody rb = null;
    private CapsuleCollider capsuleCollider = null;

    // Initializes just a few components...
    void Awake()
    {
        circularProgressImage.fillAmount = 0f;

        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        gunController = GetComponent<PlayerGunController>();

        slopeMaxAngle = Mathf.Tan((settings.slopeMaxAngle * Mathf.PI) / 180);
        stepMaxAngle = Mathf.Tan((settings.stepMaxAngle * Mathf.PI) / 180);
    }

    // Get keys, checks keys, then respectively imposings changes unto 'kinematicVelocity' & 'ghostVelocity', and interacting with interactable enviroment
    void Update()
    {
        keyboard.Keys.GetKeys();

        // Handles jumping or multi jumping (airborne jumps) [Must be in Update() for good response times]
        if (keyboard.Keys.spacebar)
        {
            if (jmpSeries == 0 && isGrounded) // grounded jumps
            {
                ghostVelocity = new Vector3(ghostVelocity.x, Mathf.Sqrt(2 * (settings.gravity) * (settings.jumpHeight)),
                    ghostVelocity.z);
                isGrounded = false;
                isJmping = true;
                jmpSeries++;
            }
            else if (keyboard.Keys.keyDownSpacebar && jmpSeries < settings.maxJmps) // airborne jumps
            {
                // The '1.35f' scalar is used to add an extra 'oomph' when jumping so it is more distincitive when the character commences an airborne jump
                ghostVelocity = new Vector3(ghostVelocity.x, Mathf.Sqrt(2 * (settings.gravity) * (settings.jumpHeight)) * 1.35f,
                    ghostVelocity.z);
                isJmping = true;
                jmpSeries++;
            }
        }

        // Handles crouching & boost sliding (for now though, crouching merely half's the players localScale.y) [Must be in Update() for good response times]
        if (keyboard.Keys.keyDownCtrl)
        {
            isCroutched = true;
            transform.localScale = new Vector3(1f, 0.5f, 1f);

            if (isGrounded && (Time.time - dodgeTimestamp) >= settings.dodgeBoostDelay) // commences boost sliding
            {
                dodgeTimestamp = Time.time;
                ghostVelocity += kinematicVelocity * 1.2f;
            }
            else if (isGrounded) dodgeTimestamp = Time.time; // sets dodgeTimestamp to time if character crouches w/o a sufficient delay to the previous dodge boost

        }
        else if (keyboard.Keys.keyUpCtrl)
        {
            isCroutched = false;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        CheckActivateInteractWithInteractables(); // handles interactions with buttons, kiosks, and such

    }

    void FixedUpdate()
    {
        // Sets 'kinematicVelocity' to the respective recently pressed keys and then multiplies it by our settings.speed
        kinematicVelocity = transform.forward * (keyboard.Keys.keyW - keyboard.Keys.keyS) + transform.right * (keyboard.Keys.keyD - keyboard.Keys.keyA);
        kinematicVelocity *= settings.speed;

        if (rb.IsSleeping()) rb.WakeUp();

        // Ground checking & add applying gravity only when neccessary
        if (!isJmping)
        {
            isGroundWalkable = CheckGroundSlope();
            CheckIfGrounded();
            ApplyGravityWithFriction();
        }

        // Computing our applied velocity onto our environment
        Vector3 appliedVelocity = kinematicVelocity;
        if ((!isGrounded && !isJmping) || !isGroundWalkable) appliedVelocity += Vector3.up * ghostVelocity.y; // Applies gravity is we are not grounded or on a steep slope
        appliedVelocity = ApplyVectorOnWall(appliedVelocity); // Applies our velocity on a wall in the case we are colliding with one
        appliedVelocity = ApplyVectorOnGround(appliedVelocity); // Applies our velocity on the ground in the case we are colliding with ne

        if (kinematicVelocity.magnitude > 0.3f) appliedVelocity = FindStepAndStepUp(appliedVelocity); // Allows our subject to step up stairs only if we are moving (saves cpu)
        if (isJmping) appliedVelocity += Vector3.up * ghostVelocity.y; // In the case we are jumping, apply our jumping velocity that is currently stored in ghostVelocity.y
        isJmping = false; // Resets isJmping

        contactPoints.Clear();
        //Debug.DrawRay(transform.position, appliedVelocity, Color.green, 1f); // Debugging

        // Applying 'appliedVelocity' on our rigidbody's velocity
        velocity = new Vector3(appliedVelocity.x + ghostVelocity.x, appliedVelocity.y, appliedVelocity.z + ghostVelocity.z);
        rb.velocity = velocity;
    }

    // Applys gravity when not grounded & applys fiction with sliding effects when grounded
    void ApplyGravityWithFriction()
    {
        // If we head in the opposite direction of our ghostVelocity, ghostVelocity will be set to esstiantially zero and function returns
        if (Vector3.Dot(ghostVelocity.normalized, kinematicVelocity.normalized) < -0.5f && (Time.time - boosterTimestamp) >= settings.boosterInputDelay)
        {
            if (isGrounded) { ghostVelocity = new Vector3(0f, ghostVelocity.y, 0f); return; }
            else { ghostVelocity = new Vector3(0f, -1.5f, 0f); return; }
        }

        // If grounded, we will apply a sliding effect with a strength depending on whether we are standing or crouching
        if (isGrounded && isGroundWalkable)
        {
            if (isCroutched)
            {
                // Causing a stronger sliding effect (friction is weaker)
                ghostVelocity = new Vector3(ghostVelocity.x - (settings.ghostCollidingFriction / settings.ghostSlideScalar) * ghostVelocity.x, -1.5f,
                    ghostVelocity.z - (settings.ghostCollidingFriction / settings.ghostSlideScalar) * ghostVelocity.z);

            }
            else
            {
                // Causing a weaker sliding effect relative to crouching (friction is stronger relative to crouching)
                ghostVelocity = new Vector3(ghostVelocity.x - settings.ghostCollidingFriction * ghostVelocity.x, -1.5f,
                    ghostVelocity.z - settings.ghostCollidingFriction * ghostVelocity.z);
            }
        }

        // If we are not grounded or on steep ground, we will apply gravity on our ghostVelocity
        if (!isGrounded || !isGroundWalkable) {
            ghostVelocity += Vector3.up * (-settings.gravity * Time.fixedDeltaTime);
        }
    }

    // Checks if we are grounded (Also includes functions that checks whether we have bumped our head into a ceiling)
    void CheckIfGrounded()
    {
        // Checking for when we bump our head into a ceiling and halts our upward velocity
        if (rb.velocity.y < 0.05f && ghostVelocity.y > 0f)
        {
            isGrounded = false;
            ghostVelocity = new Vector3(ghostVelocity.x, 0f, ghostVelocity.z);
            return;
        }

        isGrounded = false;
        // Checksphere & downward raycast usesd to check if we are grounded or touching the ground (to then state we are grounded)
        if (Physics.CheckSphere(checkTransform.transform.position, (capsuleCollider.radius - 0.05f), groundLayer) ||
            Physics.Raycast(checkTransform.transform.position, Vector3.down, capsuleCollider.radius + settings.x, groundLayer))
        {
            if (groundSlopeCheckHit.collider != null || groundSlopeCheckHit.collider.gameObject.tag != "Booster")
            {
                jmpSeries = 0; // Reset jumps
                isGrounded = true;
            }
        }
    }

    // returns true if the ground below has a slope that is 'walkable'
    bool CheckGroundSlope()
    {
        // shoots downward raycast and measures angles
        if (Physics.Raycast(checkTransform.transform.position, Vector3.down, out groundSlopeCheckHit, Mathf.Infinity, groundLayer))
        {
            float xyTan = -groundSlopeCheckHit.normal.x / groundSlopeCheckHit.normal.y;
            float zyTan = -groundSlopeCheckHit.normal.z / groundSlopeCheckHit.normal.y;

            if (slopeMaxAngle >= xyTan && slopeMaxAngle >= zyTan)
            {
                // If under the max slope angles, return true
                return true;
            }
        }

        // else return false
        return false;
    }

    // Applys a vector onto a ground by checking all contact points and whether we are moving into a wall
    Vector3 ApplyVectorOnWall(Vector3 vector)
    {
        foreach (ContactPoint contact in contactPoints)
        {
            if ((contact.point.y - lowestPoint.position.y) > 0f &&
                Vector3.Dot(contact.normal, kinematicVelocity) < 0f)
            {
                return Vector3.ProjectOnPlane(vector, contact.normal);
            }
        }

        return vector; // No viable contact points, we return vector unchanged.
    }

    // Applys a vector on ground on if we are grounded
    Vector3 ApplyVectorOnGround(Vector3 vector)
    {
        // 'groundSlopeCheckHit' is hit data from a downward shot raycast
        if (isGrounded)
        {
            return Vector3.ProjectOnPlane(vector, groundSlopeCheckHit.normal);
        }

        return vector;
    }

    // Applys a vector onto 'stairs' (A series of box colliders) if the angle is within bounds & if it is under a certain height & if we are heading towards it
    Vector3 FindStepAndStepUp(Vector3 vector)
    {
        foreach (ContactPoint contact in contactPoints)
        {
            float xyTan = (contact.point.y) / (contact.point.x);
            float zyTan = (contact.point.y) / (contact.point.z);

            if (Vector3.Dot(contact.point, kinematicVelocity) > 0f && (contact.point.y - lowestPoint.position.y) > 0f &&
                stepMaxAngle >= xyTan && stepMaxAngle >= zyTan && ((contact.point.y - lowestPoint.position.y) <= settings.stepOffset))
            {
                return Vector3.ProjectOnPlane(vector, contact.normal);
            }

        }
        return vector; // returns vector unchanged if for no viable stairs
    }

    void CheckActivateInteractWithInteractables()
    {
        // Handles interactions with buttons and kiosks
        if (keyboard.Keys.keyDownF && !interacting) // Checks if we are holding down 'F' and that we are currently not interacting with an interactable
        {
            Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 5f, interactLayer))
            {
                switch (hit.collider.gameObject.tag) // checks for which type of interactable we are trying to activate and starts timing
                {
                    case "Kiosk":
                        interactDelay = settings.kioskActivateDelay;
                        interactTimestamp = Time.time;
                        tryingToInteract = true;
                        break;

                    default:
                        break;
                }
            }
        }

        // For activating interactables
        if (keyboard.Keys.keyF && tryingToInteract) // as for now, this code works only for 
        {
            circularProgressImage.fillAmount = Mathf.Clamp01(((Time.time - interactTimestamp) / interactDelay)); // fills progression circle

            Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 5f, interactLayer))
            {
                if ((Time.time - interactTimestamp) >= interactDelay)
                {
                    switch (hit.collider.gameObject.tag)
                    {
                        case "Kiosk": // activates the kiosk we are trying to interact with
                            Debug.Log("Interacted with a kiosk");
                            hit.collider.gameObject.GetComponent<KioskWorldspace>().ActivateKiosk();
                            tryingToInteract = false;
                            interacting = true;
                            interactingObjType = interactableObjTypes.kiosk;
                            cachedInteractableHit = hit;
                            circularProgressImage.fillAmount = 0f;
                            break;

                        default:
                            break;
                    }
                }
            }
            else { tryingToInteract = false; circularProgressImage.fillAmount = 0f; }
        }
        else { tryingToInteract = false; circularProgressImage.fillAmount = 0f; }

        // for current interacting with interactables
        if (keyboard.Keys.mouse1 && interacting)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool stoppedInteraction = false;
            // disables interactable based on what type it was once we click outside of it

            if (Physics.Raycast(ray, out hit, 5f, interactLayer))
            {
                if (hit.collider != cachedInteractableHit.collider)
                    stoppedInteraction = true;
            }
            else stoppedInteraction = true;

            if (stoppedInteraction)
            {
                interacting = false;
                switch (interactingObjType)
                {
                    case interactableObjTypes.kiosk:
                        cachedInteractableHit.collider.gameObject.GetComponent<KioskWorldspace>().DeactivateKiosk();
                        break;
                }
            }
        }
    } // handles interactions with buttons, kiosks, and such

    // Accumulates a list of contact points
    private void OnCollisionEnter(Collision collision)
    {
        contactPoints.AddRange(collision.contacts);
    }
    private void OnCollisionStay(Collision collision)
    {
        contactPoints.AddRange(collision.contacts);
    }

    //On trigger events (To be changed in the future most likely
    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "SpellPickup":
                int x = inventory.OverrideAtFirstFind(other.GetComponent<SpellPickup>().spellSlot, inventory.nullSpell);
                if (x>=0)
                {
                    Destroy(other.gameObject);
                }
                else Debug.Log("All spells slots used up");
                break;

            case "GunPickup":
                x = inventory.OverrideAtFirstFind(other.GetComponent<GunPickup>().gunSlot, inventory.nullGun);
                if (x >= 0)
                {
                    gunController.UpdateGuns(x);
                    Destroy(other.transform.parent.gameObject);
                }
                else Debug.Log("All guns slots used up");
                break;

            case "Booster":
                var script = other.GetComponent<BoosterSettings>();
                ghostVelocity = script.boosterDirection * (Mathf.Sqrt(2 * settings.gravity * script.boosterDistance));
                boosterTimestamp = Time.time;
                isJmping = true;
                jmpSeries = 0;
                isGrounded = false;
                break;

            default:
                break;
        }
    }
}
