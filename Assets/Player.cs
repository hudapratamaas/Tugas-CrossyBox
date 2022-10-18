using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;
public class Player : MonoBehaviour
{
    [SerializeField] TMP_Text stepText;
    [SerializeField, Range(0.01f,1f)] float moveDuration=0.2f;
    [SerializeField, Range(0.01f,1f)] float jumpHeight=0.5f;
    private int minZPos;
    private int extent;
    private float backBaundary;
    private float leftBaundary;
    private float rightBaundary;
    [SerializeField] private int maxTravel;
    [SerializeField] private int currentTravel;

    public int MaxTravel { get => maxTravel; }
    public int CurrentTravel {get => currentTravel;}
    public bool IsDie{ get => this.enabled == false;}

    public void SetUp(int minZPos, int extent)
    {
        backBaundary = minZPos-1;
        leftBaundary = -(extent+1);
        rightBaundary = extent + 1;
    }

    private void Update()
        {
            var moveDir = Vector3.zero;
            if(Input.GetKey(KeyCode.UpArrow))
            {
                    moveDir += new Vector3(0, 0, 1);
            }
            if(Input.GetKey(KeyCode.DownArrow))
            {
                    moveDir += new Vector3(0, 0, -1);    
            }
            if(Input.GetKey(KeyCode.RightArrow))
            {
                    moveDir += new Vector3(1, 0, 0);
            }
            if(Input.GetKey(KeyCode.LeftArrow))
            {
                    moveDir += new Vector3(-1, 0, 0); 
            }


            if(moveDir != Vector3.zero && IsJumping() ==false)
                Jump(moveDir);
        }

        private void Jump(Vector3 targetDirection)
        {
            //var TargetPosition = transform.position + new Vector3(
                //x :dir.x,
               // y : 0,
               // z : dir.y);
            
        //atur rotasi
            Vector3 TargetPosition = transform.position + targetDirection;

            transform.LookAt(TargetPosition);
        
        //loncat ke atas
            var moveSeq = DOTween.Sequence(transform);
            moveSeq.Append(transform.DOMoveY(jumpHeight, moveDuration/2));
            moveSeq.Append(transform.DOMoveY(0, moveDuration/2));

           
           if(TargetPosition.z <= backBaundary ||
              TargetPosition.x <= leftBaundary ||
              TargetPosition.x >= rightBaundary )
           return;

            if(Tree.AllPosition.Contains(TargetPosition))
            return;


        //gerak maju mundur samping
            transform.DOMoveX(TargetPosition.x, moveDuration);
            transform
                .DOMoveZ(TargetPosition.z, moveDuration)
                .OnComplete(UpdateTravel);
        }

        private void UpdateTravel()
        {
            currentTravel = (int) this.transform.position.z;
            if(currentTravel > maxTravel)
                maxTravel = currentTravel;
            
            stepText.text = "STEP " + maxTravel.ToString();
        }
    
        public bool IsJumping()
        {
            return DOTween.IsTweening(transform);
        }
        private void OnTriggerEnter(Collider other)
        {
            if(this.enabled == false)
            return;

                var car = other.GetComponent<Car>();
                if(car != null)
                {
                    AnimateCrash(car);
                }
                //if(other.tag == "Car")
                //{
                       // AnimateDie();
                //}
        }

    private void AnimateCrash(Car car)
    {
        transform.DOScaleY(0.3f,0.2f);
        transform.DOScaleX(3,0.2f);
        transform.DOScaleZ(2,0.2f);
        this.enabled = false;
    }

    private void OnTriggerStay(Collider other)
        {

        }
        private void OnTriggerExit(Collider other)
        {

        }
}
