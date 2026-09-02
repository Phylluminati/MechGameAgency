using Unity.VisualScripting;
using UnityEngine;

public class ENTracker : MonoBehaviour
{
    [SerializeField]
    GameObject ENPin;
    private float _target;
    public float _reduceSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void UpdateEN(int CurrentEN)
    {
    print(CurrentEN);
    switch(CurrentEN)
        {
            case 0:
                _target = 137;
            break;

            case 1:
                _target = 87;
            break;

            case 2:
                _target = 37;
            break;

            case 3:
                _target = 0;
            break;
            case 4:
                _target = 320;
            break;

            case 5:
                _target = 275;
            break;

            case 6:
                _target = 220;
            break;

            case var expression when CurrentEN >= 7:
                _target = -150;
            break;

        }
        print("target: " + _target);
    }

    // Update is called once per frame
    void Update()
    {
        //ENPin.transform.eulerAngles = new Vector3(0, 0, Mathf.MoveTowards(ENPin.transform.rotation.z, _target, _reduceSpeed * Time.deltaTime));
        //ENPin.transform.Rotate(0, 0, Mathf.MoveTowards(ENPin.transform.localRotation.z, _target, _reduceSpeed * Time.deltaTime));
        //ENPin.transform.eulerAngles = new Vector3(0, 0, _target);
        ENPin.transform.eulerAngles = new Vector3(0, 0, Mathf.MoveTowards(ENPin.transform.eulerAngles.z, _target, _reduceSpeed * Time.deltaTime));
    }
}
