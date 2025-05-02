#!/bin/bash
set -euo pipefail
IFS=$'\n\t'

export UNO_UITEST_ANDROID_PROJECT_PATH=$BUILD_SOURCESDIRECTORY/Chefs

cd $UNO_UITEST_ANDROID_PROJECT_PATH
dotnet publish -f net8.0-android /p:TargetFrameworkOverride=net8.0-android "/p:UseSkiaRendering=$USE_SKIA_RENDERING" -c Release /p:EmbedAssembliesIntoApk=True /p:RuntimeIdentifier=android-x64 /p:IsUiAutomationMappingEnabled=True /p:AndroidUseSharedRuntime=False /p:AotAssemblies=False /p:RunAOTCompilation=False /bl:$BUILD_ARTIFACTSTAGINGDIRECTORY/android-uitest-$VARIANT_NAME.binlog
