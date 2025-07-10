using UnityEngine.Events;

public static class EventManager
{
    public static event UnityAction TimerStart;
    public static event UnityAction TimerStop;
    public static event UnityAction SaveTimer;

    public static void OnTimerStart() => TimerStart?.Invoke();
    public static void OnTimerStop() => TimerStop?.Invoke();
    public static void OnSaveTimer() => SaveTimer?.Invoke();
}
