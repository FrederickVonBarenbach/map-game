using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Main : MonoBehaviour
{
    //Todo list:
    //- Regions Resource gathering <<<<<
    //- Region towns
    //- Armies
    //- Make work for not only red

    //Pathfinding
    private GameObject startRegion;

    //Clock
    public Text text;
    public int daysInYear;
    public Date date;
    public Month[] months = { new Month("January", 31), new Month("February", 29),
                              new Month("March", 31), new Month("April", 30),
                              new Month("May", 31), new Month("June", 30),
                              new Month("July", 31), new Month("August", 30),
                              new Month("September", 31), new Month("October", 30),
                              new Month("November", 31), new Month("December", 30)};
    public int timeStampIndex;
    private float[] timeSteps = { 1f, 0.5f, 0.25f, 0.1f };
    public bool pause; //THIS IS A TEMPORARY SUPER SIMPLE SOLUTION THAT HAS ISSUES WITH TIMING

    // Start is called before the first frame update
    void Start()
    {
        CalculateDaysInYear();
        StartCoroutine("BeginTime");
    }

    // Update is called once per frame
    void Update()
    {
        //InputHandler();
    }

    //==============================================================================

    void InputHandler()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && timeStampIndex < timeSteps.Length - 1)
        {
            timeStampIndex++;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && timeStampIndex > 0)
        {
            timeStampIndex--;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            pause = !pause;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null && hit.collider.tag == "Region")
            {
                if (startRegion == null)
                {
                    startRegion = hit.collider.gameObject;
                }
                else
                {
                    Tools.Pathfinding(startRegion, hit.collider.gameObject);
                    startRegion = null;
                }
            }
        }
    }

    //==============================================================================

    void CalculateDaysInYear()
    {
        foreach (Month month in months)
        {
            daysInYear += month.numDays;
        }
    }

    IEnumerator BeginTime()
    {
        for (; ; )
        {
            if (!pause)
            {
                if (date.day == months[date.month].numDays)
                {
                    date.day = 1;
                    if (date.month == months.Length - 1)
                    {
                        date.month = 0;
                        date.year++;
                    }
                    else
                    {
                        date.month++;
                    }
                }
                else
                {
                    date.day++;
                }
                UpdateDate();
                yield return new WaitForSeconds(timeSteps[timeStampIndex]);
            }
            else
            {
                yield return null;
            }
        }
    }

    void UpdateDate()
    {
        text.text = months[date.month].name + " " + date.day + ", " + date.year;
    }

    [System.Serializable]
    public class Date
    {
        public int year;
        public int month;
        public int day;

        public Date(int year, int month, int day)
        {
            this.year = year;
            this.month = month;
            this.day = day;
        }

        public bool Equals(Date date)
        {
            return year == date.year && month == date.month && day == date.day;
        }
    }

    public class Month
    {
        public string name;
        public int numDays;

        public Month(string name, int numDays)
        {
            this.name = name;
            this.numDays = numDays;
        }
    }
}