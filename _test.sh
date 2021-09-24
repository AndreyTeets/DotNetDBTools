#!/bin/bash
cd "$(dirname "$(readlink -f "${BASH_SOURCE[0]}")")"

pwsh -NoProfile -ExecutionPolicy Bypass -Command "./build/scripts/test.ps1"

read -n1 -r -p "Press any key to close this window..."