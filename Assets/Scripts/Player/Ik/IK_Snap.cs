using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class IK_Snap : MonoBehaviour
{
    public bool useIK;

    public bool leftHandIK;
    public bool rightHandIK;
    public bool leftFootIK;
    public bool rightFootIK;
    
    public Vector3 leftHandPos;
    public Vector3 rightHandPos;
    public Quaternion leftHandRot;
    public Quaternion rightHandRot;
    public Vector3 leftHandOffset;
    public Vector3 rightHandOffset;
    
    public Vector3 leftFootPos;
    public Vector3 rightFootPos;
    public Quaternion leftFootRot;
    public Quaternion rightFootRot;
    public Vector3 leftFootOffset;
    public Vector3 rightFootOffset;

    
    public GameObject sheathedSword;
    public GameObject leftFoot;
    public GameObject rightFoot;
    
    private Animator animator;
    private Item weapon;
    private bool isSheathed;
    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false;
        agent.updateRotation = true;
        isSheathed = false;
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        RaycastHit LHandHit;
        RaycastHit RHandHit;
        RaycastHit LFootHit;
        RaycastHit RFootHit;

        if (Physics.Raycast(transform.position + transform.forward * 1.8f + transform.up * 2.7f,
            -transform.up + transform.right * -0.5f, out LHandHit, 1.5f, LayerMask.GetMask("Climbable")))
        {
            leftHandIK = true;
            leftHandPos = LHandHit.point - leftHandOffset;
            leftHandRot = LHandHit.transform.rotation;
        }
        else
        {
            leftHandIK = false;
        }
        
        if (Physics.Raycast(transform.position + transform.forward * 1.8f + transform.up * 2.7f,
            -transform.up + transform.right * 0.5f, out RHandHit, 1.5f, LayerMask.GetMask("Climbable")))
        {
            rightHandIK = true;
            rightHandPos = RHandHit.point - rightHandOffset;
            rightHandRot = RHandHit.transform.rotation;
        }
        else
        {
            rightHandIK = false;
        }
        
        if (Physics.Raycast(leftFoot.transform.position + transform.forward * 0.5f, -transform.up,
            out LFootHit, 1.5f, LayerMask.GetMask("Climbable")) && 
            Physics.OverlapSphere(leftFoot.transform.position + transform.forward * 0.5f,
                0.05f, LayerMask.GetMask("Climbable"), QueryTriggerInteraction.Ignore).Length == 0)
        {
            leftFootIK = true;
            leftFootPos = LFootHit.point - leftFootOffset;
            leftFootRot = LFootHit.transform.rotation;
        }
        else
        {
            leftFootIK = false;
        }
        
        if (Physics.Raycast(rightFoot.transform.position + transform.forward * 0.5f, -transform.up,
            out RFootHit, 1.5f, LayerMask.GetMask("Climbable"))&& 
            Physics.OverlapSphere(rightFoot.transform.position + transform.forward * 0.5f,
                0.05f, LayerMask.GetMask("Climbable"), QueryTriggerInteraction.Ignore).Length == 0)
        {
            rightFootIK = true;
            rightFootPos = RFootHit.point - rightFootOffset;
            rightFootRot = RFootHit.transform.rotation;

            leftHandIK = false;
            rightHandIK = false;
        }
        else
        {
            rightFootIK = false;
        }
    }
    
    private void OnAnimatorIK(int layerIndex)
    {
        if (useIK)
        {
            if (leftHandIK)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPos);
                
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandRot);
            }  
            
            if (rightHandIK)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandPos);
                      
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandRot);
            }
            
            if (leftFootIK)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootPos);
                      
                animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
                animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRot);
            }
            
            if (rightFootIK)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
                animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootPos);
                      
                animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);
                animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRot);
            }
        }
    }

    private Vector3? dest;

    private bool climbIsOver = false;
    private bool jumpIsOver = false;
    private bool isJumping = false;
    private bool isClimbing = false;
    
    public void ClimbIsOver()
    {
        climbIsOver = true;
    }
    
    public void JumpIsOver() {
        jumpIsOver = true;
    }
    void Update()
    {
        
        
        if (isSheathed && !leftHandIK && !rightHandIK)
        {
            StartCoroutine(UnsheathSword());
        }
        
        if (agent.isOnOffMeshLink)
        {
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;

            if (data.endPos.y > data.startPos.y)
            {
                if (!isSheathed && (leftHandIK || rightHandIK) && !isClimbing)
                {
                    StartCoroutine(SheathSword());
                    isClimbing = true;
                }
                
                animator.SetBool("Jump", false);
                
                if (climbIsOver && dest == null)
                {
                    transform.position = endPos;
                    dest = agent.destination;
                }
            
                if(agent.transform.position == endPos)
                {
                    agent.CompleteOffMeshLink();
                    if (dest != null)
                    {
                        agent.destination = dest.Value;
                        dest = null;
                        climbIsOver = false;
                        isClimbing = false;
                    }
                }
            }
            else
            {
                animator.SetBool("Jump", true);

                if (!isJumping)
                {
                    StartCoroutine(SheathSword());
                    isJumping = true;
                }
                if (jumpIsOver && dest == null)
                {
                    transform.position = endPos;
                    dest = agent.destination;
                    agent.CompleteOffMeshLink();
                }
                if (agent.transform.position == endPos)
                {
                    if (dest != null)
                    {
                        agent.destination = dest.Value;
                        dest = null;
                        jumpIsOver = false;
                    }
                    isJumping = false;
                    StartCoroutine(UnsheathSword());
                }
                //Debug.Log(dest);
            }
        }
        Debug.DrawRay(transform.position + transform.forward + transform.up * 2.7f,
            -transform.up + transform.right * -0.5f, Color.green);
        
        Debug.DrawRay(transform.position + transform.forward + transform.up * 2.7f,
            -transform.up + transform.right * 0.5f, Color.green);
        
        Debug.DrawRay(leftFoot.transform.position + transform.forward * 0.2f, -transform.up, Color.red);

        Debug.DrawRay(rightFoot.transform.position + transform.forward * 0.2f, -transform.up, Color.red);
        
        //Debug.Log(Physics.OverlapSphere(leftFoot.transform.position + transform.forward + transform.up * 0.1f,
        //    0.05f, LayerMask.GetMask("Climbable"), QueryTriggerInteraction.Ignore).Length);
        
        
    }

    private IEnumerator SheathSword()
    {
        isSheathed = true;
        GetComponent<Animator>().SetTrigger("Sheath");
        yield return new WaitForSeconds(1.3f);
        weapon = GetComponent<PlayerContol>().sheathWeapon();
        sheathedSword.SetActive(true);
        GetComponent<Animator>().ResetTrigger("Sheath");
        useIK = true;
    }
    
    private IEnumerator UnsheathSword()
    {
        GetComponent<Animator>().SetTrigger("Unsheath");
        yield return new WaitForSeconds(.3f);
        sheathedSword.SetActive(false);
        GetComponent<PlayerContol>().equipWeapon(weapon);
        weapon = null;
        GetComponent<Animator>().ResetTrigger("Unsheath");
        isSheathed = false;
    }
}