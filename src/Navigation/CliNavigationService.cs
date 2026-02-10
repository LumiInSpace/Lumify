namespace Lumify.src.Navigation;

public class CliNavigationService
{
    private bool _startRequested;

    public void RequestStart()
    {
        _startRequested = true;
    }

    public bool ConsumeStartRequest()
    {
        if (!_startRequested)
        {
            return false;
        }

        _startRequested = false;
        return true;
    }
}
