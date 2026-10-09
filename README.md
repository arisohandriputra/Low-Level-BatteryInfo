# Low-Level Battery Info

Low-Level Battery Info (LLB) is a small Windows command-line program written in C. I made it to check what Windows can report about a laptop battery without using WMI.

LLB uses the Windows battery device interface through SetupAPI and DeviceIoControl. The usual battery information is shown first, and the raw data is kept in a separate section below it.

## What it shows

LLB looks for battery interfaces that Windows makes available. Each one is shown separately as `SLOT #1`, `SLOT #2`, and so on.

Depending on the battery and its driver, the program can show its name, manufacturer, serial number, chemistry, technology, designed capacity, full-charge capacity, current capacity, voltage, charging state, charge or discharge rate, cycle count, manufacturing date, temperature, and estimated remaining time. It also shows other battery fields when the driver provides them.

LLB does not calculate battery health. The charge level is calculated from the current capacity and full-charge capacity reported by the driver. Some fields may be unknown or unavailable on certain laptops.

## Raw data

The `RAW DEVICE DATA` section is shown below the normal information. It contains the details collected while querying each battery, including IOCTL codes, query results, byte counts, returned bytes in hexadecimal and ASCII, and Windows error codes when a request fails. Raw structures and values are shown when they are available.

If a request fails or the driver does not support it, there may be no response bytes to display. LLB reports the result instead of filling in data that was not returned.

## How it works

LLB is written in native C and uses Windows APIs. It finds battery device interfaces with SetupAPI, opens them, and requests information from the Windows battery driver with DeviceIoControl. It does not use WMI, PowerShell, .NET, or third-party runtime libraries.

This is low-level access through the Windows driver interface, not a direct read of SMBus data, embedded-controller registers, or the battery's fuel-gauge chip. What LLB can read depends on the hardware, firmware, and driver.

## Build

You need Windows and a GCC toolchain such as TDM-GCC or MinGW. Save the source as `llb.c`, open Command Prompt in that folder, and run:

```bat
gcc main.c -o llb.exe -lsetupapi
```

Then start the program:

```bat
llb.exe
```

## Using LLB

Run `llb.exe` to open the interactive view. LLB reads the battery information when it starts and then waits for your input. It does not refresh automatically. Press `R` when you want to read the data again.

Press `E` to export the displayed report, or `Q` or `Esc` to exit.

You can also run LLB with command-line options:

```bat
llb.exe --help
llb.exe --version
llb.exe --once
llb.exe --once --no-pause
llb.exe --export battery-report.txt
llb.exe --once --no-raw --export snapshot.txt
llb.exe --export "D:\Reports\Battery Report.txt"
```

The available options are:

- `-h`, `--help`: show the help text.
- `--version`: show the program version and credit.
- `-o`, `--once`: collect and show one snapshot.
- `-e`, `--export <file>`: write a report to a text file.
- `--raw`: include the raw data section. This is the default.
- `--no-raw`: hide the raw data section.
- `--no-pause`: skip the final key press after `--once`.

When you use `--export`, LLB writes a report when it starts. In interactive mode, pressing `R` refreshes the readings and updates that file with the latest snapshot. The file is overwritten each time; it is not a history log. If you do not give an export path, press `E` to create a report with a timestamp in its name in the current working directory.

Relative report paths are saved in the current working directory, which may not be the same folder as `llb.exe`. Use a full path when you want to choose the exact location.

## Multiple batteries

If Windows exposes more than one battery interface, LLB shows them as separate numbered slots. The numbers follow the order in which Windows finds the interfaces, so they are not permanent hardware IDs.

Some laptops have two physical batteries but Windows shows them as one combined device. In that case, LLB can only display the information Windows provides. The current source can display up to 32 battery interfaces per scan.

## Notes

A missing value does not automatically mean the battery is faulty. Some batteries and drivers do not provide fields such as temperature, manufacturing date, serial number, cycle count, or estimated runtime.

If no battery is detected, check that Windows can see the battery and that the ACPI and battery drivers are working. If a query fails, check the `RAW DEVICE DATA` section for its result and any Windows error code.

Reports may contain a battery serial number or unique ID. Check the file before sharing it publicly.

## Credit

Low-Level Battery Info is written by Ari Sohandri Putra.

GitHub: https://github.com/arisohandriputra
