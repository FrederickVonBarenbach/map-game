using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public static class Tools
{

    public static float Dcm(float n, int d = 2)
    {
        return (Mathf.Round(n * Mathf.Pow(10, d)) / Mathf.Pow(10, d));
    }

    //==============================================================================

    public static void Pathfinding(GameObject start, GameObject end)
    {
        DateTime startTime = DateTime.Now;
        //WE CAN ALWAYS MAKE IT GET COMPONENT REGION FROM ARMY!
        Region startRegion = start.GetComponent<Region>();
        Region endRegion = end.GetComponent<Region>();

        Debug.Log("from " + start.name + " to " + end.name);

        List<Node> openNodes = new List<Node>();
        List<Node> closedNodes = new List<Node>();

        //*****
        //DETERMINE IF ARMY TO REGION OR ARMY TO ARMY
        //*****

        //MAY NEED TO ADD COROUTINE FOR THIS ???

        openNodes.Add(new Node(startRegion));

        while (openNodes.Count > 0)
        {
            Node currentNode = openNodes[0];

            //Checks for best node
            for (int i = 1; i < openNodes.Count; i++)
            {
                if (openNodes[i].fCost < currentNode.fCost ||
                   (openNodes[i].fCost == currentNode.fCost &&
                    openNodes[i].hCost < currentNode.hCost))
                {
                    currentNode = openNodes[i];
                }
            }

            //Adds the best node to the closedNodes list and removes
            //from the openNodes list
            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);

            if (currentNode.region == endRegion)
            {
                RetracePath(startRegion, currentNode);
            }

            //Updates neighbours with new node values (gCost and hCost) if possible
            //or adds new neighbours to open list
            foreach (Region.AdjRegion adjRegion in
                currentNode.region.adjRegions)
            {
                if (closedNodes.Exists(x => x.region == adjRegion.region))
                {
                    continue;
                }

                float newMovementCostToNeighbour = currentNode.gCost + adjRegion.distance;
                Node neighbourNode = openNodes.Find(x => x.region == adjRegion.region);

                if (neighbourNode == null)
                {
                    Node neighbourNode_ = new Node(adjRegion.region);
                    neighbourNode_.gCost = newMovementCostToNeighbour;
                    neighbourNode_.hCost = Tools.Dcm(60 * Vector3.Distance(endRegion.transform.position,
                                                        neighbourNode_.region.transform.position), 0);
                    neighbourNode_.parent = currentNode;
                    openNodes.Add(neighbourNode_);
                }
                else if (newMovementCostToNeighbour < neighbourNode.gCost)
                {
                    neighbourNode.gCost = newMovementCostToNeighbour;
                    neighbourNode.hCost = Tools.Dcm(60 * Vector3.Distance(endRegion.transform.position,
                                                        neighbourNode.region.transform.position), 0);
                    neighbourNode.parent = currentNode;
                }
            }
        }

        Debug.Log("===============================");
        Debug.Log("It took " + DateTime.Now.Subtract(startTime).Milliseconds + "ms to calculate path");
        Debug.Log("===============================");
    }

    private static void RetracePath(Region startRegion, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode.region != startRegion)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Add(currentNode);
        path.Reverse();

        foreach (Node node in path)
        {
            Debug.Log(node.region.name);
        }
    }

    public class Node
    {
        public Region region;

        public float gCost;
        public float hCost;
        public float fCost
        {
            get { return gCost + hCost; }
        }

        public Node parent;

        public Node(Region region)
        {
            this.region = region;
        }
    }

    //==============================================================================

    public static Main.Date EndDate(int numDays)
    {
        DateTime startTime = DateTime.Now;
        Main main = GameObject.Find("Map").GetComponent<Main>();

        int daysInMonth = main.months[main.date.month].numDays - main.date.day;
        int remDays = numDays;
        Main.Date endDate = new Main.Date(main.date.year, main.date.month, main.date.day);

        while (remDays > 0)
        {
            if (remDays >= main.daysInYear)
            {
                endDate.year++;
                remDays -= main.daysInYear;
            }
            else if (remDays > daysInMonth)
            {
                endDate.day = 0;
                remDays -= daysInMonth;
                if (endDate.month == main.months.Length - 1)
                {
                    endDate.month = 0;
                }
                else
                {
                    endDate.month++;
                }
                daysInMonth = main.months[endDate.month].numDays;
            }
            else
            {
                endDate.day += remDays;
                remDays = 0;
            }
        }

        Debug.Log("===============================");
        Debug.Log("It took " + DateTime.Now.Subtract(startTime).Milliseconds + "ms to calculate end date");
        Debug.Log("===============================");

        return endDate;
    }
}
