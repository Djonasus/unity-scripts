using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    public Animator animator;

    public Transform plane;
    public Camera cam;

    private const float step = 22.5f;

    //public Sprite N, NW, W, SW, S, SE, E, NE;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        cam = Camera.main;
        //StartCoroutine(LoadCam());
    }


    public void Update()
    {
        //if (!(PlayerMovement.isPOV && transform.parent.tag == "Player"))
        {
            var projected = Vector3.ProjectOnPlane(cam.transform.forward, plane.up);
            var angle = Vector3.SignedAngle(projected, plane.forward, plane.up);

            var AbsAngle = Mathf.Abs(angle);

            /*if (AbsAngle < step) spriteRenderer.sprite = N;
            else if (AbsAngle < step * 3) spriteRenderer.sprite = Mathf.Sign(angle) < 0 ? NW : NE;
            else if (AbsAngle < step * 5) spriteRenderer.sprite = Mathf.Sign(angle) < 0 ? W : E;
            else if (AbsAngle < step * 7) spriteRenderer.sprite = Mathf.Sign(angle) < 0 ? SW : SE;
            else spriteRenderer.sprite = S;
            */

            if (AbsAngle < step) animator.SetFloat("Direction", 0.1f);
            //else if (AbsAngle < step * 3) animator.SetFloat("Direction", Mathf.Sign(angle) < 0 ? 7 : 1);
            else if (AbsAngle < step * 5) animator.SetFloat("Direction", Mathf.Sign(angle) < 0 ? 0.2f : 0.4f);
            //else if (AbsAngle < step * 7) animator.SetFloat("Direction", Mathf.Sign(angle) < 0 ? 5 : 3);
            else animator.SetFloat("Direction", 0.3f);
        } //else
        {
            //animator.SetFloat("Direction", 0.3f);
        }
        BillboardThis(spriteRenderer.transform, cam);
    }

    public void BillboardThis(Transform character, Camera mainCamera)
    {
        var dir = plane.position - mainCamera.transform.position;
        var LookAtRotation = Quaternion.LookRotation(dir);

        var LookAtRotationOnly_Y = Quaternion.Euler(character.rotation.eulerAngles.x, LookAtRotation.eulerAngles.y - 180, character.eulerAngles.z);
        character.rotation = LookAtRotationOnly_Y;
    }
}
