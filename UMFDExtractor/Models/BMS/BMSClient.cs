using F4SharedMem;
using F4SharedMem.Headers;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace UMFDExtractor.Models.BMS
{
    public class BMSClient : ClientBase, IEHSIProvider
    {
        readonly Reader reader = new Reader();
        protected override void InternalDispose()
        {
        }

        /*protected override void Start()
        {
            if (reader.IsFalconRunning)
            {

            }
            throw new NotImplementedException();
        }*/

        bool run = false;
        protected override void Start()
        {
            if (!run)
            {
                run = true;
                Observable.Start(() =>
                {
                    try
                    {
                        Status = "Starting";

                        EHSI.IsEnabled = false;
                        EHSI.WorkingWaypoint = true;

                        Running = true;

                        Status = "Running";
                        while (run)
                        {
                            try
                            {
                                ExecuteCycle();
                            }
                            catch (Exception ex)
                            {
                                Status = ex.Message;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Stop();
                        Status = ex.Message;
                    }
                    run = false;
                    Status = "Stopped";
                }, RxApp.TaskpoolScheduler);
            }
        }


        protected override void Stop()
        {
            try
            {
                Status = "Stopping";







                run = false;
                loaded = false;
                Running = false;
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }
        }


        bool loaded = false;

        [Category("Values")]
        [ExpandableObject]
        public EHSIModel EHSI { get; } = new EHSIModel();

        void Load(FlightData data)
        {
            loaded = true;
            //Load waypoints

            List<Waypoint> waypoints = new List<Waypoint>();

            foreach (var val in data.StringData.data.Where(x => x.strId == 0x21))
            {
                //NP:1,WP,7.7571525E5,1.307072E6,-1E1,-1E1;
                string[] values = val.value.Split(',');
                if (values.Length > 3)
                {
                    waypoints.Add(new Waypoint(WaypointType.Waypoint,values[0].Substring(3), 
                        double.Parse(values[2], CultureInfo.InvariantCulture), 
                        double.Parse(values[3], CultureInfo.InvariantCulture), 
                        double.Parse(values[4], CultureInfo.InvariantCulture)
                        ));
                }
            }

            Observable.Start(() =>
            {
                Waypoints.Clear();
                FilteredWaypoints.Clear();
                if (waypoints.Count > 0)
                {
                    Waypoints.AddRange(waypoints);
                    FilteredWaypoints.AddRange(waypoints);
                }
            }, RxApp.MainThreadScheduler);
        }
        void ExecuteCycle()
        {
            if (reader.IsFalconRunning && (runehsi))
            {
                FlightData data = reader.GetCurrentData();

                if ((((HsiBits)data.hsiBits) & HsiBits.Flying) == HsiBits.Flying)
                {
                    if (!loaded)
                        Load(data);

                    if (runehsi)
                    {
                        EHSI.FlightHeading = data.currentHeading;

                        EHSI.Bearing = data.bearingToBeacon;
                        EHSI.Course = data.desiredCourse;
                        EHSI.Heading = data.desiredHeading;

                        EHSI.Distance = data.distanceToBeacon;
                        EHSI.CourseDeviation = data.courseDeviation;

                        //EHSI.Latitude = data.latitude;
                        //EHSI.Longitude = data.longitude;
                        //EHSI.MeanAltitude

                        EHSI.CalculateCourseDeviationLog();

                        //if (data.navMode == )
                    }
                    Thread.Sleep(50);
                }
                else
                {
                    Thread.Sleep(500);
                }
            }
            else
            {
                Thread.Sleep(500);
            }
        }

        bool runehsi = false;

        public void StartEHSI()
        {
            runehsi = true;
        }

        public void StopEHSI()
        {
            runehsi = false;
            loaded = false;
        }
    }
}
