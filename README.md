Low-Level Battery Info (LLB) is a small Windows battery tool written in C. It reads battery details through the Windows battery interface without WMI.

Run `llb.exe` to view the full report. Press `R` to refresh, `E` to export the report, or `Q` to exit. The data refresh is manual.

## Build

With TDM-GCC or MinGW, run:

```bat
gcc llb.c -o llb.exe -lsetupapi
```

To include the EXE file details (version, description, and copyright), build with the resource file:

```bat
windres llb.rc -O coff -o llb_resource.o
gcc llb.c llb_resource.o -o llb.exe -lsetupapi
```

## Read one field

You can query a single value instead of showing the full report:

```bat
llb.exe --manufacturer
llb.exe --design-capacity
llb.exe --get voltage
llb.exe --get current-capacity
llb.exe --get condition
llb.exe --get manufacturer --slot 1
llb.exe --get manufacturer --export manufacturer.txt
```

Use `llb.exe --list-fields` to see the available fields. Most fields work as direct options too, for example `--voltage`, `--chemistry`, `--serial-number`, or `--cycle-count`. Single-field queries print once and exit, making them useful in scripts.

## Other options

```bat
llb.exe --help
llb.exe --version
llb.exe --once
llb.exe --export battery-report.txt
llb.exe --once --no-raw --export snapshot.txt
```

Raw device data is included in the full report by default. Use `--no-raw` to hide it. Add `--export <file>` to save the output to a text file.

## Output

```bash
============================================================
                 LOW-LEVEL BATTERY INFO
============================================================
Created by: Ari Sohandri Putra
GitHub: https://github.com/arisohandriputra
Version: 1.1
Copyright (C) Ari Sohandri Putra
Timestamp: 2026-10-09 15:53:02
Battery slots detected: 1
Slots displayed: 1
Slots with readable battery data: 1

============================================================
SLOT #1
------------------------------------------------------------

  BATTERY CONDITION CHECK
CRITICAL: The driver reports only 33 mV (0.033 V). This is implausibly low for a
 laptop battery pack.
This can indicate a battery electrical failure, an open protection circuit, a co
nnection/power-path problem, or invalid firmware/driver telemetry.
DATA CONFLICT: The driver reports 100.00% charge but only 33 mV terminal voltage
. A high reported charge level does not prove the battery can deliver power.
Assessment: CRITICAL - POSSIBLE BATTERY OR POWER-DELIVERY FAILURE
Note: A reported 100% charge level does not cancel this voltage warning. Capacit
y and voltage are separate driver-reported values.
If the laptop also shuts down immediately when AC power is removed, treat that b
ehavior as strong evidence that the battery or its power path cannot supply the
load. Inspect the battery, connector, and power circuitry.
The assessment is based on Windows driver data. Unsupported or inaccurate firmwa
re readings can produce false alarms; confirm suspected failures with a known-go
od battery or proper hardware testing.

  BATTERY SPECIFICATION
System battery: Yes
Relative capacity units: No
Short-term or fail-safe battery: No
Sealed battery: No
Battery technology: Secondary rechargeable battery
Battery chemistry: Lithium-ion battery
Capacity units: mWh
Designed capacity: 47520 mWh
Full charged capacity: 47563 mWh
Default alert level 1: 0 mWh
Default alert level 2: 3240 mWh
Critical bias: 0 mWh
Reported charge/discharge cycle count: 0

  CURRENT STATUS
Power state: AC power online
Current capacity: 47563 mWh
Reported charge level: 100.00%
Terminal voltage: 33 mV (0.033 V)
Charge/discharge rate: 0 mW

  IDENTIFICATION
Device name: AS10D51
Battery manufacturer: PANASONIC
Serial number: 00CD
Unique identifier: 00CDPANASONICAS10D51

  ADDITIONAL DATA
Manufacturing date: Not reported
Battery temperature: Not reported or unknown
Capacity reporting scales: 2
  Scale #1: capacity 0, granularity 346
  Scale #2: capacity 47520, granularity 346
Estimated remaining runtime: Not reported or unknown

Keys: [R] Refresh  [E] Export report  [Q] Quit
Press a key to continue. Data refresh is manual only.

============================================================
RAW DEVICE DATA
Complete raw responses, query structures, byte counts, and query results
============================================================
Enumerated battery interfaces: 1
Enumeration result: SUCCESS

------------------------------------------------------------
SLOT #1 | RAW DATA
------------------------------------------------------------
Device open result: OPENED

[IOCTL_BATTERY_QUERY_TAG]
IOCTL code: 0x00294040
IOCTL_BATTERY_QUERY_TAG result: SUCCESS
Bytes returned: 4
Timeout input (DWORD): 4 byte(s)
  0000  00 00 00 00                                      |....            |
Battery tag response: 4 byte(s)
  0000  01 00 00 00                                      |....            |

[BATTERY_INFORMATION]
IOCTL code: 0x00294044
IOCTL_BATTERY_QUERY_INFORMATION: SUCCESS
Bytes returned: 36
Response validation: COMPLETE
Query input structure: 12 byte(s)
  0000  01 00 00 00 00 00 00 00 00 00 00 00              |............    |
Response buffer: 36 byte(s)
  0000  00 00 00 80 01 00 00 00 4C 49 4F 4E A0 B9 00 00  |........LION....|
  0010  CB B9 00 00 00 00 00 00 A8 0C 00 00 00 00 00 00  |................|
  0020  00 00 00 00                                      |....            |

[IOCTL_BATTERY_QUERY_STATUS]
IOCTL code: 0x0029404C
IOCTL_BATTERY_QUERY_STATUS: SUCCESS
Bytes returned: 16
Response validation: COMPLETE
BATTERY_WAIT_STATUS input structure: 20 byte(s)
  0000  01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  |................|
  0010  00 00 00 00                                      |....            |
BATTERY_STATUS response: 16 byte(s)
  0000  01 00 00 00 CB B9 00 00 21 00 00 00 00 00 00 00  |........!.......|

[BATTERY_DEVICE_NAME]
IOCTL code: 0x00294044
IOCTL_BATTERY_QUERY_INFORMATION: SUCCESS
Bytes returned: 16
Query input structure: 12 byte(s)
  0000  01 00 00 00 04 00 00 00 00 00 00 00              |............    |
Response buffer: 16 byte(s)
  0000  41 00 53 00 31 00 30 00 44 00 35 00 31 00 00 00  |A.S.1.0.D.5.1...|

[BATTERY_MANUFACTURER_NAME]
IOCTL code: 0x00294044
IOCTL_BATTERY_QUERY_INFORMATION: SUCCESS
Bytes returned: 20
Query input structure: 12 byte(s)
  0000  01 00 00 00 06 00 00 00 00 00 00 00              |............    |
Response buffer: 20 byte(s)
  0000  50 00 41 00 4E 00 41 00 53 00 4F 00 4E 00 49 00  |P.A.N.A.S.O.N.I.|
  0010  43 00 00 00                                      |C...            |

[BATTERY_SERIAL_NUMBER]
IOCTL code: 0x00294044
IOCTL_BATTERY_QUERY_INFORMATION: SUCCESS
Bytes returned: 10
Query input structure: 12 byte(s)
  0000  01 00 00 00 08 00 00 00 00 00 00 00              |............    |
Response buffer: 10 byte(s)
  0000  30 00 30 00 43 00 44 00 00 00                    |0.0.C.D...      |

[BATTERY_UNIQUE_ID]
IOCTL code: 0x00294044
IOCTL_BATTERY_QUERY_INFORMATION: SUCCESS
Bytes returned: 42
Query input structure: 12 byte(s)
  0000  01 00 00 00 07 00 00 00 00 00 00 00              |............    |
Response buffer: 42 byte(s)
  0000  30 00 30 00 43 00 44 00 50 00 41 00 4E 00 41 00  |0.0.C.D.P.A.N.A.|
  0010  53 00 4F 00 4E 00 49 00 43 00 41 00 53 00 31 00  |S.O.N.I.C.A.S.1.|
  0020  30 00 44 00 35 00 31 00 00 00                    |0.D.5.1...      |

[BATTERY_MANUFACTURE_DATE]
IOCTL code: 0x00294044
IOCTL_BATTERY_QUERY_INFORMATION: FAILED
Win32 error: error 1 (0x00000001): Incorrect function.
Bytes returned: 0
Query input structure: 12 byte(s)
  0000  01 00 00 00 05 00 00 00 00 00 00 00              |............    |
Response buffer: 0 byte(s)
  <no raw bytes returned>

[BATTERY_TEMPERATURE]
IOCTL code: 0x00294044
IOCTL_BATTERY_QUERY_INFORMATION: FAILED
Win32 error: error 1 (0x00000001): Incorrect function.
Bytes returned: 0
Query input structure: 12 byte(s)
  0000  01 00 00 00 02 00 00 00 00 00 00 00              |............    |
Response buffer: 0 byte(s)
  <no raw bytes returned>

[BATTERY_REPORTING_SCALE]
IOCTL code: 0x00294044
IOCTL_BATTERY_QUERY_INFORMATION: SUCCESS
Bytes returned: 16
Response validation: COMPLETE
Query input structure: 12 byte(s)
  0000  01 00 00 00 01 00 00 00 00 00 00 00              |............    |
Response buffer: 16 byte(s)
  0000  5A 01 00 00 00 00 00 00 5A 01 00 00 A0 B9 00 00  |Z.......Z.......|

[BATTERY_ESTIMATED_TIME]
IOCTL code: 0x00294044
IOCTL_BATTERY_QUERY_INFORMATION: SUCCESS
Bytes returned: 4
Response validation: COMPLETE
Query input structure: 12 byte(s)
  0000  01 00 00 00 03 00 00 00 00 00 00 00              |............    |
Response buffer: 4 byte(s)
  0000  FF FF FF FF                                      |....            |

[Decoded identification bytes]
Chemistry identifier (4 bytes): 4 byte(s)
  0000  4C 49 4F 4E                                      |LION            |
Chemistry interpretation: Lithium-ion battery
Technology byte: 0x01 (1)
Capabilities raw value: 0x80000000
```
## Note

The values depend on what the battery firmware and Windows driver report. LLB points out unusual readings, but its condition checks are indicators, not a guaranteed diagnosis. A battery can report 100% and still fail to power the laptop.

Written by Ari Sohandri Putra: https://github.com/arisohandriputra
