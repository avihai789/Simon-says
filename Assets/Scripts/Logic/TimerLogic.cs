using System;

namespace SimonSays.Logic
{
    // This class is responsible for the game timer logic only
    public class TimerLogic
    {
        private bool _isEnabled = false;
        private float _currentRemainTime;
        private readonly SimonLogic _simonLogic;
        
        public event Action TimerEnd;
        public event Action<float> TimerChanged;

        public TimerLogic(int timeLimit)
        {
            _currentRemainTime = timeLimit;
            _isEnabled = true;
        }

        public void Update(float deltaTime)
        {
            if (!_isEnabled) return;
            _currentRemainTime -= deltaTime;
            TimerChanged?.Invoke(_currentRemainTime);
            if (_currentRemainTime <= 0)
            {
                OnTimerEnd();
            }
        }

        private void OnTimerEnd()
        {
            _isEnabled = false;
            TimerEnd?.Invoke();
        }

        public void StopTimer()
        {
            _isEnabled = false;
        }
    }
}