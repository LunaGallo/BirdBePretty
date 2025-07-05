using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {

    #region Math
    [Flags]
    public enum ComparisonType {
        NotEqual = 0,
        EqualTo = 1,
        GreaterThan = 2,
        GreaterThanOrEqualTo = 3,
        LessThan = 4,
        LessThanOrEqualTo = 5
    }
    public enum RoundMethod {
        Round,
        Floor,
        Ceil
    }
    [Flags]
    public enum Axis3Flags {
        None = 0,
        X = 1,
        Y = 2,
        Z = 4
    }
    [Flags]
    public enum Axis2Flags {
        None = 0,
        X = 1,
        Y = 2
    }
    public enum AxisPlane {
        XY,
        XZ,
        YZ
    }
    #endregion

    public enum ColorSystem {
        RGB,
        HSV,
        HSL,
        HCV,
        HCL,
        RGBA,
        HSVA,
        HSLA,
        HCVA,
        HCLA
    }

    #region UnityEngine
    public enum MessageType {
        Start,
        Update,
        FixedUpdate,
        LateUpdate,
        Enable,
        Disable,
        Destroy,
        ApplicationFocus,
        ApplicationPause,
        ApplicationQuit
    }
    [Flags]
    public enum TransformProperty {
        None = 0,
        Position = 1,
        Rotation = 2,
        Scale = 4
    }
    #endregion

}

