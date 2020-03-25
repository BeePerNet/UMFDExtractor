using KRPC.Client.Services.SpaceCenter;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace KSPDataExtractor.Models
{
    public class KSPOrbit: ReactiveObject
    {
        public void Update(Orbit orbit)
        {
            Body = orbit.Body.Name;
            Eccentricity = orbit.Eccentricity;
            Epoch = orbit.Epoch;
            Inclination = orbit.Inclination;
            LongitudeOfAscendingNode = orbit.LongitudeOfAscendingNode;
            MeanAnomaly = orbit.MeanAnomaly;
            MeanAnomalyAtEpoch = orbit.MeanAnomalyAtEpoch;
            OrbitalSpeed = orbit.OrbitalSpeed;
            Periapsis = orbit.Periapsis;
            PeriapsisAltitude = orbit.PeriapsisAltitude;
            Period = orbit.Period;
            Radius = orbit.Radius;
            SemiMajorAxis = orbit.SemiMajorAxis;
            SemiMinorAxis = orbit.SemiMinorAxis;
            Speed = orbit.Speed;
            TimeToApoapsis = orbit.TimeToApoapsis;
            TimeToPeriapsis = orbit.TimeToPeriapsis;
            EccentricAnomaly = orbit.EccentricAnomaly;
            ArgumentOfPeriapsis = orbit.ArgumentOfPeriapsis;
            ApoapsisAltitude = orbit.ApoapsisAltitude;
            TimeToSOIChange = orbit.TimeToSOIChange;
            TrueAnomaly = orbit.TrueAnomaly;
            Apoapsis = orbit.Apoapsis;
        }


        // Résumé :
        //     The eccentricity of the orbit.
        public string body;
        public string Body
        {
            get => body;
            set => this.RaiseAndSetIfChanged(ref body, value);
        }


        // Résumé :
        //     The eccentricity of the orbit.
        public double eccentricity;
        public double Eccentricity
        {
            get => eccentricity;
            set => this.RaiseAndSetIfChanged(ref eccentricity, value);
        }
        //
        // Résumé :
        //     The time since the epoch (the point at which the mean anomaly at epoch was measured,
        //     in seconds.
        public double epoch;
        public double Epoch
        {
            get => epoch;
            set => this.RaiseAndSetIfChanged(ref epoch, value);
        }
        //
        // Résumé :
        //     The inclination of the orbit, in radians.
        public double inclination;
        public double Inclination
        {
            get => inclination;
            set => this.RaiseAndSetIfChanged(ref inclination, value);
        }
        //
        // Résumé :
        //     The longitude of the ascending node, in radians.
        public double longitudeOfAscendingNode;
        public double LongitudeOfAscendingNode
        {
            get => longitudeOfAscendingNode;
            set => this.RaiseAndSetIfChanged(ref longitudeOfAscendingNode, value);
        }
        //
        // Résumé :
        //     The mean anomaly.
        public double meanAnomaly;
        public double MeanAnomaly
        {
            get => meanAnomaly;
            set => this.RaiseAndSetIfChanged(ref meanAnomaly, value);
        }
        //
        // Résumé :
        //     The mean anomaly at epoch.
        public double meanAnomalyAtEpoch;
        public double MeanAnomalyAtEpoch
        {
            get => meanAnomalyAtEpoch;
            set => this.RaiseAndSetIfChanged(ref meanAnomalyAtEpoch, value);
        }
        //
        // Résumé :
        //     If the object is going to change sphere of influence in the future, returns the
        //     new orbit after the change. Otherwise returns null.
        //[RPC("SpaceCenter", "Orbit_get_NextOrbit")]
        //public Orbit NextOrbit { get; }
        //
        // Résumé :
        //     The current orbital speed in meters per second.
        public double orbitalSpeed;
        public double OrbitalSpeed
        {
            get => orbitalSpeed;
            set => this.RaiseAndSetIfChanged(ref orbitalSpeed, value);
        }
        //
        // Résumé :
        //     The periapsis of the orbit, in meters, from the center of mass of the body being
        //     orbited.
        //
        // Remarques :
        //     For the periapsis altitude reported on the in-game map view, use SpaceCenter.Orbit.PeriapsisAltitude.
        double periapsis;

        public double Periapsis
        {
            get => periapsis;
            set => this.RaiseAndSetIfChanged(ref periapsis, value);
        }


        //
        // Résumé :
        //     The periapsis of the orbit, in meters, above the sea level of the body being
        //     orbited.
        //
        // Remarques :
        //     This is equal to SpaceCenter.Orbit.Periapsis minus the equatorial radius of the
        //     body.
        double periapsisAltitude;

        public double PeriapsisAltitude
        {
            get => periapsisAltitude;
            set => this.RaiseAndSetIfChanged(ref periapsisAltitude, value);
        }

        //
        // Résumé :
        //     The orbital period, in seconds.
        double period;

        public double Period
        {
            get => period;
            set => this.RaiseAndSetIfChanged(ref period, value);
        }
        //
        // Résumé :
        //     The current radius of the orbit, in meters. This is the distance between the
        //     center of mass of the object in orbit, and the center of mass of the body around
        //     which it is orbiting.
        //
        // Remarques :
        //     This value will change over time if the orbit is elliptical.
        public double radius;
        public double Radius
        {
            get => radius;
            set => this.RaiseAndSetIfChanged(ref radius, value);
        }
        //
        // Résumé :
        //     The semi-major axis of the orbit, in meters.
        public double semiMajorAxis;
        public double SemiMajorAxis
        {
            get => semiMajorAxis;
            set => this.RaiseAndSetIfChanged(ref semiMajorAxis, value);
        }
        //
        // Résumé :
        //     The semi-minor axis of the orbit, in meters.
        public double semiMinorAxis;
        public double SemiMinorAxis
        {
            get => semiMinorAxis;
            set => this.RaiseAndSetIfChanged(ref semiMinorAxis, value);
        }
        //
        // Résumé :
        //     The current orbital speed of the object in meters per second.
        //
        // Remarques :
        //     This value will change over time if the orbit is elliptical.
        public double speed;
        public double Speed
        {
            get => speed;
            set => this.RaiseAndSetIfChanged(ref speed, value);
        }
        //
        // Résumé :
        //     The time until the object reaches apoapsis, in seconds.
        public double timeToApoapsis;
        public double TimeToApoapsis
        {
            get => timeToApoapsis;
            set => this.RaiseAndSetIfChanged(ref timeToApoapsis, value);
        }
        //
        // Résumé :
        //     The time until the object reaches periapsis, in seconds.
        double timeToPeriapsis;
        public double TimeToPeriapsis
        {
            get => timeToPeriapsis;
            set => this.RaiseAndSetIfChanged(ref timeToPeriapsis, value);
        }
        //
        // Résumé :
        //     The eccentric anomaly.
        public double eccentricAnomaly;
        public double EccentricAnomaly
        {
            get => eccentricAnomaly;
            set => this.RaiseAndSetIfChanged(ref eccentricAnomaly, value);
        }

        //
        // Résumé :
        //     The celestial body (e.g. planet or moon) around which the object is orbiting.
        //[RPC("SpaceCenter", "Orbit_get_Body")]
        //public CelestialBody Body { get; }
        //
        // Résumé :
        //     The argument of periapsis, in radians.
        public double argumentOfPeriapsis;
        public double ArgumentOfPeriapsis
        {
            get => argumentOfPeriapsis;
            set => this.RaiseAndSetIfChanged(ref argumentOfPeriapsis, value);
        }
        //
        // Résumé :
        //     The apoapsis of the orbit, in meters, above the sea level of the body being orbited.
        //
        // Remarques :
        //     This is equal to SpaceCenter.Orbit.Apoapsis minus the equatorial radius of the
        //     body.
        public double apoapsisAltitude;
        public double ApoapsisAltitude
        {
            get => apoapsisAltitude;
            set => this.RaiseAndSetIfChanged(ref apoapsisAltitude, value);
        }
        //
        // Résumé :
        //     The time until the object changes sphere of influence, in seconds. Returns NaN
        //     if the object is not going to change sphere of influence.
        public double timeToSOIChange;
        public double TimeToSOIChange
        {
            get => timeToSOIChange;
            set => this.RaiseAndSetIfChanged(ref timeToSOIChange, value);
        }
        //
        // Résumé :
        //     The true anomaly.
        public double trueAnomaly;
        public double TrueAnomaly
        {
            get => trueAnomaly;
            set => this.RaiseAndSetIfChanged(ref trueAnomaly, value);
        }
        //
        // Résumé :
        //     Gets the apoapsis of the orbit, in meters, from the center of mass of the body
        //     being orbited.
        //
        // Remarques :
        //     For the apoapsis altitude reported on the in-game map view, use SpaceCenter.Orbit.ApoapsisAltitude.
        public double apoapsis;

        public double Apoapsis
        {
            get => apoapsis;
            set => this.RaiseAndSetIfChanged(ref apoapsis, value);
        }
    }
}
