#!/bin/bash
set -x #echo on
set -euo pipefail
IFS=$'\n\t'

export BASE_ARTIFACTS_PATH=$BUILD_ARTIFACTSTAGINGDIRECTORY/wasm-$VARIANT_NAME
export UNO_UITEST_TARGETURI=http://localhost:5000
export UNO_UITEST_DRIVERPATH_CHROME=$BUILD_SOURCESDIRECTORY/build/node_modules/chromedriver/lib/chromedriver
export UNO_UITEST_CHROME_BINARY_PATH=~/.cache/puppeteer/chrome/linux-127.0.6533.72/chrome-linux64/chrome
export UNO_UITEST_SCREENSHOT_PATH=$BASE_ARTIFACTS_PATH/screenshots
export UNO_UITEST_PLATFORM=Browser
export UNO_UITEST_PROJECT=$BUILD_SOURCESDIRECTORY/Chefs.UITests/Chefs.UITests.csproj
export UNO_UITEST_LOGFILE=$BASE_ARTIFACTS_PATH/nunit-log.txt
export UNO_UITEST_WASM_PROJECT=$BUILD_SOURCESDIRECTORY/Chefs
export UNO_UITEST_WASM_OUTPUT_PATH=$BUILD_SOURCESDIRECTORY/Chefs/bin/Release/net9.0-browserwasm/publish/wwwroot/
export UNO_UITEST_NUNIT_VERSION=3.11.1
export UNO_UITEST_NUGET_URL=https://dist.nuget.org/win-x86-commandline/v5.7.0/nuget.exe
export UNO_ORIGINAL_TEST_RESULTS=$BUILD_SOURCESDIRECTORY/build/wasm-uitest-results-$VARIANT_NAME.xml
export UNO_UITEST_RUNTIMETESTS_RESULTS_FILE_PATH=$UNO_ORIGINAL_TEST_RESULTS
export UNO_TESTS_RESPONSE_FILE=$BUILD_SOURCESDIRECTORY/build/nunit.response
export UITEST_TEST_TIMEOUT=60m
export UNO_UITEST_BINARY=$BUILD_SOURCESDIRECTORY/Chefs.UITests/bin/Release/net9.0/Chefs.UITests.dll
TEST_FAILED_FLAG=.tests-failed

cd $UNO_UITEST_WASM_PROJECT

dotnet publish /r /p:Configuration=Release /p:TargetFrameworkOverride=net9.0-browserwasm /p:TargetFramework=net9.0-browserwasm "/p:UseSkiaRendering=$USE_SKIA_RENDERING" /p:IsUiAutomationMappingEnabled=True /bl:$BASE_ARTIFACTS_PATH/wasm-uitest-$VARIANT_NAME.binlog
cd $BUILD_SOURCESDIRECTORY/build
mkdir -p tools

npm i chromedriver@127.0.0
npm i puppeteer@22.14.0

# Download chromium explicitly
pushd ./node_modules/puppeteer
npm install
popd

sudo apt install tree

tree ~/.cache/puppeteer/chrome/linux-127.0.6533.72

# install dotnet serve / Remove as needed
dotnet tool uninstall dotnet-serve -g || true
dotnet tool uninstall dotnet-serve --tool-path $BUILD_SOURCESDIRECTORY/build/tools || true
dotnet tool install dotnet-serve --version 1.10.140 --tool-path $BUILD_SOURCESDIRECTORY/build/tools || true
export PATH="$PATH:$BUILD_SOURCESDIRECTORY/build/tools"

mkdir -p $UNO_UITEST_SCREENSHOT_PATH

## The python server serves the current working directory, and may be changed by the nunit runner
dotnet-serve -p 5000 -d "$UNO_UITEST_WASM_OUTPUT_PATH" &

echo "Test Parameters:"
echo "  Timeout=$UITEST_TEST_TIMEOUT"

cd $BUILD_SOURCESDIRECTORY/Chefs.UITests

if dotnet test \
	-l:"console;verbosity=diag" \
	-c Release \
	--logger "nunit;LogFileName=$UNO_UITEST_RUNTIMETESTS_RESULTS_FILE_PATH" \
	--blame-hang-timeout $UITEST_TEST_TIMEOUT \
	-v m \
	"/p:UseSkiaRendering=$USE_SKIA_RENDERING";
then
	echo "Tests passed"
	rm -f $TEST_FAILED_FLAG
else
	echo "Tests failed"
	if [[ ! -f $TEST_FAILED_FLAG ]];
	then
		touch $TEST_FAILED_FLAG
	fi
fi

## Copy the results file to the results folder
cp --backup=t $UNO_UITEST_RUNTIMETESTS_RESULTS_FILE_PATH $UNO_UITEST_SCREENSHOT_PATH

if [[ ! -f $UNO_UITEST_RUNTIMETESTS_RESULTS_FILE_PATH ]]; then
	echo "ERROR: The test results file $UNO_UITEST_RUNTIMETESTS_RESULTS_FILE_PATH does not exist (did nunit crash ?)"
	return 1
fi

if [[ -f $TEST_FAILED_FLAG ]]; then
	echo "ERROR: The tests failed"
	return 1
fi