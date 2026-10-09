## Build

You need Windows and a GCC toolchain such as TDM-GCC or MinGW. Save the source as `main.c`, open Command Prompt in that folder, and run:

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
