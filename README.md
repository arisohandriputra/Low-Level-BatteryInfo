# Low-Level Battery Info (LLB)

A small Windows command-line tool written in C for checking what Windows reports about the batteries installed in a laptop or other Windows device.

I wanted something simple that could read battery information without going through WMI, so LLB talks to the Windows battery device interface with native Windows APIs. It shows the readable information first and keeps the byte-level details in a separate **RAW DEVICE DATA** section at the bottom.

## What it does

- Finds battery interfaces exposed by Windows and displays them separately as `SLOT #1`, `SLOT #2`, and so on.
- Shows battery details such as the battery technology, a readable chemistry name, capacity values, current power state, voltage, charge or discharge rate, and other fields when the driver provides them.
- Displays identification fields such as the battery name, manufacturer, serial number, and unique ID when available.
- Includes extra fields that may be supported by the battery or firmware, including manufacturing date, temperature, capacity reporting scales, and estimated remaining runtime.
- Keeps the raw data separate from the regular information so the main view is easier to read.
- Dumps returned bytes in hexadecimal and ASCII, along with IOCTL codes, query results, byte counts, and Windows error codes where available.
- Refreshes only when you ask it to. There is no automatic refresh timer.
- Exports the current report to a plain-text `.txt` file.
- Works without WMI, PowerShell, .NET, or third-party runtime libraries.

LLB does not calculate battery health. The reported charge percentage is based on the current capacity and the full-charge capacity reported by the driver; it is not a health rating.

## How it reads battery data

LLB is written in native C and uses the Windows API. It discovers battery device interfaces with SetupAPI, opens each available interface, and sends battery IOCTL requests with `DeviceIoControl()`.

This is a low-level Windows driver-interface approach, but it is **not** a direct dump of the battery's SMBus, embedded-controller registers, or internal fuel-gauge chip. Windows and the installed battery/ACPI drivers still control which values are exposed to the program.

The information available depends on the laptop, battery firmware, BIOS/UEFI, and Windows driver. A field may appear as `Unknown`, `Not reported`, or an error if the device does not provide it.

## Information shown

### Battery specification

Depending on what Windows reports, each slot can include:

- Battery type: primary non-rechargeable or secondary rechargeable.
- Chemistry name, such as lithium-ion, lithium-polymer, nickel-cadmium, nickel-metal hydride, nickel-zinc, or lead-acid. Unknown or vendor-specific chemistry is identified as such rather than guessed.
- Battery capability flags, including system battery, relative capacity units, short-term/fail-safe battery, and sealed battery.
- Designed capacity and full-charge capacity.
- Default alert levels, critical bias, and cycle count.
- The capacity unit reported by the driver. Values may be in mWh or relative units.

### Current status

- AC power, charging, discharging, and critical-state flags.
- Current battery capacity and the charge level calculated from the reported current and full-charge capacities.
- Terminal voltage in millivolts and volts.
- Charge/discharge rate. Depending on the capacity unit, the value is shown in mW or relative units per hour.

### Identification and additional fields

- Battery device name.
- Manufacturer name.
- Serial number.
- Unique ID.
- Manufacturing date.
- Battery temperature, if supported by the driver.
- Capacity reporting scales and granularity, if available.
- Estimated remaining runtime, if provided by the driver.

Not every battery reports every field. Missing information is normal and does not automatically mean the battery is faulty.

## RAW DEVICE DATA

The raw section appears below the regular battery information by default. Each detected slot has its own raw section.

It can include:

- The number of battery interfaces found and the overall enumeration result.
- The result of opening each device and querying its battery tag.
- IOCTL codes used for the battery queries.
- Query input structures sent to the driver.
- Response bytes returned by the driver, when any were returned.
- The size of each response and basic response-length checks.
- Success or failure status and the Windows error code for failed requests.
- Raw `BATTERY_INFORMATION`, `BATTERY_STATUS`, manufacturing-date, temperature, reporting-scale, estimated-time, and identification responses where available.
- The raw four-byte chemistry identifier, the technology byte, and the raw capability value.
- Hexadecimal bytes with an ASCII view alongside them.

A failed or unsupported query will not have real response data to dump. In that case, the report shows the query status and any available error details instead. This is intentional; the program does not make up bytes for information Windows did not return.

LLB keeps the display focused on battery information. Device paths and Windows device-instance details are not part of the regular battery view.

## Requirements

- Windows with a working battery driver.
- TDM-GCC/MinGW or another compatible GCC toolchain for compiling the source.
- The Windows `SetupAPI` library, which is normally available with the MinGW/TDM-GCC toolchain.

The program is a console application. When opened normally from Explorer, it is intended to keep its console open so you can read the result and use the keyboard controls. You can also run it from an existing Command Prompt.

## Build from source

Put `main.c` in a folder, open Command Prompt in that folder, and run:

```bat
gcc main.c -o llb.exe -lsetupapi
```

Then start it:

```bat
llb.exe
```

If `gcc` is not recognized, add the TDM-GCC `bin` directory to your `PATH`, or run the command from the toolchain's `bin` directory. Use a compiler build that matches the Windows architecture you want to target.

## Running the program

### Start the interactive view

```bat
llb.exe
```

LLB reads a snapshot when it starts, then waits for a key. It does not keep polling the battery in the background.

### Take one snapshot

```bat
llb.exe --once
```

The program collects and prints the data once. In an interactive console, it waits for a key before closing. To skip that final wait, add `--no-pause`:

```bat
llb.exe --once --no-pause
```

### Export a report

```bat
llb.exe --export battery-report.txt
```

You can also use the shorter `-e` option:

```bat
llb.exe -e battery-report.txt
```

This writes the first report when the program starts. In interactive mode, pressing `R` refreshes the readings and updates the specified report file. The file is overwritten with the latest snapshot; it is not an accumulating history log.

For a path containing spaces, put the path in quotes:

```bat
llb.exe --export "D:\Reports\Battery Report.txt"
```

If you do not specify an export path, press `E` while the program is running to create a report with a timestamp in its filename, for example `llb-report-20261009-104500.txt`. The report is saved to the current working directory.

### Hide the raw section

Raw output is included by default. To hide it:

```bat
llb.exe --no-raw
```

To explicitly enable it, you can use:

```bat
llb.exe --raw
```

`--raw` is optional because it is already the default.

## Command-line options

| Option | What it does |
| --- | --- |
| `-h`, `--help` | Shows the help text and examples. |
| `--version` | Shows the program version and creator details. |
| `-o`, `--once` | Collects and displays one snapshot. |
| `-e`, `--export <file>` | Writes a report to the specified text file. |
| `--raw` | Includes the raw data section. This is the default. |
| `--no-raw` | Hides the raw data section in the display and report. |
| `--no-pause` | Skips the final key press after `--once`. |

Options can be combined. A few useful examples:

```bat
llb.exe --help
llb.exe --version
llb.exe --once
llb.exe --once --no-pause
llb.exe --export battery-report.txt
llb.exe --once --no-raw --export snapshot.txt
llb.exe --export "D:\Reports\Battery Report.txt"
```

## Keyboard controls

| Key | Action |
| --- | --- |
| `R` | Read the battery information again and refresh the display. |
| `E` | Export the currently displayed snapshot. |
| `Q` or `Esc` | Exit the program. |

Refreshing is manual by design. If you leave the program sitting on the screen, the displayed values will stay as they were until you press `R`.

## Multiple batteries and slots

When Windows exposes more than one battery interface, LLB gives each detected interface its own numbered section, such as `SLOT #1` and `SLOT #2`.

These numbers are the order in which interfaces are enumerated; they are not guaranteed to be permanent hardware identifiers. Some laptops have two physical batteries but expose them as one combined device through firmware or the Windows driver. In that case, LLB cannot split the combined reading into two physical batteries.

The current source keeps up to 32 interface records per scan. If more interfaces are enumerated, the report notes that the display limit was reached.

## Understanding the values

- **Designed capacity** is the design capacity reported by the battery driver.
- **Full-charge capacity** is the capacity reported as available when the battery is fully charged.
- **Current capacity** is the current capacity reported by the driver.
- **Reported charge level** is calculated from current capacity divided by full-charge capacity. It is not the same as battery health.
- **Terminal voltage** is shown in mV and V when the driver returns a known value.
- **Unknown** or **Not reported** means the value was not available in a usable form. It should not be treated as zero.
- **Cycle count** is shown exactly as reported by the driver. Some firmware reports zero or does not maintain a useful count.

LLB does not make up missing information or infer a value when the driver reports it as unknown.

## Troubleshooting

### No battery slots detected

Make sure Windows can see the battery and that the appropriate ACPI/battery driver is working. A desktop PC or a system without an exposed battery interface may correctly report no slots.

### A field says `Not reported` or `Unknown`

That field may not be supported by the battery firmware or driver. Temperature, manufacturing date, serial number, cycle count, and estimated runtime are not available on every device.

### A query fails

Look in **RAW DEVICE DATA** for the query result and Windows error code. Those details help distinguish an unsupported request from a device-access or driver problem.

### The program closes too quickly

Run `llb.exe` from an existing Command Prompt so the output remains visible. Avoid `--no-pause` if you want a one-shot run to wait for a key before closing.

### The exported report is not where you expected

Relative filenames are saved in the current working directory, which may not be the same folder as `llb.exe`. Use a full path to choose the destination explicitly.

## A note before sharing reports

Reports can contain a battery serial number, manufacturer details, and a unique ID if the device exposes them. Check the report before posting it publicly.

## Project credit

**Low-Level Battery Info** is written by **Ari Sohandri Putra**.

GitHub: https://github.com/arisohandriputra
