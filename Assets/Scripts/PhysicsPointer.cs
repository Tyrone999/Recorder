using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhysicsPointer : MonoBehaviour
{
    public Transform parent;

    private GameObject item;
    private float increment;
    public GameObject pointOfTransformation;
    private RaycastHit hit;
    private float playerHeight;
    public GameObject grabberToDisable;

    public GameObject enableAfterYouGrabbedTheWeapon;

    public TMP_Text objective;

    public OVRInput.RawButton indexTrigger;
    public OVRInput.RawButton handTrigger;
    public OVRInput.Controller controller;
    public OVRInput.RawAxis2D thumbStick;

    public bool itemPickup;

    public float defaultLength = 3.0f;
    public float defaultHeight = 3.0f;

    public GameObject teleportEndVisual;

    private LineRenderer lineRenderer;

    private float itteration;
    private float distance;
    private Vector3[] lerpedPoints;

    public LineRenderer straightLine;
    public LineRenderer curvedLine;

    private bool curved;

    public float turnSpeed = 3;
    public float speed = 3;

    public Material materialBool;

    public bool smoothRotation;
    public bool movementHand;
    public bool inverseTeleport;

    public LayerMask rayCastItems;
    public LayerMask interactableItems;

    private float h;
    private float x;
    private float c;

    private Vector3 startPointPosition;
    private Vector3 midPointPosition;
    private Vector3 endPointPosition;

    private GameObject destroyAfterNewInitialisation;

    public GameObject heldItem;
    public GameObject playerObject;

    private bool hasItem;
    private bool resetRotation;
    private bool hitFloor;
    private bool disableStraightLine;
    private bool grabbedWeapon = false;
    private bool pressedGrab;

    private void Start()
    {
        pressedGrab = false;
        increment = 0;
        grabbedWeapon = false;
        disableStraightLine = false;
        if (itemPickup)
        {
            objective.text = "~ Grab The Bow";
            curved = false;
        }
        else
        {
            curved = true;
        }
        smoothRotation = false;
        playerHeight = playerObject.transform.position.y;
    }
    private void Awake()
    {
        if (curved)
        {
            curvedLine.enabled = true;
            straightLine.enabled = false;
            lineRenderer = curvedLine;
            lineRenderer.gameObject.SetActive(false);
        }
        else if (!curved)
        {
            straightLine.enabled = true;
            curvedLine.enabled = false;
            lineRenderer = straightLine;
            lineRenderer.gameObject.SetActive(true);
        }
    }
    private void FixedUpdate()
    {
        //if (OVRInput.GetDown(OVRInput.RawButton.A)) //movement toggle
        //{
        //    movementHand = !movementHand;
        //}
        Vector2 _thumbStick = OVRInput.Get(thumbStick);

        if (movementHand)
        {
            playerObject.transform.Translate(Vector3.forward * _thumbStick.y * speed * Time.deltaTime);
            playerObject.transform.Translate(Vector3.right * _thumbStick.x * speed * Time.deltaTime);
        }
        else if (!movementHand)
        {
            if (smoothRotation)
            {
                playerObject.transform.localRotation = Quaternion.Euler(playerObject.transform.localRotation.eulerAngles + new Vector3(0, _thumbStick.x * turnSpeed, 0));
            }
            else if (!smoothRotation)
            {
                if (_thumbStick.x > 0.01)
                {
                    if (resetRotation)
                    {
                        playerObject.transform.localRotation = Quaternion.Euler(playerObject.transform.localRotation.eulerAngles + new Vector3(0, 30, 0));
                        resetRotation = false;
                    }
                }
                else if (_thumbStick.x < -0.01)
                {
                    if (resetRotation)
                    {
                        playerObject.transform.localRotation = Quaternion.Euler(playerObject.transform.localRotation.eulerAngles + new Vector3(0, -30, 0));
                        resetRotation = false;
                    }
                }
                else
                {
                    resetRotation = true;
                }
            }
        }
    }
    private void Update()
    {
        if (movementHand)
        {
            if (!pressedGrab)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Interactable"))
                    {
                        if (OVRInput.Get(handTrigger, controller) && pressedGrab == false)
                        {
                            pressedGrab = true;
                        }
                    }
                }
            }
            if (pressedGrab && increment <= 1)
            {
                if(item == null)
                {
                    item = hit.collider.gameObject;
                }

                Destroy(item.GetComponent<Rigidbody>());
                Destroy(item.GetComponent<Collider>());

                Vector3 posA = item.transform.position;
                Vector3 posB = pointOfTransformation.transform.position;
                Quaternion rotA = item.transform.rotation;
                Quaternion rotB = pointOfTransformation.transform.rotation;

                item.transform.rotation = Quaternion.Slerp(rotA, rotB, increment);
                item.transform.position = Vector3.Slerp(posA, posB, increment);
                increment += 0.01f;
                grabbedWeapon = false;
            }

            if (increment >= 1 && objective.text == "~ Grab The Bow")
            {
                objective.text = "~ Pull The Bow String To Shoot";
                grabbedWeapon = true;
                grabberToDisable.SetActive(false);
                disableStraightLine = true;
                item.transform.SetParent(parent);
            }

            enableAfterYouGrabbedTheWeapon.SetActive(grabbedWeapon);
        }

        if (disableStraightLine)
        {
            lineRenderer.gameObject.SetActive(false);
        }
        if (heldItem != null)
        {
            hasItem = true;
        }
        else
        {
            hasItem = false;
        }
        if (curved)
        {
            curvedLine.enabled = true;
            straightLine.enabled = false;
            lineRenderer = curvedLine;
            lineRenderer.enabled = true;
        }
        else if (!curved)
        {
            straightLine.enabled = true;
            curvedLine.enabled = false;
            lineRenderer = straightLine;
            if (OVRInput.Get(handTrigger))
            {
                lineRenderer.enabled = false;
            }
            else
            {
                lineRenderer.enabled = true;
            }
        }

        UpdateLength();
        if (curved)
        {
            if (OVRInput.GetDown(indexTrigger, controller))
            {
                lineRenderer.gameObject.SetActive(true);
                if (destroyAfterNewInitialisation != null)
                {
                    destroyAfterNewInitialisation.gameObject.SetActive(false);
                }
            }
            else if (OVRInput.GetUp(indexTrigger, controller))
            {
                if (destroyAfterNewInitialisation != null)
                {
                    destroyAfterNewInitialisation.gameObject.SetActive(true);
                    playerObject.transform.position = new Vector3(destroyAfterNewInitialisation.transform.position.x, playerHeight + destroyAfterNewInitialisation.transform.position.y, destroyAfterNewInitialisation.transform.position.z);
                    lineRenderer.gameObject.SetActive(false);
                }
            }

            if (OVRInput.Get(indexTrigger, controller))
            {
                if (destroyAfterNewInitialisation != null)
                {
                    destroyAfterNewInitialisation.gameObject.SetActive(true);
                }
            }
            else
            {
                lineRenderer.enabled = false;
                if (destroyAfterNewInitialisation != null)
                {
                    destroyAfterNewInitialisation.gameObject.SetActive(false);
                }
            }

            if (lineRenderer.positionCount == 0)
            {
                if (destroyAfterNewInitialisation != null)
                {
                    Destroy(destroyAfterNewInitialisation);
                }
            }
        }
    }

    private void UpdateLength()
    {
        startPointPosition = transform.position;
        if (!curved)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, CalculateEnd());
        }
        else if (curved)
        {
            lineRenderer.positionCount = CalculateDistance();
            lerpedPoints = new Vector3[lineRenderer.positionCount];
            for (int i = 0; i < lerpedPoints.Length; i++)
            {
                lerpedPoints[i] = DetermineNewVector3(i);
            }
            for (int i = 0; i < lerpedPoints.Length; i++)
            {
                if (destroyAfterNewInitialisation != null)
                {
                    Destroy(destroyAfterNewInitialisation);
                }
                if (hitFloor)
                {
                    lineRenderer.SetPosition(i, lerpedPoints[i]);
                    if (lerpedPoints.Length == i + 1)
                    {
                        destroyAfterNewInitialisation = Instantiate(teleportEndVisual, lineRenderer.GetPosition(i), Quaternion.Euler(Vector3.zero));
                    }
                }
                else
                {
                    lineRenderer.enabled = false;
                }
            }
        }
    }

    private int CalculateDistance()
    {
        hit = CreateForwardRaycast();
        if (hit.collider != null)
        {
            midPointPosition = Vector3.Lerp(startPointPosition, endPointPosition, 0.5f) + new Vector3(0, defaultHeight, 0);
            endPointPosition = hit.point;
            h = defaultHeight;
            x = hit.distance * 0.5f;
            c = Mathf.Sqrt((x * x) + (h * h));
            float answer = c * 2;
            int finalAnswer = Mathf.RoundToInt(answer * 10);
            for (float i = answer; i < 0; i -= 0.1f)
            {
                finalAnswer++;
            }
            itteration = 1.0f / finalAnswer;
            hitFloor = true;
            return finalAnswer;
        }
        return 0;
    }

    private Vector3 DetermineNewVector3(int i)
    {
        distance = itteration * i;
        Vector3 startToMid = Vector3.Lerp(startPointPosition, midPointPosition, distance);
        Vector3 midToEnd = Vector3.Lerp(midPointPosition, endPointPosition, distance);
        return Vector3.Lerp(startToMid, midToEnd, distance);
    }

    private Vector3 CalculateEnd()
    {
        hit = CreateForwardRaycast();
        Vector3 endPosition = DefaultEnd(defaultLength);

        if (hit.collider)
        {
            endPosition = hit.point;
            if (hit.collider.CompareTag("Interactable"))
            {
                lineRenderer.GetComponent<ColorSelector>().colorState = true;
                if (OVRInput.Get(handTrigger))
                {
                    heldItem = hit.collider.gameObject;
                    hasItem = true;
                }

            }
            else
            {
                lineRenderer.GetComponent<ColorSelector>().colorState = false;
            }
        }
        else
        {
            lineRenderer.GetComponent<ColorSelector>().colorState = false;
        }

        return endPosition;

    }

    private RaycastHit CreateForwardRaycast()
    {
        Ray ray;
        RaycastHit hit;
        if (curved)
        {
            if (inverseTeleport)
            {
                ray = new Ray(transform.position, -transform.up);
            }
            else
            {
                ray = new Ray(transform.position, transform.forward);
            }
            Physics.Raycast(ray, out hit, defaultLength, rayCastItems);
            return hit;
        }
        else
        {
            ray = new Ray(transform.position, transform.forward);
            Physics.Raycast(ray, out hit, defaultLength, interactableItems);
            return hit;
        }
    }

    private Vector3 DefaultEnd(float length)
    {
        return transform.position + (transform.forward * length);
    }
}
