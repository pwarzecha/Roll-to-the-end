using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        direction.z = forwardSpeed;

    }

    private void FixedUpdate() {

        
    }

    private void LateUpdate() {
        controller.Move(direction * 0.07f * Time.fixedDeltaTime);
    }
}
