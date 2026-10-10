Low-Level Battery Info (LLB) is a small Windows battery tool written in C. It reads battery details through the Windows battery interface.

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

## Note

The values depend on what the battery firmware and Windows driver report. LLB points out unusual readings, but its condition checks are indicators, not a guaranteed diagnosis. A battery can report 100% and still fail to power the laptop.

Written by Ari Sohandri Putra: https://github.com/arisohandriputra
