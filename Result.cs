using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XbimFederationChecker
{
    public class Result
    {
        public Result(String name, Xbim.ModelGeometry.Scene.XbimRegion region)
        {
            ModelName = name;
            WorldCoordinateOffsetX = region.WorldCoordinateSystem.OffsetX;
            WorldCoordinateOffsetY = region.WorldCoordinateSystem.OffsetY;
            WorldCoordinateOffsetZ = region.WorldCoordinateSystem.OffsetZ;
            ModelSizeX = region.Size.X;
            ModelSizeY = region.Size.Y;
            ModelSizeZ = region.Size.Z;
            CentrePointX = region.Centre.X;
            CentrePointY = region.Centre.Y;
            CentrePointZ = region.Centre.Z;
        }

        public String ModelName { get; set; }
        public Double WorldCoordinateOffsetX { get; set; }
        public Double WorldCoordinateOffsetY { get; set; }
        public Double WorldCoordinateOffsetZ { get; set; }
        public Double ModelSizeX { get; set; }
        public Double ModelSizeY { get; set; }
        public Double ModelSizeZ { get; set; }

        public Double CentrePointX { get; set; }
        public Double CentrePointY { get; set; }
        public Double CentrePointZ { get; set; }
    }
}
