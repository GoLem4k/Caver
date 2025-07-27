using System;
using UnityEngine;

public class PausedBehaviour : MonoBehaviour
{
    public static bool PAUSE;
    
    private void Update()
    {
        if (PAUSE) PausedUpdate();
        else GameUpdate();
    }

    protected void SwitchPause()
    {
        PAUSE = !PAUSE;
    }
    
    private void FixedUpdate()
    {
        if (PAUSE) PausedFixedUpdate();
        else GameFixedUpdate();
    }

    protected virtual void GameUpdate()
    {
    }

    protected virtual void PausedUpdate()
    {
    }
    
    protected virtual void GameFixedUpdate()
    {
    }

    protected virtual void PausedFixedUpdate()
    {
    }
}
