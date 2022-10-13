using System;
using System.Linq;
using System.Threading;

namespace RemoteHealthCare.Bikes
{
    internal class SimulationBike : IBike
    {
        public OnUpdate OnUpdate { get; set; }

        public decimal ElapsedTime => (decimal)_elapsedTime;

        public int DistanceTravelled => (int)_distanceTravelled;

        public decimal Speed => (decimal)_speed;

        public int HeartRate => (int)_heartRate;

        //toggles the thread running the bike 
        public bool IsRunning { get { return _isRunning; } set { if (!_isRunning && value) Run(); _isRunning = value; } }

        private int Gear { get; set; }

        private double _elapsedTime;
        
        private double _distanceTravelled;

        private double _speed;

        private double _heartRate;

        private bool _isRunning;

        //runs the simulation
        private void Run()
        {
            _isRunning = true;
            if (Gear < 1 || Gear > 7)
                Gear = 4;
            new Thread(Simulate).Start();
        }
        //the simulation code
        private void Simulate()
        {
            //calculation resolution
            double deltaTime = 0.05;

            //amount of time units per notification
            int callTicks = 5;
            int tickCounter = 0;

            _elapsedTime = 0;
            _distanceTravelled = 0;
            _speed = 0;
            _heartRate = 70;

            Random random = new Random();

            double targetSpeed = 0;

            //angle of the pedals
            double pedalAngle = random.NextDouble() * Math.PI; Math.Sin(pedalAngle);

            while (IsRunning)
            {
                //occasionally picks a new speed
                if (random.NextDouble() < deltaTime * 0.25)
                    targetSpeed = Enumerable.Repeat(random.NextDouble(), 10).Sum() + 1;
                if (targetSpeed < 3)
                    targetSpeed = 0;

                double acceleration = 0;
                double force = 0;

                //when aiming to go faster
                if (targetSpeed > _speed) 
                {
                    //the force is manipulated by the angle of the pedals to get the acceleration
                    force = Math.Min((targetSpeed - _speed), 5);
                    acceleration += (Math.Pow(Math.Sin(pedalAngle), 2) + 0.1) * force;
                    pedalAngle += acceleration * Math.Pow(deltaTime, 2) * Math.PI / Gear;
                }
                //friction
                acceleration -= (_speed + 2) * 0.3 * deltaTime;

                //change the speed by the acceleration (v = a * t)
                _speed += acceleration * deltaTime;
                if (_speed < 0)
                    _speed = 0;

                //change the distance by the speed (s = v * t)
                _distanceTravelled += _speed * deltaTime;

                //changes the heart rate according to the force exerted
                double interpolate = Math.Pow(0.9, deltaTime);
                _heartRate = _heartRate * interpolate + (70 + force * 10 + Math.Sqrt(_speed) * 15) * (1 - interpolate);

                //notifies through OnUpdate; random is to simulate the cases where a value was invalid
                if (tickCounter % callTicks == 0 && random.NextDouble() < 0.99)
                    OnUpdate();

                //passes one tick
                tickCounter++;
                _elapsedTime = tickCounter * deltaTime;
                Thread.Sleep((int)(deltaTime * 1000));
            }

            //resets all info
            _elapsedTime = 0;
            _distanceTravelled = 0;
            _speed = 0;
            _heartRate = 255;
        }

        public void Init()
        {
            IsRunning = true;
        }

        public void Stop()
        {
            IsRunning = false;
        }
    }
}
