# Mycobot Sharp

A Mycobot C# library for .NET Core / .NET Framework.

## Supported API

### Overall status

```C#
// get robot version
public int RobotVersion();

// get system version
public int SystemVersion();

// enable Atom connection
public void PowerOn();

// disable Atom connection
public void PoweOff();

// power supplied?
public bool IsPowerOn();

// release all servos
public void ReleaseAllServos();

// Atom connected?
public bool IsControllerConnected();
```

### MDI mode and operation

```c#
// get all angles
public int[] GetAngles();

// send specific joint angle
public void SendAngle(int jointNo, int angle, int speed);

// send all joint angles
public void SendAngles(int[] angles, int speed);

// get all coords
public int[] GetCoords();

// send specific coord
public void SendCoord(int coord, int value, int speed);

// send all coords
public void SendCoords(int[] coords, int speed, int mode);
```

### Servo control

```c#
// servo enabled?
public bool IsServoEnable(int servoId);

// all servos enabled?
public bool IsAllServoEnable();

// get servo data
public byte GetServoData(int servoNo, int dataId);

// power off designated servo
public void ReleaseServo(int servoId);

// power on designated servo
public void FocusServo(int servoId);
```

### Atom IO

```C#
// Set Atom LED
public void SetColor(byte r, byte g, byte b);
```
