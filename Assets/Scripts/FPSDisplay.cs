using UnityEngine;

#if UNITY_EDITOR
public class FPSDisplay : MonoBehaviour
{
    float deltaTime;
    float timer;
    float refreshRate = 1f; // обновлять каждую секунду

    float currentFPS;
    float minFPS = float.MaxValue;
    float maxFPS = float.MinValue;
    float avgFPS;
    int frameCount;

    void Update()
    {
        float frameTime = Time.unscaledDeltaTime;
        deltaTime += frameTime;
        frameCount++;

        float fps = 1f / frameTime;
        minFPS = Mathf.Min(minFPS, fps);
        maxFPS = Mathf.Max(maxFPS, fps);

        timer += Time.unscaledDeltaTime;
        if (timer >= refreshRate)
        {
            avgFPS = frameCount / deltaTime;
            currentFPS = 1f / Time.unscaledDeltaTime;

            timer = 0f;
            deltaTime = 0f;
            frameCount = 0;
            minFPS = float.MaxValue;
            maxFPS = float.MinValue;
        }
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(10, 10, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 50;
        style.normal.textColor = Color.white;

        string text = $"FPS: {currentFPS:0}\nMin: {minFPS:0}\nMax: {maxFPS:0}\nAvg: {avgFPS:0}";
        GUI.Label(rect, text, style);
    }
}
#endif