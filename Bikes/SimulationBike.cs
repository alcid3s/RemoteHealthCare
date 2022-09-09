using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteHealthCare.Bikes
{
    internal class SimulationBike : IBike
    {
        public OnUpdate OnUpdate { get; set; }

        public decimal ElapsedTime { get { return (decimal)_elapsedTime; } }

        public int DistanceTravelled { get { return (int)_distanceTravelled; } }

        public decimal Speed { get { return (decimal)_speed; } }

        public int HeartRate { get { return (int)_heartRate; } }

        public bool IsRunning { get { return _isRunning; } set { if (!_isRunning && value) Run(); _isRunning = value; } }

        public int Gear { get; set; }

        private double _elapsedTime;
        
        private double _distanceTravelled;

        private double _speed;

        private double _heartRate;

        private bool _isRunning;

        private void Run()
        {
            _isRunning = true;
            if (Gear < 1 || Gear > 7)
                Gear = 4;
            new Thread(x => Simulate()).Start();
        }

        private void Simulate()
        {
            double deltaTime = 0.05;

            int callTicks = 5;
            int tickCounter = 0;

            _elapsedTime = 0;
            _distanceTravelled = 0;
            _speed = 0;
            _heartRate = 70;

            Random random = new Random();

            double targetSpeed = 0;

            IsRunning = true;

            double pedalAngle = random.NextDouble() * Math.PI; Math.Sin(pedalAngle);

            while (IsRunning)
            {
                if (random.NextDouble() < 0.001 / deltaTime)
                    targetSpeed = Enumerable.Repeat(random.NextDouble(), 15).Sum() - 1;

                double acceleration = 0;
                double force = 0;

                if (targetSpeed > _speed) 
                {
                    force = Math.Min((targetSpeed - _speed), 5);
                    acceleration += (Math.Pow(Math.Sin(pedalAngle), 2) + 0.1) * force;
                    pedalAngle += acceleration * Math.Pow(deltaTime, 2) * Math.PI / Gear;
                }
                acceleration -= (_speed + 2) * 0.2 * deltaTime;

                _speed += acceleration * deltaTime;
                if (_speed < 0)
                    _speed = 0;

                _distanceTravelled += _speed * deltaTime;

                double interpolate = Math.Pow(0.9, deltaTime);
                _heartRate = _heartRate * interpolate + (70 + force * 10) * (1 - interpolate);

                if (tickCounter % callTicks == 0)
                    OnUpdate();
                tickCounter++;
                _elapsedTime = tickCounter * deltaTime;

                Thread.Sleep((int)(deltaTime * 1000));
            }
        }
    }
}
