#define _WIN32_WINNT 0x0501

#include <windows.h>
#include <setupapi.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <conio.h>
#include <io.h>
#include <signal.h>

#define LLB_PROGRAM_NAME "Low-Level Battery Info"
#define LLB_VERSION "1.1"
#define LLB_AUTHOR "Ari Sohandri Putra"
#define LLB_GITHUB "https://github.com/arisohandriputra"
#define LLB_MAX_SLOTS 32
#define LLB_TEXT_SIZE 2048
#define LLB_WIDE_STRING_COUNT 256
#define LLB_UNKNOWN_VALUE 0xFFFFFFFFUL
#define LLB_UNKNOWN_RATE 0x80000000UL
#define LLB_DEVICE_TYPE 0x29
#define LLB_METHOD_BUFFERED 0
#define LLB_FILE_READ_ACCESS 0x0001
#define LLB_CAPACITY_RELATIVE 0x40000000UL
#define LLB_SYSTEM_BATTERY 0x80000000UL
#define LLB_IS_SHORT_TERM 0x20000000UL
#define LLB_SEALED 0x10000000UL
#define LLB_POWER_ON_LINE 0x00000001UL
#define LLB_DISCHARGING 0x00000002UL
#define LLB_CHARGING 0x00000004UL
#define LLB_CRITICAL 0x00000008UL

#ifndef CTL_CODE
#define CTL_CODE(DeviceType, Function, Method, Access) \
    (((DWORD)(DeviceType) << 16) | ((DWORD)(Access) << 14) | \
     ((DWORD)(Function) << 2) | (DWORD)(Method))
#endif

#define LLB_IOCTL_QUERY_TAG \
    CTL_CODE(LLB_DEVICE_TYPE, 0x10, LLB_METHOD_BUFFERED, LLB_FILE_READ_ACCESS)
#define LLB_IOCTL_QUERY_INFORMATION \
    CTL_CODE(LLB_DEVICE_TYPE, 0x11, LLB_METHOD_BUFFERED, LLB_FILE_READ_ACCESS)
#define LLB_IOCTL_QUERY_STATUS \
    CTL_CODE(LLB_DEVICE_TYPE, 0x13, LLB_METHOD_BUFFERED, LLB_FILE_READ_ACCESS)

typedef enum _LLB_QUERY_INFORMATION_LEVEL {
    LLB_BatteryInformation = 0,
    LLB_BatteryGranularityInformation = 1,
    LLB_BatteryTemperature = 2,
    LLB_BatteryEstimatedTime = 3,
    LLB_BatteryDeviceName = 4,
    LLB_BatteryManufactureDate = 5,
    LLB_BatteryManufactureName = 6,
    LLB_BatteryUniqueID = 7,
    LLB_BatterySerialNumber = 8
} LLB_QUERY_INFORMATION_LEVEL;

typedef struct _LLB_QUERY_INFORMATION {
    DWORD BatteryTag;
    LLB_QUERY_INFORMATION_LEVEL InformationLevel;
    LONG AtRate;
} LLB_QUERY_INFORMATION;

typedef struct _LLB_BATTERY_INFORMATION {
    DWORD Capabilities;
    BYTE Technology;
    BYTE Reserved[3];
    BYTE Chemistry[4];
    DWORD DesignedCapacity;
    DWORD FullChargedCapacity;
    DWORD DefaultAlert1;
    DWORD DefaultAlert2;
    DWORD CriticalBias;
    DWORD CycleCount;
} LLB_BATTERY_INFORMATION;

typedef struct _LLB_WAIT_STATUS {
    DWORD BatteryTag;
    DWORD Timeout;
    DWORD PowerState;
    DWORD LowCapacity;
    DWORD HighCapacity;
} LLB_WAIT_STATUS;

typedef struct _LLB_BATTERY_STATUS {
    DWORD PowerState;
    DWORD Capacity;
    DWORD Voltage;
    LONG Rate;
} LLB_BATTERY_STATUS;

typedef struct _LLB_MANUFACTURE_DATE {
    BYTE Day;
    BYTE Month;
    WORD Year;
} LLB_MANUFACTURE_DATE;

typedef struct _LLB_WIDE_FIELD {
    WCHAR Value[LLB_WIDE_STRING_COUNT];
    DWORD BytesReturned;
    DWORD ErrorCode;
    BOOL Available;
} LLB_WIDE_FIELD;

typedef struct _LLB_BATTERY_RECORD {
    DWORD SlotNumber;
    char DevicePath[LLB_TEXT_SIZE];
    DWORD OpenError;
    DWORD TagError;
    DWORD TagBytesReturned;
    DWORD TagTimeout;
    DWORD BatteryTag;
    BOOL DevicePathAvailable;
    BOOL DeviceOpened;
    BOOL TagAvailable;
    LLB_BATTERY_INFORMATION Information;
    DWORD InformationBytes;
    DWORD InformationError;
    BOOL InformationAvailable;
    LLB_BATTERY_STATUS Status;
    DWORD StatusBytes;
    DWORD StatusError;
    BOOL StatusAvailable;
    LLB_WIDE_FIELD DeviceName;
    LLB_WIDE_FIELD Manufacturer;
    LLB_WIDE_FIELD SerialNumber;
    LLB_WIDE_FIELD UniqueId;
    LLB_MANUFACTURE_DATE ManufactureDate;
    DWORD ManufactureDateBytes;
    DWORD ManufactureDateError;
    BOOL ManufactureDateAvailable;
    DWORD Temperature;
    DWORD TemperatureBytes;
    DWORD TemperatureError;
    BOOL TemperatureAvailable;
    BATTERY_REPORTING_SCALE Scales[4];
    DWORD ScalesBytes;
    DWORD ScalesError;
    BOOL ScalesAvailable;
    DWORD EstimatedSeconds;
    DWORD EstimatedBytes;
    DWORD EstimatedError;
    BOOL EstimatedAvailable;
} LLB_BATTERY_RECORD;

typedef struct _LLB_OPTIONS {
    BOOL Once;
    BOOL IncludeRaw;
    BOOL PauseAtEnd;
    char ExportPath[LLB_TEXT_SIZE];
} LLB_OPTIONS;

static const GUID LLB_BATTERY_INTERFACE_GUID = {
    0x72631e54, 0x78a4, 0x11d0,
    { 0xbc, 0xf7, 0x00, 0xaa, 0x00, 0xb7, 0xb3, 0x2a }
};

static volatile sig_atomic_t LLB_StopRequested = 0;

static void HandleSignal(int signalNumber)
{
    (void)signalNumber;
    LLB_StopRequested = 1;
}

static DWORD LimitBytes(DWORD value, DWORD maximum)
{
    return value > maximum ? maximum : value;
}

static void CopyText(char *destination, size_t destinationSize, const char *source)
{
    if (destinationSize == 0) {
        return;
    }
    destination[0] = '\0';
    if (source == NULL) {
        return;
    }
    strncpy(destination, source, destinationSize - 1);
    destination[destinationSize - 1] = '\0';
}

static void PrintWindowsError(FILE *out, const char *operation, DWORD errorCode)
{
    char message[512];
    DWORD length;

    memset(message, 0, sizeof(message));
    length = FormatMessageA(
        FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
        NULL,
        errorCode,
        0,
        message,
        (DWORD)(sizeof(message) - 1),
        NULL
    );

    while (length > 0 && (message[length - 1] == '\r' || message[length - 1] == '\n')) {
        message[length - 1] = '\0';
        length--;
    }

    if (length != 0) {
        fprintf(out, "%s: error %lu (0x%08lX): %s\n",
                operation, (unsigned long)errorCode,
                (unsigned long)errorCode, message);
    } else {
        fprintf(out, "%s: error %lu (0x%08lX)\n",
                operation, (unsigned long)errorCode,
                (unsigned long)errorCode);
    }
}

static void DumpBytes(FILE *out, const char *label, const void *data, DWORD size)
{
    const BYTE *bytes;
    DWORD offset;
    DWORD i;
    DWORD current;
    unsigned char ch;

    fprintf(out, "%s: %lu byte(s)\n", label, (unsigned long)size);
    if (size == 0 || data == NULL) {
        fprintf(out, "  <no raw bytes returned>\n");
        return;
    }

    bytes = (const BYTE *)data;
    offset = 0;
    while (offset < size) {
        current = size - offset;
        if (current > 16) {
            current = 16;
        }
        fprintf(out, "  %04lX  ", (unsigned long)offset);
        for (i = 0; i < 16; i++) {
            if (i < current) {
                fprintf(out, "%02X ", (unsigned int)bytes[offset + i]);
            } else {
                fprintf(out, "   ");
            }
        }
        fprintf(out, " |");
        for (i = 0; i < current; i++) {
            ch = bytes[offset + i];
            fputc((ch >= 32 && ch <= 126) ? ch : '.', out);
        }
        for (; i < 16; i++) {
            fputc(' ', out);
        }
        fprintf(out, "|\n");
        offset += current;
    }
}

static const char *GetChemistryName(const BYTE chemistry[4])
{
    if (memcmp(chemistry, "PbAc", 4) == 0) {
        return "Lead-acid battery";
    }
    if (memcmp(chemistry, "LION", 4) == 0 || memcmp(chemistry, "Li-I", 4) == 0) {
        return "Lithium-ion battery";
    }
    if (memcmp(chemistry, "LIPO", 4) == 0 || memcmp(chemistry, "LiPo", 4) == 0) {
        return "Lithium-polymer battery";
    }
    if (memcmp(chemistry, "NiCd", 4) == 0) {
        return "Nickel-cadmium battery";
    }
    if (memcmp(chemistry, "NiMH", 4) == 0) {
        return "Nickel-metal hydride battery";
    }
    if (memcmp(chemistry, "NiZn", 4) == 0) {
        return "Nickel-zinc battery";
    }
    if (memcmp(chemistry, "RAM ", 4) == 0 || memcmp(chemistry, "RAM\0", 4) == 0) {
        return "Rechargeable alkaline-manganese battery";
    }
    return "Unknown or vendor-specific battery chemistry";
}

static void PrintDWORD(FILE *out, const char *label, DWORD value)
{
    if (value == LLB_UNKNOWN_VALUE) {
        fprintf(out, "%s: Unknown\n", label);
    } else {
        fprintf(out, "%s: %lu\n", label, (unsigned long)value);
    }
}

static void PrintCapacity(FILE *out, const char *label, DWORD value, BOOL relativeUnits)
{
    const char *unit;

    unit = relativeUnits ? "relative units" : "mWh";
    if (value == LLB_UNKNOWN_VALUE) {
        fprintf(out, "%s: Unknown\n", label);
    } else {
        fprintf(out, "%s: %lu %s\n", label, (unsigned long)value, unit);
    }
}

static void PrintCapabilityFlags(FILE *out, DWORD flags)
{
    DWORD known;

    known = LLB_SYSTEM_BATTERY | LLB_CAPACITY_RELATIVE | LLB_IS_SHORT_TERM | LLB_SEALED;
    fprintf(out, "System battery: %s\n", (flags & LLB_SYSTEM_BATTERY) ? "Yes" : "No");
    fprintf(out, "Relative capacity units: %s\n", (flags & LLB_CAPACITY_RELATIVE) ? "Yes" : "No");
    fprintf(out, "Short-term or fail-safe battery: %s\n", (flags & LLB_IS_SHORT_TERM) ? "Yes" : "No");
    fprintf(out, "Sealed battery: %s\n", (flags & LLB_SEALED) ? "Yes" : "No");
    if ((flags & ~known) != 0) {
        fprintf(out, "Additional capability flags: Present\n");
    }
}

static BOOL QueryBatteryInformation(
    HANDLE battery,
    DWORD batteryTag,
    LLB_QUERY_INFORMATION_LEVEL level,
    LONG atRate,
    void *outputBuffer,
    DWORD outputSize,
    DWORD *bytesReturned,
    DWORD *errorCode)
{
    LLB_QUERY_INFORMATION query;
    DWORD localBytes;
    BOOL success;

    localBytes = 0;
    if (bytesReturned == NULL) {
        bytesReturned = &localBytes;
    }
    *bytesReturned = 0;
    if (errorCode != NULL) {
        *errorCode = ERROR_SUCCESS;
    }

    memset(&query, 0, sizeof(query));
    query.BatteryTag = batteryTag;
    query.InformationLevel = level;
    query.AtRate = atRate;

    success = DeviceIoControl(
        battery,
        LLB_IOCTL_QUERY_INFORMATION,
        &query,
        sizeof(query),
        outputBuffer,
        outputSize,
        bytesReturned,
        NULL
    );

    if (!success && errorCode != NULL) {
        *errorCode = GetLastError();
    }
    return success;
}



static void ReadWideField(
    HANDLE battery,
    DWORD batteryTag,
    LLB_QUERY_INFORMATION_LEVEL level,
    LLB_WIDE_FIELD *field)
{
    DWORD returned;
    DWORD errorCode;
    BOOL success;

    memset(field, 0, sizeof(*field));
    returned = 0;
    errorCode = ERROR_SUCCESS;
    success = QueryBatteryInformation(
        battery, batteryTag, level, 0,
        field->Value, sizeof(field->Value), &returned, &errorCode);

    field->BytesReturned = LimitBytes(returned, sizeof(field->Value));
    field->ErrorCode = success ? ERROR_SUCCESS : errorCode;
    field->Available = success;
    field->Value[(sizeof(field->Value) / sizeof(field->Value[0])) - 1] = L'\0';
}

static void ReadBatteryDetails(HANDLE battery, LLB_BATTERY_RECORD *record)
{
    LLB_WAIT_STATUS waitStatus;
    DWORD returned;
    DWORD errorCode;
    BOOL success;

    memset(&record->Information, 0, sizeof(record->Information));
    returned = 0;
    success = QueryBatteryInformation(
        battery, record->BatteryTag, LLB_BatteryInformation, 0,
        &record->Information, sizeof(record->Information),
        &returned, &errorCode);
    record->InformationBytes = LimitBytes(returned, sizeof(record->Information));
    record->InformationError = success ? ERROR_SUCCESS : errorCode;
    record->InformationAvailable = success && returned >= sizeof(record->Information);

    memset(&waitStatus, 0, sizeof(waitStatus));
    waitStatus.BatteryTag = record->BatteryTag;
    waitStatus.Timeout = 0;
    waitStatus.PowerState = 0;
    waitStatus.LowCapacity = 0;
    waitStatus.HighCapacity = 0;
    memset(&record->Status, 0, sizeof(record->Status));
    returned = 0;
    success = DeviceIoControl(
        battery,
        LLB_IOCTL_QUERY_STATUS,
        &waitStatus,
        sizeof(waitStatus),
        &record->Status,
        sizeof(record->Status),
        &returned,
        NULL);
    errorCode = success ? ERROR_SUCCESS : GetLastError();
    record->StatusBytes = LimitBytes(returned, sizeof(record->Status));
    record->StatusError = errorCode;
    record->StatusAvailable = success && returned >= sizeof(record->Status);

    ReadWideField(battery, record->BatteryTag, LLB_BatteryDeviceName, &record->DeviceName);
    ReadWideField(battery, record->BatteryTag, LLB_BatteryManufactureName, &record->Manufacturer);
    ReadWideField(battery, record->BatteryTag, LLB_BatterySerialNumber, &record->SerialNumber);
    ReadWideField(battery, record->BatteryTag, LLB_BatteryUniqueID, &record->UniqueId);

    memset(&record->ManufactureDate, 0, sizeof(record->ManufactureDate));
    returned = 0;
    success = QueryBatteryInformation(
        battery, record->BatteryTag, LLB_BatteryManufactureDate, 0,
        &record->ManufactureDate, sizeof(record->ManufactureDate),
        &returned, &errorCode);
    record->ManufactureDateBytes = LimitBytes(returned, sizeof(record->ManufactureDate));
    record->ManufactureDateError = success ? ERROR_SUCCESS : errorCode;
    record->ManufactureDateAvailable = success && returned >= sizeof(record->ManufactureDate);

    record->Temperature = LLB_UNKNOWN_VALUE;
    returned = 0;
    success = QueryBatteryInformation(
        battery, record->BatteryTag, LLB_BatteryTemperature, 0,
        &record->Temperature, sizeof(record->Temperature), &returned, &errorCode);
    record->TemperatureBytes = LimitBytes(returned, sizeof(record->Temperature));
    record->TemperatureError = success ? ERROR_SUCCESS : errorCode;
    record->TemperatureAvailable = success && returned >= sizeof(record->Temperature);

    memset(record->Scales, 0, sizeof(record->Scales));
    returned = 0;
    success = QueryBatteryInformation(
        battery, record->BatteryTag, LLB_BatteryGranularityInformation, 0,
        record->Scales, sizeof(record->Scales), &returned, &errorCode);
    record->ScalesBytes = LimitBytes(returned, sizeof(record->Scales));
    record->ScalesError = success ? ERROR_SUCCESS : errorCode;
    record->ScalesAvailable = success && returned >= sizeof(BATTERY_REPORTING_SCALE);

    record->EstimatedSeconds = LLB_UNKNOWN_VALUE;
    returned = 0;
    success = QueryBatteryInformation(
        battery, record->BatteryTag, LLB_BatteryEstimatedTime, 0,
        &record->EstimatedSeconds, sizeof(record->EstimatedSeconds),
        &returned, &errorCode);
    record->EstimatedBytes = LimitBytes(returned, sizeof(record->EstimatedSeconds));
    record->EstimatedError = success ? ERROR_SUCCESS : errorCode;
    record->EstimatedAvailable = success && returned >= sizeof(record->EstimatedSeconds);
}

static void InitializeRecord(LLB_BATTERY_RECORD *record, DWORD slotNumber)
{
    memset(record, 0, sizeof(*record));
    record->SlotNumber = slotNumber;
    record->BatteryTag = LLB_UNKNOWN_VALUE;
    record->TagBytesReturned = 0;
    record->TagTimeout = 0;
    record->Temperature = LLB_UNKNOWN_VALUE;
    record->EstimatedSeconds = LLB_UNKNOWN_VALUE;
}

static BOOL CollectBatteries(
    LLB_BATTERY_RECORD records[],
    DWORD *recordCount,
    DWORD *interfaceCount,
    BOOL *truncated,
    DWORD *collectionError)
{
    HDEVINFO deviceInfo;
    SP_DEVICE_INTERFACE_DATA interfaceData;
    SP_DEVICE_INTERFACE_DETAIL_DATA_A *detailData;
    LLB_BATTERY_RECORD *record;
    HANDLE battery;
    DWORD requiredSize;
    DWORD errorCode;
    DWORD index;
    DWORD timeout;
    DWORD batteryTag;
    DWORD bytesReturned;
    DWORD slotNumber;

    *recordCount = 0;
    *interfaceCount = 0;
    *truncated = FALSE;
    *collectionError = ERROR_SUCCESS;

    deviceInfo = SetupDiGetClassDevsA(
        &LLB_BATTERY_INTERFACE_GUID,
        NULL,
        NULL,
        DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);

    if (deviceInfo == INVALID_HANDLE_VALUE) {
        *collectionError = GetLastError();
        return FALSE;
    }

    index = 0;
    for (;;) {
        memset(&interfaceData, 0, sizeof(interfaceData));
        interfaceData.cbSize = sizeof(interfaceData);

        if (!SetupDiEnumDeviceInterfaces(
                deviceInfo, NULL, &LLB_BATTERY_INTERFACE_GUID,
                index, &interfaceData)) {
            errorCode = GetLastError();
            if (errorCode != ERROR_NO_MORE_ITEMS) {
                *collectionError = errorCode;
            }
            break;
        }

        index++;
        (*interfaceCount)++;
        slotNumber = *interfaceCount;

        if (*recordCount >= LLB_MAX_SLOTS) {
            *truncated = TRUE;
            continue;
        }

        record = &records[*recordCount];
        InitializeRecord(record, slotNumber);
        (*recordCount)++;

        requiredSize = 0;

        SetupDiGetDeviceInterfaceDetailA(
            deviceInfo, &interfaceData, NULL, 0, &requiredSize, NULL);
        errorCode = GetLastError();

        if (requiredSize == 0) {
            continue;
        }

        detailData = (SP_DEVICE_INTERFACE_DETAIL_DATA_A *)malloc(requiredSize);
        if (detailData == NULL) {
            continue;
        }

        memset(detailData, 0, requiredSize);
        detailData->cbSize = sizeof(SP_DEVICE_INTERFACE_DETAIL_DATA_A);

        if (!SetupDiGetDeviceInterfaceDetailA(
                deviceInfo, &interfaceData, detailData, requiredSize,
                NULL, NULL)) {
            free(detailData);
            continue;
        }

        CopyText(record->DevicePath, sizeof(record->DevicePath), detailData->DevicePath);
        record->DevicePathAvailable = TRUE;
        battery = CreateFileA(
            record->DevicePath,
            GENERIC_READ,
            FILE_SHARE_READ | FILE_SHARE_WRITE,
            NULL,
            OPEN_EXISTING,
            FILE_ATTRIBUTE_NORMAL,
            NULL);

        free(detailData);

        if (battery == INVALID_HANDLE_VALUE) {
            record->OpenError = GetLastError();
            continue;
        }

        record->DeviceOpened = TRUE;
        timeout = 0;
        record->TagTimeout = timeout;
        batteryTag = 0;
        bytesReturned = 0;

        if (!DeviceIoControl(
                battery,
                LLB_IOCTL_QUERY_TAG,
                &timeout,
                sizeof(timeout),
                &batteryTag,
                sizeof(batteryTag),
                &bytesReturned,
                NULL)) {
            record->BatteryTag = batteryTag;
            record->TagBytesReturned = bytesReturned;
            record->TagError = GetLastError();
            CloseHandle(battery);
            continue;
        }

        record->BatteryTag = batteryTag;
        record->TagBytesReturned = bytesReturned;
        if (bytesReturned < sizeof(batteryTag) || batteryTag == 0 ||
            batteryTag == LLB_UNKNOWN_VALUE) {
            record->TagError = ERROR_INVALID_DATA;
            CloseHandle(battery);
            continue;
        }

        record->BatteryTag = batteryTag;
        record->TagAvailable = TRUE;
        ReadBatteryDetails(battery, record);
        CloseHandle(battery);
        }

    SetupDiDestroyDeviceInfoList(deviceInfo);
    return *collectionError == ERROR_SUCCESS;
}

static void PrintWideField(FILE *out, const char *label, const LLB_WIDE_FIELD *field)
{
    char text[LLB_TEXT_SIZE];
    int converted;

    if (!field->Available) {
        fprintf(out, "%s: Not reported by the battery or driver", label);
        if (field->ErrorCode != ERROR_SUCCESS) {
            fprintf(out, " (error %lu)", (unsigned long)field->ErrorCode);
        }
        fprintf(out, "\n");
        return;
    }

    memset(text, 0, sizeof(text));
    converted = WideCharToMultiByte(
        CP_ACP, 0, field->Value, -1, text, sizeof(text) - 1, NULL, NULL);
    if (converted > 0 && text[0] != '\0') {
        fprintf(out, "%s: %s\n", label, text);
    } else {
        fprintf(out, "%s: Empty response\n", label);
    }
}

static void PrintPowerState(FILE *out, DWORD flags)
{
    BOOL printed;

    printed = FALSE;
    fprintf(out, "Power state: ");
    if ((flags & LLB_POWER_ON_LINE) != 0) {
        fprintf(out, "AC power online");
        printed = TRUE;
    }
    if ((flags & LLB_CHARGING) != 0) {
        fprintf(out, "%sCharging", printed ? ", " : "");
        printed = TRUE;
    }
    if ((flags & LLB_DISCHARGING) != 0) {
        fprintf(out, "%sDischarging", printed ? ", " : "");
        printed = TRUE;
    }
    if ((flags & LLB_CRITICAL) != 0) {
        fprintf(out, "%sCritical state reported", printed ? ", " : "");
        printed = TRUE;
    }
    if (!printed) {
        fprintf(out, "No standard state reported");
    }
    fprintf(out, "\n");
}

static void PrintBatteryInformationSection(FILE *out, const LLB_BATTERY_RECORD *record)
{
    BOOL relativeUnits;
    double percentage;
    double temperatureC;
    DWORD scaleCount;
    DWORD i;

    relativeUnits = FALSE;
    fprintf(out, "\n============================================================\n");
    fprintf(out, "SLOT #%lu\n", (unsigned long)record->SlotNumber);
    fprintf(out, "------------------------------------------------------------\n");

    if (!record->DevicePathAvailable) {
        fprintf(out, "Device status: Unavailable\n");
    } else if (!record->DeviceOpened) {
        fprintf(out, "Battery status: Unable to open the battery device\n");
        if (record->OpenError != ERROR_SUCCESS) {
            PrintWindowsError(out, "Device access", record->OpenError);
        }
    } else if (!record->TagAvailable) {
        fprintf(out, "Battery status: Battery data could not be queried\n");
        if (record->TagError != ERROR_SUCCESS) {
            PrintWindowsError(out, "Battery query", record->TagError);
        }
    }

    if (!record->TagAvailable) {
        return;
    }

    fprintf(out, "\n  BATTERY SPECIFICATION\n");
    if (record->InformationAvailable) {
        relativeUnits = (record->Information.Capabilities & LLB_CAPACITY_RELATIVE) != 0;
        PrintCapabilityFlags(out, record->Information.Capabilities);
        fprintf(out, "Battery technology: %s\n",
                record->Information.Technology == 0 ? "Primary non-rechargeable battery" :
                record->Information.Technology == 1 ? "Secondary rechargeable battery" :
                "Unknown battery technology");
        fprintf(out, "Battery chemistry: %s\n", GetChemistryName(record->Information.Chemistry));
        fprintf(out, "Capacity units: %s\n",
                relativeUnits ? "Relative units reported by the driver" : "mWh");
        PrintCapacity(out, "Designed capacity", record->Information.DesignedCapacity, relativeUnits);
        PrintCapacity(out, "Full charged capacity", record->Information.FullChargedCapacity, relativeUnits);
        PrintCapacity(out, "Default alert level 1", record->Information.DefaultAlert1, relativeUnits);
        PrintCapacity(out, "Default alert level 2", record->Information.DefaultAlert2, relativeUnits);
        PrintCapacity(out, "Critical bias", record->Information.CriticalBias, relativeUnits);
        PrintDWORD(out, "Reported charge/discharge cycle count", record->Information.CycleCount);
    } else {
        fprintf(out, "Battery specification: Not reported\n");
        if (record->InformationError != ERROR_SUCCESS) {
            PrintWindowsError(out, "Battery information query", record->InformationError);
        }
    }

    fprintf(out, "\n  CURRENT STATUS\n");
    if (record->StatusAvailable) {
        PrintPowerState(out, record->Status.PowerState);
        PrintCapacity(out, "Current capacity", record->Status.Capacity,
                      record->InformationAvailable && relativeUnits);
        if (record->InformationAvailable &&
            record->Status.Capacity != LLB_UNKNOWN_VALUE &&
            record->Information.FullChargedCapacity != 0 &&
            record->Information.FullChargedCapacity != LLB_UNKNOWN_VALUE) {
            percentage = ((double)record->Status.Capacity * 100.0) /
                         (double)record->Information.FullChargedCapacity;
            fprintf(out, "Reported charge level: %.2f%%\n", percentage);
        } else {
            fprintf(out, "Reported charge level: Not available\n");
        }
        if (record->Status.Voltage == LLB_UNKNOWN_VALUE) {
            fprintf(out, "Terminal voltage: Unknown\n");
        } else {
            fprintf(out, "Terminal voltage: %lu mV (%.3f V)\n",
                    (unsigned long)record->Status.Voltage,
                    (double)record->Status.Voltage / 1000.0);
        }
        if ((DWORD)record->Status.Rate == LLB_UNKNOWN_RATE) {
            fprintf(out, "Charge/discharge rate: Unknown\n");
        } else if (record->InformationAvailable && relativeUnits) {
            fprintf(out, "Charge/discharge rate: %ld relative units per hour\n",
                    (long)record->Status.Rate);
        } else {
            fprintf(out, "Charge/discharge rate: %ld mW\n", (long)record->Status.Rate);
        }
    } else {
        fprintf(out, "Current battery status: Not reported\n");
        if (record->StatusError != ERROR_SUCCESS) {
            PrintWindowsError(out, "Battery status query", record->StatusError);
        }
    }

    fprintf(out, "\n  IDENTIFICATION\n");
    PrintWideField(out, "Device name", &record->DeviceName);
    PrintWideField(out, "Battery manufacturer", &record->Manufacturer);
    PrintWideField(out, "Serial number", &record->SerialNumber);
    PrintWideField(out, "Unique identifier", &record->UniqueId);

    fprintf(out, "\n  ADDITIONAL DATA\n");
    if (record->ManufactureDateAvailable) {
        fprintf(out, "Manufacturing date: %04u-%02u-%02u\n",
                (unsigned int)record->ManufactureDate.Year,
                (unsigned int)record->ManufactureDate.Month,
                (unsigned int)record->ManufactureDate.Day);
    } else {
        fprintf(out, "Manufacturing date: Not reported\n");
    }

    if (record->TemperatureAvailable && record->Temperature != LLB_UNKNOWN_VALUE) {
        temperatureC = ((double)record->Temperature / 10.0) - 273.15;
        fprintf(out, "Battery temperature: %.2f C\n", temperatureC);
    } else {
        fprintf(out, "Battery temperature: Not reported or unknown\n");
    }

    if (record->ScalesAvailable) {
        scaleCount = record->ScalesBytes / sizeof(BATTERY_REPORTING_SCALE);
        if (scaleCount > 4) {
            scaleCount = 4;
        }
        fprintf(out, "Capacity reporting scales: %lu\n", (unsigned long)scaleCount);
        for (i = 0; i < scaleCount; i++) {
            fprintf(out, "  Scale #%lu: capacity %lu, granularity %lu\n",
                    (unsigned long)(i + 1),
                    (unsigned long)record->Scales[i].Capacity,
                    (unsigned long)record->Scales[i].Granularity);
        }
    } else {
        fprintf(out, "Capacity reporting scales: Not reported\n");
    }

    if (record->EstimatedAvailable && record->EstimatedSeconds != LLB_UNKNOWN_VALUE) {
        fprintf(out, "Estimated remaining runtime: %lu seconds (%lu h %lu min %lu sec)\n",
                (unsigned long)record->EstimatedSeconds,
                (unsigned long)(record->EstimatedSeconds / 3600),
                (unsigned long)((record->EstimatedSeconds % 3600) / 60),
                (unsigned long)(record->EstimatedSeconds % 60));
    } else {
        fprintf(out, "Estimated remaining runtime: Not reported or unknown\n");
    }
}

static void PrintRawCallStatus(
    FILE *out,
    const char *label,
    DWORD errorCode,
    DWORD bytesReturned,
    DWORD expectedBytes)
{
    if (errorCode == ERROR_SUCCESS) {
        fprintf(out, "%s: SUCCESS\n", label);
    } else {
        fprintf(out, "%s: FAILED\n", label);
        PrintWindowsError(out, "Win32 error", errorCode);
    }
    fprintf(out, "Bytes returned: %lu\n", (unsigned long)bytesReturned);
    if (expectedBytes != 0 && bytesReturned < expectedBytes && errorCode == ERROR_SUCCESS) {
        fprintf(out, "Response validation: SHORT RESPONSE (expected at least %lu bytes)\n",
                (unsigned long)expectedBytes);
    } else if (expectedBytes != 0 && bytesReturned >= expectedBytes) {
        fprintf(out, "Response validation: COMPLETE\n");
    }
}

static void DumpInformationQuery(
    FILE *out,
    const char *label,
    DWORD batteryTag,
    LLB_QUERY_INFORMATION_LEVEL level,
    DWORD errorCode,
    DWORD bytesReturned,
    DWORD expectedBytes,
    const void *response,
    DWORD responseSize)
{
    LLB_QUERY_INFORMATION query;

    memset(&query, 0, sizeof(query));
    query.BatteryTag = batteryTag;
    query.InformationLevel = level;
    query.AtRate = 0;

    fprintf(out, "\n[%s]\n", label);
    fprintf(out, "IOCTL code: 0x%08lX\n", (unsigned long)LLB_IOCTL_QUERY_INFORMATION);
    PrintRawCallStatus(out, "IOCTL_BATTERY_QUERY_INFORMATION", errorCode,
                       bytesReturned, expectedBytes);
    DumpBytes(out, "Query input structure", &query, sizeof(query));
    DumpBytes(out, "Response buffer", response, responseSize);
}

static void PrintRawSection(
    FILE *out,
    const LLB_BATTERY_RECORD records[],
    DWORD recordCount,
    DWORD interfaceCount,
    BOOL truncated,
    DWORD collectionError)
{
    DWORD i;
    const LLB_BATTERY_RECORD *record;
    LLB_WAIT_STATUS waitStatus;

    fprintf(out, "\n============================================================\n");
    fprintf(out, "RAW DEVICE DATA\n");
    fprintf(out, "Complete raw responses, query structures, byte counts, and query results\n");
    fprintf(out, "============================================================\n");
    fprintf(out, "Enumerated battery interfaces: %lu\n", (unsigned long)interfaceCount);
    fprintf(out, "Enumeration result: %s\n",
            collectionError == ERROR_SUCCESS ? "SUCCESS" : "FAILED");
    if (collectionError != ERROR_SUCCESS) {
        PrintWindowsError(out, "Battery enumeration", collectionError);
    }
    if (truncated) {
        fprintf(out, "Warning: Raw output is limited to the first %d interface records.\n", LLB_MAX_SLOTS);
    }
    fprintf(out, "Created by: %s\n", LLB_AUTHOR);
    fprintf(out, "GitHub: %s\n", LLB_GITHUB);

    if (recordCount == 0) {
        fprintf(out, "No battery interface records are available.\n");
        return;
    }

    for (i = 0; i < recordCount; i++) {
        record = &records[i];
        fprintf(out, "\n------------------------------------------------------------\n");
        fprintf(out, "SLOT #%lu | RAW DATA\n", (unsigned long)record->SlotNumber);
        fprintf(out, "------------------------------------------------------------\n");

        fprintf(out, "Device open result: %s\n", record->DeviceOpened ? "OPENED" : "NOT OPENED");
        if (record->OpenError != ERROR_SUCCESS) {
            PrintWindowsError(out, "CreateFile result", record->OpenError);
        }

        fprintf(out, "\n[IOCTL_BATTERY_QUERY_TAG]\n");
        fprintf(out, "IOCTL code: 0x%08lX\n", (unsigned long)LLB_IOCTL_QUERY_TAG);
        if (!record->DeviceOpened) {
            fprintf(out, "IOCTL_BATTERY_QUERY_TAG result: NOT ISSUED (device could not be opened)\n");
        } else {
            fprintf(out, "IOCTL_BATTERY_QUERY_TAG result: %s\n",
                    record->TagAvailable ? "SUCCESS" : "FAILED OR INVALID RESPONSE");
        }
        fprintf(out, "Bytes returned: %lu\n", (unsigned long)record->TagBytesReturned);
        if (record->DeviceOpened) {
            DumpBytes(out, "Timeout input (DWORD)", &record->TagTimeout, sizeof(record->TagTimeout));
        } else {
            fprintf(out, "Timeout input: Not sent\n");
        }
        if (record->TagBytesReturned > 0) {
            DumpBytes(out, "Battery tag response", &record->BatteryTag,
                      LimitBytes(record->TagBytesReturned, sizeof(record->BatteryTag)));
        } else {
            DumpBytes(out, "Battery tag response", NULL, 0);
        }
        if (record->TagError != ERROR_SUCCESS) {
            PrintWindowsError(out, "Battery tag query", record->TagError);
        }

        if (!record->TagAvailable) {
            fprintf(out, "Additional battery queries were not issued because no valid battery tag was obtained.\n");
            continue;
        }

        DumpInformationQuery(out, "BATTERY_INFORMATION", record->BatteryTag,
            LLB_BatteryInformation, record->InformationError,
            record->InformationBytes, sizeof(record->Information),
            &record->Information, record->InformationBytes);

        memset(&waitStatus, 0, sizeof(waitStatus));
        waitStatus.BatteryTag = record->BatteryTag;
        waitStatus.Timeout = 0;
        waitStatus.PowerState = 0;
        waitStatus.LowCapacity = 0;
        waitStatus.HighCapacity = 0;
        fprintf(out, "\n[IOCTL_BATTERY_QUERY_STATUS]\n");
        fprintf(out, "IOCTL code: 0x%08lX\n", (unsigned long)LLB_IOCTL_QUERY_STATUS);
        PrintRawCallStatus(out, "IOCTL_BATTERY_QUERY_STATUS",
            record->StatusError, record->StatusBytes, sizeof(record->Status));
        DumpBytes(out, "BATTERY_WAIT_STATUS input structure", &waitStatus, sizeof(waitStatus));
        DumpBytes(out, "BATTERY_STATUS response", &record->Status, record->StatusBytes);

        DumpInformationQuery(out, "BATTERY_DEVICE_NAME", record->BatteryTag,
            LLB_BatteryDeviceName, record->DeviceName.ErrorCode,
            record->DeviceName.BytesReturned, 0,
            record->DeviceName.Value, record->DeviceName.BytesReturned);
        DumpInformationQuery(out, "BATTERY_MANUFACTURER_NAME", record->BatteryTag,
            LLB_BatteryManufactureName, record->Manufacturer.ErrorCode,
            record->Manufacturer.BytesReturned, 0,
            record->Manufacturer.Value, record->Manufacturer.BytesReturned);
        DumpInformationQuery(out, "BATTERY_SERIAL_NUMBER", record->BatteryTag,
            LLB_BatterySerialNumber, record->SerialNumber.ErrorCode,
            record->SerialNumber.BytesReturned, 0,
            record->SerialNumber.Value, record->SerialNumber.BytesReturned);
        DumpInformationQuery(out, "BATTERY_UNIQUE_ID", record->BatteryTag,
            LLB_BatteryUniqueID, record->UniqueId.ErrorCode,
            record->UniqueId.BytesReturned, 0,
            record->UniqueId.Value, record->UniqueId.BytesReturned);

        DumpInformationQuery(out, "BATTERY_MANUFACTURE_DATE", record->BatteryTag,
            LLB_BatteryManufactureDate, record->ManufactureDateError,
            record->ManufactureDateBytes, sizeof(record->ManufactureDate),
            &record->ManufactureDate, record->ManufactureDateBytes);
        DumpInformationQuery(out, "BATTERY_TEMPERATURE", record->BatteryTag,
            LLB_BatteryTemperature, record->TemperatureError,
            record->TemperatureBytes, sizeof(record->Temperature),
            &record->Temperature, record->TemperatureBytes);
        DumpInformationQuery(out, "BATTERY_REPORTING_SCALE", record->BatteryTag,
            LLB_BatteryGranularityInformation, record->ScalesError,
            record->ScalesBytes, sizeof(BATTERY_REPORTING_SCALE),
            record->Scales, record->ScalesBytes);
        DumpInformationQuery(out, "BATTERY_ESTIMATED_TIME", record->BatteryTag,
            LLB_BatteryEstimatedTime, record->EstimatedError,
            record->EstimatedBytes, sizeof(record->EstimatedSeconds),
            &record->EstimatedSeconds, record->EstimatedBytes);

        fprintf(out, "\n[Decoded identification bytes]\n");
        DumpBytes(out, "Chemistry identifier (4 bytes)", record->Information.Chemistry,
                  sizeof(record->Information.Chemistry));
        fprintf(out, "Chemistry interpretation: %s\n",
                GetChemistryName(record->Information.Chemistry));
        fprintf(out, "Technology byte: 0x%02X (%u)\n",
                (unsigned int)record->Information.Technology,
                (unsigned int)record->Information.Technology);
        fprintf(out, "Capabilities raw value: 0x%08lX\n",
                (unsigned long)record->Information.Capabilities);
    }
}

static void RenderReport(
    FILE *out,
    const LLB_BATTERY_RECORD records[],
    DWORD recordCount,
    DWORD interfaceCount,
    BOOL truncated,
    DWORD collectionError,
    const LLB_OPTIONS *options)
{
    SYSTEMTIME now;
    DWORD i;
    DWORD readableCount;

    GetLocalTime(&now);
    readableCount = 0;
    for (i = 0; i < recordCount; i++) {
        if (records[i].TagAvailable) {
            readableCount++;
        }
    }

    fprintf(out, "============================================================\n");
    fprintf(out, "                 LOW-LEVEL BATTERY INFO\n");
    fprintf(out, "============================================================\n");
    fprintf(out, "Created by: %s\n", LLB_AUTHOR);
    fprintf(out, "GitHub: %s\n", LLB_GITHUB);
    fprintf(out, "Version: %s\n", LLB_VERSION);
    fprintf(out, "Timestamp: %04u-%02u-%02u %02u:%02u:%02u\n",
            (unsigned int)now.wYear, (unsigned int)now.wMonth, (unsigned int)now.wDay,
            (unsigned int)now.wHour, (unsigned int)now.wMinute, (unsigned int)now.wSecond);
    fprintf(out, "Battery slots detected: %lu\n", (unsigned long)interfaceCount);
    fprintf(out, "Slots displayed: %lu\n", (unsigned long)recordCount);
    fprintf(out, "Slots with readable battery data: %lu\n", (unsigned long)readableCount);
    if (truncated) {
        fprintf(out, "Warning: The display limit of %d battery slots was reached.\n", LLB_MAX_SLOTS);
    }
    if (collectionError != ERROR_SUCCESS) {
        PrintWindowsError(out, "Battery enumeration", collectionError);
    }

    if (recordCount == 0) {
        fprintf(out, "\nNo battery slots were detected.\n");
    }

    for (i = 0; i < recordCount; i++) {
        PrintBatteryInformationSection(out, &records[i]);
    }

    if (out == stdout && !options->Once) {
        fprintf(out, "\nKeys: [R] Refresh  [E] Export report  [Q] Quit\n");
        fprintf(out, "Press a key to continue. Data refresh is manual only.\n");
    }

    if (options->IncludeRaw) {
        PrintRawSection(out, records, recordCount, interfaceCount, truncated, collectionError);
    }
}

static BOOL SaveReport(
    const char *path,
    const LLB_BATTERY_RECORD records[],
    DWORD recordCount,
    DWORD interfaceCount,
    BOOL truncated,
    DWORD collectionError,
    const LLB_OPTIONS *options)
{
    FILE *file;
    BOOL success;

    file = fopen(path, "w");
    if (file == NULL) {
        return FALSE;
    }

    RenderReport(file, records, recordCount, interfaceCount, truncated,
                 collectionError, options);
    success = (ferror(file) == 0);
    if (fclose(file) != 0) {
        success = FALSE;
    }
    return success;
}

static void MakeTimestampFilename(char *path, size_t pathSize)
{
    SYSTEMTIME now;
    GetLocalTime(&now);
    snprintf(path, pathSize,
             "llb-report-%04u%02u%02u-%02u%02u%02u.txt",
             (unsigned int)now.wYear, (unsigned int)now.wMonth, (unsigned int)now.wDay,
             (unsigned int)now.wHour, (unsigned int)now.wMinute, (unsigned int)now.wSecond);
    path[pathSize - 1] = '\0';
}

static void PrintUsage(FILE *out)
{
    fprintf(out, "Low-Level Battery Info (llb.exe), version %s\n", LLB_VERSION);
    fprintf(out, "Created by: %s\n", LLB_AUTHOR);
    fprintf(out, "GitHub: %s\n\n", LLB_GITHUB);
    fprintf(out, "Usage: llb.exe [options]\n\n");
    fprintf(out, "Options:\n");
    fprintf(out, "  -h, --help              Show this help text\n");
    fprintf(out, "  --version               Show program version\n");
    fprintf(out, "  -o, --once              Collect and display one snapshot\n");
    fprintf(out, "  -e, --export <file>     Export a report to a text file\n");
    fprintf(out, "      --raw               Include the raw data section (default)\n");
    fprintf(out, "      --no-raw            Hide the raw data section\n");
    fprintf(out, "      --no-pause          Do not wait after --once\n\n");
    fprintf(out, "Interactive keys:\n");
    fprintf(out, "  R  Refresh battery data manually\n");
    fprintf(out, "  E  Export the current report\n");
    fprintf(out, "  Q or Esc  Exit\n\n");
    fprintf(out, "Examples:\n");
    fprintf(out, "  llb.exe\n");
    fprintf(out, "  llb.exe --once\n");
    fprintf(out, "  llb.exe --export battery-report.txt\n");
    fprintf(out, "  llb.exe --once --no-raw --export snapshot.txt\n");
}

static int ParseArguments(int argc, char **argv, LLB_OPTIONS *options)
{
    int i;

    memset(options, 0, sizeof(*options));
    options->IncludeRaw = TRUE;
    options->PauseAtEnd = TRUE;

    for (i = 1; i < argc; i++) {
        if (strcmp(argv[i], "-h") == 0 || strcmp(argv[i], "--help") == 0) {
            PrintUsage(stdout);
            return 0;
        } else if (strcmp(argv[i], "--version") == 0) {
            printf("Low-Level Battery Info %s\n", LLB_VERSION);
            printf("Created by: %s\n", LLB_AUTHOR);
            printf("GitHub: %s\n", LLB_GITHUB);
            return 0;
        } else if (strcmp(argv[i], "-o") == 0 || strcmp(argv[i], "--once") == 0) {
            options->Once = TRUE;
        } else if (strcmp(argv[i], "--raw") == 0) {
            options->IncludeRaw = TRUE;
        } else if (strcmp(argv[i], "--no-raw") == 0) {
            options->IncludeRaw = FALSE;
        } else if (strcmp(argv[i], "--no-pause") == 0) {
            options->PauseAtEnd = FALSE;
        } else if (strcmp(argv[i], "-e") == 0 || strcmp(argv[i], "--export") == 0) {
            if (i + 1 >= argc) {
                fprintf(stderr, "Missing file path for %s.\n", argv[i]);
                PrintUsage(stderr);
                return -1;
            }
            i++;
            if (argv[i][0] == '\0') {
                fprintf(stderr, "The export path cannot be empty.\n");
                return -1;
            }
            CopyText(options->ExportPath, sizeof(options->ExportPath), argv[i]);
        } else {
            fprintf(stderr, "Unknown option: %s\n\n", argv[i]);
            PrintUsage(stderr);
            return -1;
        }
    }
    return 1;
}

int main(int argc, char **argv)
{
    LLB_OPTIONS options;
    LLB_BATTERY_RECORD *records;
    DWORD recordCount;
    DWORD interfaceCount;
    DWORD collectionError;
    BOOL truncated;
    BOOL consoleAvailable;
    BOOL running;
    BOOL forceRefresh;
    int key;
    char exportPath[LLB_TEXT_SIZE];
    int parseResult;

    parseResult = ParseArguments(argc, argv, &options);
    if (parseResult == 0) {
        return 0;
    }
    if (parseResult < 0) {
        return 2;
    }

    signal(SIGINT, HandleSignal);
    SetConsoleTitleA("Low-Level Battery Info - llb.exe");
    consoleAvailable = (GetConsoleWindow() != NULL && _isatty(_fileno(stdout)) != 0);
    records = (LLB_BATTERY_RECORD *)calloc(LLB_MAX_SLOTS, sizeof(LLB_BATTERY_RECORD));
    if (records == NULL) {
        fprintf(stderr, "Memory allocation failed.\n");
        if (consoleAvailable && options.PauseAtEnd) {
            fprintf(stderr, "Press any key to exit...\n");
            _getch();
        }
        return 1;
    }

    running = TRUE;
    forceRefresh = TRUE;
    recordCount = 0;
    interfaceCount = 0;
    collectionError = ERROR_SUCCESS;
    truncated = FALSE;

    while (running && !LLB_StopRequested) {
        if (forceRefresh) {
            forceRefresh = FALSE;
            CollectBatteries(records, &recordCount, &interfaceCount,
                             &truncated, &collectionError);

            if (consoleAvailable && !options.Once) {
                system("cls");
            }

            RenderReport(stdout, records, recordCount, interfaceCount,
                         truncated, collectionError, &options);
            fflush(stdout);

            if (options.ExportPath[0] != '\0') {
                if (!SaveReport(options.ExportPath, records, recordCount,
                                interfaceCount, truncated, collectionError, &options)) {
                    fprintf(stderr, "Unable to write report: %s\n", options.ExportPath);
                }
            }
        }

        if (options.Once || !consoleAvailable) {
            break;
        }

        key = _getch();
        if (key == 0 || key == 224) {
            _getch();
            continue;
        }

        if (key == 'q' || key == 'Q' || key == 27) {
            running = FALSE;
        } else if (key == 'r' || key == 'R') {
            forceRefresh = TRUE;
        } else if (key == 'e' || key == 'E') {
            if (options.ExportPath[0] != '\0') {
                CopyText(exportPath, sizeof(exportPath), options.ExportPath);
            } else {
                MakeTimestampFilename(exportPath, sizeof(exportPath));
            }
            if (SaveReport(exportPath, records, recordCount, interfaceCount,
                           truncated, collectionError, &options)) {
                printf("Report exported: %s\n", exportPath);
            } else {
                printf("Report export failed: %s\n", exportPath);
            }
            printf("Press any key to return to the current snapshot.\n");
            fflush(stdout);
            _getch();
            system("cls");
            RenderReport(stdout, records, recordCount, interfaceCount,
                         truncated, collectionError, &options);
            fflush(stdout);
            forceRefresh = FALSE;
        }
    }

    if (options.Once && options.PauseAtEnd && consoleAvailable) {
        fprintf(stdout, "\nPress any key to close Low-Level Battery Info...\n");
        fflush(stdout);
        _getch();
    }

    free(records);
    return 0;
}
