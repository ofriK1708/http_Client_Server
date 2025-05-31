namespace calc_server;

public static class RequestTracker
{
    private static int _counter = 1;
    public static int GetAndIncrementCounter() => _counter++;
}