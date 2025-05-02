#!/bin/bash
set -euo pipefail
IFS=$'\n\t'

export UNO_UITEST_PLATFORM=iOS
export BASE_ARTIFACTS_PATH=$BUILD_ARTIFACTSTAGINGDIRECTORY/ios/
export UNO_UITEST_IOSBUNDLE_PATH=$BUILD_SOURCESDIRECTORY/build/iOS_UITest_$VARIANT_NAME/ios-uitest/Chefs.app
export UNO_UITEST_SCREENSHOT_PATH=$BASE_ARTIFACTS_PATH/screenshots
export UNO_UITEST_PROJECT_PATH=$BUILD_SOURCESDIRECTORY/Chefs.UITests
export UNO_UITEST_PROJECT=$UNO_UITEST_PROJECT_PATH/Chefs.UITests.csproj
export UNO_UITEST_LOGFILE=$UNO_UITEST_SCREENSHOT_PATH/nunit-log.txt
export UNO_UITEST_NUNIT_VERSION=3.12.0
export UNO_UITEST_NUGET_URL=https://dist.nuget.org/win-x86-commandline/v5.7.0/nuget.exe
export UNO_ORIGINAL_TEST_RESULTS=$BUILD_SOURCESDIRECTORY/build/ios-uitest-results-$VARIANT_NAME.xml
export UNO_UITEST_RUNTIMETESTS_RESULTS_FILE_PATH=$UNO_ORIGINAL_TEST_RESULTS
export UNO_TESTS_RESPONSE_FILE=$BUILD_SOURCESDIRECTORY/build/nunit.response
export UNO_UITEST_SIMULATOR_VERSION="com.apple.CoreSimulator.SimRuntime.iOS-17-5"
export UNO_UITEST_SIMULATOR_NAME="iPad (10th generation)"
export UNO_UITEST_BINARY=$BUILD_SOURCESDIRECTORY/Chefs.UITests/bin/Release/net9.0/Chefs.UITests.dll

export UITEST_TEST_TIMEOUT=120m

echo "Listing iOS simulators"
xcrun simctl list devices --json

/Applications/Xcode.app/Contents/Developer/Applications/Simulator.app/Contents/MacOS/Simulator &

# Prime the output directory
mkdir -p $UNO_UITEST_SCREENSHOT_PATH/_logs

##
## Pre-install the application to avoid https://github.com/microsoft/appcenter/issues/2389
##

while true; do
	export UITEST_IOSDEVICE_ID=`xcrun simctl list -j | jq -r --arg sim "$UNO_UITEST_SIMULATOR_VERSION" --arg name "$UNO_UITEST_SIMULATOR_NAME" '.devices[$sim] | .[] | select(.name==$name) | .udid'`
	export UITEST_IOSDEVICE_DATA_PATH=`xcrun simctl list -j | jq -r --arg sim "$UNO_UITEST_SIMULATOR_VERSION" --arg name "$UNO_UITEST_SIMULATOR_NAME" '.devices[$sim] | .[] | select(.name==$name) | .dataPath'`

	if [ -n "$UITEST_IOSDEVICE_ID" ]; then
		break
	fi

	echo "Waiting for the simulator to be available"
	sleep 5
done

echo "Simulator Data Path: $UITEST_IOSDEVICE_DATA_PATH"
cp "$UITEST_IOSDEVICE_DATA_PATH/../device.plist" $UNO_UITEST_SCREENSHOT_PATH/_logs

echo "Starting simulator: [$UITEST_IOSDEVICE_ID] ($UNO_UITEST_SIMULATOR_VERSION / $UNO_UITEST_SIMULATOR_NAME)"

# check for the presence of idb, and install it if it's not present
export PATH=$PATH:~/.local/bin

if ! command -v idb &> /dev/null
then
	echo "Installing idb"
	brew install pipx
	# # https://github.com/microsoft/appcenter/issues/2605#issuecomment-1854414963
	brew tap facebook/fb
	brew install idb-companion
	pipx install fb-idb
else
	echo "Using idb from:" `command -v idb`
fi

xcrun simctl boot "$UITEST_IOSDEVICE_ID" || true

idb install --udid "$UITEST_IOSDEVICE_ID" "$UNO_UITEST_IOSBUNDLE_PATH"

cd $BUILD_SOURCESDIRECTORY/build

# Imported app bundle from artifacts is not executable
sudo chmod -R +x $UNO_UITEST_IOSBUNDLE_PATH

cd $UNO_UITEST_PROJECT_PATH

dotnet build /r /p:TargetFrameworkOverride=net9.0-android "/p:UseSkiaRendering=$USE_SKIA_RENDERING" /p:Configuration=Release

## Run tests
dotnet test $UNO_UITEST_BINARY \
	-l:"console;verbosity=normal" \
	--logger "nunit;LogFileName=$UNO_UITEST_RUNTIMETESTS_RESULTS_FILE_PATH" \
	--blame-hang-timeout $UITEST_TEST_TIMEOUT \
	-v m \
	|| true

echo "Current system date"
date

## Copy the results file to the results folder
cp $UNO_UITEST_RUNTIMETESTS_RESULTS_FILE_PATH $BASE_ARTIFACTS_PATH

# export the simulator logs
export LOG_FILEPATH=$UNO_UITEST_SCREENSHOT_PATH/_logs
export TMP_LOG_FILEPATH=/tmp/DeviceLog-`date +"%Y%m%d%H%M%S"`.logarchive
export LOG_FILEPATH_FULL=$LOG_FILEPATH/DeviceLog-`date +"%Y%m%d%H%M%S"`.txt

xcrun simctl spawn booted log collect --output $TMP_LOG_FILEPATH
log show --style syslog $TMP_LOG_FILEPATH > $LOG_FILEPATH_FULL
