REM C:\Windows\Microsoft.NET\Framework\v2.0.50727\CONFIG\machine.config
REM C:\Windows\Microsoft.NET\Framework\v4.0.30319\Config\machine.config
REM C:\Windows\Microsoft.NET\Framework64\v2.0.50727\CONFIG\machine.config
REM C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Config\machine.config


SET siteName="https://test3.com"
SET count="24"

.\UpdateMachineConfigYo.exe "c:\users\jeremy\desktop\machine.config" "*" "2"
.\UpdateMachineConfigYo.exe "c:\users\jeremy\desktop\machine.config" "https://test.com" %count%
.\UpdateMachineConfigYo.exe "c:\users\jeremy\desktop\machine.config" "https://test2.com" %count%
.\UpdateMachineConfigYo.exe "c:\users\jeremy\desktop\machine.config" "https://test3.com" %count%