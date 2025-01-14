# Mycobot Sharp

A Mycobot C# library for .NET Core / .NET Framework.

## Prerequisite

### Tool

[Mystudio v3.1.2](https://github.com/elephantrobotics/myStudio/releases/tag/v3.1.2)

### Firmware

|Device|Firmware|
|---|---|
|BASIC|minirobot v0.4|
|ATOM|AtomMain v4.0|

![BASIC](images/basic.png)
![ATOM](images/atom.png)

### IDE

[Visual Studio 2019](https://visualstudio.microsoft.com/downloads/)

## Supported API

### Overall status

```C#
// get robot version
public int GetRobotVersion();

// get system version
public int GetSystemVersion();

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
// Set Pin mode
public void SetPinMode(int pinNo, bool pinState);

// Set Digital Output
public void SetDigitalOuput(int pinNo, bool pinState);

// Set Digital Input
 public bool GetDigitalInput(int pinNo);

// Set PWM Output 
public void SetPwmOutput(int pinNo, short freq, byte duty);

// Set Atom LED
public void SetColor(byte r, byte g, byte b);
```

### Basic IO

```C#
// Set Digital Output for BASIC (Suction pump control: G2, G5)
public void SetBasicDigitalOutput(int pinNo, bool pinState)
```
