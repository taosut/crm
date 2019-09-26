#!/bin/bash

commitID=$BUILD_SOURCEVERSION

echo $BUILD_SOURCEVERSION

echo "##vso[task.setvariable variable=shortCommit;isOutput=true]${commitID:0:7}"
echo "##vso[task.setvariable variable=helmVersion;isOutput=true]3.0.0-beta.3"
echo "##vso[task.setvariable variable=kubectlVersion;isOutput=true]1.12.8"

if [[ $BUILD_SOURCEBRANCHNAME != "master" ]]; then
  echo "##vso[task.setvariable variable=dockerImageTag;isOutput=true]$BUILD_SOURCEBRANCHNAME"
else
  echo "##vso[task.setvariable variable=dockerImageTag;isOutput=true]$commitID"
fi
