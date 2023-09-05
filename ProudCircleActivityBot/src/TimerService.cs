using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProudCircleActivityBot;

public class TimerService {
    private Timer _timer;
    private Func<Task> _callback;
    private int _intervalInSeconds;
    private bool _isRunning;

    public TimerService(Func<Task> callback, int intervalInSeconds) {
        _callback = callback;
        _intervalInSeconds = intervalInSeconds;
        _isRunning = false;
    }

    public void Start() {
        if (!_isRunning) {
            _timer = new Timer(async _ => { await RunCallbackAsync(); }, null, 0,
                _intervalInSeconds * 1000); // Convert seconds to milliseconds
            _isRunning = true;
        }
    }

    public void Pause() {
        if (_isRunning) {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            _isRunning = false;
        }
    }

    public void Resume() {
        if (!_isRunning) {
            _timer.Change(0, _intervalInSeconds * 1000);
            _isRunning = true;
        }
    }

    public void Reset() {
        Pause();
        Start();
    }

    public void Stop() {
        Pause();
        _timer.Dispose();
        _isRunning = false;
    }

    private async Task RunCallbackAsync() {
        await Task.Run(async () => await _callback.Invoke());
    }
}