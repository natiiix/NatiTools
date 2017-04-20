using System;

namespace NatiTools.xRepeater
{
    public class Repeater
    {
        #region event CooldownExpired
        public event EventHandler CooldownExpired;

        protected virtual void OnCooldownExpired(EventArgs e)
        {
            CooldownExpired?.Invoke(this, e);
        }
        #endregion

        #region Variables
        public bool Enabled;
        public bool AutoRestart;
        public TimeSpan Cooldown;
        private DateTime StartTime;
        #endregion

        #region public NatiRepeater
        public Repeater(TimeSpan _Cooldown)
        {
            Enabled = false;
            Cooldown = _Cooldown;
            AutoRestart = true;
        }

        public Repeater(TimeSpan _Cooldown, bool _AutoRestart)
        {
            Enabled = false;
            Cooldown = _Cooldown;
            AutoRestart = _AutoRestart;
        }
        #endregion

        #region public void Start()
        public void Start()
        {
            Enabled = true;
            StartTime = DateTime.Now;
        }
        #endregion
        #region public void Stop()
        public void Stop()
        {
            Enabled = false;
        }
        #endregion
        #region public void Update()
        public void Update()
        {
            if (Enabled && DateTime.Now > StartTime + Cooldown)
            {
                OnCooldownExpired(new EventArgs());
                if (AutoRestart) StartTime = DateTime.Now;
                else Enabled = false;
            }
        }
        #endregion
    }
}
