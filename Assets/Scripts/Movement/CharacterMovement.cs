using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class CharacterMovement : MonoBehaviour {

    public CapsuleCollider playerCollider;

    public Vector3 velocity;
    public bool isGrounded;

    public bool slowerBackwardsMovement = false;
    public float backwardsVeclocityScale = 0.7f;
    public float backwardsAccelerationScale = 1f;

    public bool slowerSidewaysMovement = false;
    public float sidewaysVeclocityScale = 0.7f;
    public float sidewaysAccelerationScale = 1f;

    public bool extraForwardsMovement = true;
    public float extraForwardsAcceleration = 3f;
    public float extraForwardsMaxVelocity = 7.75f;
    public float extraForwardsAngle = 45f;

    public int maxCollisionChecks = 10;
    public float safetyWidth = 0.02f;
    public float maxVeclocity = 5.5f;
    public float acceleration = 8.5f;
    public float friction = 6f;
    public float airAcceleration = 0.85f;
    public float oppositeAccelerationScale = 0.5f;
    public float jumpForce = 7.3f;
    public float gravity = 22f;
    public float slopeAngleLimit = 45f;
    public float stepHeight = 0.5f;

    // Use this for initialization
    void Awake () {
        playerCollider = GetComponent<CapsuleCollider>();
	}

    //TODO make a summary
    public void Move(Vector3 dir, float deltaTime)
    {
        Vector3 prevVel = velocity;
        Vector3 flatDir = new Vector3(dir.x, 0f, dir.z).normalized;
        float flatDirMagnitude = Mathf.Abs(dir.x) + Mathf.Abs(dir.z);
        float flatVelMagnitude = Mathf.Abs(velocity.x) + Mathf.Abs(velocity.z);
        float slopeScale = 1f;

        //Check if we're grounded
        RaycastHit groundHitInfo;
        float castDistance = playerCollider.height / 2f + safetyWidth * 1.01f;
        if (Physics.SphereCast(transform.position + playerCollider.center, playerCollider.radius, -transform.up, out groundHitInfo, castDistance))
        {
            float groundAngle = Vector3.Angle(Vector3.up, groundHitInfo.normal);
            if (groundAngle < 89f)
            {
                float flatVelVsNormal = Vector3.Angle(flatDir, new Vector3(groundHitInfo.normal.x, 0f, groundHitInfo.normal.z));
                flatVelVsNormal = 1f - Mathf.Max(1f - flatVelVsNormal / 150f, 0f);
                flatVelVsNormal = Mathf.Clamp(flatVelVsNormal * 2f - 1f, 0f, 1f);
                slopeScale = Mathf.Max(1f - (groundAngle / slopeAngleLimit) * flatVelVsNormal, 0f);
            }
            velocity = Vector3.ProjectOnPlane(velocity, groundHitInfo.normal); //Also helps prevent falling through the ground.
            if(groundHitInfo.distance < castDistance - safetyWidth * 1.01f)
            {
                transform.position += Vector3.up * ((castDistance - safetyWidth * 1.01f) - groundHitInfo.distance); //Prevent falling through ground on rare occasions.
            }
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        //Apply friction
        if (isGrounded && flatVelMagnitude != 0f)
        {
            velocity = ApplyFriction(velocity, flatDir, flatVelMagnitude, flatDirMagnitude, deltaTime);
        }

        //Apply gravity
        if (!isGrounded)
        {
            velocity.y -= gravity * deltaTime;
        }

        //Apply movement if needed
        //TODO make movement work on steps/stairs
        if (flatDirMagnitude != 0f)
        {
            velocity = Accelerate(velocity, flatDir, slopeScale, deltaTime);
        }

        //Jumping
        if(dir.y > 0f && isGrounded)
        {
            velocity.y = jumpForce;
        }

        velocity = CheckForCollision(velocity, playerCollider, deltaTime, 0);
        transform.position += velocity * deltaTime;
    }

    Vector3 Accelerate(Vector3 currVel, Vector3 flatDir, float scale, float deltaTime)
    {
        //Limiting velocity
        Vector3 flatVel = new Vector3(currVel.x, 0f, currVel.z).normalized;
        float oppositeScaling = 1f + Mathf.Abs(Vector3.Dot((flatDir - flatVel), flatVel)) * oppositeAccelerationScale;
        Debug.Log(oppositeScaling);
        float projVel = Vector3.Dot(currVel, flatDir);
        float accelVel = isGrounded ? acceleration * oppositeScaling * scale * deltaTime : airAcceleration * deltaTime;
        
        if (projVel + accelVel > maxVeclocity * scale)
        {
            accelVel = maxVeclocity * scale - projVel;
        }

        return currVel + flatDir * accelVel;
    }

    Vector3 CheckForCollision(Vector3 currVel, CapsuleCollider characterCollider, float deltaTime, int checks)
    {
        Vector3 capsuleTopSpherePos = transform.position + characterCollider.center + characterCollider.height / 2f * Vector3.up;
        Vector3 capsuleBottomSpherePos = transform.position + characterCollider.center - characterCollider.height / 2f * Vector3.up;
        Vector3 rayVector = currVel * deltaTime;
        RaycastHit hitInfo;
        //TODO make movement work on slopes

        if (checks < 10 && Physics.CapsuleCast(capsuleTopSpherePos, capsuleBottomSpherePos, characterCollider.radius, rayVector, out hitInfo, rayVector.magnitude, Physics.AllLayers))
        {
            checks++;
            Vector3 pokeThroughVector = currVel - currVel * hitInfo.distance;
            pokeThroughVector = Vector3.Project(pokeThroughVector, hitInfo.normal);
            Vector3 returnVector = currVel - pokeThroughVector;

            /*if (returnVector.magnitude > maxVeclocity)
            {
                returnVector = returnVector.normalized * maxVeclocity;
            }
            if (Vector3.Angle(Vector3.up, hitInfo.normal) > slopeAngleLimit)
            {
                
            }*/
            return CheckForCollision(returnVector, characterCollider, deltaTime, checks);
        }
        else
        {
            return currVel;
        }
    }

    /// <summary>
    /// Applies friction.
    /// </summary>
    /// <param name="currVel"></param>
    /// <param name="flatDir"></param>
    /// <param name="flatVelMagnitude"></param>
    /// <param name="flatDirMagnitude"></param>
    /// <param name="deltaTime"></param>
    /// <returns></returns>
    Vector3 ApplyFriction(Vector3 currVel, Vector3 flatDir, float flatVelMagnitude, float flatDirMagnitude, float deltaTime)
    {
        Vector3 flatVel = new Vector3(currVel.x, 0f, currVel.z);
        Vector3 vel = currVel;

        //Apply friction in all directions
        if (flatDirMagnitude == 0f)
        {
            return ApplyNormalFriction(currVel, flatVel, deltaTime);
        }
        else //Apply frictions in a right angle to the wished direction.
        {
            return ApplyRunningFriction(currVel, flatVel, flatDir, deltaTime);
        }
    }

    /// <summary>
    /// Applies normal friction in all directions.
    /// </summary>
    /// <param name="currVel"></param>
    /// <param name="flatVel"></param>
    /// <param name="deltaTime"></param>
    /// <returns></returns>
    Vector3 ApplyNormalFriction(Vector3 currVel, Vector3 flatVel, float deltaTime)
    {
        Vector3 vel = currVel;

        Vector3 frictionVector = -flatVel.normalized * friction * deltaTime;
        vel += frictionVector;

        vel.x = currVel.x > 0f ? Mathf.Clamp(vel.x, 0f, currVel.x) : Mathf.Clamp(vel.x, currVel.x, 0f);
        vel.z = currVel.z > 0f ? Mathf.Clamp(vel.z, 0f, currVel.z) : Mathf.Clamp(vel.z, currVel.z, 0f);

        return vel;
    }

    /// <summary>
    /// Applies friction perpendicularly to the direction wished direction.
    /// </summary>
    /// <param name="currVel"></param>
    /// <param name="flatVel"></param>
    /// <param name="flatDir"></param>
    /// <param name="deltaTime"></param>
    /// <returns></returns>
    Vector3 ApplyRunningFriction(Vector3 currVel, Vector3 flatVel, Vector3 flatDir, float deltaTime)
    {
        Vector3 vel = currVel;

        Vector3 perpDesiredDir = Vector3.Cross(flatDir, Vector3.up);
        Vector3 velProjToPerpDir = Vector3.Project(flatVel, perpDesiredDir);
        Vector3 frictionVector = velProjToPerpDir.normalized * friction * deltaTime;
        Vector3 newVelProjToPerpDir = velProjToPerpDir - frictionVector;

        if (Mathf.Sign(newVelProjToPerpDir.x) != Mathf.Sign(velProjToPerpDir.x))
        {
            frictionVector += newVelProjToPerpDir;
        }

        return vel - frictionVector;
    }
}
