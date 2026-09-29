$ErrorActionPreference = "Stop"
cd E:\Semester_7\PRN232\LAB_1\prn232-lab1-grader_1\prn232-lab1-grader
. .\lib\Common.ps1
. .\lib\DynamicChecks.ps1
$rubric = Get-Content .\rubric.json -Raw | ConvertFrom-Json
$manifest = Get-Content E:\Semester_7\PRN232\LAB_1\submission.json -Raw | ConvertFrom-Json
$r = Invoke-DynamicChecks -BaseUrl 'http://localhost:8080' -Manifest $manifest -Settings $rubric.settings

Write-Host "--- DY-08 ---"
$r['DY-08'] | ConvertTo-Json -Depth 5

Write-Host "--- DY-11 ---"
$r['DY-11'] | ConvertTo-Json -Depth 5

