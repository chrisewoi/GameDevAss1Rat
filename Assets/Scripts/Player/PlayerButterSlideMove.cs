using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class PlayerButterSlideMove : MonoBehaviour, IMove
{
    public Transform butterOrientationTarget;
    private Vector3 butterOrientationLast;
    private Vector3 velocity;
    private static bool onButter;
    public GameObject butterPrefab;
    private GameObject butter;
    public new GameObject camera;
    public float butterYOffset;
    private float butterYFall, butterYFallSmooth;
    public float butterFallMult;
    public float butterFallDelay;
    private PlayerJumpMove jumpScript;
    private float butterPickupY;
    public float mountTime;
    public float butterTiltTime;
    public float maxTurnAngle;
    public float timeActivated;
    public MeshCollider butterMeshCollider;
    private static float butterHeight;
    private bool runOnce;

    public float butterUngroundedDismountTime;
    public float disableMountAfterDismountTime;
    public float timeDismounted;
    public PhysicsMaterial butterPhysicsMaterial;
    private PlayerMovement playerMovement;

    private Vector3 smoothNormal;
    MoveType IMove.moveType => MoveType.ButterSlide;
    Vector3 IMove.v => velocity;

    public float speed;

    private float yV, xV, zV, yFallV;
    private Vector3 smoothNormalV;


    public float height;

    public float radius;

    public LayerMask layerMask;

    void Start()
    {
        jumpScript = GetComponent<PlayerJumpMove>();
        playerMovement = GetComponent<PlayerMovement>();
        runOnce = true;
        timeDismounted = 10f;
    }

    void Update()
    {
        if (onButter && butter != null)
        {
            if (runOnce)
            {
                runOnce = false;
                Destroy(butter.GetComponent<Rigidbody>());
                butterHeight = butterMeshCollider.bounds.size.y;
                Destroy(butterMeshCollider);
            }

            butterYFall = Mathf.Clamp01((GroundCheck.UngroundedTime() - butterFallDelay )/ butterUngroundedDismountTime);
            butterYFallSmooth = Mathf.SmoothDamp(butterYFallSmooth, Mathf.SmoothStep(0, 1, butterYFall), ref yFallV, 0.1f);

            butter.transform.position = transform.position - new Vector3(0, butterYOffset + butterYFallSmooth * butterFallMult, 0);
            //butter.transform.position -= new Vector3(0, butterYOffset + butterYFall * butterFallMult, 0);
            butterOrientationTarget.position = butter.transform.position;

            //this is the world-space normal, representing the direction you have to move away from the face to go "up"
            Vector3 groundNormal = GroundCheck.GroundNormal();



            Vector3 cameraFlatForward = camera.transform.forward;

            cameraFlatForward.y = 0;
            
            //figure out how far we need to rotate from global forward to our desired forward
            float angleDifference = Vector3.SignedAngle(Vector3.forward, cameraFlatForward, Vector3.up);

            bool falling = true;
            
            // If we aren't falling, basically
            if (GroundCheck.UngroundedTime() < 0.5f)
            {
                falling = false;
                //force the butter's up to match the normal (this rotates the butter)
                butterOrientationTarget.up = groundNormal;
                //rotate globally around our Y axis, to match the rotation of the ground 
                butterOrientationTarget.Rotate(Vector3.up, angleDifference + 90f);

                // last recorded orientation before falling
                butterOrientationLast = butterOrientationTarget.eulerAngles;
                // now tweak it so the butter can fall away at a little (random) angle
                float randomAmount = 20f;
                butterOrientationLast = new Vector3(butterOrientationLast.x + Random.Range(-randomAmount*2f, randomAmount*2f),
                                                    butterOrientationLast.y + Random.Range(-randomAmount/2f, randomAmount/2f),
                                                    butterOrientationLast.z + Random.Range(-randomAmount*3f, randomAmount*3f));
            }
            
            Vector3 setAngles = butterOrientationTarget.eulerAngles;

            float yRotSmoothTimeMult = 1f;
            if (Mathf.Abs(butterOrientationTarget.localEulerAngles.y - butter.transform.localEulerAngles.y) >
                maxTurnAngle)
            {
                yRotSmoothTimeMult = 0.5f;
            }

            float fallingSmoothTimeMult = 1f;
            if (falling)
            {
                setAngles = butterOrientationLast;
                fallingSmoothTimeMult = 6f;
            }

            float freshlyGroundedSmoothTimeMult = 1f;
            if (GroundCheck.GroundedTime() > 0f && GroundCheck.GroundedTime() < 0.5f)
            {
                freshlyGroundedSmoothTimeMult = 0.5f;
            }
            
            float smoothX = Mathf.SmoothDampAngle(butter.transform.localEulerAngles.x,
                setAngles.x, ref xV, butterTiltTime * fallingSmoothTimeMult * freshlyGroundedSmoothTimeMult);
            float smoothY = Mathf.SmoothDampAngle(butter.transform.localEulerAngles.y,
                setAngles.y, ref yV, falling?butterTiltTime * yRotSmoothTimeMult * fallingSmoothTimeMult:0.02f * 1f/math.pow(freshlyGroundedSmoothTimeMult, 3f));
            float smoothZ = Mathf.SmoothDampAngle(butter.transform.localEulerAngles.z,
                setAngles.z, ref zV, butterTiltTime * fallingSmoothTimeMult * freshlyGroundedSmoothTimeMult);


            butter.transform.localEulerAngles = new Vector3(smoothX, smoothY, smoothZ);
            /*
            
            butter.transform.position -= new Vector3(0,butterYOffset, 0);
            smoothNormal = Vector3.SmoothDamp(smoothNormal, GroundCheck.GroundNormal(), ref smoothNormalV, butterTiltTime);
            Vector3 angles = butter.transform.InverseTransformDirection(smoothNormal * maxTiltAngle);
            Vector3 currentAngles = transform.eulerAngles;
            //float smoothX = Mathf.SmoothDampAngle(currentAngles.x, angles.z, ref xV, butterTiltTime);
            //float smoothZ = Mathf.SmoothDampAngle(currentAngles.z, -angles.x, ref zV, butterTiltTime);
            
            angles.y = camera.transform.eulerAngles.y + 90f;
            float currentY = angles.y;  
            angles.y = Mathf.SmoothDampAngle(butter.transform.rotation.eulerAngles.y, angles.y, ref yV, 0.03f);
            //angles.z = GroundCheck.GroundNormal().z;
            //angles.z = butter.transform.InverseTransformDirection(GroundCheck.GroundNormal()).z;
            butter.transform.rotation = Quaternion.Euler(new Vector3(angles.z, angles.y, -angles.x));
            //butter.transform.rotation = Quaternion.Slerp(butter.transform.rotation, Quaternion.Euler(angles), Time.deltaTime * 100f);
            */
            
            // sets butter yPos after mount time has passed

            if (timeActivated > Time.time - mountTime)
            {
                Vector3 clampedPos = new Vector3(butter.transform.position.x,
                    butterPickupY,
                    butter.transform.position.z);
                butter.transform.position = clampedPos;
            }

            timeDismounted = 0f;
            
            // Dismount check
            if (GroundCheck.UngroundedTime() > butterUngroundedDismountTime && onButter && butter != null)
            {
                DismountButter();
                //reset variables for next butter mount
                runOnce = true;
                onButter = false;
                butter = null;
            }
        }
        else
        {
            butterHeight = 0;
            timeDismounted += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (timeDismounted > disableMountAfterDismountTime && other.gameObject.CompareTag("Butter"))
        {
            butterPickupY = other.transform.position.y;
            jumpScript.jumpValue = 1f; // force a jump
            onButter = true;
            butter = Instantiate(butterPrefab, transform, true);
            Destroy(butter.GetComponent<Rigidbody>());
            butter.gameObject.tag = "Ground";
            butterMeshCollider = butter.GetComponent<MeshCollider>();
            butterMeshCollider.material.dynamicFriction = 0f;
            Destroy(other.gameObject);

            timeActivated = Time.time;
            Physics.SyncTransforms();
        }
    }

    public static bool IsSliding()
    {
        return onButter;
    }

    public static float ButterHeight()
    {
        return butterHeight;
    }

    Vector3 GetNormal()
    {
        if (GroundCheck.GroundNormal() == Vector3.zero) // No ground angle detected
        {
            return Vector3.up;
        }

        return GroundCheck.GroundNormal();
    }

    public void DismountButter()
    {
        Debug.Log("DismountButter", gameObject);
        butter.transform.SetParent(null);
        if(butter.GetComponent<MeshCollider>() == null) butter.AddComponent<MeshCollider>();
        MeshCollider dismountMesh = butter.GetComponent<MeshCollider>();
        dismountMesh.material = butterPhysicsMaterial;
        dismountMesh.material.dynamicFriction = 0.4f;
        dismountMesh.convex = true;
        Rigidbody butterRb = butter.AddComponent<Rigidbody>();
        //butterRb.
        butterRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        butterRb.interpolation = RigidbodyInterpolation.Extrapolate;
        butterRb.AddForce(playerMovement.GetVelocity(), ForceMode.Impulse);
        butter.gameObject.tag = "Butter";
    }
}
