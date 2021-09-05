namespace Mycobot
{
    public class Code
    {
        // Basic
        public const byte Header = 0xfe;
        public const byte Footer = 0xfa;

        // System Info
        public const byte GetRobotVersion = 0x01;
        public const byte GetSystemVersion = 0x02;

        public const byte GetRobotId = 0x03;
        public const byte SetRobotId = 0x04;

        // Overall Status
        public const byte PowerOn = 0x10;
        public const byte PowerOff = 0x11;
        public const byte IsPoweredOn = 0x12;
        public const byte ReleaseAllServos = 0x13;
        public const byte IsControllerConnected = 0x14;
        public const byte ReadNextError = 0x15;

        // MDI mode and operation
        public const byte GetAngles = 0x20;
        public const byte WriteAngle = 0x21;
        public const byte WriteAngles = 0x22;
        public const byte GetCoords = 0x23;
        public const byte WriteCoord = 0x24;

        public const byte WriteCoords = 0x25;
        public const byte ProgramPause = 0x26;

        public const byte IsProgramPaused = 0x27;
        public const byte ProgramResume = 0x28;
        public const byte TaskStop = 0x29;
        public const byte IsInPosition = 0x2A;
        public const byte CheckRunning = 0x2B;

        // JOG mode and operation
        public const byte JogAngle = 0x30;
        public const byte JogAbsolute = 0x31;
        public const byte JogCoord = 0x32;
        public const byte SendJogIncrement = 0x33;
        public const byte JogStop = 0x34;

        public const byte SetEncoder = 0x3A;
        public const byte GetEncoder = 0x3B;
        public const byte SetEncoders = 0x3C;

        // Running Status and Settings
        public const byte GetSpeed = 0x40;
        public const byte SetSpeed = 0x41;

        public const byte GetFeedOverride = 0x42;
        public const byte SendOverride = 0x43;
        public const byte GetAcceleration = 0x44;
        public const byte SetAcceleration = 0x45;

        public const byte GetJointMin = 0x4A;
        public const byte GetJointMax = 0x4B;
        public const byte SetJointMin = 0x4C;
        public const byte SetJointMax = 0x4D;

        // Servo Control
        public const byte IsServoEnabled = 0x50;
        public const byte IsAllServoEnabled = 0x51;
        public const byte SetServoData = 0x52;
        public const byte GetServoData = 0x53;
        public const byte SetServoCalibration = 0x54;
        public const byte VoidJointBrake = 0x55;
        public const byte ReleaseServo = 0x56;
        public const byte FocusServo = 0x57;

        //// ATOM IO
        public const byte SetPinMode = 0x60;
        public const byte SetDigitalOutput = 0x61;
        public const byte GetDigitalInput = 0x62;
        public const byte SetPwmMode = 0x63;
        public const byte SetPwmOutput = 0x64;
        public const byte GetGripperValue = 0x65;
        public const byte SetGripperState = 0x66;
        public const byte SetGripperValue = 0x67;

        public const byte SetGripperIni = 0x68;
        public const byte IsGripperMoving = 0x69;
        public const byte SetLed = 0x6A;

        //// Base basic and IO control
        public const byte RoboticMessage = 0x80;
        public const byte SetToolRef = 0x81;
        public const byte GetToolRef = 0x82;
        public const byte SetWorldRef = 0x83;
        public const byte GetWorldRef = 0x84;
        public const byte SetRefFrame = 0x85;
        public const byte GetRefFrame = 0x86;
        public const byte SetMovementType = 0x87;
        public const byte GetMovementType = 0x88;
        public const byte SetEndType = 0x89;
        public const byte GetEndType = 0x8A;
        public const byte MovecCoordsDefault = 0x8B;
        public const byte MovecCoords = 0x8C;

        public const byte SetBasicDigitalOutput = 0xa0;
    }
}
