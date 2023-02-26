using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveControl : MonoBehaviour
{
    public float WalkSpeed = 3.0f;
    public float RunSpeed = 6.0f;
    public float Gravity = 9.8f;
    public Vector3 Velocity = Vector3.zero;
    public float JumpHeight = 1.2f;

    [HideInInspector]
    public CharacterController PlayerCharacterController = null;

    private AudioSource m_AudioSource = null;

    public float StandStepTime = 0.6f;
    private float StepTime = 0.0f;

    public FootSoundData FootSound;

    public Animator AnimatorControl = null;

    // Start is called before the first frame update
    void Start()
    {
        PlayerCharacterController = GetComponent<CharacterController>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void OnEquipmentItem(object obj)
    {
        AnimatorControl = ((GameObject)obj).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move_Update();
        Height_Update();
        PlayerCharacterController.Move(Velocity * Time.deltaTime);
        Sound_Update();
    }

    private void Sound_Update()
    {
        if (PlayerCharacterController.isGrounded)
        {
            Vector3 vT = Velocity;
            vT.y = 0;
            float Speed = vT.magnitude;

            if(Speed > 0)
            {
                StepTime += Time.deltaTime;

                float fP = WalkSpeed / Speed;

                if(StepTime > StandStepTime * fP)
                {
                    string tag = GetGoundTag();
                    AudioClip ac = FootSound.GetSoundByTag(tag);
                    m_AudioSource.clip = ac;
                    m_AudioSource.Play();

                    StepTime = 0.0f;
                }
            }
        }
    }

    private void Height_Update()
    {
        if (PlayerCharacterController.isGrounded)
        {
            if(Velocity.y < -1)
            {
                Velocity.y = -1;
            }

            if (Input.GetButtonDown("Jump"))
            {
                Velocity.y = Mathf.Sqrt(2 * Gravity * JumpHeight);
            }
        }
        Velocity.y -= Gravity * Time.deltaTime;
    }

    private void Move_Update()
    {
        float InputX = Input.GetAxis("Horizontal");
        float InputY = Input.GetAxis("Vertical");
        Vector3 vDir = transform.right * InputX + transform.forward * InputY;

        float Speed = WalkSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Speed = RunSpeed;
        }

        vDir *= Speed;

        Velocity.x = vDir.x;
        Velocity.z = vDir.z;

        if(AnimatorControl != null)
        {
            AnimatorControl.SetFloat("Speed", vDir.magnitude / RunSpeed);
        }
    }

    private string GetGoundTag()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 0.2f))
        {
            return hit.collider.tag;
        }
        return null;
    }
}
