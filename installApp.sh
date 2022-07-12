sudo killall -9 dotnet
nohup dotnet /home/azureuser/app/publish/Admin.API.dll &>/dev/null &
