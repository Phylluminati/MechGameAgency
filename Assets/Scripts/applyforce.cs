using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class applyforce : MonoBehaviour
{
    private Rigidbody2D _rig;
    float magnitude;
    Vector3 scaleChange = new Vector3(-0.4f, -0.4f, -0.4f);
    /*public Colors pickedColor = Colors.Black;
    public enum Colors
    {
        Red,
        Black,
        Green,
        Purple
    } */
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*switch(pickedColor)
        {
            case Colors.Red:
            this.GetComponent<TMP_Text>().color = Color.purple;
            break;
            case Colors.Green:
            this.GetComponent<TMP_Text>().color = Color.purple;
            break;
            case Colors.Black:
            this.GetComponent<TMP_Text>().color = Color.purple;
            break;
            case Colors.Purple:
            this.GetComponent<TMP_Text>().color = Color.purple;
            break;
        }*/
    
        magnitude = 10;
        float random = Random.Range(0f, 260f);
        _rig = GetComponent<Rigidbody2D>();
        _rig.AddForce(new Vector2(Mathf.Cos(random), Mathf.Sin(random))*magnitude, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale += scaleChange * Time.deltaTime;
        if(transform.localScale.x <=  0.4f && transform.localScale.y <= 0.4f)
        {
            Destroy(this.gameObject);
        }
    }
}
