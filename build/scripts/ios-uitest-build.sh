#!/bin/bash
set -euo pipefail
IFS=$'\n\t'

export UNO_UITEST_IOS_PROJECT_PATH=$BUILD_SOURCESDIRECTORY/Chefs

cd $UNO_UITEST_IOS_PROJECT_PATH
dotnet build -f net9.0-ios -c Release "/p:UseSkiaRendering=$USE_SKIA_RENDERING" /p:RuntimeIdentifier=iossimulator-x64 /p:IsUiAutomationMappingEnabled=True /bl:$BUILD_ARTIFACTSTAGINGDIRECTORY/ios-uitest-$VARIANT_NAME.binlog
