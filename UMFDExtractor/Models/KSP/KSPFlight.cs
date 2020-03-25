using KRPC.Client.Services.SpaceCenter;
using ReactiveUI;
using System;

namespace UMFDExtractor.Models.KSP
{
    public class KSPFlight : ReactiveObject
    {

        public void Update(Flight flight)
        {
            Normal = flight.Normal;
            Pitch = flight.Pitch;
            Prograde = flight.Prograde;
            Radial = flight.Radial;
            Retrograde = flight.Retrograde;
            Roll = flight.Roll;
            Rotation = flight.Rotation;
            SideslipAngle = flight.SideslipAngle;
            MeanAltitude = flight.MeanAltitude;
            Speed = flight.Speed;
            StaticAirTemperature = flight.StaticAirTemperature;
            StaticPressure = flight.StaticPressure;
            StaticPressureAtMSL = flight.StaticPressureAtMSL;
            SurfaceAltitude = flight.SurfaceAltitude;
            TerminalVelocity = flight.TerminalVelocity;
            TotalAirTemperature = flight.TotalAirTemperature;
            TrueAirSpeed = flight.TrueAirSpeed;
            SpeedOfSound = flight.SpeedOfSound;
            Velocity = flight.Velocity;
            Mach = flight.Mach;
            AerodynamicForce = flight.AerodynamicForce;
            AngleOfAttack = flight.AngleOfAttack;
            AntiNormal = flight.AntiNormal;
            AntiRadial = flight.AntiRadial;
            AtmosphereDensity = flight.AtmosphereDensity;
            BedrockAltitude = flight.BedrockAltitude;
            CenterOfMass = flight.CenterOfMass;
            Longitude = flight.Longitude;
            Direction = flight.Direction;
            DynamicPressure = flight.DynamicPressure;
            Elevation = flight.Elevation;
            EquivalentAirSpeed = flight.EquivalentAirSpeed;
            GForce = flight.GForce;
            Heading = flight.Heading;
            HorizontalSpeed = flight.HorizontalSpeed;
            Latitude = flight.Latitude;
            Lift = flight.Lift;
            Drag = flight.Drag;
            VerticalSpeed = flight.VerticalSpeed;
    }

    //
    // Résumé :
    //     The direction normal to the vessels orbit, in the reference frame SpaceCenter.ReferenceFrame.
    //
    // Retourne :
    //     The direction as a unit vector.
    public Tuple<double, double, double> normal;
        public Tuple<double, double, double> Normal
        {
            get => normal;
            set => this.RaiseAndSetIfChanged(ref normal, value);
        }
        //
        // Résumé :
        //     The pitch of the vessel relative to the horizon, in degrees. A value between
        //     -90° and +90°.
        public float pitch;

        public float Pitch
        {
            get => pitch;
            set => this.RaiseAndSetIfChanged(ref pitch, value);
        }
        //
        // Résumé :
        //     The prograde direction of the vessels orbit, in the reference frame SpaceCenter.ReferenceFrame.
        //
        // Retourne :
        //     The direction as a unit vector.
        public Tuple<double, double, double> prograde;
        public Tuple<double, double, double> Prograde
        {
            get => prograde;
            set => this.RaiseAndSetIfChanged(ref prograde, value);
        }
        //
        // Résumé :
        //     The radial direction of the vessels orbit, in the reference frame SpaceCenter.ReferenceFrame.
        //
        // Retourne :
        //     The direction as a unit vector.
        public Tuple<double, double, double> radial;
        public Tuple<double, double, double> Radial
        {
            get => radial;
            set => this.RaiseAndSetIfChanged(ref radial, value);
        }
        //
        // Résumé :
        //     The retrograde direction of the vessels orbit, in the reference frame SpaceCenter.ReferenceFrame.
        //
        // Retourne :
        //     The direction as a unit vector.
        public Tuple<double, double, double> retrograde;
        public Tuple<double, double, double> Retrograde
        {
            get => retrograde;
            set => this.RaiseAndSetIfChanged(ref retrograde, value);
        }
        //
        // Résumé :
        //     The roll of the vessel relative to the horizon, in degrees. A value between -180°
        //     and +180°.
        public float roll;
        public float Roll
        {
            get => roll;
            set => this.RaiseAndSetIfChanged(ref roll, value);
        }
        //
        // Résumé :
        //     The rotation of the vessel, in the reference frame SpaceCenter.ReferenceFrame
        //
        // Retourne :
        //     The rotation as a quaternion of the form (x, y, z, w).
        public Tuple<double, double, double, double> rotation;

        public Tuple<double, double, double, double> Rotation
        {
            get => rotation;
            set => this.RaiseAndSetIfChanged(ref rotation, value);
        }
        //
        // Résumé :
        //     The yaw angle between the orientation of the vessel and its velocity vector,
        //     in degrees.
        public float sideslipAngle;
        public float SideslipAngle
        {
            get => sideslipAngle;
            set => this.RaiseAndSetIfChanged(ref sideslipAngle, value);
        }
        //
        // Résumé :
        //     The altitude above sea level, in meters. Measured from the center of mass of
        //     the vessel.
        public double meanAltitude;
        public double MeanAltitude
        {
            get => meanAltitude;
            set => this.RaiseAndSetIfChanged(ref meanAltitude, value);
        }
        //
        // Résumé :
        //     The speed of the vessel in meters per second, in the reference frame SpaceCenter.ReferenceFrame.
        public double speed;
        public double Speed
        {
            get => speed;
            set => this.RaiseAndSetIfChanged(ref speed, value);
        }

        //
        // Résumé :
        //     The static (ambient) temperature of the atmosphere around the vessel, in Kelvin.
        public float staticAirTemperature;
        public float StaticAirTemperature
        {
            get => staticAirTemperature;
            set => this.RaiseAndSetIfChanged(ref staticAirTemperature, value);
        }
        //
        // Résumé :
        //     The static atmospheric pressure acting on the vessel, in Pascals.
        public float staticPressure;
        public float StaticPressure
        {
            get => staticPressure;
            set => this.RaiseAndSetIfChanged(ref staticPressure, value);
        }
        //
        // Résumé :
        //     The static atmospheric pressure at mean sea level, in Pascals.
        public float staticPressureAtMSL;
        public float StaticPressureAtMSL
        {
            get => staticPressureAtMSL;
            set => this.RaiseAndSetIfChanged(ref staticPressureAtMSL, value);
        }
        //
        // Résumé :
        //     The altitude above the surface of the body or sea level, whichever is closer,
        //     in meters. Measured from the center of mass of the vessel.
        public double surfaceAltitude;

        public double SurfaceAltitude
        {
            get => staticPressureAtMSL;
            set => this.RaiseAndSetIfChanged(ref surfaceAltitude, value);
        }
        //
        // Résumé :
        //     An estimate of the current terminal velocity of the vessel, in meters per second.
        //     This is the speed at which the drag forces cancel out the force of gravity.
        public float terminalVelocity;

        public float TerminalVelocity
        {
            get => terminalVelocity;
            set => this.RaiseAndSetIfChanged(ref terminalVelocity, value);
        }
        //
        // Résumé :
        //     The total air temperature of the atmosphere around the vessel, in Kelvin. This
        //     includes the SpaceCenter.Flight.StaticAirTemperature and the vessel's kinetic
        //     energy.
        public float totalAirTemperature;
        public float TotalAirTemperature
        {
            get => totalAirTemperature;
            set => this.RaiseAndSetIfChanged(ref totalAirTemperature, value);
        }
        //
        // Résumé :
        //     The true air speed of the vessel, in meters per second.
        public float trueAirSpeed;
        public float TrueAirSpeed
        {
            get => trueAirSpeed;
            set => this.RaiseAndSetIfChanged(ref trueAirSpeed, value);
        }
        //
        // Résumé :
        //     The speed of sound, in the atmosphere around the vessel, in m/s.
        public float speedOfSound;
        public float SpeedOfSound
        {
            get => speedOfSound;
            set => this.RaiseAndSetIfChanged(ref speedOfSound, value);
        }
        //
        // Résumé :
        //     The velocity of the vessel, in the reference frame SpaceCenter.ReferenceFrame.
        //
        // Retourne :
        //     The velocity as a vector. The vector points in the direction of travel, and its
        //     magnitude is the speed of the vessel in meters per second.
        public Tuple<double, double, double> velocity;
        public Tuple<double, double, double> Velocity
        {
            get => velocity;
            set => this.RaiseAndSetIfChanged(ref velocity, value);
        }
        //
        // Résumé :
        //     The speed of the vessel, in multiples of the speed of sound.
        public float mach;
        public float Mach
        {
            get => mach;
            set => this.RaiseAndSetIfChanged(ref mach, value);
        }
        //
        // Résumé :
        //     The total aerodynamic forces acting on the vessel, in reference frame SpaceCenter.ReferenceFrame.
        //
        // Retourne :
        //     A vector pointing in the direction that the force acts, with its magnitude equal
        //     to the strength of the force in Newtons.
        public Tuple<double, double, double> aerodynamicForce;
        public Tuple<double, double, double> AerodynamicForce
        {
            get => aerodynamicForce;
            set => this.RaiseAndSetIfChanged(ref aerodynamicForce, value);
        }
        //
        // Résumé :
        //     The pitch angle between the orientation of the vessel and its velocity vector,
        //     in degrees.
        public float angleOfAttack;
        public float AngleOfAttack
        {
            get => angleOfAttack;
            set => this.RaiseAndSetIfChanged(ref angleOfAttack, value);
        }
        //
        // Résumé :
        //     The direction opposite to the normal of the vessels orbit, in the reference frame
        //     SpaceCenter.ReferenceFrame.
        //
        // Retourne :
        //     The direction as a unit vector.
        public Tuple<double, double, double> antiNormal;
        public Tuple<double, double, double> AntiNormal
        {
            get => antiNormal;
            set => this.RaiseAndSetIfChanged(ref antiNormal, value);
        }
        //
        // Résumé :
        //     The direction opposite to the radial direction of the vessels orbit, in the reference
        //     frame SpaceCenter.ReferenceFrame.
        //
        // Retourne :
        //     The direction as a unit vector.
        public Tuple<double, double, double> antiRadial;
        public Tuple<double, double, double> AntiRadial
        {
            get => antiRadial;
            set => this.RaiseAndSetIfChanged(ref antiRadial, value);
        }
        //
        // Résumé :
        //     The current density of the atmosphere around the vessel, in kg/m^3.
        public float atmosphereDensity;
        public float AtmosphereDensity
        {
            get => atmosphereDensity;
            set => this.RaiseAndSetIfChanged(ref atmosphereDensity, value);
        }
        //
        // Résumé :
        //     The altitude above the surface of the body, in meters. When over water, this
        //     is the altitude above the sea floor. Measured from the center of mass of the
        //     vessel.
        public double bedrockAltitude;
        public double BedrockAltitude
        {
            get => bedrockAltitude;
            set => this.RaiseAndSetIfChanged(ref bedrockAltitude, value);
        }
        //
        // Résumé :
        //     The position of the center of mass of the vessel, in the reference frame SpaceCenter.ReferenceFrame
        //
        // Retourne :
        //     The position as a vector.
        public Tuple<double, double, double> centerOfMass;
        public Tuple<double, double, double> CenterOfMass
        {
            get => centerOfMass;
            set => this.RaiseAndSetIfChanged(ref centerOfMass, value);
        }
        //
        // Résumé :
        //     The longitude of the vessel for the body being orbited, in degrees.
        public double longitude;
        public double Longitude
        {
            get => longitude;
            set => this.RaiseAndSetIfChanged(ref longitude, value);
        }
        //
        // Résumé :
        //     The direction that the vessel is pointing in, in the reference frame SpaceCenter.ReferenceFrame.
        //
        // Retourne :
        //     The direction as a unit vector.
        public Tuple<double, double, double> direction;
        public Tuple<double, double, double> Direction
        {
            get => direction;
            set => this.RaiseAndSetIfChanged(ref direction, value);
        }
        //
        // Résumé :
        //     The dynamic pressure acting on the vessel, in Pascals. This is a measure of the
        //     strength of the aerodynamic forces. It is equal to \frac{1}{2} . \mbox{air density}
        //     . \mbox{velocity}^2. It is commonly denoted Q.
        public float dynamicPressure;
        public float DynamicPressure
        {
            get => dynamicPressure;
            set => this.RaiseAndSetIfChanged(ref dynamicPressure, value);
        }
        //
        // Résumé :
        //     The elevation of the terrain under the vessel, in meters. This is the height
        //     of the terrain above sea level, and is negative when the vessel is over the sea.
        public double elevation;
        public double Elevation
        {
            get => elevation;
            set => this.RaiseAndSetIfChanged(ref elevation, value);
        }
        //
        // Résumé :
        //     The equivalent air speed of the vessel, in meters per second.
        public float equivalentAirSpeed;
        public float EquivalentAirSpeed
        {
            get => equivalentAirSpeed;
            set => this.RaiseAndSetIfChanged(ref equivalentAirSpeed, value);
        }
        //
        // Résumé :
        //     The current G force acting on the vessel in g.
        public float gForce;
        public float GForce
        {
            get => gForce;
            set => this.RaiseAndSetIfChanged(ref gForce, value);
        }
        //
        // Résumé :
        //     The heading of the vessel (its angle relative to north), in degrees. A value
        //     between 0° and 360°.
        public float heading;
        public float Heading
        {
            get => heading;
            set => this.RaiseAndSetIfChanged(ref heading, value);
        }
        //
        // Résumé :
        //     The horizontal speed of the vessel in meters per second, in the reference frame
        //     SpaceCenter.ReferenceFrame.
        public double horizontalSpeed;
        public double HorizontalSpeed
        {
            get => horizontalSpeed;
            set => this.RaiseAndSetIfChanged(ref horizontalSpeed, value);
        }
        //
        // Résumé :
        //     The latitude of the vessel for the body being orbited, in degrees.
        public double latitude;
        public double Latitude
        {
            get => latitude;
            set => this.RaiseAndSetIfChanged(ref latitude, value);
        }
        //
        // Résumé :
        //     The aerodynamic lift currently acting on the vessel.
        //
        // Retourne :
        //     A vector pointing in the direction that the force acts, with its magnitude equal
        //     to the strength of the force in Newtons.
        public Tuple<double, double, double> lift;
        public Tuple<double, double, double> Lift
        {
            get => lift;
            set => this.RaiseAndSetIfChanged(ref lift, value);
        }
        //
        // Résumé :
        //     The aerodynamic drag currently acting on the vessel.
        //
        // Retourne :
        //     A vector pointing in the direction of the force, with its magnitude equal to
        //     the strength of the force in Newtons.
        public Tuple<double, double, double> drag;
        public Tuple<double, double, double> Drag
        {
            get => drag;
            set => this.RaiseAndSetIfChanged(ref drag, value);
        }
        //
        // Résumé :
        //     The vertical speed of the vessel in meters per second, in the reference frame
        //     SpaceCenter.ReferenceFrame.
        public double verticalSpeed;
        public double VerticalSpeed
        {
            get => verticalSpeed;
            set => this.RaiseAndSetIfChanged(ref verticalSpeed, value);
        }


    }
}
