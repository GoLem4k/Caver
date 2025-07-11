using System;
using UnityEngine;

public class PausedBehaviour : MonoBehaviour
{
    public static bool PAUSE;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PAUSE = !PAUSE;
        }
        if (PAUSE) PausedUpdate();
        else GameUpdate();
    }

    private void FixedUpdate()
    {
        if (PAUSE) PausedFixedUpdate();
        else GameFixedUpdate();
    }

    public virtual void GameUpdate()
    {
    }

    public virtual void PausedUpdate()
    {
    }
    
    public virtual void GameFixedUpdate()
    {
    }

    public virtual void PausedFixedUpdate()
    {
    }
}
