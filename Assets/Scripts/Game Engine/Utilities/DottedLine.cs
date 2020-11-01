using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedLine : Singleton<DottedLine>
{
    // Inspector fields
    public GameObject ArrowPrefab;
    [Range(0.01f, 1f)]
    public float Size;
    [Range(0.1f, 2f)]
    public float Delta;

    // Utility fields
    List<Vector2> positions = new List<Vector2>();
    List<GameObject> dots = new List<GameObject>();
    
    public void DrawDottedLine(Vector2 start, Vector2 end)
    {
        //DestroyAllPaths();

        Vector2 point = start;
        Vector2 direction = (end - start).normalized;

        while ((end - start).magnitude > (point - start).magnitude)
        {
            positions.Add(point);
            point += (direction * Delta);
        }

        Render(start, end);
    }
    public void DestroyAllPaths()
    {        
        foreach (var dot in dots)
        {
            Destroy(dot);
        }

        positions.Clear();
        dots.Clear();
    }

    /*
    GameObject GetOneDot()
    {
        var gameObject = new GameObject();
        gameObject.transform.localScale = Vector3.one * Size;
        gameObject.transform.parent = transform;

        var sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = Dot;
        return gameObject;
    }
    */
    GameObject GetOneArrow()
    {
        var gameObject = Instantiate(ArrowPrefab);
        gameObject.transform.localScale = Vector3.one * Size;
        gameObject.transform.parent = transform;
        return gameObject;
    }

    private void Render(Vector2 start, Vector2 end)
    {
        var dir = end - start;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        foreach (var position in positions)
        {
            // var g = GetOneDot();
            var g = GetOneArrow();
            g.transform.position = position;
            g.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            dots.Add(g);
        }

        if(dots.Count > 1)
        {
            Destroy(dots[0].gameObject);
        }
       
    }

}

