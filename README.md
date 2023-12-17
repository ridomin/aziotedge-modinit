# Azure IoT Edge Module Initializer 

[![NuGet](https://img.shields.io/nuget/v/aziotedge-modinit.svg)](https://www.nuget.org/packages/aziotedge-modinit)


dotnet tool to initialize Azure IoT Edge modules in Azure IoT Hub.

## Installation

```bash
dotnet tool install -g aziotedge-modinit
```

> requires donet sdk 6.0 or later

## Usage

```bash
aziotedge-modinit --ConnectionStrings:IoTEdge="HostName=<HUB_ID>.azure-devices.net;DeviceId=<EDGEDEVICE_ID_>;SharedAccessKey=<EDGEDEVICE_SASKEY>" --moduleId="<$dgeHub|custom>"
```

> ⚠️ Warning ⚠️
> When using `$edgeHub` or `$edgeAgent` in `--moduleId` make sure you escape the `$` with `\$` in bash or `` `$`` in powershell.

# TL;DR;

Azure IoT Hub creates Azure IoT Edge device identities, from the Azure Portal, Azure CLI, Azure PowerShell, or Azure IoT Hub REST API. 

However, once created the system modules `$edgeHub` and `$edgeAgent` are not automatically initialized, and lack the connection string.
Additionally, to create new module identities, you need to complete the `SetModules` workflow, which requires the connection string of the IoT Hub.

Both problems can be solved by using some _hidden_ APIs that allow to initialize the system modules and create new modules without the need of the IoT Hub connection string, and using the Azure IoT edge connection string.


## Quick Start

The following scripts create a new Azure IoT Edge device, initialize the system modules, and create a new module identity. Requires the `az cli`.

### Set configuration variables

PowerShell: 

```ps1
$SUB_ID="<azure_sub>"
$HUB_ID="<hubname>"
$EDGE_ID="<edge_device>"
```

Bash
```bash
export SUB_ID=<azure_sub>
export HUB_ID=<hubname>
export EDGE_ID=<edge_device>
```

### Create IoT Edge Device and System Modules

Powershell:


```ps1
az account set -s $SUB_ID
az iot hub device-identity create -n $HUB_ID -d $EDGE_ID --edge-enabled
az iot edge set-modules -n $HUB_ID -d $EDGE_ID -k deploy.json
```

Bash 

```bash
az account set -s $SUB_ID
az iot hub device-identity create -n $HUB_ID -d $EDGE_ID --edge-enabled
az iot edge set-modules -n $HUB_ID -d $EDGE_ID -k deploy.json
```

### Initialize System Modules

Powershell:

```ps1
$sasKey=(az iot hub device-identity show -n $HUB_ID -d $EDGE_ID --query authentication.symmetricKey.primaryKey -o tsv)
aziotedge-modinit --ConnectionStrings:IoTEdge="HostName=$HUB_ID.azure-devices.net;DeviceId=$EDGE_ID;SharedAccessKey=$sasKey" --moduleId="`$edgeHub"
```

Bash

```bash
sasKey=$(az iot hub device-identity show -n $HUB_ID -d $EDGE_ID --query authentication.symmetricKey.primaryKey -o tsv)
aziotedge-modinit --ConnectionStrings:IoTEdge="HostName=$HUB_ID.azure-devices.net;DeviceId=$EDGE_ID;SharedAccessKey=$sasKey" --moduleId="\$edgeHub"
```

### Create new module

Bash/ Powershell:

```text
aziotedge-modinit --ConnectionStrings:IoTEdge="HostName=$HUB_ID.azure-devices.net;DeviceId=$EDGE_ID;SharedAccessKey=$sasKey" --moduleId="myModule"
```
